using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Usadosbr.Contas.WebApi.Controllers
{
    public class HealthCheckController : ApiControllerBase
    {
        private readonly ILogger<HealthCheckController> _logger;
        private readonly IDiagnosticContext _diagnostic;

        public HealthCheckController(ILogger<HealthCheckController> logger, IDiagnosticContext diagnostic)
        {
            _logger = logger;
            _diagnostic = diagnostic;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            _diagnostic.Set("HealthCheck", 42);
            _logger.LogInformation("Health check sendo executado");
            
            return Ok("I'm alive");
        }
    }
}