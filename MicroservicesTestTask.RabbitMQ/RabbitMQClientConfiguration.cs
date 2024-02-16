namespace MicroservicesTestTask.RabbitMQ
{
    public class RabbitMQClientConfiguration
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; }
        public string Username { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string Queue { get; set; } = "DefaultQueue";

        public override string ToString() =>
            $"Hostname: {HostName}; " +
            $"Port: {Port}; " +
            $"Username: {Username}; " +
            $"Password: {Password}; " +
            $"Queue: {Queue}; ";
    }
}
