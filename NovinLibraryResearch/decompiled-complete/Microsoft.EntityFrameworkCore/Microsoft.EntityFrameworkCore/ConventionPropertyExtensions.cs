using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore;

public static class ConventionPropertyExtensions
{
	public static IConventionProperty FindFirstPrincipal([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return (IConventionProperty)PropertyExtensions.FindFirstPrincipal(property);
	}

	public static IEnumerable<IConventionForeignKey> GetContainingForeignKeys([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return PropertyExtensions.GetContainingForeignKeys(property).Cast<IConventionForeignKey>();
	}

	public static IEnumerable<IConventionIndex> GetContainingIndexes([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return ((Property)property).GetContainingIndexes();
	}

	public static IConventionKey FindContainingPrimaryKey([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return (IConventionKey)PropertyExtensions.FindContainingPrimaryKey(property);
	}

	public static IEnumerable<IConventionKey> GetContainingKeys([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return ((Property)property).GetContainingKeys();
	}

	public static ConfigurationSource? GetTypeMappingConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("TypeMapping")?.GetConfigurationSource();
	}

	public static void SetMaxLength([JetBrains.Annotations.NotNull] this IConventionProperty property, int? maxLength, bool fromDataAnnotation = false)
	{
		property.AsProperty("SetMaxLength").SetMaxLength(maxLength, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetMaxLengthConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("MaxLength")?.GetConfigurationSource();
	}

	public static void SetIsUnicode([JetBrains.Annotations.NotNull] this IConventionProperty property, bool? unicode, bool fromDataAnnotation = false)
	{
		property.AsProperty("SetIsUnicode").SetIsUnicode(unicode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetIsUnicodeConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("Unicode")?.GetConfigurationSource();
	}

	public static void SetBeforeSaveBehavior([JetBrains.Annotations.NotNull] this IConventionProperty property, PropertySaveBehavior? beforeSaveBehavior, bool fromDataAnnotation = false)
	{
		property.AsProperty("SetBeforeSaveBehavior").SetBeforeSaveBehavior(beforeSaveBehavior, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetBeforeSaveBehaviorConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("BeforeSaveBehavior")?.GetConfigurationSource();
	}

	public static void SetAfterSaveBehavior([JetBrains.Annotations.NotNull] this IConventionProperty property, PropertySaveBehavior? afterSaveBehavior, bool fromDataAnnotation = false)
	{
		property.AsProperty("SetAfterSaveBehavior").SetAfterSaveBehavior(afterSaveBehavior, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetAfterSaveBehaviorConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("AfterSaveBehavior")?.GetConfigurationSource();
	}

	public static void SetValueGeneratorFactory([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.NotNull] Func<IProperty, IEntityType, ValueGenerator> valueGeneratorFactory, bool fromDataAnnotation = false)
	{
		property.AsProperty("SetValueGeneratorFactory").SetValueGeneratorFactory(valueGeneratorFactory, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetValueGeneratorFactoryConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("ValueGeneratorFactory")?.GetConfigurationSource();
	}

	public static void SetValueConverter([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] ValueConverter converter, bool fromDataAnnotation = false)
	{
		property.AsProperty("SetValueConverter").SetValueConverter(converter, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetValueConverterConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("ValueConverter")?.GetConfigurationSource();
	}

	public static void SetProviderClrType([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] Type providerClrType, bool fromDataAnnotation = false)
	{
		property.AsProperty("SetProviderClrType").SetProviderClrType(providerClrType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetProviderClrTypeConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("ProviderClrType")?.GetConfigurationSource();
	}

	public static void SetValueComparer([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] ValueComparer comparer, bool fromDataAnnotation = false)
	{
		property.AsProperty("SetValueComparer").SetValueComparer(comparer, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetValueComparerConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("ValueComparer")?.GetConfigurationSource();
	}

	public static void SetKeyValueComparer([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] ValueComparer comparer, bool fromDataAnnotation = false)
	{
		property.AsProperty("SetKeyValueComparer").SetKeyValueComparer(comparer, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetKeyValueComparerConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("KeyValueComparer")?.GetConfigurationSource();
	}

	public static void SetStructuralValueComparer([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] ValueComparer comparer, bool fromDataAnnotation = false)
	{
		property.AsProperty("SetStructuralValueComparer").SetStructuralValueComparer(comparer, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetStructuralValueComparerConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("StructuralValueComparer")?.GetConfigurationSource();
	}
}
