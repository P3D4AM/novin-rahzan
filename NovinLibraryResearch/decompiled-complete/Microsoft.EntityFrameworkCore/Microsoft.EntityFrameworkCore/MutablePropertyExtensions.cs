using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore;

public static class MutablePropertyExtensions
{
	public static IMutableProperty FindFirstPrincipal([JetBrains.Annotations.NotNull] this IMutableProperty property)
	{
		return (IMutableProperty)PropertyExtensions.FindFirstPrincipal(property);
	}

	public static IEnumerable<IMutableForeignKey> GetContainingForeignKeys([JetBrains.Annotations.NotNull] this IMutableProperty property)
	{
		return ((Property)property).GetContainingForeignKeys();
	}

	public static IEnumerable<IMutableIndex> GetContainingIndexes([JetBrains.Annotations.NotNull] this IMutableProperty property)
	{
		return ((Property)property).GetContainingIndexes();
	}

	[Obsolete("Use FindContainingPrimaryKey()")]
	public static IMutableKey GetContainingPrimaryKey([JetBrains.Annotations.NotNull] this IMutableProperty property)
	{
		return property.FindContainingPrimaryKey();
	}

	public static IMutableKey FindContainingPrimaryKey([JetBrains.Annotations.NotNull] this IMutableProperty property)
	{
		return (IMutableKey)PropertyExtensions.FindContainingPrimaryKey(property);
	}

	public static IEnumerable<IMutableKey> GetContainingKeys([JetBrains.Annotations.NotNull] this IMutableProperty property)
	{
		return ((Property)property).GetContainingKeys();
	}

	public static void SetMaxLength([JetBrains.Annotations.NotNull] this IMutableProperty property, int? maxLength)
	{
		property.AsProperty("SetMaxLength").SetMaxLength(maxLength, ConfigurationSource.Explicit);
	}

	public static void SetIsUnicode([JetBrains.Annotations.NotNull] this IMutableProperty property, bool? unicode)
	{
		property.AsProperty("SetIsUnicode").SetIsUnicode(unicode, ConfigurationSource.Explicit);
	}

	public static void SetBeforeSaveBehavior([JetBrains.Annotations.NotNull] this IMutableProperty property, PropertySaveBehavior? beforeSaveBehavior)
	{
		property.AsProperty("SetBeforeSaveBehavior").SetBeforeSaveBehavior(beforeSaveBehavior, ConfigurationSource.Explicit);
	}

	public static void SetAfterSaveBehavior([JetBrains.Annotations.NotNull] this IMutableProperty property, PropertySaveBehavior? afterSaveBehavior)
	{
		property.AsProperty("SetAfterSaveBehavior").SetAfterSaveBehavior(afterSaveBehavior, ConfigurationSource.Explicit);
	}

	public static void SetValueGeneratorFactory([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.NotNull] Func<IProperty, IEntityType, ValueGenerator> valueGeneratorFactory)
	{
		property.AsProperty("SetValueGeneratorFactory").SetValueGeneratorFactory(valueGeneratorFactory, ConfigurationSource.Explicit);
	}

	public static void SetValueConverter([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] ValueConverter converter)
	{
		property.AsProperty("SetValueConverter").SetValueConverter(converter, ConfigurationSource.Explicit);
	}

	public static void SetProviderClrType([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] Type providerClrType)
	{
		property.AsProperty("SetProviderClrType").SetProviderClrType(providerClrType, ConfigurationSource.Explicit);
	}

	public static void SetValueComparer([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] ValueComparer comparer)
	{
		property.AsProperty("SetValueComparer").SetValueComparer(comparer, ConfigurationSource.Explicit);
	}

	public static void SetKeyValueComparer([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] ValueComparer comparer)
	{
		property.AsProperty("SetKeyValueComparer").SetKeyValueComparer(comparer, ConfigurationSource.Explicit);
	}

	public static void SetStructuralValueComparer([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] ValueComparer comparer)
	{
		property.AsProperty("SetStructuralValueComparer").SetStructuralValueComparer(comparer, ConfigurationSource.Explicit);
	}
}
