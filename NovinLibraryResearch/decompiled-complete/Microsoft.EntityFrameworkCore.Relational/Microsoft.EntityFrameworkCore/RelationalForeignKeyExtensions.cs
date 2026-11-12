using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalForeignKeyExtensions
{
	public static string GetConstraintName([JetBrains.Annotations.NotNull] this IForeignKey foreignKey)
	{
		return ((string)foreignKey["Relational:Name"]) ?? foreignKey.GetDefaultName();
	}

	public static string GetDefaultName([JetBrains.Annotations.NotNull] this IForeignKey foreignKey)
	{
		return Uniquifier.Truncate(new StringBuilder().Append("FK_").Append(foreignKey.DeclaringEntityType.GetTableName()).Append("_")
			.Append(foreignKey.PrincipalEntityType.GetTableName())
			.Append("_")
			.AppendJoin(foreignKey.Properties.Select((IProperty p) => p.GetColumnName()), "_")
			.ToString(), foreignKey.DeclaringEntityType.Model.GetMaxIdentifierLength());
	}

	public static void SetConstraintName([JetBrains.Annotations.NotNull] this IMutableForeignKey foreignKey, [JetBrains.Annotations.CanBeNull] string value)
	{
		foreignKey.SetOrRemoveAnnotation("Relational:Name", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"));
	}

	public static void SetConstraintName([JetBrains.Annotations.NotNull] this IConventionForeignKey foreignKey, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
	{
		foreignKey.SetOrRemoveAnnotation("Relational:Name", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetConstraintNameConfigurationSource([JetBrains.Annotations.NotNull] this IConventionForeignKey foreignKey)
	{
		return foreignKey.FindAnnotation("Relational:Name")?.GetConfigurationSource();
	}
}
