using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class SqlServerIndexBuilderExtensions
{
	public static IndexBuilder IsClustered([JetBrains.Annotations.NotNull] this IndexBuilder indexBuilder, bool clustered = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexBuilder, "indexBuilder");
		indexBuilder.Metadata.SetIsClustered(clustered);
		return indexBuilder;
	}

	public static IndexBuilder<TEntity> IsClustered<TEntity>([JetBrains.Annotations.NotNull] this IndexBuilder<TEntity> indexBuilder, bool clustered = true)
	{
		return (IndexBuilder<TEntity>)((IndexBuilder)indexBuilder).IsClustered(clustered);
	}

	public static IConventionIndexBuilder IsClustered([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, bool? clustered, bool fromDataAnnotation = false)
	{
		if (indexBuilder.CanSetIsClustered(clustered, fromDataAnnotation))
		{
			indexBuilder.Metadata.SetIsClustered(clustered, fromDataAnnotation);
			return indexBuilder;
		}
		return null;
	}

	public static bool CanSetIsClustered([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, bool? clustered, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexBuilder, "indexBuilder");
		return indexBuilder.CanSetAnnotation("SqlServer:Clustered", clustered, fromDataAnnotation);
	}

	public static IndexBuilder IncludeProperties([JetBrains.Annotations.NotNull] this IndexBuilder indexBuilder, [JetBrains.Annotations.NotNull] params string[] propertyNames)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexBuilder, "indexBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyNames, "propertyNames");
		indexBuilder.Metadata.SetIncludeProperties(propertyNames);
		return indexBuilder;
	}

	public static IndexBuilder<TEntity> IncludeProperties<TEntity>([JetBrains.Annotations.NotNull] this IndexBuilder<TEntity> indexBuilder, [JetBrains.Annotations.NotNull] Expression<Func<TEntity, object>> includeExpression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexBuilder, "indexBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(includeExpression, "includeExpression");
		indexBuilder.IncludeProperties(includeExpression.GetPropertyAccessList().Select(System.Reflection.MemberInfoExtensions.GetSimpleMemberName).ToArray());
		return indexBuilder;
	}

	public static IConventionIndexBuilder IncludeProperties([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, [JetBrains.Annotations.NotNull] IReadOnlyList<string> propertyNames, bool fromDataAnnotation = false)
	{
		if (indexBuilder.CanSetIncludeProperties(propertyNames, fromDataAnnotation))
		{
			indexBuilder.Metadata.SetIncludeProperties(propertyNames, fromDataAnnotation);
			return indexBuilder;
		}
		return null;
	}

	public static bool CanSetIncludeProperties([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, [JetBrains.Annotations.CanBeNull] IReadOnlyList<string> propertyNames, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexBuilder, "indexBuilder");
		if (!(fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention).Overrides(indexBuilder.Metadata.GetIncludePropertiesConfigurationSource()))
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(propertyNames, indexBuilder.Metadata.GetIncludeProperties());
		}
		return true;
	}

	public static IndexBuilder IsCreatedOnline([JetBrains.Annotations.NotNull] this IndexBuilder indexBuilder, bool createdOnline = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexBuilder, "indexBuilder");
		indexBuilder.Metadata.SetIsCreatedOnline(createdOnline);
		return indexBuilder;
	}

	public static IndexBuilder<TEntity> IsCreatedOnline<TEntity>([JetBrains.Annotations.NotNull] this IndexBuilder<TEntity> indexBuilder, bool createdOnline = true)
	{
		return (IndexBuilder<TEntity>)((IndexBuilder)indexBuilder).IsCreatedOnline(createdOnline);
	}

	public static IConventionIndexBuilder IsCreatedOnline([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, bool? createdOnline, bool fromDataAnnotation = false)
	{
		if (indexBuilder.CanSetIsCreatedOnline(createdOnline, fromDataAnnotation))
		{
			indexBuilder.Metadata.SetIsCreatedOnline(createdOnline, fromDataAnnotation);
			return indexBuilder;
		}
		return null;
	}

	public static bool CanSetIsCreatedOnline([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, bool? createdOnline, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexBuilder, "indexBuilder");
		return indexBuilder.CanSetAnnotation("SqlServer:Online", createdOnline, fromDataAnnotation);
	}

	[Obsolete("Use IsClustered")]
	public static IndexBuilder ForSqlServerIsClustered([JetBrains.Annotations.NotNull] this IndexBuilder indexBuilder, bool clustered = true)
	{
		return indexBuilder.IsClustered(clustered);
	}

	[Obsolete("Use IsClustered")]
	public static IndexBuilder<TEntity> ForSqlServerIsClustered<TEntity>([JetBrains.Annotations.NotNull] this IndexBuilder<TEntity> indexBuilder, bool clustered = true)
	{
		return indexBuilder.IsClustered(clustered);
	}

	[Obsolete("Use IsClustered")]
	public static IConventionIndexBuilder ForSqlServerIsClustered([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, bool? clustered, bool fromDataAnnotation = false)
	{
		return indexBuilder.IsClustered(clustered, fromDataAnnotation);
	}

	[Obsolete("Use IncludeProperties")]
	public static IndexBuilder ForSqlServerInclude([JetBrains.Annotations.NotNull] this IndexBuilder indexBuilder, [JetBrains.Annotations.NotNull] params string[] propertyNames)
	{
		return indexBuilder.IncludeProperties(propertyNames);
	}

	[Obsolete("Use IncludeProperties")]
	public static IndexBuilder<TEntity> ForSqlServerInclude<TEntity>([JetBrains.Annotations.NotNull] this IndexBuilder<TEntity> indexBuilder, [JetBrains.Annotations.NotNull] Expression<Func<TEntity, object>> includeExpression)
	{
		return indexBuilder.IncludeProperties(includeExpression);
	}

	[Obsolete("Use IncludeProperties")]
	public static IConventionIndexBuilder ForSqlServerInclude([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, [JetBrains.Annotations.NotNull] IReadOnlyList<string> propertyNames, bool fromDataAnnotation = false)
	{
		return indexBuilder.IncludeProperties(propertyNames, fromDataAnnotation);
	}

	[Obsolete("Use IsCreatedOnline")]
	public static IndexBuilder ForSqlServerIsCreatedOnline([JetBrains.Annotations.NotNull] this IndexBuilder indexBuilder, bool createdOnline = true)
	{
		return indexBuilder.IsCreatedOnline(createdOnline);
	}

	[Obsolete("Use IsCreatedOnline")]
	public static IndexBuilder<TEntity> ForSqlServerIsCreatedOnline<TEntity>([JetBrains.Annotations.NotNull] this IndexBuilder<TEntity> indexBuilder, bool createdOnline = true)
	{
		return indexBuilder.IsCreatedOnline(createdOnline);
	}

	[Obsolete("Use IsCreatedOnline")]
	public static IConventionIndexBuilder ForSqlServerIsCreatedOnline([JetBrains.Annotations.NotNull] this IConventionIndexBuilder indexBuilder, bool? createdOnline, bool fromDataAnnotation = false)
	{
		return indexBuilder.IsCreatedOnline(createdOnline, fromDataAnnotation);
	}
}
