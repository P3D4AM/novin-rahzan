using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class SqlServerModelExtensions
{
	public const string DefaultHiLoSequenceName = "EntityFrameworkHiLoSequence";

	public static string GetHiLoSequenceName([JetBrains.Annotations.NotNull] this IModel model)
	{
		return ((string)model["SqlServer:HiLoSequenceName"]) ?? "EntityFrameworkHiLoSequence";
	}

	public static void SetHiLoSequenceName([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.CanBeNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		model.SetOrRemoveAnnotation("SqlServer:HiLoSequenceName", name);
	}

	public static void SetHiLoSequenceName([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		model.SetOrRemoveAnnotation("SqlServer:HiLoSequenceName", name, fromDataAnnotation);
	}

	public static ConfigurationSource? GetHiLoSequenceNameConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return model.FindAnnotation("SqlServer:HiLoSequenceName")?.GetConfigurationSource();
	}

	public static string GetHiLoSequenceSchema([JetBrains.Annotations.NotNull] this IModel model)
	{
		return (string)model["SqlServer:HiLoSequenceSchema"];
	}

	public static void SetHiLoSequenceSchema([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.CanBeNull] string value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value");
		model.SetOrRemoveAnnotation("SqlServer:HiLoSequenceSchema", value);
	}

	public static void SetHiLoSequenceSchema([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value");
		model.SetOrRemoveAnnotation("SqlServer:HiLoSequenceSchema", value, fromDataAnnotation);
	}

	public static ConfigurationSource? GetHiLoSequenceSchemaConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return model.FindAnnotation("SqlServer:HiLoSequenceSchema")?.GetConfigurationSource();
	}

	public static int GetIdentitySeed([JetBrains.Annotations.NotNull] this IModel model)
	{
		return ((int?)model["SqlServer:IdentitySeed"]) ?? 1;
	}

	public static void SetIdentitySeed([JetBrains.Annotations.NotNull] this IMutableModel model, int? seed)
	{
		model.SetOrRemoveAnnotation("SqlServer:IdentitySeed", seed);
	}

	public static void SetIdentitySeed([JetBrains.Annotations.NotNull] this IConventionModel model, int? seed, bool fromDataAnnotation = false)
	{
		model.SetOrRemoveAnnotation("SqlServer:IdentitySeed", seed, fromDataAnnotation);
	}

	public static ConfigurationSource? GetIdentitySeedConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return model.FindAnnotation("SqlServer:IdentitySeed")?.GetConfigurationSource();
	}

	public static int GetIdentityIncrement([JetBrains.Annotations.NotNull] this IModel model)
	{
		return ((int?)model["SqlServer:IdentityIncrement"]) ?? 1;
	}

	public static void SetIdentityIncrement([JetBrains.Annotations.NotNull] this IMutableModel model, int? increment)
	{
		model.SetOrRemoveAnnotation("SqlServer:IdentityIncrement", increment);
	}

	public static void SetIdentityIncrement([JetBrains.Annotations.NotNull] this IConventionModel model, int? increment, bool fromDataAnnotation = false)
	{
		model.SetOrRemoveAnnotation("SqlServer:IdentityIncrement", increment, fromDataAnnotation);
	}

	public static ConfigurationSource? GetIdentityIncrementConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return model.FindAnnotation("SqlServer:IdentityIncrement")?.GetConfigurationSource();
	}

	public static SqlServerValueGenerationStrategy? GetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IModel model)
	{
		return (SqlServerValueGenerationStrategy?)model["SqlServer:ValueGenerationStrategy"];
	}

	public static void SetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IMutableModel model, SqlServerValueGenerationStrategy? value)
	{
		model.SetOrRemoveAnnotation("SqlServer:ValueGenerationStrategy", value);
	}

	public static void SetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IConventionModel model, SqlServerValueGenerationStrategy? value, bool fromDataAnnotation = false)
	{
		model.SetOrRemoveAnnotation("SqlServer:ValueGenerationStrategy", value, fromDataAnnotation);
	}

	public static ConfigurationSource? GetValueGenerationStrategyConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return model.FindAnnotation("SqlServer:ValueGenerationStrategy")?.GetConfigurationSource();
	}

	public static string GetDatabaseMaxSize([JetBrains.Annotations.NotNull] this IModel model)
	{
		return (string)model["SqlServer:DatabaseMaxSize"];
	}

	public static void SetDatabaseMaxSize([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.CanBeNull] string value)
	{
		model.SetOrRemoveAnnotation("SqlServer:DatabaseMaxSize", value);
	}

	public static void SetDatabaseMaxSize([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
	{
		model.SetOrRemoveAnnotation("SqlServer:DatabaseMaxSize", value, fromDataAnnotation);
	}

	public static ConfigurationSource? GetDatabaseMaxSizeConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return model.FindAnnotation("SqlServer:DatabaseMaxSize")?.GetConfigurationSource();
	}

	public static string GetServiceTierSql([JetBrains.Annotations.NotNull] this IModel model)
	{
		return (string)model["SqlServer:ServiceTierSql"];
	}

	public static void SetServiceTierSql([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.CanBeNull] string value)
	{
		model.SetOrRemoveAnnotation("SqlServer:ServiceTierSql", value);
	}

	public static void SetServiceTierSql([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
	{
		model.SetOrRemoveAnnotation("SqlServer:ServiceTierSql", value, fromDataAnnotation);
	}

	public static ConfigurationSource? GetServiceTierSqlConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return model.FindAnnotation("SqlServer:ServiceTierSql")?.GetConfigurationSource();
	}

	public static string GetPerformanceLevelSql([JetBrains.Annotations.NotNull] this IModel model)
	{
		return (string)model["SqlServer:PerformanceLevelSql"];
	}

	public static void SetPerformanceLevelSql([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.CanBeNull] string value)
	{
		model.SetOrRemoveAnnotation("SqlServer:PerformanceLevelSql", value);
	}

	public static void SetPerformanceLevelSql([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
	{
		model.SetOrRemoveAnnotation("SqlServer:PerformanceLevelSql", value, fromDataAnnotation);
	}

	public static ConfigurationSource? GetPerformanceLevelSqlConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return model.FindAnnotation("SqlServer:PerformanceLevelSql")?.GetConfigurationSource();
	}
}
