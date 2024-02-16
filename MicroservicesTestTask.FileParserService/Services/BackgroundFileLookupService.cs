using System.Configuration;

using MicroservicesTestTask.FileParserService.Extensions;

namespace MicroservicesTestTask.FileParserService.Services
{
    internal class BackgroundFileLookupService
    {
        public event EventHandler<IEnumerable<string>>? OnNewFilesFound;

        private readonly string _fileSearchPath = ConfigurationManager.AppSettings["FileSearchPath"] ?? "DefaultSearchPath";
        private readonly string _fileSearchPattern = ConfigurationManager.AppSettings["FileSearchPattern"] ?? "*.xml";
        private readonly int _delayBetweenFileSearches = int.TryParse(ConfigurationManager.AppSettings["DelayBetweenFileSearches"], out var result) ? result : 5000;

        public Task FileLookupTask(CancellationToken token) => Task.Run(
            () =>
            {
                while (!token.IsCancellationRequested)
                {
                    DirectoryExtensions.EnsureDirectoryExists(_fileSearchPath);

                    var files = Directory.GetFiles(_fileSearchPath, _fileSearchPattern);
                    if (files.Length > 0)
                    {
                        OnNewFilesFound?.Invoke(this, files);
                    }

                    Thread.Sleep(_delayBetweenFileSearches);
                }
            },
        token);
    }
}
