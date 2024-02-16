using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroservicesTestTask.RabbitMQ
{
    public class RabbitMQClient : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queue;

        private bool _disposedValue;

        ~RabbitMQClient() => Dispose(false);

        public RabbitMQClient(RabbitMQClientConfiguration config)
        {
            _queue = config.Queue;
            var factory = new ConnectionFactory
            {
                HostName = config.HostName,
                Port = config.Port,
                UserName = config.Username,
                Password = config.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _channel.Dispose();
                    _connection.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void SendMessage(string message)
        {
            ObjectDisposedException.ThrowIf(_disposedValue, this);

            _ = _channel.QueueDeclare(queue: _queue,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: string.Empty,
                                  routingKey: _queue,
                                  basicProperties: null,
                                  body: body);
        }

        public void AttachReceiver(Action<string> onReceive)
        {
            _ = _channel.QueueDeclare(queue: _queue,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                onReceive(message);
            };

            _ = _channel.BasicConsume(queue: _queue,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
