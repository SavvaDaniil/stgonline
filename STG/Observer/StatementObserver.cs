using STG.Factory;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace STG.Observer
{
    public class StatementObserver
    {
        private const string mailDirector = "stgonline.pro@yandex.ru";
        private const string mailLogin = "info@stgonline.pro";
        private const string mailPassword = "F@q9fu13";


        public void sendStatementToAdmins(User user, Statement statement, List<Teacher> curators, bool isTest = false)
        {
            StatementMailFactory statementMailFactory = new StatementMailFactory();
            string messageHTML = statementMailFactory.create(user, statement, curators);

            MainObserver mainObserver = new MainObserver();
            mainObserver.sendMailToDirector("STG Online - Новая заявка на кураторство", messageHTML, isTest);

            foreach (Teacher curator in curators)
            {
                if (curator.email != null) mainObserver.sendMailToUser(curator.email, "STG Online - Новая заявка на кураторство", messageHTML);
            }
        }



    }
}
