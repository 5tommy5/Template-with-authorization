
namespace EmailService
{
    public interface IEmailSender
    {
        Task<bool> Send(string to, string templateId, object templateData);
    }
}
