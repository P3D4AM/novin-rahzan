using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore;

public static class SqlServerKeyExtensions
{
	public static bool? IsClustered([JetBrains.Annotations.NotNull] this IKey key)
	{
		return ((bool?)key["SqlServer:Clustered"]) ?? GetDefaultIsClustered(key);
	}

	private static bool? GetDefaultIsClustered(IKey key)
	{
		return key.Properties[0].FindSharedTableRootPrimaryKeyProperty()?.FindContainingPrimaryKey().IsClustered();
	}

	public static void SetIsClustered([JetBrains.Annotations.NotNull] this IMutableKey key, bool? clustered)
	{
		key.SetOrRemoveAnnotation("SqlServer:Clustered", clustered);
	}

	public static void SetIsClustered([JetBrains.Annotations.NotNull] this IConventionKey key, bool? clustered, bool fromDataAnnotation = false)
	{
		key.SetOrRemoveAnnotation("SqlServer:Clustered", clustered, fromDataAnnotation);
	}

	public static ConfigurationSource? GetIsClusteredConfigurationSource([JetBrains.Annotations.NotNull] this IConventionKey key)
	{
		return key.FindAnnotation("SqlServer:Clustered")?.GetConfigurationSource();
	}
}
