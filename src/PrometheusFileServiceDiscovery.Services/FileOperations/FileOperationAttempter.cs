using System;
using System.IO;
using System.Threading.Tasks;

namespace PrometheusFileServiceDiscoveryApi.Services.FileOperations
{
    public class FileOperationAttempter : IAttemptFileOperations
    {
        public async Task<T> AttemptFileAction<T>(Func<Task<T>> func)
        {
            var started = DateTime.UtcNow;

            while ((DateTime.UtcNow - started).TotalMilliseconds < 2000)
            {
                try
                {
                    return await func();
                }
                catch (IOException exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }

            return default(T);
        }
    }
}