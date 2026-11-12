using System;
using System.Collections.Concurrent;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Internal;

public class EntityFinderSource : IEntityFinderSource
{
	private static readonly MethodInfo _genericCreate = typeof(EntityFinderSource).GetTypeInfo().GetDeclaredMethod("CreateConstructor");

	private readonly ConcurrentDictionary<Type, Func<IStateManager, IDbSetSource, IDbSetCache, IEntityType, IEntityFinder>> _cache = new ConcurrentDictionary<Type, Func<IStateManager, IDbSetSource, IDbSetCache, IEntityType, IEntityFinder>>();

	public virtual IEntityFinder Create(IStateManager stateManager, IDbSetSource setSource, IDbSetCache setCache, IEntityType type)
	{
		return _cache.GetOrAdd(type.ClrType, (Type t) => (Func<IStateManager, IDbSetSource, IDbSetCache, IEntityType, IEntityFinder>)_genericCreate.MakeGenericMethod(t).Invoke(null, null))(stateManager, setSource, setCache, type);
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static Func<IStateManager, IDbSetSource, IDbSetCache, IEntityType, IEntityFinder> CreateConstructor<TEntity>() where TEntity : class
	{
		return (IStateManager s, IDbSetSource src, IDbSetCache c, IEntityType t) => new EntityFinder<TEntity>(s, src, c, t);
	}
}
