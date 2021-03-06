using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Siemens.Audiology.BirthdayWisher.Business.Contract;
using Siemens.Audiology.BirthdayWisher.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Siemens.Audiology.BirthdayWisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirthdayInformationController : ControllerBase
    {
        private readonly IExcelHandler _excelHandler;
        private readonly IBirthdayCalendarProcessor _birthdayCalendarProcessor;
        public BirthdayInformationController(IBirthdayCalendarProcessor birthdayCalendarProcessor, IExcelHandler excelHandler)
        {
            _birthdayCalendarProcessor = birthdayCalendarProcessor;
            _excelHandler = excelHandler;
        }

        // GET: api/<BirthdayInformationController>
        [HttpGet]
        public async Task<IEnumerable<BirthdayInformation>> GetToday()
        {
            return await _birthdayCalendarProcessor.GetBirthDayDetailsForToday();
        }

        // GET: api/<BirthdayInformationController>
        [HttpGet]
        [Route("[action]")]
        public async Task<IEnumerable<BirthdayInformation>> GetAll()
        {
            return await _birthdayCalendarProcessor.GetAllBirthDayDetails();
        }

        // GET api/<BirthdayInformationController>/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            return "value";
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> DownloadExcel()
        {
            //var information = await _excelHandler.DownloadData();
            //return File(information.Item1, information.Item2, information.Item3);
            var list = await GetAll();
            var stream = _excelHandler.DownloadData2(list.ToList());
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "information.xlsx");
        }

        // POST api/<BirthdayInformationController>
        [HttpPost]
        public async Task Post([FromBody] BirthdayInformation birthdayInformation)
        {
            await _birthdayCalendarProcessor.AddBirthDayDetails(birthdayInformation);
        }

        // POST api/<BirthdayInformationController>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UploadExcel(IFormFile value)
        {
            try
            {
                var list = _excelHandler.ReadData(value);
                await _birthdayCalendarProcessor.ClearDetails();
                await _birthdayCalendarProcessor.AddBirthDayDetailsList(list);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Please upload a valid excel file.\n" + ex);
            }
        }

        // PUT api/<BirthdayInformationController>/5
        [HttpPut]
        public async Task Put([FromBody] BirthdayInformation birthdayInformation)
        {
            await _birthdayCalendarProcessor.UpdateBirthDayDetails(birthdayInformation);
        }

        // DELETE api/<BirthdayInformationController>/5
        [HttpDelete("{emailId}")]
        public async Task Delete(string emailId)
        {
            await _birthdayCalendarProcessor.DeleteBirthdayByEmailId(emailId);
        }
    }
}
