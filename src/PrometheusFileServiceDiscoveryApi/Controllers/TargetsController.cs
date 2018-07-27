using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrometheusFileServiceDiscovery.Contracts.Models;
using PrometheusFileServiceDiscoveryApi.Services.Targets;

namespace PrometheusFileServiceDiscoveryApi.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/targets")]
    public class TargetsController : Controller
    {
        private readonly IProvideTargets _targetsProvider;
        private readonly IPersistTargets _targetPersister;
        private readonly IDeleteTargets _targetDeleter;

        public TargetsController(IProvideTargets targetsProvider, IPersistTargets targetPersister,
            IDeleteTargets targetDeleter)
        {
            _targetsProvider = targetsProvider;
            _targetPersister = targetPersister;
            _targetDeleter = targetDeleter;
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var targets = await _targetsProvider.Provide();

            return Json(targets);
        }

        [HttpGet("{targetname}")]
        public async Task<TargetModel> Get(string targetName)
        {
            var targets = await _targetsProvider.Provide();
            
            return targets.SingleOrDefault(x =>
                x.Targets.Any(t => t.Equals(targetName, StringComparison.InvariantCultureIgnoreCase)));
        }

        [HttpDelete("{targetname}")]
        public async Task<HttpStatusCode> Delete(string targetname)
        {
            await _targetDeleter.Delete(targetname);

            return HttpStatusCode.OK;
        }

        [HttpPut]
        public async Task<HttpStatusCode> Put([FromBody] TargetModel targetModel)
        {
            await _targetPersister.Add(targetModel);

            return HttpStatusCode.OK;
        }

        [HttpPatch("{targetname}")]
        public async Task<HttpStatusCode> Patch(string targetname, [FromBody] TargetModel targetModel)
        {
            await _targetPersister.Update(targetname, targetModel);

            return HttpStatusCode.OK;
        }
    }
}