using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class RelationalCompiledQueryCacheKeyGeneratorDependencies
{
	public IDbContextOptions ContextOptions { get; }

	[EntityFrameworkInternal]
	public RelationalCompiledQueryCacheKeyGeneratorDependencies([JetBrains.Annotations.NotNull] IDbContextOptions contextOptions)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(contextOptions, "contextOptions");
		ContextOptions = contextOptions;
	}

	public RelationalCompiledQueryCacheKeyGeneratorDependencies With([JetBrains.Annotations.NotNull] IDbContextOptions contextOptions)
	{
		return new RelationalCompiledQueryCacheKeyGeneratorDependencies(contextOptions);
	}
}
