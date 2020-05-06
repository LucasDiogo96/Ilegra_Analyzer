using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Analyzer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        [HttpGet]
        [Route("Monitoring")]
        public IActionResult Create()
        {
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
 
    }
}
