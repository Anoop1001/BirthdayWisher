using Microsoft.AspNetCore.Http;
using Siemens.Audiology.BirthdayWisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Siemens.Audiology.BirthdayWisher.Business.Contract
{
    public interface IExcelReader
    {
        public List<BirthdayInformation> ReadData(IFormFile FileName);
        
    }
}
