using System.Collections.Generic;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal interface IActionStore
{
	MatsAction StartMsalAction(MatsScenario scenario, string correlationId, IEnumerable<string> scopes);

	void ProcessMsalTelemetryBlob(IDictionary<string, string> blob);

	void EndMsalAction(MatsAction action, AuthOutcome outcome, ErrorSource errorSource, string error, string errorDescription);

	IEnumerable<IPropertyBag> GetEventsForUpload();
}
