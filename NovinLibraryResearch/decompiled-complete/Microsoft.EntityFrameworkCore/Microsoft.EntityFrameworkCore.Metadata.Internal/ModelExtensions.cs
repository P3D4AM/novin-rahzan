using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class ModelExtensions
{
	public static void SetProductVersion([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] string value)
	{
		model["ProductVersion"] = value;
	}

	public static IEnumerable<IEntityType> GetRootEntityTypes([JetBrains.Annotations.NotNull] this IModel model)
	{
		return from e in model.GetEntityTypes()
			where e.BaseType == null
			select e;
	}

	public static string ToDebugString([JetBrains.Annotations.NotNull] this IModel model, [JetBrains.Annotations.NotNull] string indent = "")
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(indent).Append("Model: ");
		if (model.GetPropertyAccessMode() != PropertyAccessMode.PreferField)
		{
			stringBuilder.Append(" PropertyAccessMode.").Append(model.GetPropertyAccessMode());
		}
		if (model.GetChangeTrackingStrategy() != 0)
		{
			stringBuilder.Append(" ChangeTrackingStrategy.").Append(model.GetChangeTrackingStrategy());
		}
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			stringBuilder.AppendLine().Append(entityType.ToDebugString(singleLine: false, indent + "  "));
		}
		stringBuilder.Append(model.AnnotationsToDebugString(indent));
		return stringBuilder.ToString();
	}

	public static Model AsModel([JetBrains.Annotations.NotNull] this IModel model, [CallerMemberName][JetBrains.Annotations.NotNull] string methodName = "")
	{
		return MetadataExtensions.AsConcreteMetadataType<IModel, Model>(model, methodName);
	}
}
