using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class CollectionTypeFactory
{
	public virtual Type TryFindTypeToInstantiate([JetBrains.Annotations.NotNull] Type entityType, [JetBrains.Annotations.NotNull] Type collectionType)
	{
		Type type = collectionType.TryGetElementType(typeof(IEnumerable<>));
		if (type == null)
		{
			return null;
		}
		if (!collectionType.GetTypeInfo().IsAbstract)
		{
			ConstructorInfo declaredConstructor = collectionType.GetDeclaredConstructor(null);
			if ((object)declaredConstructor != null && declaredConstructor.IsPublic)
			{
				return collectionType;
			}
		}
		if (typeof(INotifyPropertyChanged).GetTypeInfo().IsAssignableFrom(entityType.GetTypeInfo()))
		{
			Type type2 = typeof(ObservableHashSet<>).MakeGenericType(type);
			if (collectionType.GetTypeInfo().IsAssignableFrom(type2.GetTypeInfo()))
			{
				return type2;
			}
		}
		Type type3 = typeof(HashSet<>).MakeGenericType(type);
		if (collectionType.GetTypeInfo().IsAssignableFrom(type3.GetTypeInfo()))
		{
			return type3;
		}
		Type type4 = typeof(List<>).MakeGenericType(type);
		if (!collectionType.GetTypeInfo().IsAssignableFrom(type4.GetTypeInfo()))
		{
			return null;
		}
		return type4;
	}
}
