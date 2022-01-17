using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
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

            string tempFilePath = Path.Combine(path, "Temp.xlsx");
            using (FileStream stream = new FileStream(tempFilePath, FileMode.Create))
            {
                value.CopyTo(stream);
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(tempFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    //var rowCount = reader.RowCount;
                    reader.Read(); //Header Row
                    while (reader.Read()) //Each row of the file
                    {
                        if (!IsValidEmail(reader[1].ToString()))
                            throw new Exception(reader[1].ToString()+ " is not a valid emailId"); 
                        _birthdayInformationList.Add(new BirthdayInformation
                        {
                            Name = reader[0].ToString(),
                            Email = reader[1].ToString(),
                            GId = reader[2].ToString(),
                            BirthDate = Convert.ToDateTime(reader[3])
                        }); 
                    }
                }
            }

            string filePath = Path.Combine(path, "Information.xlsx");
            File.Copy(tempFilePath, filePath, true);
            File.Delete(tempFilePath);

            return _birthdayInformationList;
        }

        private bool IsValidEmail(string emailId)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emailId);
            return match.Success;
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

        public MemoryStream DownloadData2(List<BirthdayInformation> birthdayInformationList)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ToDataTable(birthdayInformationList));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream;
                }
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                if(prop.Name == "Name" || prop.Name == "GId" || prop.Name == "BirthDate" || prop.Name == "Email")
                //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                int i = 0;
                var values = new object[dataTable.Columns.Count];
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.Name == "Name" || prop.Name == "GId" || prop.Name == "BirthDate" || prop.Name == "Email")
                    {
                        //inserting property values to datatable rows
                        values[i] = prop.GetValue(item, null);

                        if (prop.Name == "BirthDate")
                            values[i] = Convert.ToDateTime(values[i]).ToShortDateString();

                        i++;
                    }
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
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
