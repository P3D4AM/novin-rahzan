using System;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class ScenarioHolder
{
	public IPropertyBag PropertyBag { get; }

	public bool ShouldUpload { get; set; }

	public DateTime StartTime { get; }

	public ScenarioHolder(IPropertyBag propertyBag)
	{
		PropertyBag = propertyBag;
		StartTime = DateTime.UtcNow;
		ShouldUpload = false;
	}
}
