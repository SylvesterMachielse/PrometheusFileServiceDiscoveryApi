using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrometheusFileServiceDiscovery.Contracts.Models;
using PrometheusFileServiceDiscoveryApi.Services.FileOperations;
using PrometheusFileServiceDiscoveryApi.Services.Settings;

namespace PrometheusFileServiceDiscoveryApi.Services.Targets
{
    public class TargetPersister : IPersistTargets
    {
        private readonly IProvideTargets _targetsProvider;
        private readonly IWriteFiles _fileWriter;
        private readonly IProvideSettings _settingsProvider;

        public TargetPersister(IProvideTargets targetsProvider, IWriteFiles fileWriter,
            IProvideSettings settingsProvider)
        {
            _targetsProvider = targetsProvider;
            _fileWriter = fileWriter;
            _settingsProvider = settingsProvider;
        }

        public async Task Add(TargetModel targetModel)
        {
            var allTargets = await _targetsProvider.Provide();

            var existingTargets = allTargets.Where(x =>
                x.Targets.Any(t =>
                    targetModel.Targets.Any(newTarget =>
                        newTarget.Equals(t, StringComparison.InvariantCultureIgnoreCase))));

            if (existingTargets.Any())
            {
                throw new ArgumentException($"Target with same name already exists");
            }

            allTargets.Add(targetModel);

            var allTargetsJsonText = JsonConvert.SerializeObject(allTargets);

            var targetsFileLocation = _settingsProvider.Provide().TargetFileLocation;

            await _fileWriter.TryWrite(allTargetsJsonText, targetsFileLocation);
        }

        public async Task Update(string targetName, TargetModel patchedTargetModel)
        {
            EnsureResourceIdentifierIsNotPatched(targetName, patchedTargetModel);

            var targets = await _targetsProvider.Provide();

            var targetToPatch = targets.SingleOrDefault(x =>
                x.Targets.Any(t => t.Equals(targetName, StringComparison.InvariantCultureIgnoreCase)));

            EnsureTargetExists(targetName, targetToPatch);

            var indexToReplace = targets.IndexOf(targetToPatch);

            var targetsList = targets.ToList();
            targetsList[indexToReplace] = patchedTargetModel;

            EnsureAllTargetsAreDistinct(targetsList);

            var allTargetsJsonText = JsonConvert.SerializeObject(targetsList);

            var targetsFileLocation = _settingsProvider.Provide().TargetFileLocation;

            await _fileWriter.TryWrite(allTargetsJsonText, targetsFileLocation);
        }

        private void EnsureTargetExists(string targetName, TargetModel targetToPatch)
        {
            if (targetToPatch == default(TargetModel))
            {
                throw new ArgumentException($"target {targetName} not found");
            }
        }

        private void EnsureResourceIdentifierIsNotPatched(string targetName, TargetModel patchedTargetModel)
        {
            if (patchedTargetModel.Targets.All(x => !x.Equals(targetName, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ArgumentException($"Cannot change target name");
            }
        }

        private void EnsureAllTargetsAreDistinct(List<TargetModel> targetsList)
        {
            var allTargets = targetsList.SelectMany(x => x.Targets);
            if (allTargets.Count() != allTargets.Distinct().Count())
            {
                var targetsThatOccurMoreThanOnce = allTargets.GroupBy(s => s).Where(x => x.Count() > 1);
                throw new ArgumentException($"Targets must all be distinct: {string.Join(", ", targetsThatOccurMoreThanOnce)}");
            }
        }
    }
}