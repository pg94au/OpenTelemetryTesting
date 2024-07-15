using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTelemetry1;
public class Unrelated
{
    public void DoSomething()
    {
        var activitySource = new ActivitySource("OpenTelemetry1");

        using var activity = activitySource.StartActivity(
            "Unrelated",
            ActivityKind.Internal,
            default(string?));

        activity?.SetTag("abc", "123");
    }
}
