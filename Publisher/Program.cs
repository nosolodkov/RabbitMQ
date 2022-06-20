using RabbitMQ.Client;
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

    channel.BasicAcks += (sender, e) =>
    {
        Console.Write($"P[{id}]: ACK received");
    };

    var counter = 0;
    do
    {
        // Delay between sending.
        Thread.Sleep(Random.Shared.Next(1000, 3000)); // From 1 to 3 sec

        var message = $"Message #{counter} from publisher [{id}]";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: string.Empty,
            "dev-queue",
            basicProperties: null,
            body: body);

        Console.WriteLine($"P[{id}]: message #{counter} sent.");

        counter++;
    } while (true);
}