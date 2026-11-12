using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore;

public static class SqlServerIndexExtensions
{
	public static bool? IsClustered([JetBrains.Annotations.NotNull] this IIndex index)
	{
		return (bool?)index["SqlServer:Clustered"];
	}

	public static void SetIsClustered([JetBrains.Annotations.NotNull] this IMutableIndex index, bool? value)
	{
		index.SetOrRemoveAnnotation("SqlServer:Clustered", value);
	}

	public static void SetIsClustered([JetBrains.Annotations.NotNull] this IConventionIndex index, bool? value, bool fromDataAnnotation = false)
	{
		index.SetOrRemoveAnnotation("SqlServer:Clustered", value, fromDataAnnotation);
	}

	public static ConfigurationSource? GetIsClusteredConfigurationSource([JetBrains.Annotations.NotNull] this IConventionIndex property)
	{
		return property.FindAnnotation("SqlServer:Clustered")?.GetConfigurationSource();
	}

	public static IReadOnlyList<string> GetIncludeProperties([JetBrains.Annotations.NotNull] this IIndex index)
	{
		return (string[])index["SqlServer:Include"];
	}

	public static void SetIncludeProperties([JetBrains.Annotations.NotNull] this IMutableIndex index, [JetBrains.Annotations.NotNull] IReadOnlyList<string> properties)
	{
		index.SetOrRemoveAnnotation("SqlServer:Include", properties);
	}

	public static void SetIncludeProperties([JetBrains.Annotations.NotNull] this IConventionIndex index, [JetBrains.Annotations.NotNull] IReadOnlyList<string> properties, bool fromDataAnnotation = false)
	{
		index.SetOrRemoveAnnotation("SqlServer:Include", properties, fromDataAnnotation);
	}

	public static ConfigurationSource? GetIncludePropertiesConfigurationSource([JetBrains.Annotations.NotNull] this IConventionIndex index)
	{
		return index.FindAnnotation("SqlServer:Include")?.GetConfigurationSource();
	}

	public static bool? IsCreatedOnline([JetBrains.Annotations.NotNull] this IIndex index)
	{
		return (bool?)index["SqlServer:Online"];
	}

	public static void SetIsCreatedOnline([JetBrains.Annotations.NotNull] this IMutableIndex index, bool? createdOnline)
	{
		index.SetOrRemoveAnnotation("SqlServer:Online", createdOnline);
	}

	public static void SetIsCreatedOnline([JetBrains.Annotations.NotNull] this IConventionIndex index, bool? createdOnline, bool fromDataAnnotation = false)
	{
		index.SetOrRemoveAnnotation("SqlServer:Online", createdOnline, fromDataAnnotation);
	}

	public static ConfigurationSource? GetIsCreatedOnlineConfigurationSource([JetBrains.Annotations.NotNull] this IConventionIndex index)
	{
		return index.FindAnnotation("SqlServer:Online")?.GetConfigurationSource();
	}
}
