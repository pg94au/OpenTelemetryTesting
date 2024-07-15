using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry1;

namespace TestApp;

internal class Program
{
    static void Main(string[] args)
    {
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource("OpenTelemetry1", "OpenTelemetry1")
            .ConfigureResource(res => res.AddService("OpenTelemetry1"))
            .AddConsoleExporter()
            .AddOtlpExporter(options =>
            {
                options.Protocol = OtlpExportProtocol.HttpProtobuf;
                options.Endpoint = new Uri("http://localhost:4318");
            })
            .Build();

        var first = new First();
        var second = new Second();
        var unrelated = new Unrelated();

        var messageAttributes = first.DoSomething();

        foreach (var pair in messageAttributes)
        {
            Console.WriteLine($"Message attribute {pair.Key} = {pair.Value}");
        }

        second.DoSomething(messageAttributes);

        unrelated.DoSomething();

        tracerProvider.ForceFlush();
    }
}
