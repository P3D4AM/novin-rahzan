using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore;

public static class SqlServerEntityTypeExtensions
{
	public static bool IsMemoryOptimized([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return (entityType["SqlServer:MemoryOptimized"] as bool?).GetValueOrDefault();
	}

	public static void SetIsMemoryOptimized([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, bool memoryOptimized)
	{
		entityType.SetOrRemoveAnnotation("SqlServer:MemoryOptimized", memoryOptimized);
	}

	public static void SetIsMemoryOptimized([JetBrains.Annotations.NotNull] this IConventionEntityType entityType, bool? memoryOptimized, bool fromDataAnnotation = false)
	{
		entityType.SetOrRemoveAnnotation("SqlServer:MemoryOptimized", memoryOptimized, fromDataAnnotation);
	}

	public static ConfigurationSource? GetIsMemoryOptimizedConfigurationSource([JetBrains.Annotations.NotNull] this IConventionEntityType entityType)
	{
		return entityType.FindAnnotation("SqlServer:MemoryOptimized")?.GetConfigurationSource();
	}
}
