namespace Siemens.Audiology.Notification.Models
{
    public class SmtpConfigutationDetails
    {
        public string CredentialEmail { get; set; }
        public string CredentialPassword { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
    }
}
