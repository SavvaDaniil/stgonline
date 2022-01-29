using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace STG.Observer
{
    public class MainObserver
    {
        private const string mailDirector = "stgonline.pro@yandex.ru";
        private const string mailDeveloper = "savva.d@mail.ru";
        private const string mailLogin = "info@stgonline.pro";
        private const string mailPassword = "F@q9fu13";

        public void sendMailToDirector(string title, string messageHTML, bool isTest = false)
        {
            //var tMail = new Thread(() => threadSend(mailDirector, title, messageHTML));
            //tMail.Start();
            if (!isTest)
            {
                var tMail = new Thread(() => threadSend(mailDirector, title, messageHTML));
                tMail.Start();
            }

            var tMailDeveloper = new Thread(() => threadSend(mailDeveloper, title, messageHTML));
            tMailDeveloper.Start();
        }

        public void sendMailToDirectorWithAttechment(string title, string messageHTML, string filepath, string filename)
        {
            var tMail = new Thread(async () => {
                await threadSendWithVideoFile(
                    mailDirector,
                    title,
                    messageHTML,
                    filepath,
                    filename
                );
            }
            );
            tMail.Start();

            var tMailDeveloper = new Thread(async () => {
                    await threadSendWithVideoFile(
                        mailDeveloper,
                        title,
                        messageHTML,
                        filepath,
                        filename
                    );
                }
            );
            tMailDeveloper.Start();
        }

        public void sendCheck(string messageHTML)
        {
            var tMailDeveloper = new Thread(() => threadSend(mailDeveloper, "STG Online - Тестовое письмо", messageHTML));
            tMailDeveloper.Start();
        }

        public void sendMailToUser(string username, string title, string messageHTML)
        {
            var tMailDeveloper = new Thread(() => threadSend(username, title, messageHTML));
            tMailDeveloper.Start();
        }


        private void threadSend(
            string mail_to,
            string subject,
            string messageHtml
        )
        {
            System.Diagnostics.Debug.WriteLine("threadSend to: " + mail_to);
            try
            {
                MailAddress from = new MailAddress(mailLogin, "DoNotReply");
                MailAddress to = new MailAddress(mail_to);
                MailMessage m = new MailMessage(from, to);
                m.Subject = subject;
                m.Body = messageHtml;
                m.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("wpl42.hosting.reg.ru", 587);

                smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential(mailLogin, mailPassword);
                smtp.Credentials = new NetworkCredential("info@stgonline.pro", "F@q9fu13");
                smtp.EnableSsl = true;

                smtp.Send(m);
                smtp.Dispose();

                System.Diagnostics.Debug.WriteLine("Сообщение отправлено");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception caught in threadSend(): {0}", ex.ToString());
            }
        }




        public async Task threadSendWithVideoFile(
            string mail_to,
            string subject,
            string messageHtml,
            string filepath,
            string filename
        )
        {

            if (!File.Exists(filepath)) return;

            try
            {
                MailAddress from = new MailAddress(mailLogin, "DoNotReply");
                MailAddress to = new MailAddress(mail_to);
                MailMessage m = new MailMessage(from, to);
                m.Subject = subject;
                m.Body = messageHtml;
                m.IsBodyHtml = true;

                byte[] fileBytes = await File.ReadAllBytesAsync(filepath);

                MemoryStream ms = new MemoryStream(fileBytes);
                m.Attachments.Add(new Attachment(ms, filename, "video/" + filename.Substring(filename.LastIndexOf(".")).ToLower()));

                SmtpClient smtp = new SmtpClient("wpl42.hosting.reg.ru", 587);

                smtp.Credentials = new NetworkCredential(mailLogin, mailPassword);
                smtp.EnableSsl = true;

                smtp.Send(m);
                smtp.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка отправки письма: {0}",
                    ex.ToString());
            }
        }

    }
}
