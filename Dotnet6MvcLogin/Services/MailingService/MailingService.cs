using Dotnet6MvcLogin.Models.Mail;
using NuGet.Protocol.Plugins;
using System.Net;
using System.Net.Mail;

namespace Dotnet6MvcLogin.Services.MailingService
{
    public class MailingService : IMailingService
    {
        private static string sMailHost = "";
        private static int iMailPort = 0;
        private static string sMailUserName = "";
        private static string sMailPassword = "";
        private static string sMailFrom = "";

        


        public MailingService()
        {
           

            SetMailConfiguration();
        }
        public async Task<MailServiceModel> EmailSettings(MailRequestModel mailRequest)
        {
            var mailSettings = new MailServiceModel();
            string htmlBody = "";

            //var mailsetting = await _mailRepository.MailMessageSettings();
            //var mailMessageSettings = mailsetting.FirstOrDefault(m => (m.MailFor ?? "").ToLower().Trim() == (mailRequest.MailFor ?? "").ToLower().Trim());

            //mailSettings.MailTo = mailMessageSettings == null ? (mailRequest.MailTo ?? "").Trim() : (mailRequest.MailTo ?? "").Trim() + "," + (mailMessageSettings.MailTo ?? "").Trim();
            //mailSettings.MailCc = mailMessageSettings == null ? "" : (mailMessageSettings.MailCC ?? "").Trim();
            //mailSettings.MailBcc = mailMessageSettings == null ? "" : (mailMessageSettings.MailBCC ?? "").Trim();
            //mailSettings.MailSubject = mailMessageSettings == null ? (mailRequest.MailSubject ?? "").Trim() : (mailMessageSettings.MailSubject ?? "").Trim();

            mailSettings.MailTo = "yash.adhikari671@gmail.com";
            mailSettings.MailSubject = "Testing for something";

            ///// For: reset-password
            if (mailRequest.MailFor.ToLower() == "reset-password")
            {
                htmlBody += $"Dear {mailRequest.RecipientName}, ";
                htmlBody += $"<p><span style='text-color:purple'>{mailRequest.Content}<span></p>";
                htmlBody += "<br><p>Thank you,<br>Mypay Money Transfer</P>";
            }

            ////// For: confirm-email
            if (mailRequest.MailFor.ToLower() == "confirm-email")
            {
                htmlBody += $"Dear {mailRequest.RecipientName}, ";
                htmlBody += $"<p><span style='text-color:purple'>{mailRequest.Content}<span></p>";
                htmlBody += "<br><p>Thank you,<br>Mypay Money Transfer</P>";
            }



            if (mailRequest.Attachments is not null && mailRequest.Attachments.Count > 0)
            {
                mailSettings.MailAttachements = mailRequest.Attachments;
            }
            mailSettings.MailBody = htmlBody;
            return mailSettings;
        }

        public async Task<bool> SendMail(MailServiceModel mailModel)
        {
            string sMailTo = mailModel.MailTo;
            string cc = mailModel.MailCc;
            string bcc = mailModel.MailBcc;
            string strMailSubject = mailModel.MailSubject;
            string strMailBody = mailModel.MailBody;

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(sMailFrom);

                if (!string.IsNullOrEmpty(sMailTo))
                {
                    string[] mailToId = sMailTo.Split(',');
                    foreach (string ToEmail in mailToId)
                    {
                        if (!string.IsNullOrEmpty(ToEmail))
                            mail.To.Add(ToEmail);
                    }
                }

                if (!string.IsNullOrEmpty(cc))
                {
                    string[] CCId = cc.Split(',');
                    foreach (string CCEmail in CCId)
                    {
                        if (!string.IsNullOrEmpty(CCEmail))
                            mail.CC.Add(new MailAddress(CCEmail));
                    }
                }
                if (!string.IsNullOrEmpty(bcc))
                {
                    string[] BCCId = bcc.Split(',');
                    foreach (string BCCEmail in BCCId)
                    {
                        if (!string.IsNullOrEmpty(BCCEmail))
                            mail.Bcc.Add(new MailAddress(BCCEmail));
                    }
                }
                if (mailModel.MailAttachements is not null && mailModel.MailAttachements.Any())
                {

                    foreach (var entry in mailModel.MailAttachements)
                        mail.Attachments.Add(new Attachment(new MemoryStream(entry.Value), entry.Key));


                }


                mail.Subject = strMailSubject;
                mail.Body = strMailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = sMailHost;
                smtp.Port = iMailPort;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("yash-7@outlook.com", sMailPassword); // Enter senders User name and password       
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async void SetMailConfiguration()
        {
            //var mailConfig = await _mailRepository.GetMailConfiguration();
            //check if mailconfig is null 


            sMailHost = "smtp.office365.com";
            iMailPort = 587;
            sMailFrom = "yash-7@outlook.com";
            sMailUserName = "salam";
            sMailPassword = "123456789@yash";
        }



        ////tosendmail

        //var mailRequest = new MailRequestModel
        //{
        //    MailFor = "invoice",
        //    MailTo = "yash.adhikari671@gmail.com",
        //    MailSubject = "This is test from yash",
        //    RecipientName = "yash Adhikari",
        //    Content = "<h1>Hello world</h1>"

        //};
        //var mailServiceModel = await _mailService.EmailSettings(mailRequest);

        ////Thread email = new(delegate ()
        ////{
        ////    _mailService.SendMail(mailServiceModel);
        ////});
        //var sendmail = await _mailService.SendMail(mailServiceModel);
    }
}
