using System.Threading.Tasks;
using PrometheusFileServiceDiscovery.Contracts.Models;

namespace PrometheusFileServiceDiscoveryApi.Services.Targets
{
    public interface IPersistTargets
    {
        Task Add(string group, TargetModel targetModel);
        Task Update(string group, string targetName, TargetModel patchedTargetModel);
    }
}