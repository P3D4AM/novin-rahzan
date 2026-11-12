using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class ForeignKeyEventData : EventData
{
	public virtual IForeignKey ForeignKey { get; }

	public ForeignKeyEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IForeignKey foreignKey)
		: base(eventDefinition, messageGenerator)
	{
		ForeignKey = foreignKey;
	}
}
