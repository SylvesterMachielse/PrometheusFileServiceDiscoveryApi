using System;
using System.Linq;
using PrometheusFileServiceDiscoveryApi.Services.Models;

namespace PrometheusFileServiceDiscoveryApi.Services.Settings
{
    public class SettingsProvider : IProvideSettings
    {
       private readonly AppConfiguration _appConfiguration;

        public SettingsProvider(AppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public AppConfiguration Provide()
        {
            return _appConfiguration;
        }

        public string ProvideTargetFileLocation(string group)
        {
            return _appConfiguration.TargetFileLocations.Single(x=> x.Name.Equals(group, StringComparison.InvariantCultureIgnoreCase)).TargetsFileLocation;
        }
    }
}