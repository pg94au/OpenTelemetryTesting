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

        using var activity = activitySource.StartActivity(
            "First",
            ActivityKind.Producer,
            default(string?));

        activity?.SetTag("foo", "bar");

        Baggage.SetBaggage("user", "bob");

        activity?.AddEvent(new ActivityEvent("boom"));

        SomeMethod(activitySource);

        var x = Baggage.Current;

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

    private void SomeMethod(ActivitySource activitySource)
    {
        using var activity = activitySource.StartActivity("SomeMethod", ActivityKind.Internal);

        //using var activity = activitySource.StartActivity("Blah", ActivityKind.Internal, Activity.Current?.Context ?? default);

        //using var activity = activitySource.StartActivity("SomeMethod", ActivityKind.Internal, default(string?));

        activity?.SetTag("aaa", "bbb");
    }
}
