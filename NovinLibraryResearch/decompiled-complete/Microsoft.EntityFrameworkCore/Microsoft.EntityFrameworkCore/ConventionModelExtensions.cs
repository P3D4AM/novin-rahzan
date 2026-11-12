using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class ConventionModelExtensions
{
	public static IConventionEntityType FindEntityType([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		return (IConventionEntityType)ModelExtensions.FindEntityType(model, type);
	}

	public static IConventionEntityType FindEntityType([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IConventionEntityType definingEntityType)
	{
		return (IConventionEntityType)ModelExtensions.FindEntityType(model, type, definingNavigationName, definingEntityType);
	}

	[DebuggerStepThrough]
	public static IReadOnlyCollection<IConventionEntityType> GetEntityTypes([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		return ((Model)model).GetEntityTypes(type);
	}

	[DebuggerStepThrough]
	public static IReadOnlyCollection<IConventionEntityType> GetEntityTypes([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] string name)
	{
		return ((Model)model).GetEntityTypes(name);
	}

	public static IConventionEntityType RemoveEntityType([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		return ((Model)model).RemoveEntityType(name);
	}

	public static IConventionEntityType RemoveEntityType([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IConventionEntityType definingEntityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(definingNavigationName, "definingNavigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(definingEntityType, "definingEntityType");
		return ((Model)model).RemoveEntityType(name);
	}

	public static IConventionEntityType RemoveEntityType([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		return ((Model)model).RemoveEntityType(type);
	}

	public static IConventionEntityType RemoveEntityType([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IConventionEntityType definingEntityType)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").RemoveEntityType(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type"), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(definingNavigationName, "definingNavigationName"), (EntityType)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(definingEntityType, "definingEntityType"));
	}

	public static IReadOnlyList<IConventionEntityType> FindLeastDerivedEntityTypes([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.CanBeNull] Func<IConventionEntityType, bool> condition = null)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").FindLeastDerivedEntityTypes(type, condition);
	}

	public static void SetPropertyAccessMode([JetBrains.Annotations.NotNull] this IConventionModel model, PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").SetPropertyAccessMode(propertyAccessMode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetPropertyAccessModeConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return model.FindAnnotation("PropertyAccessMode")?.GetConfigurationSource();
	}

	public static void SetChangeTrackingStrategy([JetBrains.Annotations.NotNull] this IConventionModel model, ChangeTrackingStrategy? changeTrackingStrategy, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").SetChangeTrackingStrategy(changeTrackingStrategy, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetChangeTrackingStrategyConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return model.FindAnnotation("ChangeTrackingStrategy")?.GetConfigurationSource();
	}

	public static bool IsOwned([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type clrType)
	{
		return model.FindIsOwnedConfigurationSource(clrType).HasValue;
	}

	public static ConfigurationSource? FindIsOwnedConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type clrType)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").FindIsOwnedConfigurationSource(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType"));
	}

	public static void AddOwned([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type clrType, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").AddOwned(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType"), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static bool IsIgnored([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] string typeName)
	{
		return model.FindIgnoredConfigurationSource(typeName).HasValue;
	}

	public static bool IsIgnored([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").IsIgnored(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type"));
	}

	public static ConfigurationSource? FindIgnoredConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").FindIgnoredConfigurationSource(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type"));
	}

	public static void RemoveOwned([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type clrType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").RemoveOwned(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType"));
	}

	public static void AddIgnored([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] Type clrType, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").AddIgnored(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType"), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static IModel FinalizeModel([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return ((Model)model).FinalizeModel();
	}
}
