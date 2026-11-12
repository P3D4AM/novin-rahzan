using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class ConventionPropertyBaseExtensions
{
	public static void SetField([JetBrains.Annotations.NotNull] this IConventionPropertyBase property, [JetBrains.Annotations.CanBeNull] string fieldName, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").AsPropertyBase("SetField").SetField(fieldName, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static void SetPropertyAccessMode([JetBrains.Annotations.NotNull] this IConventionPropertyBase property, PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").AsPropertyBase("SetPropertyAccessMode").SetPropertyAccessMode(propertyAccessMode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static ConfigurationSource? GetPropertyAccessModeConfigurationSource([JetBrains.Annotations.NotNull] this IConventionPropertyBase property)
	{
		return property.FindAnnotation("PropertyAccessMode")?.GetConfigurationSource();
	}
}
