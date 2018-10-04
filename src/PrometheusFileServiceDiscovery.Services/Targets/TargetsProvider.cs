using System.Threading.Tasks;
using Newtonsoft.Json;
using PrometheusFileServiceDiscovery.Contracts.Models;
using PrometheusFileServiceDiscoveryApi.Services.FileOperations;
using PrometheusFileServiceDiscoveryApi.Services.Settings;

namespace PrometheusFileServiceDiscoveryApi.Services.Targets
{
    public class TargetsProvider : IProvideTargets
    {
        private readonly IProvideSettings _settingsProvider;
        private readonly IReadFiles _fileReader;

        private TargetsModel _targetsModel = new TargetsModel();

        public TargetsProvider(IProvideSettings settingsProvider, IReadFiles fileReader)
        {
            _settingsProvider = settingsProvider;
            _fileReader = fileReader;
        }

        public async Task<TargetsModel> Provide(string group)
        {
            var targetsFileLocation = _settingsProvider.ProvideTargetFileLocation(group);

            var targetsFileContent = await _fileReader.TryRead(targetsFileLocation);

            var deserializeObject = JsonConvert.DeserializeObject<TargetsModel>(targetsFileContent);

            if (deserializeObject == null)
            {
                return new TargetsModel();
            }

            return deserializeObject;
        }
    }
}