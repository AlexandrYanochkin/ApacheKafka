using ApacheKafka.Common.Models.Dto;

namespace ApacheKafka.Common;

public static class KafkaConfiguration
{
    public const string ServerUrl = "localhost:9092";

    public static TopicInfo TestTopic { get; } = new TopicInfo
    {
        Name = "TestTopic2",
        Partitions = 3,
        ReplicationFactor = 1,
    };

    public static TopicInfo[] Topics { get; } =
    [
        new TopicInfo
        {
            Name = "Topic1",
            Partitions = 3,
            ReplicationFactor = 1,
        },
        new TopicInfo
        {
            Name = "Topic2",
            Partitions = 3,
            ReplicationFactor = 1,
        },
        new TopicInfo
        {
            Name = "Topic3",
            Partitions = 3,
            ReplicationFactor = 1,
        },
    ];

    public static TopicInfo[] OutputTopics { get; } =
    [
        new TopicInfo
        {
            Name = "Output-Topic1",
            Partitions = 3,
            ReplicationFactor = 1,
        },
        new TopicInfo
        {
            Name = "Output-Topic2",
            Partitions = 3,
            ReplicationFactor = 1,
        },
        new TopicInfo
        {
            Name = "Output-Topic3",
            Partitions = 3,
            ReplicationFactor = 1,
        },
    ];
}
