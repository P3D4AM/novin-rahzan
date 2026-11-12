using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Internal;

public class DatabaseFacadeDependencies : IDatabaseFacadeDependencies
{
	public virtual IDbContextTransactionManager TransactionManager { get; }

	public virtual IDatabaseCreator DatabaseCreator { get; }

	public virtual IExecutionStrategyFactory ExecutionStrategyFactory { get; }

	public virtual IEnumerable<IDatabaseProvider> DatabaseProviders { get; }

	public virtual IDiagnosticsLogger<DbLoggerCategory.Database.Command> CommandLogger { get; }

	public virtual IConcurrencyDetector ConcurrencyDetector { get; }

	public virtual DbContext Context => null;

	public DatabaseFacadeDependencies([JetBrains.Annotations.NotNull] IDbContextTransactionManager transactionManager, [JetBrains.Annotations.NotNull] IDatabaseCreator databaseCreator, [JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory, [JetBrains.Annotations.NotNull] IEnumerable<IDatabaseProvider> databaseProviders, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger, [JetBrains.Annotations.NotNull] IConcurrencyDetector concurrencyDetector)
	{
		TransactionManager = transactionManager;
		DatabaseCreator = databaseCreator;
		ExecutionStrategyFactory = executionStrategyFactory;
		DatabaseProviders = databaseProviders;
		CommandLogger = commandLogger;
		ConcurrencyDetector = concurrencyDetector;
	}
}
