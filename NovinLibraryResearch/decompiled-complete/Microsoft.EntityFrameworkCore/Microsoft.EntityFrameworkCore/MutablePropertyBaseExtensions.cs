using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class MutablePropertyBaseExtensions
{
	public static void SetField([JetBrains.Annotations.NotNull] this IMutablePropertyBase property, [JetBrains.Annotations.CanBeNull] string fieldName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").AsPropertyBase("SetField").SetField(fieldName, ConfigurationSource.Explicit);
	}

	public static void SetPropertyAccessMode([JetBrains.Annotations.NotNull] this IMutablePropertyBase property, PropertyAccessMode? propertyAccessMode)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").AsPropertyBase("SetPropertyAccessMode").SetPropertyAccessMode(propertyAccessMode, ConfigurationSource.Explicit);
	}
}
