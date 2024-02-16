using System.Text;
using System.Xml.Serialization;

namespace MicroservicesTestTask.Serialization.Implementations
{
    internal class Xml : ISerializer
    {
        public void Serialize(Stream s, object data)
        {
            var serializer = new XmlSerializer(data.GetType());

            serializer.Serialize(s, data);
        }

        public string Serialize(object data, Encoding encoding)
        {
            using var stream = new MemoryStream();
            Serialize(stream, data);
            return encoding.GetString(stream.ToArray());
        }

        public void Serialize<T>(Stream s, T data) where T : class?, new() => Serialize(s, data);

        public string Serialize<T>(T data, Encoding encoding) where T : class?, new() => Serialize(data, encoding);

        public object? Deserialize(Type t, Stream s)
        {
            var serializer = new XmlSerializer(t);

            return serializer.Deserialize(s);
        }

        public object? Deserialize(Type t, string s, Encoding encoding)
        {
            using MemoryStream stream = GenerateStreamFromString(s, encoding);
            return Deserialize(t, stream);
        }

        public T? Deserialize<T>(Stream s) where T : class? => (T?)Deserialize(typeof(T), s);

        public T? Deserialize<T>(string s, Encoding encoding) where T : class? => (T?)Deserialize(typeof(T), s, encoding);

        private static MemoryStream GenerateStreamFromString(string s, Encoding encoding)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, encoding);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
