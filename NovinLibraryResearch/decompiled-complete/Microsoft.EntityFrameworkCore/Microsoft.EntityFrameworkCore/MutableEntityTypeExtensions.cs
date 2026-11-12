using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class MutableEntityTypeExtensions
{
	public static IMutableEntityType GetRootType([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return (IMutableEntityType)EntityTypeExtensions.GetRootType(entityType);
	}

	[Obsolete("Use GetRootType")]
	public static IMutableEntityType RootType([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return (IMutableEntityType)EntityTypeExtensions.GetRootType(entityType);
	}

	public static IEnumerable<IMutableEntityType> GetDerivedTypes([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetDerivedTypes();
	}

	public static IEnumerable<IMutableEntityType> GetDerivedTypesInclusive([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetDerivedTypesInclusive();
	}

	public static IEnumerable<IMutableEntityType> GetDirectlyDerivedTypes([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetDirectlyDerivedTypes();
	}

	public static IEnumerable<IMutableKey> GetDeclaredKeys([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredKeys();
	}

	public static IEnumerable<IMutableProperty> GetDeclaredProperties([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredProperties();
	}

	public static IEnumerable<IMutableNavigation> GetDeclaredNavigations([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredNavigations();
	}

	public static IEnumerable<IMutableServiceProperty> GetDeclaredServiceProperties([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredServiceProperties();
	}

	public static IEnumerable<IMutableIndex> GetDeclaredIndexes([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredIndexes();
	}

	public static IMutableProperty RemoveProperty([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return ((EntityType)entityType).RemoveProperty(name);
	}

	public static IMutableKey SetPrimaryKey([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.CanBeNull] IMutableProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return entityType.SetPrimaryKey((property == null) ? null : new IMutableProperty[1] { property });
	}

	public static IMutableKey FindKey([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return entityType.FindKey(new IProperty[1] { property });
	}

	public static IMutableKey AddKey([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IMutableProperty property)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AddKey(new IMutableProperty[1] { property });
	}

	public static IMutableKey RemoveKey([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IMutableProperty> properties)
	{
		return ((EntityType)entityType).RemoveKey(properties);
	}

	public static IEnumerable<IMutableForeignKey> GetDeclaredForeignKeys([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredForeignKeys();
	}

	public static IEnumerable<IMutableForeignKey> GetDerivedForeignKeys([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetDerivedForeignKeys();
	}

	public static IEnumerable<IMutableForeignKey> FindForeignKeys([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property)
	{
		return entityType.FindForeignKeys(new IProperty[1] { property });
	}

	public static IEnumerable<IMutableForeignKey> FindForeignKeys([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		return ((EntityType)entityType).FindForeignKeys(properties);
	}

	public static IMutableForeignKey FindForeignKey([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IKey principalKey, [JetBrains.Annotations.NotNull] IEntityType principalEntityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return entityType.FindForeignKey(new IProperty[1] { property }, principalKey, principalEntityType);
	}

	public static IEnumerable<IMutableForeignKey> FindDeclaredForeignKeys([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		return ((EntityType)entityType).FindDeclaredForeignKeys(properties);
	}

	public static IEnumerable<IMutableForeignKey> GetReferencingForeignKeys([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetReferencingForeignKeys();
	}

	public static IEnumerable<IMutableForeignKey> GetDeclaredReferencingForeignKeys([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredReferencingForeignKeys();
	}

	public static IMutableForeignKey AddForeignKey([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IMutableProperty property, [JetBrains.Annotations.NotNull] IMutableKey principalKey, [JetBrains.Annotations.NotNull] IMutableEntityType principalEntityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return entityType.AddForeignKey(new IMutableProperty[1] { property }, principalKey, principalEntityType);
	}

	public static IMutableForeignKey FindOwnership([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).FindOwnership();
	}

	public static IMutableForeignKey RemoveForeignKey([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IMutableProperty> properties, [JetBrains.Annotations.NotNull] IMutableKey principalKey, [JetBrains.Annotations.NotNull] IMutableEntityType principalEntityType)
	{
		return ((EntityType)entityType).RemoveForeignKey(properties, principalKey, principalEntityType);
	}

	public static IMutableNavigation FindNavigation([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] MemberInfo memberInfo)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").FindNavigation(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memberInfo, "memberInfo").GetSimpleMemberName());
	}

	public static IMutableNavigation FindNavigation([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return ((EntityType)entityType).FindNavigation(name);
	}

	public static IMutableNavigation FindDeclaredNavigation([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return ((EntityType)entityType).FindDeclaredNavigation(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name"));
	}

	public static IMutableNavigation FindDefiningNavigation([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return (IMutableNavigation)EntityTypeExtensions.FindDefiningNavigation(entityType);
	}

	public static IEnumerable<IMutableNavigation> GetNavigations([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return ((EntityType)entityType).GetNavigations();
	}

	public static IMutableProperty FindProperty([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] PropertyInfo propertyInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyInfo, "propertyInfo");
		return entityType.FindProperty(propertyInfo.GetSimpleMemberName());
	}

	public static IReadOnlyList<IMutableProperty> FindProperties([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<string> propertyNames)
	{
		return ((EntityType)entityType).FindProperties(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyNames, "propertyNames"));
	}

	public static IMutableProperty FindDeclaredProperty([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return ((EntityType)entityType).FindDeclaredProperty(name);
	}

	public static IMutableProperty AddProperty([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] MemberInfo memberInfo)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AddProperty(memberInfo.GetSimpleMemberName(), memberInfo.GetMemberType(), memberInfo);
	}

	public static IMutableProperty AddProperty([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return ((EntityType)entityType).AddProperty(name, ConfigurationSource.Explicit);
	}

	public static IMutableProperty AddProperty([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Type propertyType)
	{
		return entityType.AddProperty(name, propertyType, null);
	}

	public static IMutableIndex FindIndex([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return entityType.FindIndex(new IProperty[1] { property });
	}

	public static IMutableIndex AddIndex([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IMutableProperty property)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AddIndex(new IMutableProperty[1] { property });
	}

	public static IMutableIndex RemoveIndex([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IMutableProperty> properties)
	{
		return ((EntityType)entityType).RemoveIndex(properties);
	}

	public static void SetChangeTrackingStrategy([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, ChangeTrackingStrategy? changeTrackingStrategy)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("SetChangeTrackingStrategy").SetChangeTrackingStrategy(changeTrackingStrategy, ConfigurationSource.Explicit);
	}

	public static void SetQueryFilter([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.CanBeNull] LambdaExpression queryFilter)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("SetQueryFilter").SetQueryFilter(queryFilter, ConfigurationSource.Explicit);
	}

	public static void SetDefiningQuery([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.CanBeNull] LambdaExpression definingQuery)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("SetDefiningQuery").SetDefiningQuery(definingQuery, ConfigurationSource.Explicit);
	}

	public static IMutableProperty GetDiscriminatorProperty([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		return (IMutableProperty)EntityTypeExtensions.GetDiscriminatorProperty(entityType);
	}

	public static void SetDiscriminatorProperty([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.CanBeNull] IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("SetDiscriminatorProperty").SetDiscriminatorProperty(property, ConfigurationSource.Explicit);
	}

	public static void SetDiscriminatorValue([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.CanBeNull] object value)
	{
		entityType.AsEntityType("SetDiscriminatorValue").CheckDiscriminatorValue(entityType, value);
		entityType.SetAnnotation("DiscriminatorValue", value);
	}

	public static void RemoveDiscriminatorValue([JetBrains.Annotations.NotNull] this IMutableEntityType entityType)
	{
		entityType.RemoveAnnotation("DiscriminatorValue");
	}
}
