using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

const string Exchange = "direct_logs";
const string RoutingKeyError = "error";
const string RoutingKeyWarn = "warn";
const string RoutingKeyInfo = "info";

var id = args.FirstOrDefault("");
var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = factory.CreateConnection())
{
    using var channel = connection.CreateModel();
    channel.ExchangeDeclare(
           exchange: Exchange,
           type: ExchangeType.Direct);

    var queueName = channel.QueueDeclare().QueueName;

    channel.QueueBind(
        queue: queueName,
        exchange: Exchange,
        routingKey: RoutingKeyError);

    channel.QueueBind(
        queue: queueName,
        exchange: Exchange,
        routingKey: RoutingKeyWarn);

    channel.QueueBind(
        queue: queueName,
        exchange: Exchange,
        routingKey: RoutingKeyInfo);

    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (model, ea) =>
    {
        var body = ea.Body;
        var result = ProcessData(body.ToArray());

        Console.WriteLine($"C[{id}]: message received from the exchange '{Exchange}': {result}");
    };

    channel.BasicConsume(
        queue: queueName,
        autoAck: true,
        consumer: consumer);

    Console.WriteLine($"C[{id}] - all: subscribed to the '{queueName}'");
    Console.ReadLine();
}

static string ProcessData(byte[] body)
{
    // Emulate a long-term operation
    Thread.Sleep(Random.Shared.Next(100, 300));

    var message = Encoding.UTF8.GetString(body);
    return message;
}