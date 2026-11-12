using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class InternalClrEntityEntry : InternalEntityEntry
{
	public override object Entity { get; }

	public InternalClrEntityEntry([JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] object entity)
		: base(stateManager, entityType)
	{
		Entity = entity;
	}
}
