using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Observer
{
    public class UserForgetObserver
    {
        public void sendCodeToUser(string username, string code)
        {
            string messageHtml = "<p>Код подтверждения: <b>"+code+"</b></p>";
            MainObserver mainObserver = new MainObserver();
            mainObserver.sendMailToUser(
                username,
                "STG - Восстановление пароля",
                messageHtml
            );
        }


        public void sendNewPasswodToUser(string username, string newPassword)
        {
            string messageHtml = "<p>Ваш новый пароль: <b>" + newPassword + "</b></p>";
            MainObserver mainObserver = new MainObserver();
            mainObserver.sendMailToUser(
                username,
                "STG - Новый автоматически сгенерированный пароль",
                messageHtml
            );
        }
    }
}
