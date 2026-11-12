using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class EntityEntryEventArgs : EventArgs
{
	private readonly InternalEntityEntry _internalEntityEntry;

	private EntityEntry _entry;

	public virtual EntityEntry Entry => _entry ?? (_entry = new EntityEntry(_internalEntityEntry));

	[EntityFrameworkInternal]
	public EntityEntryEventArgs([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry)
	{
		_internalEntityEntry = internalEntityEntry;
	}
}
