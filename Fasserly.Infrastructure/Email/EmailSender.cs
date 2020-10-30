using Fasserly.Infrastructure.Interface;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Email
{
    public class EmailSender : IEMailSender
    {
        private readonly IOptions<SendGridSettings> _options;

        public EmailSender(IOptions<SendGridSettings> options)
        {
            _options = options;
        }

        public async Task SendEmailAsync(string userMail, string subject, string message)
        {
            var client = new SendGridClient(_options.Value.Key);
            var msg = new SendGridMessage
            {
                From = new EmailAddress("fasserly@outlook.com", _options.Value.User),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(userMail));
            msg.SetClickTracking(false, false);
            await client.SendEmailAsync(msg);
        }
    }
}
