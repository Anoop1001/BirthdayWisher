using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Siemens.Audiology.BirthdayWisher.Business.Contract;
using Siemens.Audiology.BirthdayWisher.Data.Models;

namespace Siemens.Audiology.BirthdayWisher.Business
{
    public class ExcelHandler : IExcelHandler
    {
        List<BirthdayInformation> _birthdayInformationList;

        public List<BirthdayInformation> ReadData(IFormFile value)
        {
            _birthdayInformationList = new List<BirthdayInformation>();
            string path = Path.Combine(Environment.CurrentDirectory, @"..\", "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, "Information.xlsx");
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                value.CopyTo(stream);
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var rowCount = reader.RowCount;
                    for (int i = 0; reader.Read(); i++) //Each row of the file
                    {
                        if (i == 0)
                            continue;

                        _birthdayInformationList.Add(new BirthdayInformation
                        {
                            Name = reader.GetValue(0).ToString(),
                            GId = reader.GetValue(1).ToString(),
                            BirthDate = Convert.ToDateTime(reader.GetValue(2))
                        });
                    }
                }
            }
            return _birthdayInformationList;
        }

        public async System.Threading.Tasks.Task<Tuple<MemoryStream, string, string>> DownloadData()
        {
            var directory = Path.Combine(Environment.CurrentDirectory, @"..\", "Uploads");

            string path = Path.Combine(directory, "Information.xlsx");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Tuple.Create(memory, GetContentType(path), Path.GetFileName(path));

        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
