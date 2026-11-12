using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.TelemetryCore;

internal interface IHttpTelemetryManager
{
	void RecordStoppedEvent(ApiEvent apiEvent);

	string GetCurrentRequestHeader(ApiEvent currentApiEvent);

	string GetLastRequestHeader();

	void ResetPreviousUnsentData();
}
