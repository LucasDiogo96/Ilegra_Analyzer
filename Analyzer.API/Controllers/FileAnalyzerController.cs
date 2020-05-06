using Analyzer.CrossCutting.Lib.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace Analyzer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileAnalyzerController : Controller
    {
        private IHostingEnvironment Environment;

        public FileAnalyzerController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        [HttpPost]
        [Route("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Upload(List<IFormFile> files)
        {
            try
            {
                string path = PathUtil.GetInputPathMonitor();

                List<string> uploadedFiles = new List<string>();
                foreach (IFormFile postedFile in files)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
