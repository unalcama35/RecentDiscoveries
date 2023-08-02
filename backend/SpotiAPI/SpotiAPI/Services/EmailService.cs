using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SpotiAPI.Services
{
    public class EmailService
    {
        private readonly string emailAddress = "";
        private readonly string emailPw = "";

        public async Task SendEmail(string to, string subject, string body )
        {
            try
            {
                using (var message = new MailMessage(emailAddress, to))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = false;

                    using (var client = new SmtpClient("smtp.gmail.com", 587))
                    {
                        client.EnableSsl = true;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(emailAddress, emailPw);

                        await client.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending email: " + ex.Message);
            }
        }
    }
}
