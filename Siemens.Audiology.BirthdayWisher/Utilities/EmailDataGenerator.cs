using Microsoft.Extensions.Hosting;
using Siemens.Audiology.BirthdayWisher.Data.Enums;
using Siemens.Audiology.BirthdayWisher.Data.Models;
using Siemens.Audiology.BirthdayWisher.Models;
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
        private static Dictionary<Gender, ImageTemplate> genderBasedImages;

        public EmailDataGenerator(IHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            InitializeImageDictionary();
        }



        public IEnumerable<EmailData> GetEmailDataList(List<BirthdayInformation> birthdays)
        {
            string emailTemplate = GetEmailTemplate();
            foreach (var birthday in birthdays)
            {
                yield return new EmailData
                {
                    Body = EmbedImagesBasedOnGender(birthday.Gender, emailTemplate.Replace("{{name}}", birthday.Name)),
                    To = new List<string> { birthday.Email },
                    IsBodyHtml = true,
                    Subject = $"Happy Birthday ${birthday.Name}"
                };
            }
        }

        public byte[] GetTemplateData(Gender gender, string name)
        {
            var template = GetEmailTemplate().Replace("{{name}}", name);
            template = EmbedImagesBasedOnGender(gender, template);
            var utf8 = new UTF8Encoding();
            return utf8.GetBytes(template);
        }

        private string GetEmailTemplate()
        {
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, $"StaticFile/template.html");
            var emailTemplate = File.ReadAllText(filePath);
            emailTemplate = EmbedFontsInTemplate(emailTemplate, "comforta-bold-font");
            emailTemplate = EmbedFontsInTemplate(emailTemplate, "comforta-medium-font");
            emailTemplate = EmbedFontsInTemplate(emailTemplate, "comforta-regular-font");

            return emailTemplate;
        }

        private static string EmbedFontsInTemplate(string emailTemplate, string fontFamily)
        {
            byte[] fontArray = File.ReadAllBytes(@$"StaticFile/{fontFamily}.ttf");
            string base64Font = $"data:font/truetype;charset=utf-8;base64, {Convert.ToBase64String(fontArray)}";
            emailTemplate = emailTemplate.Replace("{{" + fontFamily + "}}", base64Font);
            return emailTemplate;
        }

        private static string EmbedImagesBasedOnGender(Gender gender, string emailTemplate)
        {
            var imageTemplate = genderBasedImages.ContainsKey(gender) ? genderBasedImages[gender] : genderBasedImages[Gender.Female];
            emailTemplate = emailTemplate.Replace("{{hat-image}}", imageTemplate.HatImage);
            emailTemplate = emailTemplate.Replace("{{background-image}}", imageTemplate.BackgroundImage);
            return emailTemplate;
        }

        private static void InitializeImageDictionary()
        {
            byte[] bgBoyArray = File.ReadAllBytes(@"StaticFile/bg-boy.png");
            string base64BgBoy = $"data:image/jpg;base64, { Convert.ToBase64String(bgBoyArray)}";

            byte[] bgGirlArray = File.ReadAllBytes(@"StaticFile/bg-girl.png");
            string base64BgGirl = $"data:image/jpg;base64, { Convert.ToBase64String(bgGirlArray)}";

            byte[] femaleHatArray = File.ReadAllBytes(@"StaticFile/female-hat.png");
            string base64FemaleHat = $"data:image/jpg;base64, { Convert.ToBase64String(femaleHatArray)}";

            byte[] maleHatArray = File.ReadAllBytes(@"StaticFile/male-hat.png");
            string base64maleHat = $"data:image/jpg;base64, { Convert.ToBase64String(maleHatArray)}";

            genderBasedImages = new Dictionary<Gender, ImageTemplate>()
            {
                {Gender.Male, new  ImageTemplate{ BackgroundImage = base64BgBoy, HatImage = base64maleHat} },
                {Gender.Female, new  ImageTemplate{ BackgroundImage = base64BgGirl, HatImage = base64FemaleHat} },
            };
        }
    }
}
