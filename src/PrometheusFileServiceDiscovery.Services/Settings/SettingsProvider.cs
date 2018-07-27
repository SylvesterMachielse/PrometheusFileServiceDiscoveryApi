using PrometheusFileServiceDiscoveryApi.Services.Models;

namespace PrometheusFileServiceDiscoveryApi.Services.Settings
{
    public class SettingsProvider : IProvideSettings
    {
        private readonly string _targetsFileLocation;
        
        public SettingsProvider(string targetsFileLocation)
        {
            _targetsFileLocation = targetsFileLocation;
        }

        public SettingsModel Provide()
        {
           return new SettingsModel()
           {
               TargetFileLocation = _targetsFileLocation
           };
        }
    }
}