using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrometheusFileServiceDiscovery.Contracts.Models;
using PrometheusFileServiceDiscoveryApi.Services.FileOperations;
using PrometheusFileServiceDiscoveryApi.Services.Settings;

namespace PrometheusFileServiceDiscoveryApi.Services.Targets
{
    public class TargetDeleter : IDeleteTargets
    {
        private readonly IProvideTargets _targetsProvider;
        private readonly IWriteFiles _fileWriter;
        private readonly IProvideSettings _settingsProvider;

        public TargetDeleter(IProvideTargets targetsProvider, IWriteFiles fileWriter,
            IProvideSettings settingsProvider)
        {
            _targetsProvider = targetsProvider;
            _fileWriter = fileWriter;
            _settingsProvider = settingsProvider;
        }

        public async Task Delete(string group, string targetName)
        {
            var allTargets = await _targetsProvider.Provide(group);

            var targetToDelete = allTargets.ToList().SingleOrDefault(x =>
                x.Targets.Any(t => t.Equals(targetName, StringComparison.InvariantCultureIgnoreCase)));

            if (targetToDelete == default(TargetModel))
            {
                Console.WriteLine($"Cannot delete target {targetName} -- not found");

                return;
            }

            var targetIndex = allTargets.IndexOf(targetToDelete);

            allTargets.RemoveAt(targetIndex);

            var allTargetsJsonText = JsonConvert.SerializeObject(allTargets);

            var targetsFileLocation = _settingsProvider.ProvideTargetFileLocation(group);

            await _fileWriter.TryWrite(allTargetsJsonText, targetsFileLocation);
        }
    }
}