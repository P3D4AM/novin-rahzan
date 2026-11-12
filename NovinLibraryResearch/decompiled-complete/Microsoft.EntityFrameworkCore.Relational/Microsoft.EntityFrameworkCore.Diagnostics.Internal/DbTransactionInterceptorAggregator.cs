using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics.Internal;

public class DbTransactionInterceptorAggregator : InterceptorAggregator<IDbTransactionInterceptor>
{
	private sealed class CompositeDbTransactionInterceptor : IDbTransactionInterceptor, IInterceptor
	{
		private readonly IDbTransactionInterceptor[] _interceptors;

		public CompositeDbTransactionInterceptor([JetBrains.Annotations.NotNull] IEnumerable<IDbTransactionInterceptor> interceptors)
		{
			_interceptors = interceptors.ToArray();
		}

		public InterceptionResult<DbTransaction> TransactionStarting(DbConnection connection, TransactionStartingEventData eventData, InterceptionResult<DbTransaction> result)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = _interceptors[i].TransactionStarting(connection, eventData, result);
			}
			return result;
		}

		public DbTransaction TransactionStarted(DbConnection connection, TransactionEndEventData eventData, DbTransaction result)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = _interceptors[i].TransactionStarted(connection, eventData, result);
			}
			return result;
		}

		public async Task<InterceptionResult<DbTransaction>> TransactionStartingAsync(DbConnection connection, TransactionStartingEventData eventData, InterceptionResult<DbTransaction> result, CancellationToken cancellationToken = default(CancellationToken))
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = await _interceptors[i].TransactionStartingAsync(connection, eventData, result, cancellationToken);
			}
			return result;
		}

		public async Task<DbTransaction> TransactionStartedAsync(DbConnection connection, TransactionEndEventData eventData, DbTransaction result, CancellationToken cancellationToken = default(CancellationToken))
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = await _interceptors[i].TransactionStartedAsync(connection, eventData, result, cancellationToken);
			}
			return result;
		}

		public DbTransaction TransactionUsed(DbConnection connection, TransactionEventData eventData, DbTransaction result)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = _interceptors[i].TransactionUsed(connection, eventData, result);
			}
			return result;
		}

		public async Task<DbTransaction> TransactionUsedAsync(DbConnection connection, TransactionEventData eventData, DbTransaction result, CancellationToken cancellationToken = default(CancellationToken))
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = await _interceptors[i].TransactionUsedAsync(connection, eventData, result, cancellationToken);
			}
			return result;
		}

		public InterceptionResult TransactionCommitting(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = _interceptors[i].TransactionCommitting(transaction, eventData, result);
			}
			return result;
		}

		public void TransactionCommitted(DbTransaction transaction, TransactionEndEventData eventData)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				_interceptors[i].TransactionCommitted(transaction, eventData);
			}
		}

		public async Task<InterceptionResult> TransactionCommittingAsync(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default(CancellationToken))
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = await _interceptors[i].TransactionCommittingAsync(transaction, eventData, result, cancellationToken);
			}
			return result;
		}

		public async Task TransactionCommittedAsync(DbTransaction transaction, TransactionEndEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				await _interceptors[i].TransactionCommittedAsync(transaction, eventData, cancellationToken);
			}
		}

		public InterceptionResult TransactionRollingBack(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = _interceptors[i].TransactionRollingBack(transaction, eventData, result);
			}
			return result;
		}

		public void TransactionRolledBack(DbTransaction transaction, TransactionEndEventData eventData)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				_interceptors[i].TransactionRolledBack(transaction, eventData);
			}
		}

		public async Task<InterceptionResult> TransactionRollingBackAsync(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default(CancellationToken))
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = await _interceptors[i].TransactionRollingBackAsync(transaction, eventData, result, cancellationToken);
			}
			return result;
		}

		public async Task TransactionRolledBackAsync(DbTransaction transaction, TransactionEndEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				await _interceptors[i].TransactionRolledBackAsync(transaction, eventData, cancellationToken);
			}
		}

		public void TransactionFailed(DbTransaction transaction, TransactionErrorEventData eventData)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				_interceptors[i].TransactionFailed(transaction, eventData);
			}
		}

		public async Task TransactionFailedAsync(DbTransaction transaction, TransactionErrorEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				await _interceptors[i].TransactionFailedAsync(transaction, eventData, cancellationToken);
			}
		}
	}

	protected override IDbTransactionInterceptor CreateChain(IEnumerable<IDbTransactionInterceptor> interceptors)
	{
		return new CompositeDbTransactionInterceptor(interceptors);
	}
}
