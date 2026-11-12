using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class EntityEntryEventData : EventData
{
	public virtual EntityEntry EntityEntry { get; }

	public EntityEntryEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] EntityEntry entityEntry)
		: base(eventDefinition, messageGenerator)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityEntry, "entityEntry");
		EntityEntry = entityEntry;
	}
}
