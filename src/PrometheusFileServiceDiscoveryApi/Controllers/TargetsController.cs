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

        [HttpGet("{group}")]
        public async Task<JsonResult> Get(string group)
        {
            var targets = await _targetsProvider.Provide(group);

            return Json(targets);
        }

        [HttpGet("{group}/{targetname}")]
        public async Task<TargetModel> Get(string group, string targetName)
        {
            var targets = await _targetsProvider.Provide(group);
            
            return targets.SingleOrDefault(x =>
                x.Targets.Any(t => t.Equals(targetName, StringComparison.InvariantCultureIgnoreCase)));
        }

        [HttpDelete("{group}/{targetname}")]
        public async Task<HttpStatusCode> Delete(string group, string targetname)
        {
            await _targetDeleter.Delete(group, targetname);

            return HttpStatusCode.OK;
        }

        [HttpPut("{group}")]
        public async Task<HttpStatusCode> Put(string group, [FromBody] TargetModel targetModel)
        {
            await _targetPersister.Add(group, targetModel);

            return HttpStatusCode.OK;
        }

        [HttpPatch("{group}/{targetname}")]
        public async Task<HttpStatusCode> Patch(string group, string targetname, [FromBody] TargetModel targetModel)
        {
            await _targetPersister.Update(group, targetname, targetModel);

            return HttpStatusCode.OK;
        }
    }
}