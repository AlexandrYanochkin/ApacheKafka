using ApacheKafka.Common.Models;
using ApacheKafka.Common.Serializers;
using Confluent.Kafka;

namespace ApacheKafka.Common.Factories;

public static class KafkaFactory
{
    public static Producer<string> CreateAtLeastOnceProducer(string serverUrl, string topicName, Action<string> logger)
    {
        return CreateAtLeastOnceProducer<string>(serverUrl, topicName, logger, null);
    }

    public static Consumer<string> CreateAtMostOnceConsumer(string serverUrl, string groupId, string topicName)
    {
        return CreateAtMostOnceConsumer<string>(serverUrl, groupId, topicName, null);
    }

    public static Producer<TValue> CreateAtLeastOnceProducer<TValue>(string serverUrl, string topicName, Action<string> logger)
    {
        return CreateAtLeastOnceProducer(serverUrl, topicName, logger, new ValueSerializer<TValue>());
    }

    public static Consumer<TValue> CreateAtMostOnceConsumer<TValue>(string serverUrl, string groupId, string topicName)
    {
        return CreateAtMostOnceConsumer(serverUrl, groupId, topicName, new ValueSerializer<TValue>());
    }

    private static Producer<TValue> CreateAtLeastOnceProducer<TValue>(
        string serverUrl,
        string topicName,
        Action<string> logger,
        ISerializer<TValue>? serializer)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = serverUrl,
        };

        var producerBuilder = serializer is null
            ? new ProducerBuilder<Null, TValue>(config)
            : new ProducerBuilder<Null, TValue>(config)
                .SetValueSerializer(serializer);
        var messageProducer = producerBuilder
            .SetErrorHandler((_, e) => logger($"Error: {e.Reason}"))
            .Build();

        return new Producer<TValue>(topicName, messageProducer) { Logger = logger };
    }

    private static Consumer<TValue> CreateAtMostOnceConsumer<TValue>(
        string serverUrl,
        string groupId,
        string topicName,
        IDeserializer<TValue>? deserializer)
    {
        var config = new ConsumerConfig
        {
            GroupId = groupId,
            BootstrapServers = serverUrl,
            AutoOffsetReset = AutoOffsetReset.Latest,
        };

        var messageConsumer = deserializer is null
            ? new ConsumerBuilder<Null, TValue>(config)
                .Build()
            : new ConsumerBuilder<Null, TValue>(config)
                .SetValueDeserializer(deserializer)
                .Build();

        return new Consumer<TValue>(topicName, messageConsumer);
    }
}