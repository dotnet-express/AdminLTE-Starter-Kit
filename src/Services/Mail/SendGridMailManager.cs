using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.WebApplication1.Services.Mail
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class SendGridMailManager : IMailManager
    {
        public SendGridMailManager(IConfiguration configuration, IOptions<MailManagerOptions> optionsManager, IOptions<SendGridAuthOptions> optionsSendGrid)
        {
            Configuration = configuration;
            ManagerOptions = optionsManager.Value;
            SendGridOptions = optionsSendGrid.Value;
        }

        public IConfiguration Configuration { get; }
        public MailManagerOptions ManagerOptions { get; }
        public SendGridAuthOptions SendGridOptions { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(SendGridOptions.ApiKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);

            var emailAddress = ManagerOptions.SupportTeamEmail ?? "support@dotnet.express";
            var emailName = ManagerOptions.SupportTeamName ?? ".NET Express - Support Team";

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(emailAddress, emailName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
    }
}
