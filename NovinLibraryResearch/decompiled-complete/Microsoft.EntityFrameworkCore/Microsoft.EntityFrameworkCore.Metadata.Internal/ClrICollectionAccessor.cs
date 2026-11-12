using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ClrICollectionAccessor<TEntity, TCollection, TElement> : IClrCollectionAccessor where TEntity : class where TCollection : class, IEnumerable<TElement> where TElement : class
{
	private readonly string _propertyName;

	private readonly Func<TEntity, TCollection> _getCollection;

	private readonly Action<TEntity, TCollection> _setCollection;

	private readonly Action<TEntity, TCollection> _setCollectionForMaterialization;

	private readonly Func<TEntity, Action<TEntity, TCollection>, TCollection> _createAndSetCollection;

	private readonly Func<TCollection> _createCollection;

	public virtual Type CollectionType => typeof(TCollection);

	public ClrICollectionAccessor([JetBrains.Annotations.NotNull] string propertyName, [JetBrains.Annotations.NotNull] Func<TEntity, TCollection> getCollection, [JetBrains.Annotations.CanBeNull] Action<TEntity, TCollection> setCollection, [JetBrains.Annotations.CanBeNull] Action<TEntity, TCollection> setCollectionForMaterialization, [JetBrains.Annotations.CanBeNull] Func<TEntity, Action<TEntity, TCollection>, TCollection> createAndSetCollection, [JetBrains.Annotations.CanBeNull] Func<TCollection> createCollection)
	{
		_propertyName = propertyName;
		_getCollection = getCollection;
		_setCollection = setCollection;
		_setCollectionForMaterialization = setCollectionForMaterialization;
		_createAndSetCollection = createAndSetCollection;
		_createCollection = createCollection;
	}

	public virtual bool Add(object entity, object value, bool forMaterialization)
	{
		ICollection<TElement> orCreateCollection = GetOrCreateCollection(entity, forMaterialization);
		TElement item = (TElement)value;
		if (!Contains(orCreateCollection, value))
		{
			orCreateCollection.Add(item);
			return true;
		}
		return false;
	}

	public virtual object Create()
	{
		if (_createCollection == null)
		{
			throw new InvalidOperationException(CoreStrings.NavigationCannotCreateType(_propertyName, typeof(TEntity).ShortDisplayName(), typeof(TCollection).ShortDisplayName()));
		}
		return _createCollection();
	}

	public virtual object GetOrCreate(object entity, bool forMaterialization)
	{
		return GetOrCreateCollection(entity, forMaterialization);
	}

	private ICollection<TElement> GetOrCreateCollection(object instance, bool forMaterialization)
	{
		ICollection<TElement> collection = GetCollection(instance);
		if (collection == null)
		{
			Action<TEntity, TCollection> action = (forMaterialization ? _setCollectionForMaterialization : _setCollection);
			if (action == null)
			{
				throw new InvalidOperationException(CoreStrings.NavigationNoSetter(_propertyName, typeof(TEntity).ShortDisplayName()));
			}
			if (_createAndSetCollection == null)
			{
				throw new InvalidOperationException(CoreStrings.NavigationCannotCreateType(_propertyName, typeof(TEntity).ShortDisplayName(), typeof(TCollection).ShortDisplayName()));
			}
			collection = (ICollection<TElement>)_createAndSetCollection((TEntity)instance, action);
		}
		return collection;
	}

	private ICollection<TElement> GetCollection(object instance)
	{
		TCollection val = _getCollection((TEntity)instance);
		ICollection<TElement> collection = val as ICollection<TElement>;
		if (val != null && collection == null)
		{
			throw new InvalidOperationException(CoreStrings.NavigationBadType(_propertyName, typeof(TEntity).ShortDisplayName(), val.GetType().ShortDisplayName(), typeof(TElement).ShortDisplayName()));
		}
		return collection;
	}

	public virtual bool Contains(object entity, object value)
	{
		return Contains(GetCollection((TEntity)entity), value);
	}

	public virtual bool Remove(object entity, object value)
	{
		ICollection<TElement> collection = GetCollection((TEntity)entity);
		if (!(collection is List<TElement> list))
		{
			if (!(collection is Collection<TElement> collection2))
			{
				if (collection is SortedSet<TElement> sortedSet)
				{
					foreach (TElement item in sortedSet)
					{
						if (item == value)
						{
							sortedSet.Remove(item);
							return true;
						}
					}
					return false;
				}
				return collection?.Remove((TElement)value) ?? false;
			}
			for (int i = 0; i < collection2.Count; i++)
			{
				if (collection2[i] == value)
				{
					collection2.RemoveAt(i);
					return true;
				}
			}
			return false;
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j] == value)
			{
				list.RemoveAt(j);
				return true;
			}
		}
		return false;
	}

	private static bool Contains(ICollection<TElement> collection, object value)
	{
		if (!(collection is List<TElement> list))
		{
			if (!(collection is Collection<TElement> collection2))
			{
				if (collection is SortedSet<TElement> sortedSet)
				{
					foreach (TElement item in sortedSet)
					{
						if (item == value)
						{
							return true;
						}
					}
					return false;
				}
				return collection?.Contains((TElement)value) ?? false;
			}
			for (int i = 0; i < collection2.Count; i++)
			{
				if (collection2[i] == value)
				{
					return true;
				}
			}
			return false;
		}
		foreach (TElement item2 in list)
		{
			if (item2 == value)
			{
				return true;
			}
		}
		return false;
	}
}
