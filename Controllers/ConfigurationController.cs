using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MM26TestServer.Services;
using MM26TestServer.Models;

namespace MM26TestServer.Controllers
{
    [ApiController]
    [Route("configure/")]
    public class ConfigurationController : ControllerBase
    {
        private IConfigurationService _service;
        private ILogger<ConfigurationController> _logger;

        public ConfigurationController(
            IConfigurationService service,
            ILogger<ConfigurationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public void Post([FromBody] Configuration configuration)
        {
            _service.State = configuration.State;
            _service.Changes = configuration.Changes;

            _logger.LogInformation("Configuration received");
        }

        [HttpGet]
        public string Get()
        {
            return "configure/";
        }
    }
}
