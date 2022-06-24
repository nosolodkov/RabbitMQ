
using ServiceLauncher;
using System.Diagnostics;

var pubs = ProcessManager.PublihersCount;
var cons = ProcessManager.ConsumersCount;

Console.Write("Publishers: {0}. Put another value if needed > ", pubs);
var input = Console.ReadLine();
if (byte.TryParse(input, out byte pubsInput))
{
    pubs = pubsInput;
}
else
{
    Warn(input);
}

Console.Write("Consumers: {0}. Put another value if needed > ", cons);
input = Console.ReadLine();
if (byte.TryParse(input, out byte consInput))
{
    cons = consInput;
}
else
{
    Warn(input);
}

for (int i = 0; i < cons; i++)
{
    Process.Start(ProcessManager.ConsumerExe, Guid.NewGuid().ToString());
}

Console.WriteLine("Consumers started - {0}", cons);

for (int i = 0; i < pubs; i++)
{
    Process.Start(ProcessManager.PubliherExe, Guid.NewGuid().ToString());
}

Console.WriteLine("Publishers started - {0}", pubs);

static void Warn(string? input)
{
    if (!string.IsNullOrEmpty(input))
    {
        Console.WriteLine("Wrong value entered. Default value will be used.");
    }
}