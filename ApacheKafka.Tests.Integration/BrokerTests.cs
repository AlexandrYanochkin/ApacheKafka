using System.Diagnostics;
using ApacheKafka.Common.Helpers;
using ApacheKafka.Common.Factories;
using NUnit.Framework;

using static ApacheKafka.Common.KafkaConfiguration;

namespace ApacheKafka.Tests.Integration;

public class BrokerTests
{
    [Test]
    [TestCase("TestMessage")]
    public async Task TestAtLeastOnceProducerAndAtMostOnceConsumer(string textMessage)
    {
        using (var producer = KafkaFactory.CreateAtLeastOnceProducer(ServerUrl, TestTopic.Name, message => Debug.WriteLine(message)))
        using (var consumer = KafkaFactory.CreateAtMostOnceConsumer(ServerUrl, Guid.NewGuid().ToString(), TestTopic.Name))
        {
            await KafkaHelper.CreateTopicsAsync(ServerUrl, new[] { TestTopic }, message => Debug.WriteLine(message));

            consumer.OnReceived += result =>
            {
                consumer.StopListening();

                if (result.IsError)
                {
                    Assert.Fail(result.ErrorMessage);
                }
                else
                {
                    Assert.IsTrue(result.Value == textMessage);
                }
            };

            _ = Task.Run(consumer.StartListening);
            await producer.SendMessageAsync(textMessage);   
        }
    }
}