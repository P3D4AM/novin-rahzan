namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class MatsScenario
{
	public string ScenarioId { get; }

	public int ActionCount { get; }

	public MatsScenario(string scenarioId, int actionCount)
	{
		ScenarioId = scenarioId;
		ActionCount = actionCount;
	}
}
