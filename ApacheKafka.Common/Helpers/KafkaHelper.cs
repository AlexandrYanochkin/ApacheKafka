using Confluent.Kafka;
using Confluent.Kafka.Admin;
using ApacheKafka.Common.Models.Dto;

namespace ApacheKafka.Common.Helpers;

public static class KafkaHelper
{
    private const int DefaultTimeOffset = 10;

    public static async Task RemoveTopicAsync(string serverUrl, string topicName)
    {
        var config = new AdminClientConfig
        {
            BootstrapServers = serverUrl,
        };

        using (var adminClient = new AdminClientBuilder(config).Build())
        {
            await adminClient.DeleteTopicsAsync(new[] { topicName });
        }
    }

    public static async Task CreateTopicsAsync(string serverUrl, IEnumerable<TopicInfo> topicInfoCollection, Action<string> logger)
    {
        var config = new AdminClientConfig
        {
            BootstrapServers = serverUrl,
        };

        using (var adminClient = new AdminClientBuilder(config).Build())
        {
            var uniqueTopics = GetUniqueTopics(adminClient, topicInfoCollection);

            foreach (var topicInfo in uniqueTopics)
            {
                await TryCreateTopicAsync(adminClient, topicInfo, logger);
            }
        }
    }

    private static async Task TryCreateTopicAsync(IAdminClient adminClient, TopicInfo topicInfo, Action<string> logger)
    {
        try
        {
            await adminClient.CreateTopicsAsync(new TopicSpecification[] {
                new TopicSpecification
                {
                    Name = topicInfo.Name,
                    NumPartitions = topicInfo.Partitions,
                    ReplicationFactor = topicInfo.ReplicationFactor
                }
            });

            logger($"Topic '{topicInfo.Name}' created successfully.");
        }
        catch (Exception ex)
        {
            logger($"Error creating topic: {ex.Message}");
        }
    }

    private static TopicInfo[] GetUniqueTopics(IAdminClient adminClient, IEnumerable<TopicInfo> topicInfoCollection) 
    {
        var brokerMetadata = adminClient.GetMetadata(TimeSpan.FromSeconds(DefaultTimeOffset));
        var existingTopics = brokerMetadata.Topics
            .Select(topic => topic.Topic)
            .ToArray();
        var uniqueTopics = topicInfoCollection
            .Where(topic => !existingTopics.Contains(topic.Name))
            .ToArray();

        return uniqueTopics;
    }
}
