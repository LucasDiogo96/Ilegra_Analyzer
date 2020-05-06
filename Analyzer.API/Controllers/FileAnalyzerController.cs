using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {

            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

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
    }
}
