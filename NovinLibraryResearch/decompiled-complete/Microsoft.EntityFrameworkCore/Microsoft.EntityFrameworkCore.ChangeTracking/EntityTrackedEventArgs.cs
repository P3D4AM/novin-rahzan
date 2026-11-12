using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class EntityTrackedEventArgs : EntityEntryEventArgs
{
	public virtual bool FromQuery { get; }

	[EntityFrameworkInternal]
	public EntityTrackedEventArgs([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, bool fromQuery)
		: base(internalEntityEntry)
	{
		FromQuery = fromQuery;
	}
}
