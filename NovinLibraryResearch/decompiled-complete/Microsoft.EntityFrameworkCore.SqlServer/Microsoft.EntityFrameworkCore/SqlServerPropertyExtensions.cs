using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class SqlServerPropertyExtensions
{
	public static string GetHiLoSequenceName([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (string)property["SqlServer:HiLoSequenceName"];
	}

	public static void SetHiLoSequenceName([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] string name)
	{
		property.SetOrRemoveAnnotation("SqlServer:HiLoSequenceName", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name"));
	}

	public static void SetHiLoSequenceName([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		property.SetOrRemoveAnnotation("SqlServer:HiLoSequenceName", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetHiLoSequenceNameConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("SqlServer:HiLoSequenceName")?.GetConfigurationSource();
	}

	public static string GetHiLoSequenceSchema([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (string)property["SqlServer:HiLoSequenceSchema"];
	}

	public static void SetHiLoSequenceSchema([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] string schema)
	{
		property.SetOrRemoveAnnotation("SqlServer:HiLoSequenceSchema", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema"));
	}

	public static void SetHiLoSequenceSchema([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
	{
		property.SetOrRemoveAnnotation("SqlServer:HiLoSequenceSchema", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetHiLoSequenceSchemaConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("SqlServer:HiLoSequenceSchema")?.GetConfigurationSource();
	}

	public static ISequence FindHiLoSequence([JetBrains.Annotations.NotNull] this IProperty property)
	{
		IModel model = property.DeclaringEntityType.Model;
		if (property.GetValueGenerationStrategy() != SqlServerValueGenerationStrategy.SequenceHiLo)
		{
			return null;
		}
		string name = property.GetHiLoSequenceName() ?? model.GetHiLoSequenceName();
		string schema = property.GetHiLoSequenceSchema() ?? model.GetHiLoSequenceSchema();
		return model.FindSequence(name, schema);
	}

	public static int? GetIdentitySeed([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (int?)property["SqlServer:IdentitySeed"];
	}

	public static void SetIdentitySeed([JetBrains.Annotations.NotNull] this IMutableProperty property, int? seed)
	{
		property.SetOrRemoveAnnotation("SqlServer:IdentitySeed", seed);
	}

	public static void SetIdentitySeed([JetBrains.Annotations.NotNull] this IConventionProperty property, int? seed, bool fromDataAnnotation = false)
	{
		property.SetOrRemoveAnnotation("SqlServer:IdentitySeed", seed, fromDataAnnotation);
	}

	public static ConfigurationSource? GetIdentitySeedConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("SqlServer:IdentitySeed")?.GetConfigurationSource();
	}

	public static int? GetIdentityIncrement([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (int?)property["SqlServer:IdentityIncrement"];
	}

	public static void SetIdentityIncrement([JetBrains.Annotations.NotNull] this IMutableProperty property, int? increment)
	{
		property.SetOrRemoveAnnotation("SqlServer:IdentityIncrement", increment);
	}

	public static void SetIdentityIncrement([JetBrains.Annotations.NotNull] this IConventionProperty property, int? increment, bool fromDataAnnotation = false)
	{
		property.SetOrRemoveAnnotation("SqlServer:IdentityIncrement", increment, fromDataAnnotation);
	}

	public static ConfigurationSource? GetIdentityIncrementConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("SqlServer:IdentityIncrement")?.GetConfigurationSource();
	}

	public static SqlServerValueGenerationStrategy GetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IProperty property)
	{
		object obj = property["SqlServer:ValueGenerationStrategy"];
		if (obj != null)
		{
			return (SqlServerValueGenerationStrategy)obj;
		}
		IProperty property2 = property.FindSharedTableRootPrimaryKeyProperty();
		if (property2 != null)
		{
			if (property2.GetValueGenerationStrategy() != SqlServerValueGenerationStrategy.IdentityColumn)
			{
				return SqlServerValueGenerationStrategy.None;
			}
			return SqlServerValueGenerationStrategy.IdentityColumn;
		}
		if (property.ValueGenerated != ValueGenerated.OnAdd || property.GetDefaultValue() != null || property.GetDefaultValueSql() != null || property.GetComputedColumnSql() != null)
		{
			return SqlServerValueGenerationStrategy.None;
		}
		SqlServerValueGenerationStrategy? valueGenerationStrategy = property.DeclaringEntityType.Model.GetValueGenerationStrategy();
		if (valueGenerationStrategy == SqlServerValueGenerationStrategy.SequenceHiLo && IsCompatibleWithValueGeneration(property))
		{
			return SqlServerValueGenerationStrategy.SequenceHiLo;
		}
		if (valueGenerationStrategy != SqlServerValueGenerationStrategy.IdentityColumn || !IsCompatibleWithValueGeneration(property))
		{
			return SqlServerValueGenerationStrategy.None;
		}
		return SqlServerValueGenerationStrategy.IdentityColumn;
	}

	public static void SetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IMutableProperty property, SqlServerValueGenerationStrategy? value)
	{
		CheckValueGenerationStrategy(property, value);
		property.SetOrRemoveAnnotation("SqlServer:ValueGenerationStrategy", value);
	}

	public static void SetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IConventionProperty property, SqlServerValueGenerationStrategy? value, bool fromDataAnnotation = false)
	{
		CheckValueGenerationStrategy(property, value);
		property.SetOrRemoveAnnotation("SqlServer:ValueGenerationStrategy", value, fromDataAnnotation);
	}

	private static void CheckValueGenerationStrategy(IProperty property, SqlServerValueGenerationStrategy? value)
	{
		if (value.HasValue)
		{
			Type clrType = property.ClrType;
			if (value == SqlServerValueGenerationStrategy.IdentityColumn && !IsCompatibleWithValueGeneration(property))
			{
				throw new ArgumentException(SqlServerStrings.IdentityBadType(property.Name, property.DeclaringEntityType.DisplayName(), clrType.ShortDisplayName()));
			}
			if (value == SqlServerValueGenerationStrategy.SequenceHiLo && !IsCompatibleWithValueGeneration(property))
			{
				throw new ArgumentException(SqlServerStrings.SequenceBadType(property.Name, property.DeclaringEntityType.DisplayName(), clrType.ShortDisplayName()));
			}
		}
	}

	public static ConfigurationSource? GetValueGenerationStrategyConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
	{
		return property.FindAnnotation("SqlServer:ValueGenerationStrategy")?.GetConfigurationSource();
	}

	public static bool IsCompatibleWithValueGeneration([JetBrains.Annotations.NotNull] IProperty property)
	{
		Type clrType = property.ClrType;
		if (clrType.IsInteger() || clrType == typeof(decimal))
		{
			return (property.FindTypeMapping()?.Converter ?? property.GetValueConverter()) == null;
		}
		return false;
	}
}
