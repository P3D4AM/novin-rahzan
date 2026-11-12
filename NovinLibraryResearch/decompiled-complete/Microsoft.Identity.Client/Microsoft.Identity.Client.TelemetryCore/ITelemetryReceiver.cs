using System.Collections.Generic;

namespace Microsoft.Identity.Client.TelemetryCore;

internal interface ITelemetryReceiver
{
	void HandleTelemetryEvents(List<Dictionary<string, string>> events);
}
