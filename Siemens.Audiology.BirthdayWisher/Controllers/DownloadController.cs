﻿using Microsoft.AspNetCore.Mvc;
using Siemens.Audiology.BirthdayWisher.Utilities;

namespace Siemens.Audiology.BirthdayWisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IEmailDataGenerator _emailDataGenerator;
        public DownloadController(IEmailDataGenerator emailDataGenerator)
        {
            _emailDataGenerator = emailDataGenerator;
        }

        [HttpGet]
        public IActionResult ShowTemplate()
        {
            return File(_emailDataGenerator.GetTemplateData(), "text/html", "index.html");
        }
    }
}
