using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class ConfigurationSourceExtensions
{
	[JetBrains.Annotations.ContractAnnotation("left:notnull => notnull;right:notnull => notnull")]
	public static ConfigurationSource? Max(this ConfigurationSource? left, ConfigurationSource? right)
	{
		if (!left.Overrides(right))
		{
			return right;
		}
		return left;
	}

	public static ConfigurationSource Max(this ConfigurationSource left, ConfigurationSource? right)
	{
		if (!left.Overrides(right))
		{
			return right.Value;
		}
		return left;
	}
}
