using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly SendGridConfiguration _config;

        public EmailSender(IOptions<SendGridConfiguration> config)
        {
            _config = config.Value;
        }
        public async Task<bool> Send(string to, string templateId, object templateData)
        {

            var client = new SendGridClient(_config.ApiKey);
            var fromEmail = new EmailAddress(_config.From, _config.FromName);
            var toEmail = new EmailAddress(to, to);


            var msg = MailHelper.CreateSingleTemplateEmail(fromEmail, toEmail, templateId, templateData);

            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

    }
}
