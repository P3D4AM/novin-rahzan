using System;
using System.Collections.Concurrent;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public class DbSetSource : IDbSetSource
{
	private static readonly MethodInfo _genericCreateSet = typeof(DbSetSource).GetTypeInfo().GetDeclaredMethod("CreateSetFactory");

	private readonly ConcurrentDictionary<Type, Func<DbContext, object>> _cache = new ConcurrentDictionary<Type, Func<DbContext, object>>();

	public virtual object Create(DbContext context, Type type)
	{
		return CreateCore(context, type, _genericCreateSet);
	}

	private object CreateCore(DbContext context, Type type, MethodInfo createMethod)
	{
		return _cache.GetOrAdd(type, (Type t) => (Func<DbContext, object>)createMethod.MakeGenericMethod(t).Invoke(null, null))(context);
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static Func<DbContext, object> CreateSetFactory<TEntity>() where TEntity : class
	{
		return (DbContext c) => new InternalDbSet<TEntity>(c);
	}
}
