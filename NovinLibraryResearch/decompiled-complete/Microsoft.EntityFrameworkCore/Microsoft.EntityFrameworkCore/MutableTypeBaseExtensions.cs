using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class MutableTypeBaseExtensions
{
	public static void SetPropertyAccessMode([JetBrains.Annotations.NotNull] this IMutableTypeBase entityType, PropertyAccessMode? propertyAccessMode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsTypeBase("SetPropertyAccessMode").SetPropertyAccessMode(propertyAccessMode, ConfigurationSource.Explicit);
	}

	public static void SetNavigationAccessMode([JetBrains.Annotations.NotNull] this IMutableTypeBase entityType, PropertyAccessMode? propertyAccessMode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsTypeBase("SetNavigationAccessMode").SetNavigationAccessMode(propertyAccessMode, ConfigurationSource.Explicit);
	}
}
