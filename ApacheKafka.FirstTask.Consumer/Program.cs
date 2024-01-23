using ApacheKafka.Common.Factories;

using static ApacheKafka.Common.KafkaConfiguration;

using (var consumer = KafkaFactory.CreateAtMostOnceConsumer(ServerUrl, Guid.NewGuid().ToString(), Topics.First().Name))
{
    consumer.OnReceived += result =>
    {
        if (result.IsError)
        {
            Console.WriteLine($"Error consuming message: {result.ErrorMessage}");
        }
        else
        {
            Console.WriteLine($"Consumed message: {result.Value}");
        }
    };

    Console.WriteLine("The consumer is starting");
    _ = Task.Factory.StartNew(consumer.StartListening, TaskCreationOptions.LongRunning);

    Console.ReadLine();
    consumer.StopListening();
}