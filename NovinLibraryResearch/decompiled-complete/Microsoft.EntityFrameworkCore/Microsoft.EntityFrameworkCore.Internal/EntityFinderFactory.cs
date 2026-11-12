using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Internal;

public class EntityFinderFactory : IEntityFinderFactory
{
	private readonly IEntityFinderSource _entityFinderSource;

	private readonly IStateManager _stateManager;

	private readonly IDbSetSource _setSource;

	private readonly IDbSetCache _setCache;

	public EntityFinderFactory([JetBrains.Annotations.NotNull] IEntityFinderSource entityFinderSource, [JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IDbSetSource setSource, [JetBrains.Annotations.NotNull] IDbSetCache setCache)
	{
		_entityFinderSource = entityFinderSource;
		_stateManager = stateManager;
		_setSource = setSource;
		_setCache = setCache;
	}

	public virtual IEntityFinder Create(IEntityType type)
	{
		return _entityFinderSource.Create(_stateManager, _setSource, _setCache, type);
	}
}
