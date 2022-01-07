using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Siemens.Audiology.BirthdayWisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
       // private readonly string _filePath;
        public DownloadController(string filePath)
        {
           // _filePath = filePath;
        }

        //[HttpGet]
        //public FileContentResult ShowTemplate()
        //{
        //    string fileName = "index.html";
        //    string filePath = HttpContext.Current.Server.MapPath("~/StaticFile/") + fileName;
        //    return File(System.IO.File.ReadAllBytes(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory())), "text/html", "index.html");
        //}
    }
}
