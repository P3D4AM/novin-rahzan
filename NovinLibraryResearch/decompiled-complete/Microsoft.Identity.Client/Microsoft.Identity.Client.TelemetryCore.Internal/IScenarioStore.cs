using System.Collections.Generic;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal interface IScenarioStore
{
	MatsScenario CreateScenario();

	IEnumerable<IPropertyBag> GetEventsForUpload();

	void ClearCompletedScenarios();

	void NotifyActionCompleted(string scenarioId);
}
