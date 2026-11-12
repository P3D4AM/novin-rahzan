using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class EntityStateChangedEventArgs : EntityEntryEventArgs
{
	public virtual EntityState OldState { get; }

	public virtual EntityState NewState { get; }

	[EntityFrameworkInternal]
	public EntityStateChangedEventArgs([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, EntityState oldState, EntityState newState)
		: base(internalEntityEntry)
	{
		OldState = oldState;
		NewState = newState;
	}
}
