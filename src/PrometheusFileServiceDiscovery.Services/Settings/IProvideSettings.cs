using PrometheusFileServiceDiscoveryApi.Services.Models;

namespace PrometheusFileServiceDiscoveryApi.Services.Settings
{
    public interface IProvideSettings
    {
        AppConfiguration Provide();
        string ProvideTargetFileLocation(string group);
    }
}
