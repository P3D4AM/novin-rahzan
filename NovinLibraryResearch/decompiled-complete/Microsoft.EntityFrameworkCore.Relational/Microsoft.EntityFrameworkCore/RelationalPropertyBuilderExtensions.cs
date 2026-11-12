using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalPropertyBuilderExtensions
{
	public static PropertyBuilder HasColumnName([JetBrains.Annotations.NotNull] this PropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		propertyBuilder.Metadata.SetColumnName(name);
		return propertyBuilder;
	}

	public static PropertyBuilder<TProperty> HasColumnName<TProperty>([JetBrains.Annotations.NotNull] this PropertyBuilder<TProperty> propertyBuilder, [JetBrains.Annotations.CanBeNull] string name)
	{
		return (PropertyBuilder<TProperty>)((PropertyBuilder)propertyBuilder).HasColumnName(name);
	}

	public static IConventionPropertyBuilder HasColumnName([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		if (!propertyBuilder.CanSetColumnName(name, fromDataAnnotation))
		{
			return null;
		}
		propertyBuilder.Metadata.SetColumnName(name, fromDataAnnotation);
		return propertyBuilder;
	}

	public static bool CanSetColumnName([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		return propertyBuilder.CanSetAnnotation("Relational:ColumnName", name, fromDataAnnotation);
	}

	public static PropertyBuilder HasColumnType([JetBrains.Annotations.NotNull] this PropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string typeName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(typeName, "typeName");
		propertyBuilder.Metadata.SetColumnType(typeName);
		return propertyBuilder;
	}

	public static PropertyBuilder<TProperty> HasColumnType<TProperty>([JetBrains.Annotations.NotNull] this PropertyBuilder<TProperty> propertyBuilder, [JetBrains.Annotations.CanBeNull] string typeName)
	{
		return (PropertyBuilder<TProperty>)((PropertyBuilder)propertyBuilder).HasColumnType(typeName);
	}

	public static IConventionPropertyBuilder HasColumnType([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string typeName, bool fromDataAnnotation = false)
	{
		if (!propertyBuilder.CanSetColumnType(typeName, fromDataAnnotation))
		{
			return null;
		}
		propertyBuilder.Metadata.SetColumnType(typeName, fromDataAnnotation);
		return propertyBuilder;
	}

	public static bool CanSetColumnType([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string typeName, bool fromDataAnnotation = false)
	{
		return propertyBuilder.CanSetAnnotation("Relational:ColumnType", typeName, fromDataAnnotation);
	}

	public static PropertyBuilder IsFixedLength([JetBrains.Annotations.NotNull] this PropertyBuilder propertyBuilder, bool fixedLength = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
		propertyBuilder.Metadata.SetIsFixedLength(fixedLength);
		return propertyBuilder;
	}

	public static PropertyBuilder<TProperty> IsFixedLength<TProperty>([JetBrains.Annotations.NotNull] this PropertyBuilder<TProperty> propertyBuilder, bool fixedLength = true)
	{
		return (PropertyBuilder<TProperty>)((PropertyBuilder)propertyBuilder).IsFixedLength(fixedLength);
	}

	public static IConventionPropertyBuilder IsFixedLength([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, bool? fixedLength, bool fromDataAnnotation = false)
	{
		if (!propertyBuilder.CanSetIsFixedLength(fixedLength, fromDataAnnotation))
		{
			return null;
		}
		propertyBuilder.Metadata.SetIsFixedLength(fixedLength, fromDataAnnotation);
		return propertyBuilder;
	}

	public static bool CanSetIsFixedLength([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, bool? fixedLength, bool fromDataAnnotation = false)
	{
		return propertyBuilder.CanSetAnnotation("Relational:IsFixedLength", fixedLength, fromDataAnnotation);
	}

	public static PropertyBuilder HasDefaultValueSql([JetBrains.Annotations.NotNull] this PropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string sql)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(sql, "sql");
		propertyBuilder.Metadata.SetDefaultValueSql(sql);
		return propertyBuilder;
	}

	public static PropertyBuilder<TProperty> HasDefaultValueSql<TProperty>([JetBrains.Annotations.NotNull] this PropertyBuilder<TProperty> propertyBuilder, [JetBrains.Annotations.CanBeNull] string sql)
	{
		return (PropertyBuilder<TProperty>)((PropertyBuilder)propertyBuilder).HasDefaultValueSql(sql);
	}

	public static IConventionPropertyBuilder HasDefaultValueSql([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string sql, bool fromDataAnnotation = false)
	{
		if (!propertyBuilder.CanSetDefaultValueSql(sql, fromDataAnnotation))
		{
			return null;
		}
		propertyBuilder.Metadata.SetDefaultValueSql(sql, fromDataAnnotation);
		return propertyBuilder;
	}

	public static bool CanSetDefaultValueSql([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string sql, bool fromDataAnnotation = false)
	{
		return propertyBuilder.CanSetAnnotation("Relational:DefaultValueSql", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(sql, "sql"), fromDataAnnotation);
	}

	public static PropertyBuilder HasComputedColumnSql([JetBrains.Annotations.NotNull] this PropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string sql)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(sql, "sql");
		propertyBuilder.Metadata.SetComputedColumnSql(sql);
		return propertyBuilder;
	}

	public static PropertyBuilder<TProperty> HasComputedColumnSql<TProperty>([JetBrains.Annotations.NotNull] this PropertyBuilder<TProperty> propertyBuilder, [JetBrains.Annotations.CanBeNull] string sql)
	{
		return (PropertyBuilder<TProperty>)((PropertyBuilder)propertyBuilder).HasComputedColumnSql(sql);
	}

	public static IConventionPropertyBuilder HasComputedColumnSql([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string sql, bool fromDataAnnotation = false)
	{
		if (!propertyBuilder.CanSetComputedColumnSql(sql, fromDataAnnotation))
		{
			return null;
		}
		propertyBuilder.Metadata.SetComputedColumnSql(sql, fromDataAnnotation);
		return propertyBuilder;
	}

	public static bool CanSetComputedColumnSql([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string sql, bool fromDataAnnotation = false)
	{
		return propertyBuilder.CanSetAnnotation("Relational:ComputedColumnSql", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(sql, "sql"), fromDataAnnotation);
	}

	public static PropertyBuilder HasDefaultValue([JetBrains.Annotations.NotNull] this PropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] object value = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
		propertyBuilder.Metadata.SetDefaultValue(value ?? DBNull.Value);
		return propertyBuilder;
	}

	public static PropertyBuilder<TProperty> HasDefaultValue<TProperty>([JetBrains.Annotations.NotNull] this PropertyBuilder<TProperty> propertyBuilder, [JetBrains.Annotations.CanBeNull] object value = null)
	{
		return (PropertyBuilder<TProperty>)((PropertyBuilder)propertyBuilder).HasDefaultValue(value);
	}

	public static IConventionPropertyBuilder HasDefaultValue([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false)
	{
		if (!propertyBuilder.CanSetDefaultValue(value, fromDataAnnotation))
		{
			return null;
		}
		propertyBuilder.Metadata.SetDefaultValue(value, fromDataAnnotation);
		return propertyBuilder;
	}

	public static bool CanSetDefaultValue([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false)
	{
		return propertyBuilder.CanSetAnnotation("Relational:DefaultValue", value, fromDataAnnotation);
	}

	public static PropertyBuilder HasComment([JetBrains.Annotations.NotNull] this PropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string comment)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
		propertyBuilder.Metadata.SetComment(comment);
		return propertyBuilder;
	}

	public static PropertyBuilder<TProperty> HasComment<TProperty>([JetBrains.Annotations.NotNull] this PropertyBuilder<TProperty> propertyBuilder, [JetBrains.Annotations.CanBeNull] string comment)
	{
		return (PropertyBuilder<TProperty>)((PropertyBuilder)propertyBuilder).HasComment(comment);
	}

	public static IConventionPropertyBuilder HasComment([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string comment, bool fromDataAnnotation = false)
	{
		if (!propertyBuilder.CanSetComment(comment, fromDataAnnotation))
		{
			return null;
		}
		propertyBuilder.Metadata.SetComment(comment, fromDataAnnotation);
		return propertyBuilder;
	}

	public static bool CanSetComment([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] object comment, bool fromDataAnnotation = false)
	{
		return propertyBuilder.CanSetAnnotation("Relational:Comment", comment, fromDataAnnotation);
	}
}
