using System.Threading.Tasks;

namespace PrometheusFileServiceDiscoveryApi.Services.FileOperations
{
    public interface IWriteFiles
    {
        Task TryWrite(string content, string filePath);
    }
}