using System.Threading.Tasks;

namespace PrometheusFileServiceDiscoveryApi.Services.Targets
{
    public interface IDeleteTargets
    {
        Task Delete(string targetName);
    }
}