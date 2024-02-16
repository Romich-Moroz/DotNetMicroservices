using System.Configuration;

using MicroservicesTestTask.Data.Models;
using MicroservicesTestTask.FileParserService.Extensions;
using MicroservicesTestTask.FileParserService.Workers;
using MicroservicesTestTask.Logging;

namespace MicroservicesTestTask.FileParserService.Services
{
    internal class XmlFileParserService
    {
        private readonly string _fileSearchArchiveFolderName = Path.Combine(
            Environment.CurrentDirectory,
            ConfigurationManager.AppSettings["FileSearchArchiveFolderName"] ?? "DefaultArchiveFolder"
        );

        private readonly BackgroundFileLookupService _backgroundFileFinder;
        private readonly CancellationTokenSource _backgroundFileFinderCancellationTokenSource = new();

        private readonly XmlWorker _xmlWorker;
        private readonly JsonWorker _jsonWorker;

        private readonly ILogger _logger;

        private Task _backgroundFileFinderTask = Task.CompletedTask;

        public XmlFileParserService(BackgroundFileLookupService backgroundFileFinder, XmlWorker xmlWorker, JsonWorker jsonWorker, ILogger logger)
        {
            _backgroundFileFinder = backgroundFileFinder;
            _xmlWorker = xmlWorker;
            _jsonWorker = jsonWorker;
            _logger = logger;
            _backgroundFileFinder.OnNewFilesFound += ProcessFiles;
        }

        public void StartProcessing()
        {
            _logger.Log("Start processing");
            _backgroundFileFinderTask = _backgroundFileFinder.FileLookupTask(_backgroundFileFinderCancellationTokenSource.Token);
        }

        public async Task StopProcessing()
        {
            _logger.Log("Stop processing");
            _backgroundFileFinderCancellationTokenSource.Cancel();
            await _backgroundFileFinderTask;
        }

        private void ProcessFiles(object? sender, IEnumerable<string> files)
        {
            DirectoryExtensions.EnsureDirectoryExists(_fileSearchArchiveFolderName);

            files.AsParallel().ForAll(file =>
            {
                _logger.Log($"Parsing xml file {file}");
                InstrumentStatus instrumentStatus = _xmlWorker.ParseXml(file);

                _logger.Log("Updating device statuses");
                foreach (DeviceStatus status in instrumentStatus.DeviceStatuses)
                {
                    status.RapidControlStatus.ModuleState = RandomModuleStateGenerator.GetRandomModuleState();
                }

                _logger.Log("Sending updates to RabbitMQ as Json");
                _jsonWorker.SendToRabbitMQ(instrumentStatus.DeviceStatuses);

                var newFileName = @$"{DateTime.Now:ddMMyyyyHHmmssFFFFFFF}{Path.GetFileName(file)}";
                _logger.Log($"Moving file {file} to archive");
                File.Move(file, Path.Combine(_fileSearchArchiveFolderName, newFileName));
            });
        }
    }
}
