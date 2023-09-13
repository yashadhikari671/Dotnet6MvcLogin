using Dotnet6MvcLogin.Models.Mail;
using Dotnet6MvcLogin.Services.MailingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet6MvcLogin.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private readonly IMailingService _mailingService;

        public AdminController(IMailingService mailingService)
        {
            _mailingService = mailingService;
        }
        public IActionResult Display()
        {
            return View();
        }

        public async Task<IActionResult> SendMail()
        {

            var mailRequest = new MailRequestModel
            {
                MailFor = "invoice",
                MailTo = "yash.adhikari671@gmail.com",
                MailSubject = "This is test from yash",
                RecipientName = "yash Adhikari",
                Content = "otp is 123456"

            };
            var mailServiceModel = await _mailingService.EmailSettings(mailRequest);

            //Thread email = new(delegate ()
            //{
            //    _mailService.SendMail(mailServiceModel);
            //});
            var sendmail = await _mailingService.SendMail(mailServiceModel);
            return View();
        }
    }
}
