namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class ActionArtifacts
{
	public MatsAction Action { get; }

	public ActionPropertyBag PropertyBag { get; }

	public ActionArtifacts(MatsAction action, ActionPropertyBag propertyBag)
	{
		Action = action;
		PropertyBag = propertyBag;
	}
}
