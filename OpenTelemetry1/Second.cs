using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTelemetry.Context.Propagation;

namespace OpenTelemetry1;

public class Second
{
    public void DoSomething(IDictionary<string, string> messageAttributes)
    {
        var context = Propagators.DefaultTextMapPropagator.Extract(
            default,
            messageAttributes,
            (attributes, key) => attributes.TryGetValue(key, out var value) ? new[] { value } : Array.Empty<string>());

        Console.WriteLine($"Second: TraceId={context.ActivityContext.TraceId}");
        Console.WriteLine($"Second: SpanId={context.ActivityContext.SpanId}");
        foreach (var entry in context.Baggage)
        {
            Console.WriteLine($"Second: Baggage entry {entry.Key}={entry.Value}");
        }

        var activitySource = new ActivitySource("OpenTelemetry1");


        var activity1 = activitySource.StartActivity("Second.1", ActivityKind.Consumer, context.ActivityContext);
        activity1?.SetTag("1", "2");
        activity1?.Dispose();


        using var activity = activitySource.StartActivity(
            "Second",
            ActivityKind.Consumer,
            default(string?),
            new List<KeyValuePair<string, object?>>(),
            new[] { new ActivityLink(context.ActivityContext) });

        activity?.SetTag("x", "y");
    }
}
