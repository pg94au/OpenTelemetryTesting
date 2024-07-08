using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace OpenTelemetry1;

public class First
{
    public IDictionary<string, string> DoSomething()
    {
        var messageAttributes = new Dictionary<string, string>();

        var activitySource = new ActivitySource("OpenTelemetry1");

        var activity = activitySource.StartActivity(ActivityKind.Producer);

        if (activity != null)
        {
            Propagators.DefaultTextMapPropagator.Inject(
                new PropagationContext(activity.Context, Baggage.Current),
                messageAttributes,
                (attributes, key, value) => messageAttributes[key] = value
            );
        }

        return messageAttributes;
    }
}
