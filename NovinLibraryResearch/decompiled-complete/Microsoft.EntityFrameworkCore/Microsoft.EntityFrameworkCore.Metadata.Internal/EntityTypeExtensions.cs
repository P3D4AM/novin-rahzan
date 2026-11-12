using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class EntityTypeExtensions
{
	public static MemberInfo GetNavigationMemberInfo([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] string navigationName)
	{
		MemberInfo? memberInfo = entityType.ClrType.GetMembersInHierarchy(navigationName).FirstOrDefault();
		if (memberInfo == null)
		{
			throw new InvalidOperationException(CoreStrings.NoClrNavigation(navigationName, entityType.DisplayName()));
		}
		return memberInfo;
	}

	public static IEnumerable<IEntityType> GetAllBaseTypes([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.GetAllBaseTypesAscending().Reverse();
	}

	public static IEnumerable<IEntityType> GetAllBaseTypesAscending([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.GetAllBaseTypesInclusiveAscending().Skip(1);
	}

	public static IForeignKey FindDeclaredOwnership([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return ((EntityType)entityType).FindDeclaredOwnership();
	}

	public static IEntityType FindInDefinitionPath([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] Type targetType)
	{
		for (IEntityType entityType2 = entityType; entityType2 != null; entityType2 = entityType2.DefiningEntityType)
		{
			if (entityType2.ClrType == targetType)
			{
				return entityType2;
			}
		}
		return null;
	}

	public static EntityType FindInDefinitionPath([JetBrains.Annotations.NotNull] this EntityType entityType, [JetBrains.Annotations.NotNull] Type targetType)
	{
		return (EntityType)((IEntityType)entityType).FindInDefinitionPath(targetType);
	}

	public static IEntityType FindInDefinitionPath([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] string targetTypeName)
	{
		for (IEntityType entityType2 = entityType; entityType2 != null; entityType2 = entityType2.DefiningEntityType)
		{
			if (entityType2.Name == targetTypeName)
			{
				return entityType2;
			}
		}
		return null;
	}

	public static EntityType FindInDefinitionPath([JetBrains.Annotations.NotNull] this EntityType entityType, [JetBrains.Annotations.NotNull] string targetTypeName)
	{
		return (EntityType)((IEntityType)entityType).FindInDefinitionPath(targetTypeName);
	}

	public static bool IsInDefinitionPath([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] Type targetType)
	{
		return entityType.FindInDefinitionPath(targetType) != null;
	}

	public static bool IsInDefinitionPath([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] string targetTypeName)
	{
		return entityType.FindInDefinitionPath(targetTypeName) != null;
	}

	public static IEntityType FindInOwnershipPath([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] Type targetType)
	{
		IEntityType entityType2 = entityType;
		do
		{
			IForeignKey foreignKey = entityType2.FindOwnership();
			if (foreignKey == null)
			{
				return null;
			}
			entityType2 = foreignKey.PrincipalEntityType;
		}
		while (!entityType2.ClrType.IsAssignableFrom(targetType));
		return entityType2;
	}

	public static bool IsInOwnershipPath([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] Type targetType)
	{
		return entityType.FindInOwnershipPath(targetType) != null;
	}

	public static bool IsInOwnershipPath([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] IEntityType targetType)
	{
		IEntityType entityType2 = entityType;
		do
		{
			IForeignKey foreignKey = entityType2.FindOwnership();
			if (foreignKey == null)
			{
				return false;
			}
			entityType2 = foreignKey.PrincipalEntityType;
		}
		while (!entityType2.IsAssignableFrom(targetType));
		return true;
	}

	public static bool UseEagerSnapshots([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		ChangeTrackingStrategy changeTrackingStrategy = entityType.GetChangeTrackingStrategy();
		if (changeTrackingStrategy != 0)
		{
			return changeTrackingStrategy == ChangeTrackingStrategy.ChangedNotifications;
		}
		return true;
	}

	public static int StoreGeneratedCount([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.GetCounts().StoreGeneratedCount;
	}

	public static int RelationshipPropertyCount([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.GetCounts().RelationshipCount;
	}

	public static int OriginalValueCount([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.GetCounts().OriginalValueCount;
	}

	public static int ShadowPropertyCount([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.GetCounts().ShadowCount;
	}

	public static int NavigationCount([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.GetCounts().NavigationCount;
	}

	public static int PropertyCount([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.GetCounts().PropertyCount;
	}

	public static PropertyCounts GetCounts([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return ((EntityType)entityType).Counts;
	}

	public static PropertyCounts CalculateCounts([JetBrains.Annotations.NotNull] this EntityType entityType)
	{
		int propertyCount = 0;
		int navigationCount = 0;
		int originalValueCount = 0;
		int shadowCount = 0;
		int relationshipCount = 0;
		int storeGeneratedCount = 0;
		PropertyCounts propertyCounts = entityType.BaseType?.GetCounts();
		if (propertyCounts != null)
		{
			propertyCount = propertyCounts.PropertyCount;
			navigationCount = propertyCounts.NavigationCount;
			originalValueCount = propertyCounts.OriginalValueCount;
			shadowCount = propertyCounts.ShadowCount;
			relationshipCount = propertyCounts.RelationshipCount;
			storeGeneratedCount = propertyCounts.StoreGeneratedCount;
		}
		foreach (Property declaredProperty in entityType.GetDeclaredProperties())
		{
			PropertyIndexes propertyIndexes = new PropertyIndexes(propertyCount++, declaredProperty.RequiresOriginalValue() ? originalValueCount++ : (-1), declaredProperty.IsShadowProperty() ? shadowCount++ : (-1), declaredProperty.IsKeyOrForeignKey() ? relationshipCount++ : (-1), declaredProperty.MayBeStoreGenerated() ? storeGeneratedCount++ : (-1));
			declaredProperty.PropertyIndexes = propertyIndexes;
		}
		bool flag = entityType.GetChangeTrackingStrategy() != ChangeTrackingStrategy.Snapshot;
		foreach (Navigation declaredNavigation in entityType.GetDeclaredNavigations())
		{
			PropertyIndexes propertyIndexes2 = new PropertyIndexes(navigationCount++, -1, declaredNavigation.IsShadowProperty() ? shadowCount++ : (-1), (declaredNavigation.IsCollection() && flag) ? (-1) : relationshipCount++, -1);
			declaredNavigation.PropertyIndexes = propertyIndexes2;
		}
		foreach (ServiceProperty declaredServiceProperty in entityType.GetDeclaredServiceProperties())
		{
			PropertyIndexes propertyIndexes3 = new PropertyIndexes(-1, -1, -1, -1, -1);
			declaredServiceProperty.PropertyIndexes = propertyIndexes3;
		}
		return new PropertyCounts(propertyCount, navigationCount, originalValueCount, shadowCount, relationshipCount, storeGeneratedCount);
	}

	public static Func<ISnapshot> GetEmptyShadowValuesFactory([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.AsEntityType("GetEmptyShadowValuesFactory").EmptyShadowValuesFactory;
	}

	public static IEnumerable<IEntityType> GetTypesInHierarchy([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.GetAllBaseTypes().Concat(entityType.GetDerivedTypesInclusive());
	}

	public static bool IsSameHierarchy([JetBrains.Annotations.NotNull] this IEntityType firstEntityType, [JetBrains.Annotations.NotNull] IEntityType secondEntityType)
	{
		if (!firstEntityType.IsAssignableFrom(secondEntityType))
		{
			return secondEntityType.IsAssignableFrom(firstEntityType);
		}
		return true;
	}

	public static EntityType LeastDerivedType([JetBrains.Annotations.NotNull] this EntityType entityType, [JetBrains.Annotations.NotNull] EntityType otherEntityType)
	{
		return (EntityType)Microsoft.EntityFrameworkCore.EntityTypeExtensions.LeastDerivedType(entityType, otherEntityType);
	}

	public static IKey FindDeclaredPrimaryKey([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		if (entityType.BaseType != null)
		{
			return null;
		}
		return entityType.FindPrimaryKey();
	}

	public static IEnumerable<INavigation> FindDerivedNavigations([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] string navigationName)
	{
		return entityType.GetDerivedTypes().SelectMany((IEntityType et) => from navigation in et.GetDeclaredNavigations()
			where navigationName == navigation.Name
			select navigation);
	}

	public static IEnumerable<Navigation> GetDerivedNavigations([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.AsEntityType("GetDerivedNavigations").GetDerivedNavigations();
	}

	public static IEnumerable<IPropertyBase> GetPropertiesAndNavigations([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return ((IEnumerable<IPropertyBase>)entityType.GetProperties()).Concat((IEnumerable<IPropertyBase>)entityType.GetNavigations());
	}

	public static IEnumerable<IPropertyBase> GetNotificationProperties([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.CanBeNull] string propertyName)
	{
		if (string.IsNullOrEmpty(propertyName))
		{
			foreach (IProperty item in from p in entityType.GetProperties()
				where p.GetAfterSaveBehavior() == PropertySaveBehavior.Save
				select p)
			{
				yield return item;
			}
			foreach (INavigation navigation in entityType.GetNavigations())
			{
				yield return navigation;
			}
		}
		else
		{
			IPropertyBase propertyBase = entityType.FindProperty(propertyName);
			IPropertyBase propertyBase2 = propertyBase ?? entityType.FindNavigation(propertyName);
			if (propertyBase2 != null)
			{
				yield return propertyBase2;
			}
		}
	}

	public static string ToDebugString([JetBrains.Annotations.NotNull] this IEntityType entityType, bool singleLine = true, [JetBrains.Annotations.NotNull] string indent = "")
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(indent).Append("EntityType: ").Append(entityType.DisplayName());
		if (entityType.BaseType != null)
		{
			stringBuilder.Append(" Base: ").Append(entityType.BaseType.DisplayName());
		}
		if (entityType.IsAbstract())
		{
			stringBuilder.Append(" Abstract");
		}
		if (entityType.FindPrimaryKey() == null)
		{
			stringBuilder.Append(" Keyless");
		}
		if (entityType.GetChangeTrackingStrategy() != 0)
		{
			stringBuilder.Append(" ChangeTrackingStrategy.").Append(entityType.GetChangeTrackingStrategy());
		}
		if (!singleLine)
		{
			List<IProperty> list = entityType.GetDeclaredProperties().ToList();
			if (list.Count != 0)
			{
				stringBuilder.AppendLine().Append(indent).Append("  Properties: ");
				foreach (IProperty item in list)
				{
					stringBuilder.AppendLine().Append(item.ToDebugString(singleLine: false, includeIndexes: false, indent + "    "));
				}
			}
			List<INavigation> list2 = entityType.GetDeclaredNavigations().ToList();
			if (list2.Count != 0)
			{
				stringBuilder.AppendLine().Append(indent).Append("  Navigations: ");
				foreach (INavigation item2 in list2)
				{
					stringBuilder.AppendLine().Append(item2.ToDebugString(singleLine: false, includeIndexes: false, indent + "    ", detailed: true));
				}
			}
			List<IServiceProperty> list3 = entityType.GetDeclaredServiceProperties().ToList();
			if (list3.Count != 0)
			{
				stringBuilder.AppendLine().Append(indent).Append("  Service properties: ");
				foreach (IServiceProperty item3 in list3)
				{
					stringBuilder.AppendLine().Append(item3.ToDebugString(singleLine: false, indent + "    "));
				}
			}
			List<IKey> list4 = entityType.GetDeclaredKeys().ToList();
			if (list4.Count != 0)
			{
				stringBuilder.AppendLine().Append(indent).Append("  Keys: ");
				foreach (IKey item4 in list4)
				{
					stringBuilder.AppendLine().Append(item4.ToDebugString(singleLine: false, indent + "    "));
				}
			}
			List<IForeignKey> list5 = entityType.GetDeclaredForeignKeys().ToList();
			if (list5.Count != 0)
			{
				stringBuilder.AppendLine().Append(indent).Append("  Foreign keys: ");
				foreach (IForeignKey item5 in list5)
				{
					stringBuilder.AppendLine().Append(item5.ToDebugString(singleLine: false, indent + "    "));
				}
			}
			stringBuilder.Append(entityType.AnnotationsToDebugString(indent + "  "));
		}
		return stringBuilder.ToString();
	}

	public static IProperty GetProperty([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		IProperty property = entityType.FindProperty(name);
		if (property == null)
		{
			if (entityType.FindNavigation(name) != null)
			{
				throw new InvalidOperationException(CoreStrings.PropertyIsNavigation(name, entityType.DisplayName(), "Property", "Reference", "Collection"));
			}
			throw new InvalidOperationException(CoreStrings.PropertyNotFound(name, entityType.DisplayName()));
		}
		return property;
	}

	public static IProperty CheckPropertyBelongsToType([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		if (!property.DeclaringEntityType.IsAssignableFrom(entityType))
		{
			throw new InvalidOperationException(CoreStrings.PropertyDoesNotBelong(property.Name, property.DeclaringEntityType.DisplayName(), entityType.DisplayName()));
		}
		return property;
	}

	public static EntityType AsEntityType([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull][CallerMemberName] string methodName = "")
	{
		return MetadataExtensions.AsConcreteMetadataType<IEntityType, EntityType>(entityType, methodName);
	}
}
