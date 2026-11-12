using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class SqlServerKeyBuilderExtensions
{
	public static KeyBuilder IsClustered([JetBrains.Annotations.NotNull] this KeyBuilder keyBuilder, bool clustered = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyBuilder, "keyBuilder");
		keyBuilder.Metadata.SetIsClustered(clustered);
		return keyBuilder;
	}

	public static IConventionKeyBuilder IsClustered([JetBrains.Annotations.NotNull] this IConventionKeyBuilder keyBuilder, bool? clustered, bool fromDataAnnotation = false)
	{
		if (keyBuilder.CanSetIsClustered(clustered, fromDataAnnotation))
		{
			keyBuilder.Metadata.SetIsClustered(clustered, fromDataAnnotation);
			return keyBuilder;
		}
		return null;
	}

	public static bool CanSetIsClustered([JetBrains.Annotations.NotNull] this IConventionKeyBuilder keyBuilder, bool? clustered, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyBuilder, "keyBuilder");
		return keyBuilder.CanSetAnnotation("SqlServer:Clustered", clustered, fromDataAnnotation);
	}

	[Obsolete("Use IsClustered")]
	public static KeyBuilder ForSqlServerIsClustered([JetBrains.Annotations.NotNull] this KeyBuilder keyBuilder, bool clustered = true)
	{
		return keyBuilder.IsClustered(clustered);
	}

	[Obsolete("Use IsClustered")]
	public static IConventionKeyBuilder ForSqlServerIsClustered([JetBrains.Annotations.NotNull] this IConventionKeyBuilder keyBuilder, bool? clustered, bool fromDataAnnotation = false)
	{
		return keyBuilder.IsClustered(clustered, fromDataAnnotation);
	}
}
