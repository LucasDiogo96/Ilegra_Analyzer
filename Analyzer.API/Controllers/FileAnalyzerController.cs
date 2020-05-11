using Analyzer.CrossCutting.Lib.Util;
using DnsClient.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Analyzer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileAnalyzerController : Controller
    {
        private readonly ILogger<FileAnalyzerController> _logger;
        public FileAnalyzerController(ILogger<FileAnalyzerController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Upload(List<IFormFile> files)
        {
            try
            {

                if (!files.Any())
                    return BadRequest();

                string path = PathUtil.GetInputPathMonitor();

                //save files asynchronously
                var t = Task.Run(() =>
                {
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
                }).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }
    }
}
