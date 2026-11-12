using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalPropertyExtensions
{
	public static string GetColumnName([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return ((string)property["Relational:ColumnName"]) ?? property.GetDefaultColumnName();
	}

	public static string GetDefaultColumnName([JetBrains.Annotations.NotNull] this IProperty property)
	{
		IProperty property2 = property.FindSharedTableRootPrimaryKeyProperty();
		if (property2 != null)
		{
			return property2.GetColumnName();
		}
		IEntityType entityType = property.DeclaringEntityType;
		StringBuilder stringBuilder = null;
		do
		{
			IForeignKey foreignKey = entityType.GetForeignKeys().SingleOrDefault((IForeignKey fk) => fk.IsOwnership);
			if (foreignKey == null)
			{
				entityType = null;
				continue;
			}
			IEntityType principalEntityType = foreignKey.PrincipalEntityType;
			if (entityType.GetTableName() == principalEntityType.GetTableName() && entityType.GetSchema() == principalEntityType.GetSchema())
			{
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder();
				}
				stringBuilder.Insert(0, "_");
				stringBuilder.Insert(0, foreignKey.PrincipalToDependent.Name);
				entityType = principalEntityType;
			}
			else
			{
				entityType = null;
			}
		}
		while (entityType != null);
		string identifier = property.Name;
		if (stringBuilder != null)
		{
			stringBuilder.Append(property.Name);
			identifier = stringBuilder.ToString();
		}
		return Uniquifier.Truncate(identifier, property.DeclaringEntityType.Model.GetMaxIdentifierLength());
	}

	public static void SetColumnName([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] string name)
	{
		property.SetOrRemoveAnnotation("Relational:ColumnName", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name"));
	}

	public static void SetColumnName([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		property.SetOrRemoveAnnotation("Relational:ColumnName", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetColumnNameConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("Relational:ColumnName")?.GetConfigurationSource();
	}

	public static string GetColumnType([JetBrains.Annotations.NotNull] this IProperty property)
	{
		string text = (string)property["Relational:ColumnType"];
		if (text != null)
		{
			return text;
		}
		return GetDefaultColumnType(property);
	}

	private static string GetDefaultColumnType(IProperty property)
	{
		IProperty property2 = property.FindSharedTableRootPrimaryKeyProperty();
		if (property2 == null)
		{
			return property.FindRelationalMapping()?.StoreType;
		}
		return property2.GetColumnType();
	}

	public static void SetColumnType([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] string value)
	{
		property.SetOrRemoveAnnotation("Relational:ColumnType", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"));
	}

	public static void SetColumnType([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
	{
		property.SetOrRemoveAnnotation("Relational:ColumnType", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetColumnTypeConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("Relational:ColumnType")?.GetConfigurationSource();
	}

	public static string GetDefaultValueSql([JetBrains.Annotations.NotNull] this IProperty property)
	{
		string text = (string)property["Relational:DefaultValueSql"];
		if (text != null)
		{
			return text;
		}
		return property.FindSharedTableRootPrimaryKeyProperty()?.GetDefaultValueSql();
	}

	public static void SetDefaultValueSql([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] string value)
	{
		property.SetOrRemoveAnnotation("Relational:DefaultValueSql", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"));
	}

	public static void SetDefaultValueSql([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
	{
		property.SetOrRemoveAnnotation("Relational:DefaultValueSql", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetDefaultValueSqlConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("Relational:DefaultValueSql")?.GetConfigurationSource();
	}

	public static string GetComputedColumnSql([JetBrains.Annotations.NotNull] this IProperty property)
	{
		string text = (string)property["Relational:ComputedColumnSql"];
		if (text != null)
		{
			return text;
		}
		return property.FindSharedTableRootPrimaryKeyProperty()?.GetComputedColumnSql();
	}

	public static void SetComputedColumnSql([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] string value)
	{
		property.SetOrRemoveAnnotation("Relational:ComputedColumnSql", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"));
	}

	public static void SetComputedColumnSql([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
	{
		property.SetOrRemoveAnnotation("Relational:ComputedColumnSql", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetComputedColumnSqlConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("Relational:ComputedColumnSql")?.GetConfigurationSource();
	}

	public static object GetDefaultValue([JetBrains.Annotations.NotNull] this IProperty property)
	{
		object obj = property["Relational:DefaultValue"];
		if (obj != null)
		{
			return obj;
		}
		return property.FindSharedTableRootPrimaryKeyProperty()?.GetDefaultValue();
	}

	public static void SetDefaultValue([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] object value)
	{
		property.SetOrRemoveAnnotation("Relational:DefaultValue", ConvertDefaultValue(property, value));
	}

	public static void SetDefaultValue([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false)
	{
		property.SetOrRemoveAnnotation("Relational:DefaultValue", ConvertDefaultValue(property, value), fromDataAnnotation);
	}

	private static object ConvertDefaultValue([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.CanBeNull] object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return value;
		}
		Type type = value.GetType();
		if (property.ClrType.UnwrapNullableType() != type)
		{
			try
			{
				return Convert.ChangeType(value, property.ClrType, CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				throw new InvalidOperationException(RelationalStrings.IncorrectDefaultValueType(value, type, property.Name, property.ClrType, property.DeclaringEntityType.DisplayName()));
			}
		}
		return value;
	}

	public static ConfigurationSource? GetDefaultValueConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("Relational:DefaultValue")?.GetConfigurationSource();
	}

	public static bool IsFixedLength([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return ((bool?)property["Relational:IsFixedLength"]) ?? GetDefaultIsFixedLength(property);
	}

	private static bool GetDefaultIsFixedLength(IProperty property)
	{
		return property.FindSharedTableRootPrimaryKeyProperty()?.IsFixedLength() ?? false;
	}

	public static void SetIsFixedLength([JetBrains.Annotations.NotNull] this IMutableProperty property, bool? fixedLength)
	{
		property.SetOrRemoveAnnotation("Relational:IsFixedLength", fixedLength);
	}

	public static void SetIsFixedLength([JetBrains.Annotations.NotNull] this IConventionProperty property, bool? fixedLength, bool fromDataAnnotation = false)
	{
		property.SetOrRemoveAnnotation("Relational:IsFixedLength", fixedLength, fromDataAnnotation);
	}

	public static ConfigurationSource? GetIsFixedLengthConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("Relational:IsFixedLength")?.GetConfigurationSource();
	}

	[DebuggerStepThrough]
	public static RelationalTypeMapping GetRelationalTypeMapping([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (RelationalTypeMapping)property.GetTypeMapping();
	}

	[DebuggerStepThrough]
	public static RelationalTypeMapping FindRelationalMapping([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (RelationalTypeMapping)property.FindTypeMapping();
	}

	public static bool IsColumnNullable([JetBrains.Annotations.NotNull] this IProperty property)
	{
		if (!property.IsPrimaryKey())
		{
			if (property.DeclaringEntityType.BaseType == null && !property.IsNullable)
			{
				return IsTableSplitting(property.DeclaringEntityType);
			}
			return true;
		}
		return false;
	}

	private static bool IsTableSplitting(IEntityType entityType)
	{
		return entityType.FindPrimaryKey()?.Properties[0].FindSharedTableLink() != null;
	}

	public static IProperty FindSharedTableRootPrimaryKeyProperty([JetBrains.Annotations.NotNull] this IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		IProperty property2 = property;
		HashSet<IEntityType> hashSet = null;
		while (true)
		{
			IForeignKey foreignKey = property2.FindSharedTableLink();
			if (foreignKey == null)
			{
				break;
			}
			if (hashSet == null)
			{
				hashSet = new HashSet<IEntityType> { foreignKey.DeclaringEntityType };
			}
			if (!hashSet.Add(foreignKey.PrincipalEntityType))
			{
				return null;
			}
			property2 = foreignKey.PrincipalKey.Properties[foreignKey.Properties.IndexOf(property2)];
		}
		if (property2 != property)
		{
			return property2;
		}
		return null;
	}

	public static string GetComment([JetBrains.Annotations.NotNull] this IProperty property)
	{
		string text = (string)property["Relational:Comment"];
		if (text != null)
		{
			return text;
		}
		return property.FindSharedTableRootPrimaryKeyProperty()?.GetComment();
	}

	public static void SetComment([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] string comment)
	{
		property.SetOrRemoveAnnotation("Relational:Comment", comment);
	}

	public static void SetComment([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] string comment, bool fromDataAnnotation = false)
	{
		property.SetOrRemoveAnnotation("Relational:Comment", comment, fromDataAnnotation);
	}
}
