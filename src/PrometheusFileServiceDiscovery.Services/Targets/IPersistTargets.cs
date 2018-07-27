using System.Threading.Tasks;
using PrometheusFileServiceDiscovery.Contracts.Models;

namespace PrometheusFileServiceDiscoveryApi.Services.Targets
{
    public interface IPersistTargets
    {
        Task Add(TargetModel targetModel);
        Task Update(string targetName, TargetModel patchedTargetModel);
    }
}