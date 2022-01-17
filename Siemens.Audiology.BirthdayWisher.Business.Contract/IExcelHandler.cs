using Microsoft.AspNetCore.Http;
using Siemens.Audiology.BirthdayWisher.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Siemens.Audiology.BirthdayWisher.Business.Contract
{
    public interface IExcelHandler
    {
        public List<BirthdayInformation> ReadData(IFormFile FileName);
        public System.Threading.Tasks.Task<Tuple<MemoryStream, string, string>> DownloadData();

        public MemoryStream DownloadData2(List<BirthdayInformation> birthdayInformationList);
    }
}
