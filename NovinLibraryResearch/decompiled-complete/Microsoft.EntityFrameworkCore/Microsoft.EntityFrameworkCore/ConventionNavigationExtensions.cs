using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore;

public static class ConventionNavigationExtensions
{
	public static IConventionNavigation FindInverse([JetBrains.Annotations.NotNull] this IConventionNavigation navigation)
	{
		return (IConventionNavigation)NavigationExtensions.FindInverse(navigation);
	}

	public static IConventionEntityType GetTargetType([JetBrains.Annotations.NotNull] this IConventionNavigation navigation)
	{
		return (IConventionEntityType)NavigationExtensions.GetTargetType(navigation);
	}

	public static void SetIsEagerLoaded([JetBrains.Annotations.NotNull] this IConventionNavigation navigation, bool? eagerLoaded, bool fromDataAnnotation = false)
	{
		navigation.AsNavigation("SetIsEagerLoaded").SetIsEagerLoaded(eagerLoaded, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetIsEagerLoadedConfigurationSource([JetBrains.Annotations.NotNull] this IConventionNavigation navigation)
	{
		return navigation.FindAnnotation("EagerLoaded")?.GetConfigurationSource();
	}
}
