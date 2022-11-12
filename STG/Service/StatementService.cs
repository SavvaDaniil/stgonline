using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO.Statement;
using STG.Entities;
using STG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class StatementService
    {
        public ApplicationDbContext _dbc;
        public StatementService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public Statement findById(int id)
        {
            return _dbc.Statements
                .Include(p => p.user)
                .Include(p => p.teacher)
                .Where(p => p.id == id)
                .OrderByDescending(p => p.id).FirstOrDefault();
        }

        public Statement add(User user, PreUserWithAppointment preUserWithAppointment, int status = 0)
        {
            Statement statement = new Statement();
            statement.user = user;
            statement.is_need_curator = preUserWithAppointment.is_need_curator;
            statement.experience = preUserWithAppointment.experience;
            statement.expectations = preUserWithAppointment.expectations;
            statement.expected_time_for_lessons = preUserWithAppointment.expected_time_for_lessons;
            statement.idols = preUserWithAppointment.idols;
            statement.listOfTeachers = preUserWithAppointment.listOfTeachers;
            statement.link1 = preUserWithAppointment.link1;
            statement.link2 = preUserWithAppointment.link2;
            statement.link3 = preUserWithAppointment.link3;
            statement.is_payed = 1;
            statement.status = status;

            statement.date_of_add = DateTime.Now;
            statement.date_of_payed = DateTime.Now;

            _dbc.Statements.Add(statement);
            _dbc.SaveChanges();

            return statement;
        }

        public Statement add(User user, StatementNewDTO statementNewDTO)
        {
            Statement statement = new Statement();
            statement.user = user;
            statement.is_need_curator = statementNewDTO.is_need_curator;
            statement.experience = statementNewDTO.experience;
            statement.expectations = statementNewDTO.expectations;
            statement.expected_time_for_lessons = statementNewDTO.expected_time_for_lessons;
            statement.idols = statementNewDTO.idols;
            statement.listOfTeachers = statementNewDTO.curators;
            statement.link1 = statementNewDTO.link1;
            statement.link2 = statementNewDTO.link2;
            statement.link3 = statementNewDTO.link3;
            statement.is_payed = 0;
            statement.status = 0;

            statement.date_of_add = DateTime.Now;
            //statement.date_of_payed = DateTime.Now;

            _dbc.Statements.Add(statement);
            _dbc.SaveChanges();

            return statement;
        }

        public Statement updateAfterSuccessfullPayment(Statement statement)
        {
            //statement.status = 1;
            statement.is_payed = 1;
            statement.date_of_payed = DateTime.Now;
            _dbc.SaveChanges();
            return statement;
        }

        public bool setCurator(Statement statement, Teacher teacher)
        {
            if(teacher == null)
            {
                statement.status = 0;
                statement.teacher = null;
                statement.date_of_active = null;
            } else
            {
                statement.status = 1;
                statement.teacher = teacher;
                statement.date_of_active = DateTime.Now;
            }

            _dbc.SaveChanges();
            return true;
        }




        public List<Statement> search(StatementSearchDTO statementSearchDTO)
        {
            statementSearchDTO.page--;
            IQueryable<Statement> q = _dbc.Statements
                .Include(p => p.teacher)
                .Include(p => p.user)
                .Where(p => p.is_payed == 1)
                .OrderByDescending(p => p.id);

            q = q.Take(30).Skip(statementSearchDTO.page * 30);

            if (!string.IsNullOrEmpty(statementSearchDTO.queryString))
            {
                q = q.Where(p =>
                    EF.Functions.Like(p.idols, statementSearchDTO.queryString)
                    || p.idols.Contains(statementSearchDTO.queryString)
                );
            }
            return q.ToList();
        }

        public int getCountAllInactived()
        {
            return _dbc.Statements
                .Where(p => p.status == 0 && p.is_payed == 1)
                .Count();
        }

        public int searchCount(StatementSearchDTO statementSearchDTO)
        {
            IQueryable<Statement> q = _dbc.Statements.OrderByDescending(p => p.id);

            if (!string.IsNullOrEmpty(statementSearchDTO.queryString))
            {
                q = q.Where(p =>
                    EF.Functions.Like(p.idols, statementSearchDTO.queryString)
                    || p.idols.Contains(statementSearchDTO.queryString)
                );
            }
            return q.Count();
        }

    }
}
