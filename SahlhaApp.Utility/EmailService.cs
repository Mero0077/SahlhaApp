using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SahlhaApp.Utility
{
    public class EmailService : IEmailSender
    {
        private readonly string _emailFrom = "mohamedmagdy.182003@gmail.com";
        private readonly string _emailPassword = "szbs eoff yfwg pghb";

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailFrom, _emailPassword)
            };

            return client.SendMailAsync(
                new MailMessage(from: _emailFrom, to: email, subject, message)
                {
                    IsBodyHtml = true
                });
        }
    }
}
