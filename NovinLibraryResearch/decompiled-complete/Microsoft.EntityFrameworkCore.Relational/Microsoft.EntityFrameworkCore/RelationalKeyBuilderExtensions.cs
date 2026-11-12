using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalKeyBuilderExtensions
{
	public static KeyBuilder HasName([JetBrains.Annotations.NotNull] this KeyBuilder keyBuilder, [JetBrains.Annotations.CanBeNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyBuilder, "keyBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		keyBuilder.Metadata.SetName(name);
		return keyBuilder;
	}

	public static IConventionKeyBuilder HasName([JetBrains.Annotations.NotNull] this IConventionKeyBuilder keyBuilder, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		if (keyBuilder.CanSetName(name, fromDataAnnotation))
		{
			keyBuilder.Metadata.SetName(name, fromDataAnnotation);
			return keyBuilder;
		}
		return null;
	}

	public static bool CanSetName([JetBrains.Annotations.NotNull] this IConventionKeyBuilder keyBuilder, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		return keyBuilder.CanSetAnnotation("Relational:Name", name, fromDataAnnotation);
	}
}
