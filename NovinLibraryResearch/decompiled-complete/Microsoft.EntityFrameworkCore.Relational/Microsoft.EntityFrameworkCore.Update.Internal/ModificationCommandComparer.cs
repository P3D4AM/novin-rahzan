using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public class ModificationCommandComparer : IComparer<ModificationCommand>
{
	private readonly ConcurrentDictionary<Type, Func<object, object, int>> _comparers = new ConcurrentDictionary<Type, Func<object, object, int>>();

	private static readonly MethodInfo _compareMethod = typeof(ModificationCommandComparer).GetTypeInfo().GetDeclaredMethod("CompareValue");

	private static readonly MethodInfo _structuralCompareMethod = typeof(ModificationCommandComparer).GetTypeInfo().GetDeclaredMethod("CompareStructureValue");

	public virtual int Compare(ModificationCommand x, ModificationCommand y)
	{
		int result = 0;
		if (x == y)
		{
			return result;
		}
		if (x == null)
		{
			return -1;
		}
		if (y == null)
		{
			return 1;
		}
		result = StringComparer.Ordinal.Compare(x.Schema, y.Schema);
		if (result != 0)
		{
			return result;
		}
		result = StringComparer.Ordinal.Compare(x.TableName, y.TableName);
		if (result != 0)
		{
			return result;
		}
		EntityState entityState = x.EntityState;
		result = entityState - y.EntityState;
		if (result != 0)
		{
			return result;
		}
		if (entityState != EntityState.Added && x.Entries.Count > 0 && y.Entries.Count > 0)
		{
			IUpdateEntry updateEntry = x.Entries[0];
			IUpdateEntry updateEntry2 = y.Entries[0];
			IEntityType entityType = updateEntry.EntityType;
			IEntityType entityType2 = updateEntry2.EntityType;
			IKey key = updateEntry.EntityType.FindPrimaryKey();
			IKey key2 = key;
			if (entityType != entityType2)
			{
				result = StringComparer.Ordinal.Compare(entityType.Name, entityType2.Name);
				if (result != 0)
				{
					return result;
				}
				result = StringComparer.Ordinal.Compare(entityType.DefiningNavigationName, entityType2.DefiningNavigationName);
				if (result != 0)
				{
					return result;
				}
				key2 = updateEntry2.EntityType.FindPrimaryKey();
			}
			for (int i = 0; i < key.Properties.Count; i++)
			{
				IProperty property = key.Properties[i];
				result = GetComparer(property.ClrType)(updateEntry.GetCurrentValue(property), updateEntry2.GetCurrentValue(key2.Properties[i]));
				if (result != 0)
				{
					return result;
				}
			}
		}
		return result;
	}

	protected virtual Func<object, object, int> GetComparer([JetBrains.Annotations.NotNull] Type type)
	{
		return _comparers.GetOrAdd(type, delegate(Type t)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "x");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object), "y");
			return Expression.Lambda<Func<object, object, int>>(Expression.Call(null, (typeof(IStructuralComparable).IsAssignableFrom(type) ? _structuralCompareMethod : _compareMethod).MakeGenericMethod(t), Expression.Convert(parameterExpression, t), Expression.Convert(parameterExpression2, t)), new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
		});
	}

	private static int CompareValue<T>(T x, T y)
	{
		return Comparer<T>.Default.Compare(x, y);
	}

	private static int CompareStructureValue<T>(T x, T y)
	{
		if (x is Array array && y is Array array2 && array.Length != array2.Length)
		{
			return array.Length - array2.Length;
		}
		return StructuralComparisons.StructuralComparer.Compare(x, y);
	}
}
