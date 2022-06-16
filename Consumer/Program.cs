using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

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

    consumer.Received += Consumer_Received;

    channel.BasicConsume(
        queue: "dev-queue",
        autoAck: true,
        consumer: consumer);

    Console.WriteLine("Consumer subscribed to the dev-queue");
    Console.ReadLine();
}

void Consumer_Received(object? sender, BasicDeliverEventArgs e)
{
    var body = e.Body;
    var message = Encoding.UTF8.GetString(body.ToArray());
    Console.WriteLine("Received message: {0}", message);
}