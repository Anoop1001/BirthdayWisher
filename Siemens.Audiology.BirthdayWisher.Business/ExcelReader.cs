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
    public class ExcelReader : IExcelReader
    {
        List<BirthdayInformation> _birthdayInformationList = new List<BirthdayInformation>();

        public List<BirthdayInformation> ReadData(IFormFile value)
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"..\", "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, value.FileName);
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
    }
}
