using RabbitMQ.Client;
using System.Text;

const string Exchange = "direct_logs";
const string RoutingKeyError = "error";
const string RoutingKeyWarn = "warn";
const string RoutingKeyInfo = "info";
const int MinDelay = 1000; // ms

var id = args.FirstOrDefault("unknown");

Task.Factory.StartNew(CreateProducerTask(id, MinDelay + 4000, RoutingKeyError), TaskCreationOptions.LongRunning);
Task.Factory.StartNew(CreateProducerTask(id, MinDelay + 2000, RoutingKeyWarn), TaskCreationOptions.LongRunning);
Task.Factory.StartNew(CreateProducerTask(id, MinDelay + 1000, RoutingKeyInfo), TaskCreationOptions.LongRunning);

Console.ReadLine();

static Func<Task> CreateProducerTask(string id, int timeToSleep, string routingKey)
{
    return () =>
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: Exchange, type: ExchangeType.Direct);

        var counter = 0;
        do
        {
            // Delay between sending.
            Thread.Sleep(Random.Shared.Next(MinDelay, timeToSleep));

            var message = $"Message #{counter} type [{routingKey}] from publisher [{id}]";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: Exchange,
                routingKey: routingKey,
                basicProperties: null,
                body: body);

            Console.WriteLine($"P[{id}]: message #{counter} type [{routingKey}] sent into [{Exchange}] exchange.");
            counter++;
        } while (true);
    };
}