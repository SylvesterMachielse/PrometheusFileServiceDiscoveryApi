using System.Collections.Generic;

namespace PrometheusFileServiceDiscoveryApi.Services.Models
{
    public class AppConfiguration
    {
        public string Host { get; set; }
        public List<FileConfiguration> TargetFileLocations { get; set; }
    }

    public class FileConfiguration
    {
        public string Name { get; set; }
        public string TargetsFileLocation { get; set; }
    }
}