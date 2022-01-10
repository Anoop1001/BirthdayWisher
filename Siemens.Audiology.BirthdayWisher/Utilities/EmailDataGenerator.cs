using Microsoft.Extensions.Hosting;
using Siemens.Audiology.BirthdayWisher.Data.Models;
using Siemens.Audiology.Notification;
using System.Collections.Generic;
using System.IO;

namespace Siemens.Audiology.BirthdayWisher.Utilities
{
    public class EmailDataGenerator : IEmailDataGenerator
    {
        private readonly IHostEnvironment _hostingEnvironment;
        public EmailDataGenerator(IHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IEnumerable<EmailData> GetEmailDataList(List<BirthdayInformation> birthdays)
        {
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, $"StaticFile/index.html");
            var emailTemplate = File.ReadAllText(filePath);
            foreach (var birthday in birthdays)
            {
                yield return new EmailData
                {
                    Body = emailTemplate.Replace("{{name}}", birthday.Name),
                    To = new List<string> { birthday.Email },
                    IsBodyHtml = true,
                    Subject = $"Happy Birthday ${birthday.Name}"
                };
            }
        }
    }
}
