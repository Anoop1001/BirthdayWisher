using AutoMapper;
using Microsoft.Extensions.Options;
using Siemens.Audiology.Notification.Contract;
using Siemens.Audiology.Notification.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Siemens.Audiology.Notification
{
    public class Mailer : IMailer
    {
        private readonly IMapper _mapper;
        private readonly SmtpConfigutationDetails _smtpConfigutationDetails;
        public Mailer(IMapper mapper, IOptions<SmtpConfigutationDetails> options)
        {
            _mapper = mapper;
            _smtpConfigutationDetails = options.Value;
        }

        public async Task SendEmailAsync(EmailData emailData)
        {
            using (SmtpClient client = new SmtpClient
            {
                Port = _smtpConfigutationDetails.Port,
                Host = _smtpConfigutationDetails.Host,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpConfigutationDetails.CredentialEmail, _smtpConfigutationDetails.CredentialPassword),
                Timeout = 90000
            })
            {
                emailData.SmtpConfigutationDetails = _smtpConfigutationDetails;
                var mailMessage = _mapper.Map<MailMessage>(emailData);
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
