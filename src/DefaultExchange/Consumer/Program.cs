using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var id = args.FirstOrDefault("");
var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = factory.CreateConnection())
{
    using var channel = connection.CreateModel();
    channel.QueueDeclare(
        queue: "dev-queue",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);

    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (model, ea) =>
    {
        var body = ea.Body;
        var result = ProcessData(body.ToArray());

        Console.WriteLine($"C[{id}]: message received: {result}");
        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    };

    channel.BasicConsume(
        queue: "dev-queue",
        autoAck: false,
        consumer: consumer);

    Console.WriteLine($"C[{id}]: subscribed to the 'dev-queue'");
    Console.ReadLine();
}

string ProcessData(byte[] body)
{
    // Emulate a long-term operation
    Thread.Sleep(Random.Shared.Next(1000, 15000)); // From 1 to 15 sec

    var message = Encoding.UTF8.GetString(body);
    return message;
}