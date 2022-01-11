using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Siemens.Audiology.BirthdayWisher.Utilities;
using System.IO;

namespace Siemens.Audiology.BirthdayWisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        // private readonly string _filePath;
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly IEmailDataGenerator _emailDataGenerator;
        public DownloadController(IHostEnvironment hostingEnvironment, IEmailDataGenerator emailDataGenerator)
        {
            _hostingEnvironment = hostingEnvironment;
            _emailDataGenerator = emailDataGenerator;
        }

        [HttpGet]
        public IActionResult ShowTemplate()
        {
            return File(_emailDataGenerator.GetTemplateData(), "text/html", "index.html");
        }
    }
}
