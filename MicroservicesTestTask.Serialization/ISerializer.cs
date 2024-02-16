using System.Text;

namespace MicroservicesTestTask.Serialization
{
    public interface ISerializer
    {
        public void Serialize(Stream s, object data);
        public string Serialize(object data, Encoding encoding);
        public void Serialize<T>(Stream s, T data) where T : class?, new();
        public string Serialize<T>(T data, Encoding encoding) where T : class?, new();

        public object? Deserialize(Type t, Stream s);
        public object? Deserialize(Type t, string s, Encoding encoding);
        public T? Deserialize<T>(Stream s) where T : class?;
        public T? Deserialize<T>(string s, Encoding encoding) where T : class?;
    }
}
