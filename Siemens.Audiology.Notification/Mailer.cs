using AutoMapper;
using Siemens.Audiology.Notification.Contract;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Siemens.Audiology.Notification
{
    public class Mailer : IMailer
    {
        private readonly IMapper _mapper;
        public Mailer(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task SendEmailAsync(EmailData emailData)
        {
            using (SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com", //or another email sender provider
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("anoophn10@gmail.com", "losetowin"),
                Timeout = 90000
            })
            {
                var mailMessage = _mapper.Map<MailMessage>(emailData);
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
