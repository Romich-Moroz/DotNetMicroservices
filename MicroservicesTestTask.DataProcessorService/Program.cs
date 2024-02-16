using System.Configuration;

using MicroservicesTestTask.Data.Repositories;
using MicroservicesTestTask.DataProcessorService.DbContexts;
using MicroservicesTestTask.DataProcessorService.Models;
using MicroservicesTestTask.DataProcessorService.Repositories;
using MicroservicesTestTask.Logging;
using MicroservicesTestTask.RabbitMQ;
using MicroservicesTestTask.Serialization;

using RabbitMQ.Client.Exceptions;

namespace MicroservicesTestTask.DataProcessorService
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                RabbitMQClientConfiguration config = GetConfigurationFromFile();

                Console.WriteLine($"Creating client with config: {config}");
                var client = new RabbitMQClient(config);

                ISerializer serializer = SerializerFactory.Create(SerializerFactory.SerializerType.Json);
                IRepository<Module> repository = new ModulesRepository(
                    new ApplicationContext(ConfigurationManager.AppSettings["SqLiteDatabaseConnectionString"] ?? throw new InvalidOperationException("Setup sql connection string"))
                );

                ILogger logger = new FileLogger(ConfigurationManager.AppSettings["LogFile"] ?? "Log.txt");

                Console.WriteLine("Service is running");
                using var service = new Services.DataProcessorService(serializer, client, repository, logger);
                WaitForCancellationRequest();
                Console.WriteLine("Service is shutting down");
            }
            catch (BrokerUnreachableException e)
            {
                Console.WriteLine($"RabbitMQ broker is not responding, make sure its running and restart this service {e.HelpLink}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected exception occured: {e.Message}");
            }
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
