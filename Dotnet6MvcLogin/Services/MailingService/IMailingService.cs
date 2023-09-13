using Dotnet6MvcLogin.Models.Mail;

namespace Dotnet6MvcLogin.Services.MailingService
{
    public interface IMailingService
    {
        Task<bool> SendMail(MailServiceModel mailModel);
        void SetMailConfiguration();
        Task<MailServiceModel> EmailSettings(MailRequestModel mailRequest);
    }
}
