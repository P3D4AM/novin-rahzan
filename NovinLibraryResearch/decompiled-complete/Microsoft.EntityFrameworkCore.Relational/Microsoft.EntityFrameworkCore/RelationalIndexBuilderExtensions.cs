using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalIndexBuilderExtensions
{
	public static IndexBuilder HasName([JetBrains.Annotations.NotNull] this IndexBuilder indexBuilder, [JetBrains.Annotations.CanBeNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexBuilder, "indexBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		indexBuilder.Metadata.SetName(name);
		return indexBuilder;
	}

	public static IndexBuilder<TEntity> HasName<TEntity>([JetBrains.Annotations.NotNull] this IndexBuilder<TEntity> indexBuilder, [JetBrains.Annotations.CanBeNull] string name)
	{
		return (IndexBuilder<TEntity>)((IndexBuilder)indexBuilder).HasName(name);
	}

	public static IConventionIndexBuilder HasName([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		if (indexBuilder.CanSetName(name, fromDataAnnotation))
		{
			indexBuilder.Metadata.SetName(name, fromDataAnnotation);
			return indexBuilder;
		}
		return null;
	}

	public static bool CanSetName([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		return indexBuilder.CanSetAnnotation("Relational:Name", name, fromDataAnnotation);
	}

	public static IndexBuilder HasFilter([JetBrains.Annotations.NotNull] this IndexBuilder indexBuilder, [JetBrains.Annotations.CanBeNull] string sql)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexBuilder, "indexBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(sql, "sql");
		indexBuilder.Metadata.SetFilter(sql);
		return indexBuilder;
	}

	public static IndexBuilder<TEntity> HasFilter<TEntity>([JetBrains.Annotations.NotNull] this IndexBuilder<TEntity> indexBuilder, [JetBrains.Annotations.CanBeNull] string sql)
	{
		return (IndexBuilder<TEntity>)((IndexBuilder)indexBuilder).HasFilter(sql);
	}

	public static IConventionIndexBuilder HasFilter([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, [JetBrains.Annotations.CanBeNull] string sql, bool fromDataAnnotation = false)
	{
		if (indexBuilder.CanSetFilter(sql, fromDataAnnotation))
		{
			indexBuilder.Metadata.SetFilter(sql, fromDataAnnotation);
			return indexBuilder;
		}
		return null;
	}

	public static bool CanSetFilter([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, [JetBrains.Annotations.CanBeNull] string sql, bool fromDataAnnotation = false)
	{
		return indexBuilder.CanSetAnnotation("Relational:Filter", sql, fromDataAnnotation);
	}
}
