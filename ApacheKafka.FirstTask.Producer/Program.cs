using ApacheKafka.Common.Helpers;
using ApacheKafka.Common.Factories;

using static ApacheKafka.Common.KafkaConfiguration;

using (var producer = KafkaFactory.CreateAtLeastOnceProducer<string>(ServerUrl, Topics.First().Name, Console.WriteLine)) 
{
    await KafkaHelper.CreateTopicsAsync(ServerUrl, Topics, Console.WriteLine);

    while (true)
    {
        Console.WriteLine("Input a message:");
        var textMessage = Console.ReadLine();

        if (string.IsNullOrEmpty(textMessage))
        {
            break;
        }

        await producer.SendMessageAsync(textMessage);
    }
}