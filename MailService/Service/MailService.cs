using MailService.Interface;
using System.Net;
using System.Net.Mail;

namespace MailService.Service
{
    public class MailService : IMailService
    {
        public async Task<bool> sendMail(string htmlString)
        {
            try
            {
                await Task.Delay(0);
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("FromMailAddress");
                message.To.Add(new MailAddress("ToMailAddress"));
                message.Subject = "Test";
                message.IsBodyHtml = true; //to make message body as html
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("FromMailAddress", "password");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
            return true;
        }
    }
}
