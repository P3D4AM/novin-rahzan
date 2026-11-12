using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IInternalEntityEntryFactory
{
	InternalEntityEntry Create([JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.CanBeNull] object entity);

	InternalEntityEntry Create([JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.CanBeNull] object entity, in ValueBuffer valueBuffer);
}
