using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IDbContextTransactionManager : IResettableService
{
	IDbContextTransaction CurrentTransaction { get; }

	IDbContextTransaction BeginTransaction();

	Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));

	void CommitTransaction();

	void RollbackTransaction();
}
