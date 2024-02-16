using System.Text;

using MicroservicesTestTask.Data.Models;
using MicroservicesTestTask.RabbitMQ;
using MicroservicesTestTask.Serialization;

namespace MicroservicesTestTask.FileParserService.Workers
{
    internal class JsonWorker(ISerializer jsonSerializer, RabbitMQClient rabbitMQClient) : IDisposable
    {
        public void SendToRabbitMQ(DeviceStatus[] deviceStatuses)
        {
            var result = jsonSerializer.Serialize(deviceStatuses, Encoding.Unicode);
            rabbitMQClient.SendMessage(result);
        }

        public void Dispose() => rabbitMQClient.Dispose();
    }
}
