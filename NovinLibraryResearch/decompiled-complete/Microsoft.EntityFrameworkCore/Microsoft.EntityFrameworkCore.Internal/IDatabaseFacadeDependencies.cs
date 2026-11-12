using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IDatabaseFacadeDependencies
{
	IDbContextTransactionManager TransactionManager { get; }

	IDatabaseCreator DatabaseCreator { get; }

	IExecutionStrategyFactory ExecutionStrategyFactory { get; }

	IEnumerable<IDatabaseProvider> DatabaseProviders { get; }

	IDiagnosticsLogger<DbLoggerCategory.Database.Command> CommandLogger { get; }

	IConcurrencyDetector ConcurrencyDetector { get; }
}
