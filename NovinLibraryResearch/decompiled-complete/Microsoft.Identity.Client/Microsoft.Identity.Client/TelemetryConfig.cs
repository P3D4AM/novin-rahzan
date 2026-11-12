using System;
using System.Collections.Generic;

namespace Microsoft.Identity.Client;

internal class TelemetryConfig : ITelemetryConfig
{
	public TelemetryAudienceType AudienceType { get; set; }

	public string SessionId { get; set; }

	public Action<ITelemetryEventPayload> DispatchAction { get; set; }

	public IEnumerable<string> AllowedScopes { get; set; }
}
