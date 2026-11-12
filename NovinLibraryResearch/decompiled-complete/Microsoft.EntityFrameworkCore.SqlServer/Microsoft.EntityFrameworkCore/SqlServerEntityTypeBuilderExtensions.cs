using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class SqlServerEntityTypeBuilderExtensions
{
	public static EntityTypeBuilder IsMemoryOptimized([JetBrains.Annotations.NotNull] this EntityTypeBuilder entityTypeBuilder, bool memoryOptimized = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		entityTypeBuilder.Metadata.SetIsMemoryOptimized(memoryOptimized);
		return entityTypeBuilder;
	}

	public static EntityTypeBuilder<TEntity> IsMemoryOptimized<TEntity>([JetBrains.Annotations.NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder, bool memoryOptimized = true) where TEntity : class
	{
		return (EntityTypeBuilder<TEntity>)((EntityTypeBuilder)entityTypeBuilder).IsMemoryOptimized(memoryOptimized);
	}

	public static OwnedNavigationBuilder IsMemoryOptimized([JetBrains.Annotations.NotNull] this OwnedNavigationBuilder collectionOwnershipBuilder, bool memoryOptimized = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(collectionOwnershipBuilder, "collectionOwnershipBuilder");
		collectionOwnershipBuilder.OwnedEntityType.SetIsMemoryOptimized(memoryOptimized);
		return collectionOwnershipBuilder;
	}

	public static OwnedNavigationBuilder<TEntity, TRelatedEntity> IsMemoryOptimized<TEntity, TRelatedEntity>([JetBrains.Annotations.NotNull] this OwnedNavigationBuilder<TEntity, TRelatedEntity> collectionOwnershipBuilder, bool memoryOptimized = true) where TEntity : class where TRelatedEntity : class
	{
		return (OwnedNavigationBuilder<TEntity, TRelatedEntity>)((OwnedNavigationBuilder)collectionOwnershipBuilder).IsMemoryOptimized(memoryOptimized);
	}

	public static IConventionEntityTypeBuilder IsMemoryOptimized([JetBrains.Annotations.NotNull] this IConventionEntityTypeBuilder entityTypeBuilder, bool? memoryOptimized, bool fromDataAnnotation = false)
	{
		if (entityTypeBuilder.CanSetIsMemoryOptimized(memoryOptimized, fromDataAnnotation))
		{
			entityTypeBuilder.Metadata.SetIsMemoryOptimized(memoryOptimized, fromDataAnnotation);
			return entityTypeBuilder;
		}
		return null;
	}

	public static bool CanSetIsMemoryOptimized([JetBrains.Annotations.NotNull] this IConventionEntityTypeBuilder entityTypeBuilder, bool? memoryOptimized, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		return entityTypeBuilder.CanSetAnnotation("SqlServer:MemoryOptimized", memoryOptimized, fromDataAnnotation);
	}

	[Obsolete("Use IsMemoryOptimized")]
	public static EntityTypeBuilder ForSqlServerIsMemoryOptimized([JetBrains.Annotations.NotNull] this EntityTypeBuilder entityTypeBuilder, bool memoryOptimized = true)
	{
		return entityTypeBuilder.IsMemoryOptimized(memoryOptimized);
	}

	[Obsolete("Use IsMemoryOptimized")]
	public static EntityTypeBuilder<TEntity> ForSqlServerIsMemoryOptimized<TEntity>([JetBrains.Annotations.NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder, bool memoryOptimized = true) where TEntity : class
	{
		return entityTypeBuilder.IsMemoryOptimized(memoryOptimized);
	}

	[Obsolete("Use IsMemoryOptimized")]
	public static OwnedNavigationBuilder ForSqlServerIsMemoryOptimized([JetBrains.Annotations.NotNull] this OwnedNavigationBuilder collectionOwnershipBuilder, bool memoryOptimized = true)
	{
		return collectionOwnershipBuilder.IsMemoryOptimized(memoryOptimized);
	}

	[Obsolete("Use IsMemoryOptimized")]
	public static OwnedNavigationBuilder<TEntity, TRelatedEntity> ForSqlServerIsMemoryOptimized<TEntity, TRelatedEntity>([JetBrains.Annotations.NotNull] this OwnedNavigationBuilder<TEntity, TRelatedEntity> collectionOwnershipBuilder, bool memoryOptimized = true) where TEntity : class where TRelatedEntity : class
	{
		return collectionOwnershipBuilder.IsMemoryOptimized(memoryOptimized);
	}

	[Obsolete("Use IsMemoryOptimized")]
	public static IConventionEntityTypeBuilder ForSqlServerIsMemoryOptimized([JetBrains.Annotations.NotNull] this IConventionEntityTypeBuilder entityTypeBuilder, bool? memoryOptimized, bool fromDataAnnotation = false)
	{
		return entityTypeBuilder.IsMemoryOptimized(memoryOptimized, fromDataAnnotation);
	}
}
