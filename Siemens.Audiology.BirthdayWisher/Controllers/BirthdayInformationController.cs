using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Siemens.Audiology.BirthdayWisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirthdayInformationController : ControllerBase
    {
        // GET: api/<BirthdayInformationController>
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
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
