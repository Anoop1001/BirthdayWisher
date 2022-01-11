using Microsoft.Extensions.Hosting;
using Siemens.Audiology.BirthdayWisher.Data.Models;
using Siemens.Audiology.Notification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            string emailTemplate = GetEmailTemplate();
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

        public byte[] GetTemplateData()
        {
            var template = GetEmailTemplate();
            var utf8 = new UTF8Encoding();
            return utf8.GetBytes(template);
        }

        private string GetEmailTemplate()
        {
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, $"StaticFile/index.html");
            var emailTemplate = File.ReadAllText(filePath);
            byte[] imageArray = File.ReadAllBytes(@"StaticFile/Birthday.jpg");
            string base64ImageRepresentation = $"data:image/jpg;base64, { Convert.ToBase64String(imageArray)}";
            emailTemplate = emailTemplate.Replace("{{image}}", base64ImageRepresentation);
            return emailTemplate;
        }
    }
}
