using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class MutableModelExtensions
{
	public static IMutableEntityType FindEntityType([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		return (IMutableEntityType)ModelExtensions.FindEntityType(model, type);
	}

	public static IMutableEntityType FindEntityType([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IMutableEntityType definingEntityType)
	{
		return (IMutableEntityType)ModelExtensions.FindEntityType(model, type, definingNavigationName, definingEntityType);
	}

	[DebuggerStepThrough]
	public static IReadOnlyCollection<IMutableEntityType> GetEntityTypes([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		return ((Model)model).GetEntityTypes(type);
	}

	[DebuggerStepThrough]
	public static IReadOnlyCollection<IMutableEntityType> GetEntityTypes([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] string name)
	{
		return ((Model)model).GetEntityTypes(name);
	}

	public static IMutableEntityType RemoveEntityType([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		return ((Model)model).RemoveEntityType(type);
	}

	public static IMutableEntityType RemoveEntityType([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IMutableEntityType definingEntityType)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").RemoveEntityType(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type"), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(definingNavigationName, "definingNavigationName"), (EntityType)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(definingEntityType, "definingEntityType"));
	}

	public static IMutableEntityType RemoveEntityType([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		return ((Model)model).RemoveEntityType(name);
	}

	public static IMutableEntityType RemoveEntityType([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IMutableEntityType definingEntityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(definingNavigationName, "definingNavigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(definingEntityType, "definingEntityType");
		return ((Model)model).RemoveEntityType(name);
	}

	public static IReadOnlyList<IMutableEntityType> FindLeastDerivedEntityTypes([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.CanBeNull] Func<IMutableEntityType, bool> condition = null)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").FindLeastDerivedEntityTypes(type, condition);
	}

	public static void RemoveIgnored([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").RemoveIgnored(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type"));
	}

	public static bool IsIgnored([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").IsIgnored(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type"));
	}

	public static void SetPropertyAccessMode([JetBrains.Annotations.NotNull] this IMutableModel model, PropertyAccessMode? propertyAccessMode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").SetPropertyAccessMode(propertyAccessMode, ConfigurationSource.Explicit);
	}

	public static void SetChangeTrackingStrategy([JetBrains.Annotations.NotNull] this IMutableModel model, ChangeTrackingStrategy? changeTrackingStrategy)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").SetChangeTrackingStrategy(changeTrackingStrategy, ConfigurationSource.Explicit);
	}

	public static void AddIgnored([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] Type clrType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").AddIgnored(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType"), ConfigurationSource.Explicit);
	}

	public static bool IsOwned([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] Type clrType)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").IsOwned(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType"));
	}

	public static void AddOwned([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] Type clrType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").AddOwned(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType"), ConfigurationSource.Explicit);
	}

	public static void RemoveOwned([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] Type clrType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull((Model)model, "model").RemoveOwned(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType"));
	}

	public static IModel FinalizeModel([JetBrains.Annotations.NotNull] this IMutableModel model)
	{
		return ((Model)model).FinalizeModel();
	}
}
