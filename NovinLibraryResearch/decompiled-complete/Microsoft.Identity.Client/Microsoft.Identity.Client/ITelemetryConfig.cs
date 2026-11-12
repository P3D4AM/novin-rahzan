using System;

namespace Microsoft.Identity.Client;

public interface ITelemetryConfig
{
	TelemetryAudienceType AudienceType { get; }

	string SessionId { get; }

	Action<ITelemetryEventPayload> DispatchAction { get; }
}
