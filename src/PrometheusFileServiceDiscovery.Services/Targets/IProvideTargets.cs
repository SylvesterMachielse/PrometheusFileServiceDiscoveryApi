using System.Threading.Tasks;
using PrometheusFileServiceDiscovery.Contracts.Models;

namespace PrometheusFileServiceDiscoveryApi.Services.Targets
{
    public interface IProvideTargets
    {
        Task<TargetsModel> Provide();
    }
}