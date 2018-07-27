using System;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace PrometheusFileServiceDiscoveryApi.Services.FileOperations
{
    public class FileWriter : IWriteFiles
    {
        private readonly IFileSystem _fileSystem;
        private readonly IAttemptFileOperations _fileOperationAttempter;

        public FileWriter(IFileSystem fileSystem, IAttemptFileOperations fileOperationAttempter)
        {
            _fileSystem = fileSystem;
            _fileOperationAttempter = fileOperationAttempter;
        }

        public async Task TryWrite(string content, string filePath)
        {
            await _fileOperationAttempter.AttemptFileAction(() => WriteAllText(filePath, content));
        }

        private async Task<bool> WriteAllText(string filePath, string content)
        {
            using (var writer = _fileSystem.File.CreateText(filePath))
            {
                await writer.WriteAsync(content);
            }

            Console.WriteLine("Succesfully wrote the file");

            return true;
        }
    }
}