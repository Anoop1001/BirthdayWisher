using Siemens.Audiology.BirthdayWisher.Data.Enums;
using Siemens.Audiology.BirthdayWisher.Data.Models;
using Siemens.Audiology.Notification;
using System.Collections.Generic;

namespace Siemens.Audiology.BirthdayWisher.Utilities
{
    public interface IEmailDataGenerator
    {
        IEnumerable<EmailData> GetEmailDataList(List<BirthdayInformation> birthdays);
        byte[] GetTemplateData(Gender gender, string name);
    }
}