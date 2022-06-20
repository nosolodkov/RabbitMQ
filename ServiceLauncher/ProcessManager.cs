namespace ServiceLauncher;

internal static class ProcessManager
{
    const string Configuration =
#if DEBUG
    "Debug\\net6.0\\";
#else
    "Release\\net6.0\\";
#endif

    public const int PublihersCount = 2;
    public const int ConsumersCount = 6;

    public static readonly string PubliherExe = Path.Combine(
        Environment.CurrentDirectory,
        "..\\..\\..\\Publisher\\",
        Configuration,
        "Publisher.exe");

    public static readonly string ConsumerExe = Path.Combine(
        Environment.CurrentDirectory,
        "..\\..\\..\\Consumer\\",
        Configuration,
        "Consumer.exe");
}
