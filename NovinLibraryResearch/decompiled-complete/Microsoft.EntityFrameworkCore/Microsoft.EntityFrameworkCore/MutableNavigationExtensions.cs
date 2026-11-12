using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore;

public static class MutableNavigationExtensions
{
	public static IMutableNavigation FindInverse([JetBrains.Annotations.NotNull] this IMutableNavigation navigation)
	{
		return (IMutableNavigation)NavigationExtensions.FindInverse(navigation);
	}

	public static IMutableEntityType GetTargetType([JetBrains.Annotations.NotNull] this IMutableNavigation navigation)
	{
		return (IMutableEntityType)NavigationExtensions.GetTargetType(navigation);
	}

	public static void SetIsEagerLoaded([JetBrains.Annotations.NotNull] this IMutableNavigation navigation, bool? eagerLoaded)
	{
		navigation.AsNavigation("SetIsEagerLoaded").SetIsEagerLoaded(eagerLoaded, ConfigurationSource.Explicit);
	}
}
