using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalIndexExtensions
{
	public static string GetName([JetBrains.Annotations.NotNull] this IIndex index)
	{
		return ((string)index["Relational:Name"]) ?? index.GetDefaultName();
	}

	public static string GetDefaultName([JetBrains.Annotations.NotNull] this IIndex index)
	{
		return Uniquifier.Truncate(new StringBuilder().Append("IX_").Append(index.DeclaringEntityType.GetTableName()).Append("_")
			.AppendJoin(index.Properties.Select((IProperty p) => p.GetColumnName()), "_")
			.ToString(), index.DeclaringEntityType.Model.GetMaxIdentifierLength());
	}

	public static void SetName([JetBrains.Annotations.NotNull] this IMutableIndex index, [JetBrains.Annotations.CanBeNull] string name)
	{
		index.SetOrRemoveAnnotation("Relational:Name", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name"));
	}

	public static void SetName([JetBrains.Annotations.NotNull] this IConventionIndex index, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		index.SetOrRemoveAnnotation("Relational:Name", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetNameConfigurationSource([JetBrains.Annotations.NotNull] this IConventionIndex index)
	{
		return index.FindAnnotation("Relational:Name")?.GetConfigurationSource();
	}

	public static string GetFilter([JetBrains.Annotations.NotNull] this IIndex index)
	{
		return (string)index["Relational:Filter"];
	}

	public static void SetFilter([JetBrains.Annotations.NotNull] this IMutableIndex index, [JetBrains.Annotations.CanBeNull] string value)
	{
		index.SetOrRemoveAnnotation("Relational:Filter", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"));
	}

	public static void SetFilter([JetBrains.Annotations.NotNull] this IConventionIndex index, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
	{
		index.SetOrRemoveAnnotation("Relational:Filter", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetFilterConfigurationSource([JetBrains.Annotations.NotNull] this IConventionIndex index)
	{
		return index.FindAnnotation("Relational:Filter")?.GetConfigurationSource();
	}
}
