using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class StateChangedEventData : EntityEntryEventData
{
	public virtual EntityState OldState { get; }

	public virtual EntityState NewState { get; }

	public StateChangedEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] EntityEntry entityEntry, EntityState oldState, EntityState newState)
		: base(eventDefinition, messageGenerator, entityEntry)
	{
		OldState = oldState;
		NewState = newState;
	}
}
