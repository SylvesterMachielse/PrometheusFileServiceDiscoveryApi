using System;
using System.Threading.Tasks;

namespace PrometheusFileServiceDiscoveryApi.Services.FileOperations
{
    public interface IAttemptFileOperations
    {
        Task<T> AttemptFileAction<T>(Func<Task<T>> func);
    }
}