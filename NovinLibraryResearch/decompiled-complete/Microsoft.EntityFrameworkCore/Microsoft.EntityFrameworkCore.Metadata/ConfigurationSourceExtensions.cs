using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata;

public static class ConfigurationSourceExtensions
{
	[JetBrains.Annotations.ContractAnnotation("oldConfigurationSource:null => true")]
	public static bool Overrides(this ConfigurationSource newConfigurationSource, ConfigurationSource? oldConfigurationSource)
	{
		if (!oldConfigurationSource.HasValue)
		{
			return true;
		}
		if (newConfigurationSource == ConfigurationSource.Explicit)
		{
			return true;
		}
		if (oldConfigurationSource == ConfigurationSource.Explicit)
		{
			return false;
		}
		if (newConfigurationSource == ConfigurationSource.DataAnnotation)
		{
			return true;
		}
		return oldConfigurationSource != ConfigurationSource.DataAnnotation;
	}

	public static bool Overrides(this ConfigurationSource? newConfigurationSource, ConfigurationSource? oldConfigurationSource)
	{
		return newConfigurationSource?.Overrides(oldConfigurationSource) ?? (!oldConfigurationSource.HasValue);
	}

	public static bool OverridesStrictly(this ConfigurationSource newConfigurationSource, ConfigurationSource? oldConfigurationSource)
	{
		if (newConfigurationSource.Overrides(oldConfigurationSource))
		{
			return newConfigurationSource != oldConfigurationSource;
		}
		return false;
	}

	public static bool OverridesStrictly(this ConfigurationSource? newConfigurationSource, ConfigurationSource? oldConfigurationSource)
	{
		if (newConfigurationSource.HasValue)
		{
			return newConfigurationSource.Value.OverridesStrictly(oldConfigurationSource);
		}
		return false;
	}
}
