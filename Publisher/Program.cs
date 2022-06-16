using RabbitMQ.Client;
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

    var counter = 0;
    do
    {
        // Delay between sending.
        Thread.Sleep(Random.Shared.Next(1000, 3000));

        var message = $"Message #{counter} from publisher";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: string.Empty,
            "dev-queue",
            basicProperties: null,
            body: body);

        Console.WriteLine($"The message #{counter} was sent to the default exchange.");

        counter++;
    } while (true);
}