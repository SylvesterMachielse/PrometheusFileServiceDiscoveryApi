using System.Threading.Tasks;

namespace PrometheusFileServiceDiscoveryApi.Services.FileOperations
{
    public interface IReadFiles
    {
        Task<string> TryRead(string filePath);
    }
}