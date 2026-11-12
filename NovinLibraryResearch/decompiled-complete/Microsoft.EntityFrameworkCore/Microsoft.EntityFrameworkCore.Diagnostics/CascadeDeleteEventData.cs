using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class CascadeDeleteEventData : EntityEntryEventData
{
	public virtual EntityState State { get; }

	public virtual EntityEntry ParentEntityEntry { get; }

	public CascadeDeleteEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] EntityEntry entityEntry, [JetBrains.Annotations.NotNull] EntityEntry parentEntry, EntityState state)
		: base(eventDefinition, messageGenerator, entityEntry)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parentEntry, "parentEntry");
		ParentEntityEntry = parentEntry;
		State = state;
	}
}
