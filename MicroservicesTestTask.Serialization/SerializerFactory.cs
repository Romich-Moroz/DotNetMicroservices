using MicroservicesTestTask.Serialization.Implementations;

namespace MicroservicesTestTask.Serialization
{
    public static class SerializerFactory
    {
        public enum SerializerType
        {
            Undefined,
            Xml,
            Json
        }

        public static ISerializer Create(SerializerType type) => type switch
        {
            SerializerType.Xml => new Xml(),
            SerializerType.Json => new Json(),
            _ => throw new InvalidOperationException($"Cannot create serializer of type {type}"),
        };
    }
}
