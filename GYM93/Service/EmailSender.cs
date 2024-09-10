
using NuGet.Protocol.Plugins;
using System.Net.Mail;
using System.Net;
using GYM93.Service.IService;

namespace GYM93.Service
{
    public class EmailSender : IEmailSender
    {   
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmail(string email, string subject, string htmlMessage)
        {
            var mail = _configuration["EmailSender"];
            var pw = _configuration["EmailPassword"];

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, pw)
            };
            return client.SendMailAsync(
                new MailMessage(from: mail,
                                to: email,
                                subject,
                                htmlMessage ){
                    IsBodyHtml = true
                });
        }

    }
}
