using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalEntityTypeBuilderExtensions
{
	public static EntityTypeBuilder ToTable([JetBrains.Annotations.NotNull] this EntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		entityTypeBuilder.Metadata.SetTableName(name);
		entityTypeBuilder.Metadata.RemoveAnnotation("Relational:ViewDefinition");
		return entityTypeBuilder;
	}

	public static EntityTypeBuilder<TEntity> ToTable<TEntity>([JetBrains.Annotations.NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string name) where TEntity : class
	{
		return (EntityTypeBuilder<TEntity>)((EntityTypeBuilder)entityTypeBuilder).ToTable(name);
	}

	public static EntityTypeBuilder ToTable([JetBrains.Annotations.NotNull] this EntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		entityTypeBuilder.Metadata.SetTableName(name);
		entityTypeBuilder.Metadata.SetSchema(schema);
		entityTypeBuilder.Metadata.RemoveAnnotation("Relational:ViewDefinition");
		return entityTypeBuilder;
	}

	public static EntityTypeBuilder<TEntity> ToTable<TEntity>([JetBrains.Annotations.NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema) where TEntity : class
	{
		return (EntityTypeBuilder<TEntity>)((EntityTypeBuilder)entityTypeBuilder).ToTable(name, schema);
	}

	public static OwnedNavigationBuilder ToTable([JetBrains.Annotations.NotNull] this OwnedNavigationBuilder referenceOwnershipBuilder, [JetBrains.Annotations.CanBeNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(referenceOwnershipBuilder, "referenceOwnershipBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		referenceOwnershipBuilder.OwnedEntityType.SetTableName(name);
		return referenceOwnershipBuilder;
	}

	public static OwnedNavigationBuilder<TEntity, TRelatedEntity> ToTable<TEntity, TRelatedEntity>([JetBrains.Annotations.NotNull] this OwnedNavigationBuilder<TEntity, TRelatedEntity> referenceOwnershipBuilder, [JetBrains.Annotations.CanBeNull] string name) where TEntity : class where TRelatedEntity : class
	{
		return (OwnedNavigationBuilder<TEntity, TRelatedEntity>)((OwnedNavigationBuilder)referenceOwnershipBuilder).ToTable(name);
	}

	public static OwnedNavigationBuilder ToTable([JetBrains.Annotations.NotNull] this OwnedNavigationBuilder referenceOwnershipBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(referenceOwnershipBuilder, "referenceOwnershipBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		referenceOwnershipBuilder.OwnedEntityType.SetTableName(name);
		referenceOwnershipBuilder.OwnedEntityType.SetSchema(schema);
		return referenceOwnershipBuilder;
	}

	public static OwnedNavigationBuilder<TEntity, TRelatedEntity> ToTable<TEntity, TRelatedEntity>([JetBrains.Annotations.NotNull] this OwnedNavigationBuilder<TEntity, TRelatedEntity> referenceOwnershipBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema) where TEntity : class where TRelatedEntity : class
	{
		return (OwnedNavigationBuilder<TEntity, TRelatedEntity>)((OwnedNavigationBuilder)referenceOwnershipBuilder).ToTable(name, schema);
	}

	public static IConventionEntityTypeBuilder ToTable([JetBrains.Annotations.NotNull] this IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		if (!entityTypeBuilder.CanSetTable(name, fromDataAnnotation))
		{
			return null;
		}
		entityTypeBuilder.Metadata.SetTableName(name, fromDataAnnotation);
		return entityTypeBuilder;
	}

	public static IConventionEntityTypeBuilder ToTable([JetBrains.Annotations.NotNull] this IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
	{
		if (!entityTypeBuilder.CanSetTable(name, fromDataAnnotation) || !entityTypeBuilder.CanSetSchema(schema, fromDataAnnotation))
		{
			return null;
		}
		entityTypeBuilder.Metadata.SetTableName(name, fromDataAnnotation);
		entityTypeBuilder.Metadata.SetSchema(schema, fromDataAnnotation);
		return entityTypeBuilder;
	}

	public static bool CanSetTable([JetBrains.Annotations.NotNull] this IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		return entityTypeBuilder.CanSetAnnotation("Relational:TableName", name, fromDataAnnotation);
	}

	public static IConventionEntityTypeBuilder ToSchema([JetBrains.Annotations.NotNull] this IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
	{
		if (!entityTypeBuilder.CanSetSchema(schema, fromDataAnnotation))
		{
			return null;
		}
		entityTypeBuilder.Metadata.SetSchema(schema, fromDataAnnotation);
		return entityTypeBuilder;
	}

	public static bool CanSetSchema([JetBrains.Annotations.NotNull] this IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		return entityTypeBuilder.CanSetAnnotation("Relational:Schema", schema, fromDataAnnotation);
	}

	public static EntityTypeBuilder ToView([JetBrains.Annotations.NotNull] this EntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		entityTypeBuilder.Metadata.SetTableName(name);
		entityTypeBuilder.Metadata.SetAnnotation("Relational:ViewDefinition", null);
		return entityTypeBuilder;
	}

	public static EntityTypeBuilder<TEntity> ToView<TEntity>([JetBrains.Annotations.NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string name) where TEntity : class
	{
		return (EntityTypeBuilder<TEntity>)((EntityTypeBuilder)entityTypeBuilder).ToView(name);
	}

	public static EntityTypeBuilder ToView([JetBrains.Annotations.NotNull] this EntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		entityTypeBuilder.Metadata.SetTableName(name);
		entityTypeBuilder.Metadata.SetSchema(schema);
		entityTypeBuilder.Metadata.SetAnnotation("Relational:ViewDefinition", null);
		return entityTypeBuilder;
	}

	public static EntityTypeBuilder<TEntity> ToView<TEntity>([JetBrains.Annotations.NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema) where TEntity : class
	{
		return (EntityTypeBuilder<TEntity>)((EntityTypeBuilder)entityTypeBuilder).ToView(name, schema);
	}

	public static EntityTypeBuilder HasCheckConstraint([JetBrains.Annotations.NotNull] this EntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string sql)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(sql, "sql");
		IMutableEntityType metadata = entityTypeBuilder.Metadata;
		ICheckConstraint checkConstraint = metadata.FindCheckConstraint(name);
		if (checkConstraint != null)
		{
			if (checkConstraint.Sql == sql)
			{
				return entityTypeBuilder;
			}
			metadata.RemoveCheckConstraint(name);
		}
		if (sql != null)
		{
			metadata.AddCheckConstraint(name, sql);
		}
		return entityTypeBuilder;
	}

	public static EntityTypeBuilder<TEntity> HasCheckConstraint<TEntity>([JetBrains.Annotations.NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string sql) where TEntity : class
	{
		return (EntityTypeBuilder<TEntity>)((EntityTypeBuilder)entityTypeBuilder).HasCheckConstraint(name, sql);
	}

	public static IConventionEntityTypeBuilder HasCheckConstraint([JetBrains.Annotations.NotNull] this IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string sql, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(sql, "sql");
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		IConventionCheckConstraint conventionCheckConstraint = metadata.FindCheckConstraint(name);
		if (conventionCheckConstraint != null)
		{
			if (conventionCheckConstraint.Sql == sql)
			{
				((CheckConstraint)conventionCheckConstraint).UpdateConfigurationSource(fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
				return entityTypeBuilder;
			}
			if (!(fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention).Overrides(conventionCheckConstraint.GetConfigurationSource()))
			{
				return null;
			}
			metadata.RemoveCheckConstraint(name);
		}
		if (sql != null)
		{
			metadata.AddCheckConstraint(name, sql, fromDataAnnotation);
		}
		return entityTypeBuilder;
	}

	public static bool CanSetCheckConstraint([JetBrains.Annotations.NotNull] this IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string sql, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(sql, "sql");
		IConventionCheckConstraint conventionCheckConstraint = entityTypeBuilder.Metadata.FindCheckConstraint(name);
		if (conventionCheckConstraint != null && !(conventionCheckConstraint.Sql == sql))
		{
			return (fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention).Overrides(conventionCheckConstraint.GetConfigurationSource());
		}
		return true;
	}

	public static EntityTypeBuilder HasComment([JetBrains.Annotations.NotNull] this EntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string comment)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		entityTypeBuilder.Metadata.SetComment(comment);
		return entityTypeBuilder;
	}

	public static EntityTypeBuilder<TEntity> HasComment<TEntity>([JetBrains.Annotations.NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string comment) where TEntity : class
	{
		return (EntityTypeBuilder<TEntity>)((EntityTypeBuilder)entityTypeBuilder).HasComment(comment);
	}

	public static IConventionEntityTypeBuilder HasComment([JetBrains.Annotations.NotNull] this IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string comment, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		if (!entityTypeBuilder.CanSetComment(comment, fromDataAnnotation))
		{
			return null;
		}
		entityTypeBuilder.Metadata.SetComment(comment, fromDataAnnotation);
		return entityTypeBuilder;
	}

	public static bool CanSetComment([JetBrains.Annotations.NotNull] this IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] string comment, bool fromDataAnnotation = false)
	{
		return entityTypeBuilder.CanSetAnnotation("Relational:Comment", comment, fromDataAnnotation);
	}
}
