using System.Text;
using System.Text.Json;

namespace MicroservicesTestTask.Serialization.Implementations
{
    internal class Json : ISerializer
    {
        public object? Deserialize(Type t, Stream s) => JsonSerializer.Deserialize(s, t);
        public object? Deserialize(Type t, string s, Encoding encoding) => JsonSerializer.Deserialize(s, t);
        public T? Deserialize<T>(Stream s) where T : class? => (T?)Deserialize(typeof(T), s);
        public T? Deserialize<T>(string s, Encoding encoding) where T : class? => (T?)Deserialize(typeof(T), s, encoding);

        public void Serialize(Stream s, object data) => JsonSerializer.Serialize(s, data);
        public string Serialize(object data, Encoding encoding) => JsonSerializer.Serialize(data);
        public void Serialize<T>(Stream s, T data) where T : class?, new() => Serialize(s, data);
        public string Serialize<T>(T data, Encoding encoding) where T : class?, new() => Serialize(data, encoding);
    }
}
