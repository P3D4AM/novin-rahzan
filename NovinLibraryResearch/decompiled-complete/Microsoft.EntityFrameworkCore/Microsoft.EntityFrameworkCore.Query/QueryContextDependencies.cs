using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class QueryContextDependencies
{
	public ICurrentDbContext CurrentContext { get; }

	[EntityFrameworkInternal]
	public IStateManager StateManager => CurrentContext.GetDependencies().StateManager;

	public IQueryProvider QueryProvider => CurrentContext.GetDependencies().QueryProvider;

	public IExecutionStrategyFactory ExecutionStrategyFactory { get; }

	public IConcurrencyDetector ConcurrencyDetector { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Database.Command> CommandLogger { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Query> QueryLogger { get; }

	[EntityFrameworkInternal]
	public QueryContextDependencies([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory, [JetBrains.Annotations.NotNull] IConcurrencyDetector concurrencyDetector, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Query> queryLogger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executionStrategyFactory, "executionStrategyFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(concurrencyDetector, "concurrencyDetector");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandLogger, "commandLogger");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(queryLogger, "queryLogger");
		CurrentContext = currentContext;
		ExecutionStrategyFactory = executionStrategyFactory;
		ConcurrencyDetector = concurrencyDetector;
		CommandLogger = commandLogger;
		QueryLogger = queryLogger;
	}

	public QueryContextDependencies With([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		return new QueryContextDependencies(currentContext, ExecutionStrategyFactory, ConcurrencyDetector, CommandLogger, QueryLogger);
	}

	public QueryContextDependencies With([JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactor)
	{
		return new QueryContextDependencies(CurrentContext, executionStrategyFactor, ConcurrencyDetector, CommandLogger, QueryLogger);
	}

	public QueryContextDependencies With([JetBrains.Annotations.NotNull] IConcurrencyDetector concurrencyDetector)
	{
		return new QueryContextDependencies(CurrentContext, ExecutionStrategyFactory, concurrencyDetector, CommandLogger, QueryLogger);
	}

	public QueryContextDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger)
	{
		return new QueryContextDependencies(CurrentContext, ExecutionStrategyFactory, ConcurrencyDetector, commandLogger, QueryLogger);
	}

	public QueryContextDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Query> queryLogger)
	{
		return new QueryContextDependencies(CurrentContext, ExecutionStrategyFactory, ConcurrencyDetector, CommandLogger, queryLogger);
	}
}
