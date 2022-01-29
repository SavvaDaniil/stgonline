using STG.Data;
using STG.DTO;
using STG.DTO.UserDTO;
using STG.Entities;
using STG.Observer;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class PreUserWithAppointmentFacade
    {
        private ApplicationDbContext _dbc;
        public PreUserWithAppointmentFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<JsonAnswerStatus> registration(string domainHost, UserNewDTO userNewDTO)
        {
            UserService userService = new UserService(_dbc);

            if (userService.isAlreadyExistByUsername(userNewDTO.username))
            {
                return new JsonAnswerStatus("error", "username_already_exist");
            }

            PreUserWithAppointmentService preUserWithAppointmentService = new PreUserWithAppointmentService(_dbc);

            /*
            List<Teacher> teacherCurators = new List<Teacher>();
            if (userNewDTO.list_id_of_curators != null)
            {
                List<int> list_id_of_curators = new List<int>();
                foreach (string id_of_teacher in userNewDTO.curators.Split(","))
                {
                    try
                    {
                        list_id_of_curators.Add(int.Parse(id_of_teacher));
                    }
                    catch { }
                }

                TeacherService teacherService = new TeacherService(_dbc);
                Teacher findedTeacher;
                foreach (int id_of_teacher in list_id_of_curators)
                {
                    findedTeacher = await teacherService.findById(id_of_teacher);
                    if (findedTeacher != null) teacherCurators.Add(findedTeacher);
                }

            }
            */

            RegionService regionService = new RegionService(_dbc);
            Region region = await regionService.findById(userNewDTO.id_of_region);

            string password = BCrypt.Net.BCrypt.HashPassword(userNewDTO.password);

            PreUserWithAppointment preUserWithAppointment = await preUserWithAppointmentService.add(
                userNewDTO.username,
                userNewDTO.firstname,
                userNewDTO.secondname,
                userNewDTO.instagram,
                userNewDTO.date_of_birthday,
                password,
                userNewDTO.is_need_curator,
                region,
                userNewDTO.experience,
                userNewDTO.expectations,
                userNewDTO.expected_time_for_lessons,
                userNewDTO.idols,
                userNewDTO.link1,
                userNewDTO.link2,
                userNewDTO.link3,
                userNewDTO.curators
            );
            if (preUserWithAppointment == null) return null;

            PaymentFacade paymentFacade = new PaymentFacade(_dbc);
            PaymentLiteViewModel paymentLiteViewModel = await paymentFacade.generateForPreUserAppointment(domainHost, preUserWithAppointment);
            if (paymentLiteViewModel == null) return new JsonAnswerStatus("error", "generate_payment_error");

            return new JsonAnswerStatus(
                "success",
                null,
                paymentLiteViewModel
            );
        }


        public async Task<User> registrationAfterSuccessfullPayment(Payment payment)
        {
            PreUserWithAppointmentService preUserWithAppointmentService = new PreUserWithAppointmentService(_dbc);
            UserFacade userFacade = new UserFacade(_dbc);

            PreUserWithAppointment preUserWithAppointment = await preUserWithAppointmentService.findById(payment.preUserWithAppointment.id);
            if (preUserWithAppointment == null) return null;

            //создаем отработанную заявку в базе и пользователя
            User user = await userFacade.registrationAfterPayedStatement(payment.preUserWithAppointment);
            if (user == null) return null;
            //обновляем user у платежа

            StatementFacade statementFacade = new StatementFacade(_dbc);
            Statement statement = await statementFacade.addAfterPreUserWithAppointment(user, payment.preUserWithAppointment);
            if (statement == null) return null;

            if (!await preUserWithAppointmentService.setActive(payment.preUserWithAppointment)) return null;


            AmoCRMFacade amoCRMFacade = new AmoCRMFacade(_dbc);
            await amoCRMFacade.newLeadNewUser(user);
            UserService userService = new UserService(_dbc);
            user = await userService.findById(user.Id);
            await amoCRMFacade.newLeadNewStatement(user, payment, statement);

            StatementObserver statementObserver = new StatementObserver();
            TeacherService teacherService = new TeacherService(_dbc);
            List<Teacher> curators = await teacherService.listAllByListString(statement.listOfTeachers);


            statementObserver.sendStatementToAdmins(user, statement, curators, (payment.isTest == 1 ? true : false));

            return user;
        }
    }
}
