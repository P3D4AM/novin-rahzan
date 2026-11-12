using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public sealed class ExecutionStrategyDependencies
{
	public IDbContextOptions Options { get; }

	public ICurrentDbContext CurrentContext { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Infrastructure> Logger { get; }

	[EntityFrameworkInternal]
	public ExecutionStrategyDependencies([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.CanBeNull] IDbContextOptions options, [JetBrains.Annotations.CanBeNull] IDiagnosticsLogger<DbLoggerCategory.Infrastructure> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		Options = options;
		CurrentContext = currentContext;
		Logger = logger;
	}

	public ExecutionStrategyDependencies With([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		return new ExecutionStrategyDependencies(currentContext, Options, Logger);
	}

	public ExecutionStrategyDependencies With([JetBrains.Annotations.NotNull] IDbContextOptions options)
	{
		return new ExecutionStrategyDependencies(CurrentContext, options, Logger);
	}

	public ExecutionStrategyDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Infrastructure> logger)
	{
		return new ExecutionStrategyDependencies(CurrentContext, Options, logger);
	}
}
