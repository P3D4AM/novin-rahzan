using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class PropertyEventData : EventData
{
	public virtual IProperty Property { get; }

	public PropertyEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IProperty property)
		: base(eventDefinition, messageGenerator)
	{
		Property = property;
	}
}
