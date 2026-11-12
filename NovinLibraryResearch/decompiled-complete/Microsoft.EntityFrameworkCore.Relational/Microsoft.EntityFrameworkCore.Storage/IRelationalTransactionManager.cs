using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRelationalTransactionManager : IDbContextTransactionManager, IResettableService
{
	IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);

	Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default(CancellationToken));

	IDbContextTransaction UseTransaction([JetBrains.Annotations.CanBeNull] DbTransaction transaction);

	Task<IDbContextTransaction> UseTransactionAsync([JetBrains.Annotations.CanBeNull] DbTransaction transaction, CancellationToken cancellationToken = default(CancellationToken));
}
