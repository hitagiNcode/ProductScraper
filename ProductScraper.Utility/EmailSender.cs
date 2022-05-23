using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ProductScraper.Utility
{
    public class EmailSender : IEmailSender
    {
        public string SendGridSecret = Environment.GetEnvironmentVariable("SENDGRIDSECRET") ?? "";

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var client = new SendGridClient(SendGridSecret);
            var from = new EmailAddress("fuchangzai@gmail.com", "PuniPuni TDS");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            return client.SendEmailAsync(msg);

        }
    }
}