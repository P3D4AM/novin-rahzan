using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public sealed class RelationalConnectionDependencies
{
	public IDbContextOptions ContextOptions { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> TransactionLogger { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Database.Connection> ConnectionLogger { get; }

	public INamedConnectionStringResolver ConnectionStringResolver { get; }

	public IRelationalTransactionFactory RelationalTransactionFactory { get; }

	public ICurrentDbContext CurrentContext { get; }

	[EntityFrameworkInternal]
	public RelationalConnectionDependencies([JetBrains.Annotations.NotNull] IDbContextOptions contextOptions, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> transactionLogger, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Connection> connectionLogger, [JetBrains.Annotations.NotNull] INamedConnectionStringResolver connectionStringResolver, [JetBrains.Annotations.NotNull] IRelationalTransactionFactory relationalTransactionFactory, [JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(contextOptions, "contextOptions");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(transactionLogger, "transactionLogger");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connectionLogger, "connectionLogger");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connectionStringResolver, "connectionStringResolver");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalTransactionFactory, "relationalTransactionFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		ContextOptions = contextOptions;
		TransactionLogger = transactionLogger;
		ConnectionLogger = connectionLogger;
		ConnectionStringResolver = connectionStringResolver;
		RelationalTransactionFactory = relationalTransactionFactory;
		CurrentContext = currentContext;
	}

	public RelationalConnectionDependencies With([JetBrains.Annotations.NotNull] IDbContextOptions contextOptions)
	{
		return new RelationalConnectionDependencies(contextOptions, TransactionLogger, ConnectionLogger, ConnectionStringResolver, RelationalTransactionFactory, CurrentContext);
	}

	public RelationalConnectionDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Connection> connectionLogger)
	{
		return new RelationalConnectionDependencies(ContextOptions, TransactionLogger, connectionLogger, ConnectionStringResolver, RelationalTransactionFactory, CurrentContext);
	}

	public RelationalConnectionDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> transactionLogger)
	{
		return new RelationalConnectionDependencies(ContextOptions, transactionLogger, ConnectionLogger, ConnectionStringResolver, RelationalTransactionFactory, CurrentContext);
	}

	public RelationalConnectionDependencies With([JetBrains.Annotations.NotNull] INamedConnectionStringResolver connectionStringResolver)
	{
		return new RelationalConnectionDependencies(ContextOptions, TransactionLogger, ConnectionLogger, connectionStringResolver, RelationalTransactionFactory, CurrentContext);
	}

	public RelationalConnectionDependencies With([JetBrains.Annotations.NotNull] IRelationalTransactionFactory relationalTransactionFactory)
	{
		return new RelationalConnectionDependencies(ContextOptions, TransactionLogger, ConnectionLogger, ConnectionStringResolver, relationalTransactionFactory, CurrentContext);
	}

	public RelationalConnectionDependencies With([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		return new RelationalConnectionDependencies(ContextOptions, TransactionLogger, ConnectionLogger, ConnectionStringResolver, RelationalTransactionFactory, currentContext);
	}
}
