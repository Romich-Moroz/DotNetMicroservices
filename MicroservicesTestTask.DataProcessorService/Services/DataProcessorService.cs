using System.Text;

using MicroservicesTestTask.Data.Models;
using MicroservicesTestTask.Data.Repositories;
using MicroservicesTestTask.DataProcessorService.Models;
using MicroservicesTestTask.Logging;
using MicroservicesTestTask.RabbitMQ;
using MicroservicesTestTask.Serialization;

namespace MicroservicesTestTask.DataProcessorService.Services
{
    internal class DataProcessorService : IDisposable
    {
        private readonly ISerializer _serializer;
        private readonly RabbitMQClient _client;
        private readonly IRepository<Module> _repository;
        private readonly ILogger _logger;

        public DataProcessorService(ISerializer jsonSerializer, RabbitMQClient rabbitMQClient, IRepository<Module> modulesRepository, ILogger logger)
        {
            _serializer = jsonSerializer;
            _client = rabbitMQClient;
            _repository = modulesRepository;
            _logger = logger;

            rabbitMQClient.AttachReceiver(async (message) => await ProcessJson(message));
            _logger.Log("DataProcessorServices successfully attached to rabbitMQ");
        }

        public async Task ProcessJson(string json)
        {
            _logger.Log("Received json from rabbitMQ");
            DeviceStatus[] statuses = _serializer.Deserialize<DeviceStatus[]>(json, Encoding.Unicode) ?? [];

            _logger.Log($"Successfully deserialized json into classes Count: {statuses.Length}");
            foreach (DeviceStatus status in statuses)
            {
                _logger.Log($"Retrieving module from db id = {status.ModuleCategoryId}");
                Module? s = await _repository.GetAsync(status.ModuleCategoryId);
                if (s is not null)
                {
                    _logger.Log($"Module exists, updating value from {s.ModuleState} to {status.RapidControlStatus.ModuleState}");
                    s.ModuleState = status.RapidControlStatus.ModuleState;
                }
                else
                {
                    _logger.Log("Module should be added, adding");
                    await _repository.AddAsync(Module.FromDeviceStatus(status));
                }
            }

            _logger.Log("Saving changes");
            await _repository.SaveChangesAsync();
        }

        public void Dispose() => _client.Dispose();
    }
}
