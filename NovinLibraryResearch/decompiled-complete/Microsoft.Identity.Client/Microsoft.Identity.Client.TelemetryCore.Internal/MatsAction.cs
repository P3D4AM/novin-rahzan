namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class MatsAction
{
	public string ActionId { get; }

	public MatsScenario Scenario { get; }

	public string CorrelationId { get; }

	public MatsAction(string actionId, MatsScenario scenario, string correlationId)
	{
		ActionId = actionId;
		Scenario = scenario;
		CorrelationId = correlationId;
	}
}
