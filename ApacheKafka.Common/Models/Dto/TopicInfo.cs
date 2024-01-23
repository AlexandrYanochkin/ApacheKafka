namespace ApacheKafka.Common.Models.Dto;

public class TopicInfo
{
    public string Name { get; set; } = string.Empty;

    public int Partitions { get; set; }

    public short ReplicationFactor { get; set; }
}
