using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Internal;

public class DbSetFinder : IDbSetFinder
{
	private readonly ConcurrentDictionary<Type, IReadOnlyList<DbSetProperty>> _cache = new ConcurrentDictionary<Type, IReadOnlyList<DbSetProperty>>();

	public virtual IReadOnlyList<DbSetProperty> FindSets(Type contextType)
	{
		return _cache.GetOrAdd(contextType, FindSetsNonCached);
	}

	private static DbSetProperty[] FindSetsNonCached(Type contextType)
	{
		ClrPropertySetterFactory factory = new ClrPropertySetterFactory();
		return (from p in contextType.GetRuntimeProperties()
			where !p.IsStatic() && !p.GetIndexParameters().Any() && p.DeclaringType != typeof(DbContext) && p.PropertyType.GetTypeInfo().IsGenericType && (p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) || p.PropertyType.GetGenericTypeDefinition() == typeof(DbQuery<>))
			orderby p.Name
			select new DbSetProperty(p.Name, p.PropertyType.GetTypeInfo().GenericTypeArguments.Single(), (p.SetMethod == null) ? null : factory.Create(p), p.PropertyType.GetGenericTypeDefinition() == typeof(DbQuery<>))).ToArray();
	}
}
