using System;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRelationalTransactionFactory
{
	RelationalTransaction Create([JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> logger, bool transactionOwned);
}
