using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class ModelExtensions
{
	[DebuggerStepThrough]
	public static IEntityType FindEntityType([JetBrains.Annotations.NotNull] this IModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		return ((Model)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model")).FindEntityType(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type"));
	}

	public static IEntityType FindRuntimeEntityType([JetBrains.Annotations.NotNull] this IModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		Model model2 = (Model)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		object obj = model2.FindEntityType(type);
		if (obj == null)
		{
			if (!(type.BaseType == null))
			{
				return model2.FindEntityType(type.BaseType);
			}
			obj = null;
		}
		return (IEntityType)obj;
	}

	[DebuggerStepThrough]
	public static IEntityType FindEntityType([JetBrains.Annotations.NotNull] this IModel model, [JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IEntityType definingEntityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(definingNavigationName, "definingNavigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(definingEntityType, "definingEntityType");
		return model.AsModel("FindEntityType").FindEntityType(type, definingNavigationName, definingEntityType.AsEntityType("FindEntityType"));
	}

	[DebuggerStepThrough]
	public static IReadOnlyCollection<IEntityType> GetEntityTypes([JetBrains.Annotations.NotNull] this IModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		return model.AsModel("GetEntityTypes").GetEntityTypes(type);
	}

	[DebuggerStepThrough]
	public static IReadOnlyCollection<IEntityType> GetEntityTypes([JetBrains.Annotations.NotNull] this IModel model, [JetBrains.Annotations.NotNull] string name)
	{
		return model.AsModel("GetEntityTypes").GetEntityTypes(name);
	}

	[DebuggerStepThrough]
	public static bool HasEntityTypeWithDefiningNavigation([JetBrains.Annotations.NotNull] this IModel model, [JetBrains.Annotations.NotNull] Type type)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model").AsModel("HasEntityTypeWithDefiningNavigation").HasEntityTypeWithDefiningNavigation(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type"));
	}

	[DebuggerStepThrough]
	public static bool HasEntityTypeWithDefiningNavigation([JetBrains.Annotations.NotNull] this IModel model, [JetBrains.Annotations.NotNull] string name)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model").AsModel("HasEntityTypeWithDefiningNavigation").HasEntityTypeWithDefiningNavigation(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name"));
	}

	[DebuggerStepThrough]
	public static ChangeTrackingStrategy GetChangeTrackingStrategy([JetBrains.Annotations.NotNull] this IModel model)
	{
		return ((ChangeTrackingStrategy?)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model")["ChangeTrackingStrategy"]).GetValueOrDefault();
	}

	[DebuggerStepThrough]
	public static PropertyAccessMode GetPropertyAccessMode([JetBrains.Annotations.NotNull] this IModel model)
	{
		return ((PropertyAccessMode?)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model")["PropertyAccessMode"]) ?? PropertyAccessMode.PreferField;
	}

	public static string GetProductVersion([JetBrains.Annotations.NotNull] this IModel model)
	{
		return model["ProductVersion"] as string;
	}
}
