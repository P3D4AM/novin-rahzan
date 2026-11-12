using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Internal;

public class RelationalDatabaseFacadeDependencies : DatabaseFacadeDependencies, IRelationalDatabaseFacadeDependencies, IDatabaseFacadeDependencies
{
	public virtual IRelationalConnection RelationalConnection { get; }

	public virtual IRawSqlCommandBuilder RawSqlCommandBuilder { get; }

	public RelationalDatabaseFacadeDependencies([JetBrains.Annotations.NotNull] IDbContextTransactionManager transactionManager, [JetBrains.Annotations.NotNull] IDatabaseCreator databaseCreator, [JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory, [JetBrains.Annotations.NotNull] IEnumerable<IDatabaseProvider> databaseProviders, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger, [JetBrains.Annotations.NotNull] IConcurrencyDetector concurrencyDetector, [JetBrains.Annotations.NotNull] IRelationalConnection relationalConnection, [JetBrains.Annotations.NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder)
		: base(transactionManager, databaseCreator, executionStrategyFactory, databaseProviders, commandLogger, concurrencyDetector)
	{
		RelationalConnection = relationalConnection;
		RawSqlCommandBuilder = rawSqlCommandBuilder;
	}
}
