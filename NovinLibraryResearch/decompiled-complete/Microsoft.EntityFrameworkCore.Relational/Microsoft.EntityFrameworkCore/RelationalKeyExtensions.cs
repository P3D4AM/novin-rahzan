using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalKeyExtensions
{
	public static string GetName([JetBrains.Annotations.NotNull] this IKey key)
	{
		return ((string)key["Relational:Name"]) ?? key.GetDefaultName();
	}

	public static string GetDefaultName([JetBrains.Annotations.NotNull] this IKey key)
	{
		IProperty property = key.Properties[0].FindSharedTableRootPrimaryKeyProperty();
		if (property != null)
		{
			return property.FindContainingPrimaryKey().GetName();
		}
		StringBuilder stringBuilder = new StringBuilder();
		string tableName = key.DeclaringEntityType.GetTableName();
		if (key.IsPrimaryKey())
		{
			stringBuilder.Append("PK_").Append(tableName);
		}
		else
		{
			stringBuilder.Append("AK_").Append(tableName).Append("_")
				.AppendJoin(key.Properties.Select((IProperty p) => p.GetColumnName()), "_");
		}
		return Uniquifier.Truncate(stringBuilder.ToString(), key.DeclaringEntityType.Model.GetMaxIdentifierLength());
	}

	public static void SetName([JetBrains.Annotations.NotNull] this IMutableKey key, [JetBrains.Annotations.CanBeNull] string name)
	{
		key.SetOrRemoveAnnotation("Relational:Name", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name"));
	}

	public static void SetName([JetBrains.Annotations.NotNull] this IConventionKey key, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		key.SetOrRemoveAnnotation("Relational:Name", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetNameConfigurationSource([JetBrains.Annotations.NotNull] this IConventionKey key)
	{
		return key.FindAnnotation("Relational:Name")?.GetConfigurationSource();
	}
}
