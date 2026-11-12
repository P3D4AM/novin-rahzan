using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class ConventionTypeBaseExtensions
{
	public static bool IsIgnored([JetBrains.Annotations.NotNull] this IConventionTypeBase entityType, [JetBrains.Annotations.NotNull] string memberName)
	{
		return entityType.FindIgnoredConfigurationSource(memberName).HasValue;
	}

	public static void SetPropertyAccessMode([JetBrains.Annotations.NotNull] this IConventionTypeBase entityType, PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsTypeBase("SetPropertyAccessMode").SetPropertyAccessMode(propertyAccessMode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetPropertyAccessModeConfigurationSource([JetBrains.Annotations.NotNull] this IConventionTypeBase entityType)
	{
		return entityType.FindAnnotation("PropertyAccessMode")?.GetConfigurationSource();
	}

	public static void SetNavigationAccessMode([JetBrains.Annotations.NotNull] this IConventionTypeBase entityType, PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsTypeBase("SetNavigationAccessMode").SetNavigationAccessMode(propertyAccessMode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetNavigationAccessModeConfigurationSource([JetBrains.Annotations.NotNull] this IConventionTypeBase entityType)
	{
		return entityType.FindAnnotation("NavigationAccessMode")?.GetConfigurationSource();
	}
}
