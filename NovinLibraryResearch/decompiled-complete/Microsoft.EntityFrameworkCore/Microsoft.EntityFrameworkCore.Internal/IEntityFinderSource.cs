using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IEntityFinderSource
{
	IEntityFinder Create([JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IDbSetSource setSource, [JetBrains.Annotations.NotNull] IDbSetCache setCache, [JetBrains.Annotations.NotNull] IEntityType type);
}
