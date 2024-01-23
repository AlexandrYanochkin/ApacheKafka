using ApacheKafka.Common.Models.Dto;
using Confluent.Kafka;

namespace ApacheKafka.Common.Models;

public class Consumer<TValue> : IDisposable
{
    private bool _isActive;
    private readonly string _topicName;
    private readonly IConsumer<Null, TValue> _messageConsumer;

    public event Action<ResultInfo<TValue>>? OnReceived;

    public Consumer(string topicName, IConsumer<Null, TValue> messageConsumer)
    {
        _topicName = topicName;
        _messageConsumer = messageConsumer;
    }

    public void StartListening()
    {
        _isActive = true;
        _messageConsumer.Subscribe(_topicName);

        while (_isActive)
        {
            try
            {
                var consumeResult = _messageConsumer.Consume();
                _messageConsumer.Commit(consumeResult);

                OnReceived?.Invoke(ResultInfo<TValue>.CreateSuccessfulResult(consumeResult.Message.Value));
            }
            catch (ConsumeException e)
            {
                OnReceived?.Invoke(ResultInfo<TValue>.CreateFailedResult(e.Error.Reason));
            }
        }
    }

    public void StopListening()
    {
        _isActive = false;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _messageConsumer.Unsubscribe();
        _messageConsumer.Close();

        if (disposing)
        {
            _messageConsumer.Dispose();
        }
    }

    ~Consumer() 
    {
        Dispose(false);
    }
}