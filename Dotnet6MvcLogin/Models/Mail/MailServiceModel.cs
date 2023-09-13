namespace Dotnet6MvcLogin.Models.Mail
{
    public class MailServiceModel
    {
        public string MailFor { get; set; }
        public string RecipientName { get; set; }
        public string Content { get; set; }

        public string MailTo { get; set; }
        public string MailCc { get; set; }
        public string MailBcc { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public Dictionary<string, byte[]> MailAttachements { get; set; } = new Dictionary<string, byte[]>();
    }

    public class MailMessageSettingsModel
    {
        public string MailFor { get; set; }
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public List<IFormFile> MailAttachement { get; set; }
    }

    public class MailRequestModel
    {
        public string MailFor { get; set; }
        public string MailTo { get; set; }
        public string MailSubject { get; set; }
        public string RecipientName { get; set; }
        public string Content { get; set; }
        public Dictionary<string, byte[]> Attachments { get; set; } = new Dictionary<string, byte[]>();
    }

    public class MailConfiguration
    {
        public string Server { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string MailFrom { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Code { get; set; }
    }
}
