using Microsoft.AspNetCore.Mvc;

namespace DeployToProduction.Ads.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController: ControllerBase
    {
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
