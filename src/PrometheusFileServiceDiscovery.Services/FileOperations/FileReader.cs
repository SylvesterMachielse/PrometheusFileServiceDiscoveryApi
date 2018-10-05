using System.IO.Abstractions;
using System.Threading.Tasks;

namespace PrometheusFileServiceDiscoveryApi.Services.FileOperations
{
    public class FileReader : IReadFiles
    {
        private readonly IFileSystem _fileSystem;
        private readonly IAttemptFileOperations _fileOperationAttempter;

        public FileReader(IFileSystem fileSystem, IAttemptFileOperations fileOperationAttempter)
        {
            _fileSystem = fileSystem;
            _fileOperationAttempter = fileOperationAttempter;
        }

        public async Task<string> TryRead(string filePath)
        {
            return await _fileOperationAttempter.AttemptFileAction(() => ReadAllText(filePath));
        }

        private async Task<string> ReadAllText(string filePath)
        {
            using (var reader = _fileSystem.File.OpenText(filePath))
            {
               return await reader.ReadToEndAsync();
            }
        }
    }
}