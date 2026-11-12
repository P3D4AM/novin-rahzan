using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class InternalEntityEntryFactory : IInternalEntityEntryFactory
{
	public virtual InternalEntityEntry Create(IStateManager stateManager, IEntityType entityType, object entity)
	{
		return NewInternalEntityEntry(stateManager, entityType, entity);
	}

	public virtual InternalEntityEntry Create(IStateManager stateManager, IEntityType entityType, object entity, in ValueBuffer valueBuffer)
	{
		return NewInternalEntityEntry(stateManager, entityType, entity, in valueBuffer);
	}

	private static InternalEntityEntry NewInternalEntityEntry(IStateManager stateManager, IEntityType entityType, object entity)
	{
		if (!entityType.HasClrType())
		{
			return new InternalShadowEntityEntry(stateManager, entityType);
		}
		if (entityType.ShadowPropertyCount() <= 0)
		{
			return new InternalClrEntityEntry(stateManager, entityType, entity);
		}
		return new InternalMixedEntityEntry(stateManager, entityType, entity);
	}

	private static InternalEntityEntry NewInternalEntityEntry(IStateManager stateManager, IEntityType entityType, object entity, in ValueBuffer valueBuffer)
	{
		if (entityType.HasClrType())
		{
			if (entityType.ShadowPropertyCount() <= 0)
			{
				return new InternalClrEntityEntry(stateManager, entityType, entity);
			}
			return new InternalMixedEntityEntry(stateManager, entityType, entity, in valueBuffer);
		}
		return new InternalShadowEntityEntry(stateManager, entityType, in valueBuffer);
	}
}
