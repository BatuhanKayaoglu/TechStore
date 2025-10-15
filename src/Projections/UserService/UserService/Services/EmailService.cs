using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Services
{
    public class EmailService
    {
        private readonly IConfiguration configuration;  

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;     
        }
        public string GenerateConfirmationLink(Guid confirmationId)
        {
            var baseUrl = configuration["BaseUrl"];
            var url = $"{baseUrl}{confirmationId}";      
            return url; 
        }     
        public Task SendEmailAsync(string email, string subject, string message)
        {
            string? mail = configuration["Email:From"];
            var pw = configuration["Email:Password"];

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(mail, pw),
                EnableSsl = true
            };

            return client.SendMailAsync(
                               new MailMessage(mail!, email)
                               {
                                   Subject = subject,
                                   Body = message
                               });
        }
    }
} 
