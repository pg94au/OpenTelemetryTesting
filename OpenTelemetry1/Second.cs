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

        var activitySource = new ActivitySource("OpenTelemetry1");

        using var activity = activitySource.StartActivity(
            "Second",
            ActivityKind.Consumer,
            default(string?),
            new List<KeyValuePair<string, object?>>(),
            new[] { new ActivityLink(context.ActivityContext) });
    }
}
