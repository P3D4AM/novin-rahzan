namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal interface IEventFilter
{
	bool ShouldAggregateAction(PropertyBagContents contents);

	bool IsSilentAction(PropertyBagContents contents);

	void SetShouldAggregate(bool shouldAggregate);
}
