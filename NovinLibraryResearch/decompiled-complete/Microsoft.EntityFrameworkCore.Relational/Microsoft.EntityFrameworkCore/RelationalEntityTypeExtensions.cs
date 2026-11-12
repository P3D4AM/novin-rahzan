using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalEntityTypeExtensions
{
	public static string GetTableName([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		object obj;
		if (entityType.BaseType == null)
		{
			obj = (string)entityType["Relational:TableName"];
			if (obj == null)
			{
				return entityType.GetDefaultTableName();
			}
		}
		else
		{
			obj = entityType.GetRootType().GetTableName();
		}
		return (string)obj;
	}

	public static string GetDefaultTableName([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		IForeignKey foreignKey = entityType.FindOwnership();
		if (foreignKey != null && foreignKey.IsUnique)
		{
			return foreignKey.PrincipalEntityType.GetTableName();
		}
		return Uniquifier.Truncate(entityType.HasDefiningNavigation() ? (entityType.DefiningEntityType.GetTableName() + "_" + entityType.DefiningNavigationName) : entityType.ShortName(), entityType.Model.GetMaxIdentifierLength());
	}

	public static void SetTableName([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.CanBeNull] string name)
	{
		entityType.SetOrRemoveAnnotation("Relational:TableName", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name"));
	}

	public static void SetTableName([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		entityType.SetOrRemoveAnnotation("Relational:TableName", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetTableNameConfigurationSource([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return entityType.FindAnnotation("Relational:TableName")?.GetConfigurationSource();
	}

	public static string GetSchema([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		object obj;
		if (entityType.BaseType == null)
		{
			obj = (string)entityType["Relational:Schema"];
			if (obj == null)
			{
				return entityType.GetDefaultSchema();
			}
		}
		else
		{
			obj = entityType.GetRootType().GetSchema();
		}
		return (string)obj;
	}

	public static string GetDefaultSchema([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		IForeignKey foreignKey = entityType.FindOwnership();
		if (foreignKey != null && foreignKey.IsUnique)
		{
			return foreignKey.PrincipalEntityType.GetSchema();
		}
		if (!entityType.HasDefiningNavigation())
		{
			return entityType.Model.GetDefaultSchema();
		}
		return entityType.DefiningEntityType.GetSchema();
	}

	public static void SetSchema([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.CanBeNull] string value)
	{
		entityType.SetOrRemoveAnnotation("Relational:Schema", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"));
	}

	public static void SetSchema([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
	{
		entityType.SetOrRemoveAnnotation("Relational:Schema", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetSchemaConfigurationSource([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return entityType.FindAnnotation("Relational:Schema")?.GetConfigurationSource();
	}

	public static ICheckConstraint FindCheckConstraint([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		return CheckConstraint.FindCheckConstraint(entityType, name);
	}

	public static IConventionCheckConstraint FindCheckConstraint([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return (IConventionCheckConstraint)((IEntityType)entityType).FindCheckConstraint(name);
	}

	public static ICheckConstraint AddCheckConstraint([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string sql)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(sql, "sql");
		return new CheckConstraint(entityType, name, sql, ConfigurationSource.Explicit);
	}

	public static IConventionCheckConstraint AddCheckConstraint([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string sql, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(sql, "sql");
		return new CheckConstraint((IMutableEntityType)entityType, name, sql, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static bool RemoveCheckConstraint([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		return CheckConstraint.RemoveCheckConstraint(entityType, name);
	}

	public static bool RemoveCheckConstraint([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return ((IMutableEntityType)entityType).RemoveCheckConstraint(name);
	}

	public static IEnumerable<ICheckConstraint> GetCheckConstraints([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return CheckConstraint.GetCheckConstraints(entityType);
	}

	public static string GetComment([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return (string)entityType["Relational:Comment"];
	}

	public static void SetComment([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.CanBeNull] string comment)
	{
		entityType.SetOrRemoveAnnotation("Relational:Comment", comment);
	}

	public static void SetComment([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, [JetBrains.Annotations.CanBeNull] string comment, bool fromDataAnnotation = false)
	{
		entityType.SetOrRemoveAnnotation("Relational:Comment", comment, fromDataAnnotation);
	}

	public static bool IsIgnoredByMigrations([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		if (entityType.BaseType != null)
		{
			return entityType.BaseType.IsIgnoredByMigrations();
		}
		if (entityType.GetDefiningQuery() != null)
		{
			return true;
		}
		IAnnotation annotation = entityType.FindAnnotation("Relational:ViewDefinition");
		if (annotation == null)
		{
			IForeignKey foreignKey = entityType.FindOwnership();
			if (foreignKey != null && foreignKey.IsUnique && entityType.FindAnnotation("Relational:TableName") == null)
			{
				return foreignKey.PrincipalEntityType.IsIgnoredByMigrations();
			}
			return false;
		}
		return annotation.Value == null;
	}
}
