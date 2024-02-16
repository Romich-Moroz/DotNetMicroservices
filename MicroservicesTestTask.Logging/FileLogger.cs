namespace MicroservicesTestTask.Logging
{
    public sealed class FileLogger(string filePath) : ILogger, IDisposable
    {
        private static readonly object Locker = new();
        private readonly StreamWriter _writer = File.AppendText(filePath);

        public void Dispose() => _writer.Dispose();

        public void Log(string message)
        {
            lock (Locker)
            {
                _writer.WriteLine(message);
                _writer.Flush();
            }
        }
    }
}
