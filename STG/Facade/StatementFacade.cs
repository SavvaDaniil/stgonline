using Microsoft.AspNetCore.Http;
using STG.Data;
using STG.DTO.Statement;
using STG.Entities;
using STG.Observer;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.Payment;
using STG.ViewModels.Statement;
using STG.ViewModels.TeacherViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class StatementFacade
    {
        private ApplicationDbContext _dbc;
        public StatementFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<Statement> addAfterPreUserWithAppointment(User user, PreUserWithAppointment preUserWithAppointment)
        {
            StatementService statementService = new StatementService(_dbc);
            return await statementService.add(user, preUserWithAppointment);
        }

        public async Task<Statement> buyAfterSuccessfullPayment(Payment payment)
        {
            StatementService statementService = new StatementService(_dbc);
            Statement statement = await statementService.updateAfterSuccessfullPayment(payment.statement);
            if (statement == null) return null;

            StatementObserver statementObserver = new StatementObserver();
            TeacherService teacherService = new TeacherService(_dbc);
            List<Teacher> curators = await teacherService.listAllByListString(statement.listOfTeachers);

            UserService userService = new UserService(_dbc);
            User user = await userService.findById(payment.user.Id);

            //AmoCRM

            statementObserver.sendStatementToAdmins(user, statement, curators);

            return statement;
        }

        public async Task<JsonAnswerStatus> addByUser(HttpContext httpContext, string domainHost, StatementNewDTO statementNewDTO)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if (user == null) return new JsonAnswerStatus("error", "no user");


            StatementService statementService = new StatementService(_dbc);
            Statement statement = await statementService.add(user, statementNewDTO);
            if(statement == null)return new JsonAnswerStatus("error", "no statement");

            PaymentFacade paymentFacade = new PaymentFacade(_dbc);
            PaymentLiteViewModel paymentLiteViewModel = await paymentFacade.generateForStatement(domainHost, statement);
            if (paymentLiteViewModel == null) return new JsonAnswerStatus("error", "generate_payment_error");

            return new JsonAnswerStatus(
                "success",
                null,
                paymentLiteViewModel
            );
        }

        public async Task<JsonAnswerStatus> get(HttpContext httpContext, int id)
        {
            StatementService statementService = new StatementService(_dbc);
            Statement statement = await statementService.findById(id);
            if(statement == null)return new JsonAnswerStatus("error", "no statement");

            TeacherFacade teacherFacade = new TeacherFacade(_dbc);
            ListTeacherCuratorPreviewsViewModel mentors = await teacherFacade.listAllCuratorForMentoring();

            RegionFacade regionFacade = new RegionFacade(_dbc);
            Region region = await regionFacade.getRegionByUser(statement.user);

            AdminFacade adminFacade = new AdminFacade(_dbc);
            Admin admin = await adminFacade.getCurrentOrNull(httpContext);

            StatementEditViewModel statementEditViewModel = new StatementEditViewModel(
                statement.id,
                statement.user.Id,
                statement.status,
                statement.user.Username,
                statement.user.secondname,
                statement.user.firstname,
                (region != null ? region.name : null),
                await getListOfTeachersOfStatement(statement.listOfTeachers),
                (statement.date_of_add != null ? statement.date_of_add.Value.ToString("HH:mm:ss dd.MM.yyyy") : null),
                (statement.date_of_active != null ? statement.date_of_active.Value.ToString("HH:mm:ss dd.MM.yyyy") : null),
                statement.is_need_curator,
                statement.experience,
                statement.expectations,
                statement.expected_time_for_lessons,
                statement.idols,
                statement.link1,
                statement.link2,
                statement.link3,
                statement.teacher,
                mentors,
                (admin.level == 1 ? true : false)
            );

            return new JsonAnswerStatus("success", null, statementEditViewModel);
        }

        public async Task<JsonAnswerStatus> edit(HttpContext httpContext, StatementEditDTO statementEditDTO)
        {
            StatementService statementService = new StatementService(_dbc);
            Statement statement = await statementService.findById(statementEditDTO.id);
            if (statement == null) return new JsonAnswerStatus("error", "not_found_statement");

            //проверяем доступ, вдруг уже установлен, а доступ админа не тот
            AdminFacade adminFacade = new AdminFacade(_dbc);
            Admin admin = await adminFacade.getCurrentOrNull(httpContext);
            if(statement.teacher != null)
            {
                if(statement.teacher.id != statementEditDTO.id_of_curator && admin.level != 1)
                {
                    return new JsonAnswerStatus("error", "not_access_only_superuser");
                }
            }

            Teacher teacher = null;
            if (statementEditDTO.id_of_curator != 0)
            {
                TeacherService teacherService = new TeacherService(_dbc);
                teacher = await teacherService.findById(statementEditDTO.id_of_curator);
                if (teacher == null) return new JsonAnswerStatus("error", "not_found_teacher");
            }

            if(await statementService.setCurator(statement, teacher))return new JsonAnswerStatus("success", null);

            return new JsonAnswerStatus("error", "unknown");
        }

        public async Task<JsonAnswerStatus> search(StatementSearchDTO statementSearchDTO)
        {
            StatementService statementService = new StatementService(_dbc);
            List<Statement> statementsSearchResult = await statementService.search(statementSearchDTO);
            int searchCount = statementService.searchCount(statementSearchDTO);

            RegionFacade regionFacade = new RegionFacade(_dbc);
            //List<Region> regions = await regionService.listAll();
            Region region = null;
            string list_of_wanted_teachers = null;

            List<StatementPreviewViewModel> statementSearchPreviewViewModels = new List<StatementPreviewViewModel>();
            foreach (Statement statement in statementsSearchResult)
            {
                region = await regionFacade.getRegionByUser(statement.user);
                list_of_wanted_teachers = await getListOfTeachersOfStatement(statement.listOfTeachers);
                statementSearchPreviewViewModels.Add(new StatementPreviewViewModel(
                    statement.id,
                    statement.user.Id,
                    statement.status,
                    statement.user.Username,
                    statement.user.secondname,
                    statement.user.firstname,
                    (region != null ? region.name : null),
                    list_of_wanted_teachers,
                    (statement.date_of_add != null ? statement.date_of_add.Value.ToString("HH:mm:ss dd.MM.yyyy") : null)
                    )
                );
            }

            return new JsonAnswerStatus("success", null, new StatementsSearchViewModel(statementSearchPreviewViewModels, searchCount));
        }

        private async Task<string> getListOfTeachersOfStatement(string listOfIdOfTeachers)
        {
            TeacherService teacherService = new TeacherService(_dbc);
            List<Teacher> teachers = await teacherService.listAll();
            listOfIdOfTeachers = listOfIdOfTeachers.Replace(@" ",@"");
            string[] arrayOfIdOfTeachers = listOfIdOfTeachers.Split(",");

            StringBuilder strAnswer = new StringBuilder("");

            int id_of_teacher = 0;
            foreach (string idAsStr in arrayOfIdOfTeachers)
            {
                try
                {
                    id_of_teacher = int.Parse(idAsStr);
                    foreach (Teacher teacher in teachers)
                    {
                        if (teacher.id == id_of_teacher)
                        {
                            if (!strAnswer.Equals("")) strAnswer.Append(", ");
                            strAnswer.Append(teacher.name);
                        }
                    }
                } catch
                {
                    continue;
                }
            }

            return strAnswer.ToString();
        }
    }
}
