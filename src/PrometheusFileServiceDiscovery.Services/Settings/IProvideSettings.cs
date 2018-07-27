using PrometheusFileServiceDiscoveryApi.Services.Models;

namespace PrometheusFileServiceDiscoveryApi.Services.Settings
{
    public interface IProvideSettings
    {
        SettingsModel Provide();
    }
}
