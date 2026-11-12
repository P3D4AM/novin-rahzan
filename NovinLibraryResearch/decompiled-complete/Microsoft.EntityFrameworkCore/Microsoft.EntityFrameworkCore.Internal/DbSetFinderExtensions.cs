using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public static class DbSetFinderExtensions
{
	public static IDictionary<Type, DbSetProperty> CreateClrTypeDbSetMapping([JetBrains.Annotations.NotNull] this IDbSetFinder setFinder, [JetBrains.Annotations.NotNull] Type contextType)
	{
		Dictionary<Type, DbSetProperty> dictionary = new Dictionary<Type, DbSetProperty>();
		HashSet<Type> hashSet = new HashSet<Type>();
		foreach (DbSetProperty item in setFinder.FindSets(contextType))
		{
			if (!hashSet.Contains(item.ClrType))
			{
				hashSet.Add(item.ClrType);
				dictionary.Add(item.ClrType, item);
			}
			else
			{
				dictionary.Remove(item.ClrType);
			}
		}
		return dictionary;
	}
}
