using AutoMapper;
using Siemens.Audiology.Notification;
using System.Net.Mail;

namespace Siemens.Audiology.BirthdayWisher.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmailData, MailMessage>().ConstructUsing(x => new MailMessage(x.SmtpConfigutationDetails.CredentialEmail, string.Join(",", x.To)))
                .ForMember(x => x.From, y => y.MapFrom(m => new MailAddress(m.SmtpConfigutationDetails.CredentialEmail))).IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
