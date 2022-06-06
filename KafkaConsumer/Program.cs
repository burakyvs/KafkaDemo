// See https://aka.ms/new-console-template for more information

using Kafka.Public;
using Kafka.Public.Loggers;
using System.Text;

Console.WriteLine("Consumer started.");

ClusterClient cluster = new ClusterClient(new Configuration
{
    Seeds = "localhost:9092"
}, new ConsoleLogger());


cluster.ConsumeFromLatest("demo");
cluster.MessageReceived += record =>
{
    Console.WriteLine($"Received: {Encoding.UTF8.GetString(record.Value as byte[])}");
};

Console.WriteLine("\n\nPress ENTER to stop consuming...\n\n");
Console.ReadLine();
