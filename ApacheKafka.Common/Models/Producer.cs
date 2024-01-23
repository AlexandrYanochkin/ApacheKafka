using Confluent.Kafka;

namespace ApacheKafka.Common.Models;

public class Producer<TValue> : IDisposable
{
    private const int DefaultRetryCount = 3;

    private readonly string _topicName;
    private readonly IProducer<Null, TValue> _messageProducer;

    public required Action<string> Logger { get; init; }

    public Producer(string topicName, IProducer<Null, TValue> messageProducer)
    {
        _topicName = topicName;
        _messageProducer = messageProducer;
    }

    public async Task SendMessageAsync(TValue message)
    {
        for (int attempt = 1;attempt <= DefaultRetryCount; attempt++)
        {
            try
            {
                var deliveryReport = await _messageProducer.ProduceAsync(_topicName, new Message<Null, TValue> { Value = message });
                Logger($"Message delivered to {deliveryReport.TopicPartitionOffset}");

                break;
            }
            catch (ProduceException<Null, TValue> ex)
            {
                Logger($"Delivery attempt {attempt} failed: {ex.Error.Reason}");
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) 
        {
            _messageProducer.Dispose();
        }
    }

    ~Producer()
    {
        Dispose(false);
    }
}