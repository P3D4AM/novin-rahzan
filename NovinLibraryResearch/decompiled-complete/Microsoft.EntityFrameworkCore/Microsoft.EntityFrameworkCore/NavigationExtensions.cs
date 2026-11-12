using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class NavigationExtensions
{
	public static IClrCollectionAccessor GetCollectionAccessor([JetBrains.Annotations.NotNull] this INavigation navigation)
	{
		return navigation.AsNavigation("GetCollectionAccessor").CollectionAccessor;
	}

	[DebuggerStepThrough]
	public static bool IsDependentToPrincipal([JetBrains.Annotations.NotNull] this INavigation navigation)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigation, "navigation").ForeignKey.DependentToPrincipal == navigation;
	}

	[DebuggerStepThrough]
	public static bool IsCollection([JetBrains.Annotations.NotNull] this INavigation navigation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigation, "navigation");
		if (!navigation.IsDependentToPrincipal())
		{
			return !navigation.ForeignKey.IsUnique;
		}
		return false;
	}

	[DebuggerStepThrough]
	public static INavigation FindInverse([JetBrains.Annotations.NotNull] this INavigation navigation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigation, "navigation");
		if (!navigation.IsDependentToPrincipal())
		{
			return navigation.ForeignKey.DependentToPrincipal;
		}
		return navigation.ForeignKey.PrincipalToDependent;
	}

	[DebuggerStepThrough]
	public static IEntityType GetTargetType([JetBrains.Annotations.NotNull] this INavigation navigation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigation, "navigation");
		if (!navigation.IsDependentToPrincipal())
		{
			return navigation.ForeignKey.DeclaringEntityType;
		}
		return navigation.ForeignKey.PrincipalEntityType;
	}

	public static bool IsEagerLoaded([JetBrains.Annotations.NotNull] this INavigation navigation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigation, "navigation");
		return ((bool?)navigation["EagerLoaded"]).GetValueOrDefault();
	}
}
