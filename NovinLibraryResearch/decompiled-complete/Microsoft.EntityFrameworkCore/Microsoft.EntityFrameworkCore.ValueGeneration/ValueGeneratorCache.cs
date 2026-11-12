using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public class ValueGeneratorCache : IValueGeneratorCache
{
	private readonly struct CacheKey
	{
		public IProperty Property { get; }

		public IEntityType EntityType { get; }

		public Func<IProperty, IEntityType, ValueGenerator> Factory { get; }

		public CacheKey(IProperty property, IEntityType entityType, Func<IProperty, IEntityType, ValueGenerator> factory)
		{
			Property = property;
			EntityType = entityType;
			Factory = factory;
		}

		private bool Equals(CacheKey other)
		{
			if (Property.Equals(other.Property))
			{
				return EntityType.Equals(other.EntityType);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				if (obj is CacheKey other)
				{
					return Equals(other);
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Property, EntityType);
		}
	}

	private readonly ConcurrentDictionary<CacheKey, ValueGenerator> _cache = new ConcurrentDictionary<CacheKey, ValueGenerator>();

	public ValueGeneratorCache([JetBrains.Annotations.NotNull] ValueGeneratorCacheDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
	}

	public virtual ValueGenerator GetOrAdd(IProperty property, IEntityType entityType, Func<IProperty, IEntityType, ValueGenerator> factory)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(factory, "factory");
		return _cache.GetOrAdd(new CacheKey(property, entityType, factory), (CacheKey ck) => ck.Factory(ck.Property, ck.EntityType));
	}
}
