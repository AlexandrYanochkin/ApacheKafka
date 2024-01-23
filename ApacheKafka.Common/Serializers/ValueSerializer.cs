using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace ApacheKafka.Common.Serializers;

internal class ValueSerializer<TValue> : ISerializer<TValue>, IDeserializer<TValue>
{
    public byte[] Serialize(TValue data, SerializationContext context)
    {
        var jsonContent = JsonConvert.SerializeObject(data);

        return Encoding.UTF8.GetBytes(jsonContent);
    }

    public TValue Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var jsonContent = Encoding.UTF8.GetString(data);
        var deserializedObject = JsonConvert.DeserializeObject<TValue>(jsonContent);

        if (deserializedObject is null)
        {
            throw new NullReferenceException(nameof(deserializedObject));
        }

        return deserializedObject;
    }
}