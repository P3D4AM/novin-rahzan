using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class ConventionEntityTypeExtensions
{
	public static IConventionEntityType GetRootType([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return (IConventionEntityType)EntityTypeExtensions.GetRootType(entityType);
	}

	[Obsolete("Use GetRootType")]
	public static IConventionEntityType RootType([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return (IConventionEntityType)EntityTypeExtensions.GetRootType(entityType);
	}

	public static IEnumerable<IConventionEntityType> GetDerivedTypes([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetDerivedTypes();
	}

	public static IEnumerable<IConventionEntityType> GetDerivedTypesInclusive([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetDerivedTypesInclusive();
	}

	public static IEnumerable<IConventionEntityType> GetDirectlyDerivedTypes([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetDirectlyDerivedTypes();
	}

	public static IEnumerable<IConventionEntityType> GetAllBaseTypesInclusive([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return entityType.GetAllBaseTypesInclusiveAscending().Reverse();
	}

	public static IEnumerable<IConventionEntityType> GetAllBaseTypesInclusiveAscending([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		while (entityType != null)
		{
			yield return entityType;
			entityType = entityType.BaseType;
		}
	}

	public static IEnumerable<IConventionKey> GetDeclaredKeys([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredKeys();
	}

	public static IEnumerable<IConventionProperty> GetDeclaredProperties([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredProperties();
	}

	public static IEnumerable<IConventionNavigation> GetDeclaredNavigations([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredNavigations();
	}

	public static IEnumerable<IConventionServiceProperty> GetDeclaredServiceProperties([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredServiceProperties();
	}

	public static IEnumerable<IConventionIndex> GetDeclaredIndexes([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredIndexes();
	}

	public static IConventionProperty RemoveProperty([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return ((EntityType)entityType).RemoveProperty(name);
	}

	public static IConventionKey SetPrimaryKey([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.CanBeNull] IConventionProperty property, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return entityType.SetPrimaryKey((property == null) ? null : new IConventionProperty[1] { property }, fromDataAnnotation);
	}

	public static IConventionKey FindKey([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return entityType.FindKey(new IProperty[1] { property });
	}

	public static IConventionKey AddKey([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IConventionProperty property, bool fromDataAnnotation = false)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AddKey(new IConventionProperty[1] { property }, fromDataAnnotation);
	}

	public static IConventionKey RemoveKey([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties)
	{
		return ((EntityType)entityType).RemoveKey(properties);
	}

	public static IEnumerable<IConventionForeignKey> GetDeclaredForeignKeys([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredForeignKeys();
	}

	public static IEnumerable<IConventionForeignKey> GetDerivedForeignKeys([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetDerivedForeignKeys();
	}

	public static IEnumerable<IConventionForeignKey> FindForeignKeys([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property)
	{
		return entityType.FindForeignKeys(new IProperty[1] { property });
	}

	public static IEnumerable<IConventionForeignKey> FindForeignKeys([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		return ((EntityType)entityType).FindForeignKeys(properties);
	}

	public static IConventionForeignKey FindForeignKey([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IKey principalKey, [JetBrains.Annotations.NotNull] IEntityType principalEntityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return entityType.FindForeignKey(new IProperty[1] { property }, principalKey, principalEntityType);
	}

	public static IEnumerable<IConventionForeignKey> FindDeclaredForeignKeys([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		return ((EntityType)entityType).FindDeclaredForeignKeys(properties);
	}

	public static IEnumerable<IConventionForeignKey> GetReferencingForeignKeys([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetReferencingForeignKeys();
	}

	public static IEnumerable<IConventionForeignKey> GetDeclaredReferencingForeignKeys([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetDeclaredReferencingForeignKeys();
	}

	public static IConventionForeignKey FindOwnership([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).FindOwnership();
	}

	public static IConventionForeignKey AddForeignKey([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IConventionProperty property, [JetBrains.Annotations.NotNull] IConventionKey principalKey, [JetBrains.Annotations.NotNull] IConventionEntityType principalEntityType, bool fromDataAnnotation = false)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AddForeignKey(new IConventionProperty[1] { property }, principalKey, principalEntityType, fromDataAnnotation);
	}

	public static IConventionForeignKey RemoveForeignKey([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, [JetBrains.Annotations.NotNull] IConventionKey principalKey, [JetBrains.Annotations.NotNull] IConventionEntityType principalEntityType)
	{
		return ((EntityType)entityType).RemoveForeignKey(properties, principalKey, principalEntityType);
	}

	public static IConventionNavigation FindNavigation([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] MemberInfo memberInfo)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").FindNavigation(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memberInfo, "memberInfo").GetSimpleMemberName());
	}

	public static IConventionNavigation FindNavigation([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return ((EntityType)entityType).FindNavigation(name);
	}

	public static IConventionNavigation FindDeclaredNavigation([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return ((EntityType)entityType).FindDeclaredNavigation(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name"));
	}

	public static IConventionNavigation FindDefiningNavigation([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return (IConventionNavigation)EntityTypeExtensions.FindDefiningNavigation(entityType);
	}

	public static IEnumerable<IConventionNavigation> GetNavigations([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return ((EntityType)entityType).GetNavigations();
	}

	public static IConventionProperty FindProperty([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] MemberInfo memberInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memberInfo, "memberInfo");
		return entityType.FindProperty(memberInfo.GetSimpleMemberName());
	}

	public static IReadOnlyList<IConventionProperty> FindProperties([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<string> propertyNames)
	{
		return ((EntityType)entityType).FindProperties(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyNames, "propertyNames"));
	}

	public static IConventionProperty FindDeclaredProperty([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return ((EntityType)entityType).FindDeclaredProperty(name);
	}

	public static IConventionProperty AddProperty([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] MemberInfo memberInfo, bool fromDataAnnotation = false)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AddProperty(memberInfo.GetSimpleMemberName(), memberInfo.GetMemberType(), memberInfo, setTypeConfigurationSource: true, fromDataAnnotation);
	}

	public static IConventionProperty AddProperty([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] string name, bool fromDataAnnotation = false)
	{
		return ((EntityType)entityType).AddProperty(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static IConventionProperty AddProperty([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Type propertyType, bool setTypeConfigurationSource = true, bool fromDataAnnotation = false)
	{
		return entityType.AddProperty(name, propertyType, null, setTypeConfigurationSource, fromDataAnnotation);
	}

	public static IConventionIndex FindIndex([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return entityType.FindIndex(new IProperty[1] { property });
	}

	public static IConventionIndex AddIndex([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IConventionProperty property, bool fromDataAnnotation = false)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AddIndex(new IConventionProperty[1] { property }, fromDataAnnotation);
	}

	public static IConventionIndex RemoveIndex([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties)
	{
		return ((EntityType)entityType).RemoveIndex(properties);
	}

	public static void SetChangeTrackingStrategy([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, ChangeTrackingStrategy? changeTrackingStrategy, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("SetChangeTrackingStrategy").SetChangeTrackingStrategy(changeTrackingStrategy, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetChangeTrackingStrategyConfigurationSource([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return entityType.FindAnnotation("ChangeTrackingStrategy")?.GetConfigurationSource();
	}

	public static void SetQueryFilter([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.CanBeNull] LambdaExpression queryFilter, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("SetQueryFilter").SetQueryFilter(queryFilter, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetQueryFilterConfigurationSource([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return entityType.FindAnnotation("QueryFilter")?.GetConfigurationSource();
	}

	public static void SetDefiningQuery([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.CanBeNull] LambdaExpression definingQuery, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("SetDefiningQuery").SetDefiningQuery(definingQuery, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetDefiningQueryConfigurationSource([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return entityType.FindAnnotation("DefiningQuery")?.GetConfigurationSource();
	}

	public static IConventionProperty GetDiscriminatorProperty([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return (IConventionProperty)EntityTypeExtensions.GetDiscriminatorProperty(entityType);
	}

	public static void SetDiscriminatorProperty([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.CanBeNull] IProperty property, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("SetDiscriminatorProperty").SetDiscriminatorProperty(property, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetDiscriminatorPropertyConfigurationSource([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return entityType.FindAnnotation("DiscriminatorProperty")?.GetConfigurationSource();
	}

	public static void SetDiscriminatorValue([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false)
	{
		entityType.AsEntityType("SetDiscriminatorValue").CheckDiscriminatorValue(entityType, value);
		entityType.SetAnnotation("DiscriminatorValue", value, fromDataAnnotation);
	}

	public static void RemoveDiscriminatorValue([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		entityType.RemoveAnnotation("DiscriminatorValue");
	}

	public static ConfigurationSource? GetDiscriminatorValueConfigurationSource([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return entityType.FindAnnotation("DiscriminatorValue")?.GetConfigurationSource();
	}
}
