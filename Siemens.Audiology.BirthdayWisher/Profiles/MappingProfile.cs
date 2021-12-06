using AutoMapper;
using Siemens.Audiology.Notification;
using System.Net.Mail;

namespace Siemens.Audiology.BirthdayWisher.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmailData, MailMessage>().ConstructUsing(x => new MailMessage(x.From, string.Join(",", x.To)))
                .ForMember(x => x.From, y => y.MapFrom(m => new MailAddress(m.From))).IgnoreAllPropertiesWithAnInaccessibleSetter();
                //.ForMember(x => x.To, y => y.MapFrom(m => new MailAddress(string.Join(",", m.To))))
                //.ForMember(x => x.CC, y => y.MapFrom(m => new MailAddress(string.Join(",", m.Cc))))
                //.ForMember(x => x.Bcc, y => y.MapFrom(m => new MailAddress(string.Join(",", m.Bcc))));
        }
    }
}
