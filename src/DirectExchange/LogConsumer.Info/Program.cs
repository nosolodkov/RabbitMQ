using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

const string Exchange = "direct_logs";
const string RoutingKey = "info";

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
        routingKey: RoutingKey);

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

    Console.WriteLine($"C[{id}] - {RoutingKey}: subscribed to the '{queueName}'");
    Console.ReadLine();
}

static string ProcessData(byte[] body)
{
    // Emulate a long-term operation
    Thread.Sleep(Random.Shared.Next(1000, 3000)); // From 1 to 3 sec

    var message = Encoding.UTF8.GetString(body);
    return message;
}