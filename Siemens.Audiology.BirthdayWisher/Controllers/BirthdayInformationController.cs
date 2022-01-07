using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Siemens.Audiology.BirthdayWisher.Business.Contract;
using Siemens.Audiology.BirthdayWisher.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Siemens.Audiology.BirthdayWisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirthdayInformationController : ControllerBase
    {
        private readonly IExcelReader _excelReader;
        private readonly IBirthdayCalendarProcessor _birthdayCalendarProcessor;
        public BirthdayInformationController(IBirthdayCalendarProcessor birthdayCalendarProcessor, IExcelReader excelReader)
        {
            _birthdayCalendarProcessor = birthdayCalendarProcessor;
            _excelReader = excelReader;
        }
        // GET: api/<BirthdayInformationController>
        [HttpGet]
        public async Task<IEnumerable<BirthdayInformation>> Get()
        {
            return await _birthdayCalendarProcessor.GetBirthDayDetails();
        }

        // GET api/<BirthdayInformationController>/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            return "value";
        }

        // POST api/<BirthdayInformationController>
        [HttpPost]
        public async Task Post([FromBody] string value)
        {
            await _birthdayCalendarProcessor.AddBirthDayDetails();
        }

        // POST api/<BirthdayInformationController>
        [HttpPost]
        [Route("[action]")]
        public async Task UploadExcel(IFormFile value)
        {
            var list = _excelReader.ReadData(value);
            await _birthdayCalendarProcessor.ClearDetails();
            await _birthdayCalendarProcessor.AddBirthDayDetailsList(list);
        }

        // PUT api/<BirthdayInformationController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BirthdayInformationController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
        }
    }
}
