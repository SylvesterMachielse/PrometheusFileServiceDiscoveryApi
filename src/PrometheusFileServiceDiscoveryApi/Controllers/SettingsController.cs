using Microsoft.AspNetCore.Mvc;
using PrometheusFileServiceDiscoveryApi.Services.Settings;

namespace PrometheusFileServiceDiscoveryApi.Controllers
{
    [Produces("application/json")]
    [Route("api/settings")]
    public class SettingsController : Controller
    {
        private readonly IProvideSettings _settingsProvider;

        public SettingsController(IProvideSettings settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(_settingsProvider.Provide());
        }
    }
}