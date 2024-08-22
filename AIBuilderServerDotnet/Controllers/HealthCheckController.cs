using Microsoft.AspNetCore.Mvc;

namespace AIBuilderServerDotnet.Controllers
{
    public class HealthCheckController : ControllerBase
    {
        // GET: api/healthcheck
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("HealthCheck GET is working.");
        }

        // POST: api/healthcheck
        [HttpPost]
        public IActionResult Post()
        {
            return Ok("HealthCheck POST is working.");
        }

        // PUT: api/healthcheck
        [HttpPut]
        public IActionResult Put()
        {
            return Ok("Healthcheck PUT is working.");
        }

        // PATCH: api/healthcheck
        [HttpPatch]
        public IActionResult Patch()
        {
            return Ok("Healthcheck PATCH is working.");
        }

        // DELETE: api/healthcheck
        [HttpDelete]
        public IActionResult Delete()
        {
            return Ok("HealthCheck DELETE is working.");
        }

        // HEAD: api/healthcheck
        [HttpHead]
        public IActionResult Head()
        {
            return Ok("Healthcheck HEAD is working.");
        }

        // OPTIONS: api/options
        [HttpOptions]
        public IActionResult Options()
        {
            return Ok("Healthcheck OPTIONS is working.");
        }
    }
}
