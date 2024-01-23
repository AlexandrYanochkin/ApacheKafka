using ApacheKafka.Common.Models;
using ApacheKafka.Common.Models.Dto;
using ApacheKafka.Common.Factories;
using ApacheKafka.Common.Helpers;

using static ApacheKafka.Common.KafkaConfiguration;

await KafkaHelper.CreateTopicsAsync(ServerUrl, OutputTopics, Console.WriteLine);

var consumers = new List<Consumer<SignalInfo>>();

foreach (var index in Enumerable.Range(default, OutputTopics.Length))
{
    _ = Task.Factory.StartNew(() =>
    {
        using (var consumer = KafkaFactory.CreateAtMostOnceConsumer<SignalInfo>(ServerUrl, Guid.NewGuid().ToString(), Topics[1].Name))
        using (var producer = KafkaFactory.CreateAtLeastOnceProducer(ServerUrl, OutputTopics[index].Name, Console.WriteLine))
        {
            consumers.Add(consumer);
            consumer.OnReceived += async result =>
            {
                if (result.IsError)
                {
                    Console.WriteLine($"Consumer[{index}] failed to receive a vehicle signal");
                }
                else
                {
                    SignalInfo signalInfo = result.Value ?? throw new ArgumentNullException(nameof(result.Value));
                    var distance = signalInfo.CalculateDistance();
                    var fCoordinate = signalInfo.StartCoordinate;
                    var sCoordinate = signalInfo.EndCoordinate;

                    Console.WriteLine($"Consumer[{index}] received a signal from vehicle with id:{signalInfo.VehicleId}");
                    var outputMessage = $"VehicleId:{signalInfo.VehicleId} with coordinates ({fCoordinate.XValue};{fCoordinate.YValue}) and ({sCoordinate.XValue};{sCoordinate.YValue}) has the distance:{distance}";

                    Console.WriteLine(outputMessage);
                    await producer.SendMessageAsync(outputMessage);
                }
            };

            consumer.StartListening();
        }
    }, TaskCreationOptions.LongRunning);
}

Console.ReadLine();
consumers.ForEach(consumer => consumer.StopListening());