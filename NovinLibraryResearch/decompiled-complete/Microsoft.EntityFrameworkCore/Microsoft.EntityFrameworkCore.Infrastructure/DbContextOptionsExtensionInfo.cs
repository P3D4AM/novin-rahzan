using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public abstract class DbContextOptionsExtensionInfo
{
	public virtual IDbContextOptionsExtension Extension { get; }

	public abstract bool IsDatabaseProvider { get; }

	public abstract string LogFragment { get; }

	protected DbContextOptionsExtensionInfo(IDbContextOptionsExtension extension)
	{
		Extension = extension;
	}

	public abstract long GetServiceProviderHashCode();

	public abstract void PopulateDebugInfo([JetBrains.Annotations.NotNull] IDictionary<string, string> debugInfo);
}
