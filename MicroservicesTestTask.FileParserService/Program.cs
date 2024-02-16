using System.Configuration;

using MicroservicesTestTask.FileParserService.Services;
using MicroservicesTestTask.FileParserService.Workers;
using MicroservicesTestTask.Logging;
using MicroservicesTestTask.RabbitMQ;
using MicroservicesTestTask.Serialization;

using RabbitMQ.Client.Exceptions;

namespace MicroservicesTestTask.FileParserService
{
    internal class Program
    {
        private static async Task Main()
        {
            try
            {
                var _backgroundFileLookupService = new BackgroundFileLookupService();

                RabbitMQClientConfiguration config = GetConfigurationFromFile();

                Console.WriteLine($"Creating client with config: {config}");
                var client = new RabbitMQClient(config);
                using var jsonWorker = new JsonWorker(SerializerFactory.Create(SerializerFactory.SerializerType.Json), client);
                var xmlWorker = new XmlWorker(SerializerFactory.Create(SerializerFactory.SerializerType.Xml));

                ILogger logger = new FileLogger(ConfigurationManager.AppSettings["LogFile"] ?? "Log.txt");

                var dataProcessor = new XmlFileParserService(
                    _backgroundFileLookupService,
                    xmlWorker,
                    jsonWorker,
                    logger
                );

                Console.WriteLine("Service is running");
                dataProcessor.StartProcessing();

                WaitForCancellationRequest();

                Console.WriteLine("Service is shutting down");
                await dataProcessor.StopProcessing();
            }
            catch (BrokerUnreachableException e)
            {
                Console.WriteLine($"RabbitMQ broker is not responding, make sure its running and restart this service {e.HelpLink}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected exception occured: {e.Message}");
            }

            _ = Console.ReadLine();
        }

        private static void WaitForCancellationRequest()
        {
            while (!string.Equals(Console.ReadLine(), "stop", StringComparison.InvariantCultureIgnoreCase))
            {
                Thread.Sleep(100);
            }
        }

        private static RabbitMQClientConfiguration GetConfigurationFromFile() => new()
        {
            HostName = ConfigurationManager.AppSettings["RabbitMQClientHostName"] ?? "localhost",
            Port = int.TryParse(ConfigurationManager.AppSettings["RabbitMQClientPort"], out var result) ? result : 5672,
            Username = ConfigurationManager.AppSettings["RabbitMQClientUsername"] ?? "guest",
            Password = ConfigurationManager.AppSettings["RabbitMQClientPassword"] ?? "guest",
            Queue = ConfigurationManager.AppSettings["RabbitMQClientQueue"] ?? "DefaultQueue"
        };
    }
}
