using System;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class RelationalTransactionFactory : IRelationalTransactionFactory
{
	protected virtual RelationalTransactionFactoryDependencies Dependencies { get; }

	public RelationalTransactionFactory([JetBrains.Annotations.NotNull] RelationalTransactionFactoryDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual RelationalTransaction Create(IRelationalConnection connection, DbTransaction transaction, Guid transactionId, IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> logger, bool transactionOwned)
	{
		return new RelationalTransaction(connection, transaction, transactionId, logger, transactionOwned);
	}
}
