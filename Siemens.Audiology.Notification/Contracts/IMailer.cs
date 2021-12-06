using System.Threading.Tasks;

namespace Siemens.Audiology.Notification.Contract
{
    public interface IMailer
    {
        Task SendEmailAsync(EmailData emailData);
    }
}
