using System;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class RelationalTransaction : IDbContextTransaction, IDisposable, IAsyncDisposable, IInfrastructure<DbTransaction>
{
	private readonly DbTransaction _dbTransaction;

	private readonly bool _transactionOwned;

	private bool _connectionClosed;

	private bool _disposed;

	protected virtual IRelationalConnection Connection { get; }

	protected virtual IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> Logger { get; }

	public virtual Guid TransactionId { get; }

	DbTransaction IInfrastructure<DbTransaction>.Instance => _dbTransaction;

	public RelationalTransaction([JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> logger, bool transactionOwned)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(transaction, "transaction");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logger, "logger");
		if (connection.DbConnection != transaction.Connection)
		{
			throw new InvalidOperationException(RelationalStrings.TransactionAssociatedWithDifferentConnection);
		}
		Connection = connection;
		TransactionId = transactionId;
		_dbTransaction = transaction;
		Logger = logger;
		_transactionOwned = transactionOwned;
	}

	public virtual void Commit()
	{
		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			if (!Logger.TransactionCommitting(Connection, _dbTransaction, TransactionId, utcNow).IsSuppressed)
			{
				_dbTransaction.Commit();
			}
			Logger.TransactionCommitted(Connection, _dbTransaction, TransactionId, utcNow, stopwatch.Elapsed);
		}
		catch (Exception exception)
		{
			Logger.TransactionError(Connection, _dbTransaction, TransactionId, "Commit", exception, utcNow, stopwatch.Elapsed);
			throw;
		}
		ClearTransaction();
	}

	public virtual void Rollback()
	{
		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			if (!Logger.TransactionRollingBack(Connection, _dbTransaction, TransactionId, utcNow).IsSuppressed)
			{
				_dbTransaction.Rollback();
			}
			Logger.TransactionRolledBack(Connection, _dbTransaction, TransactionId, utcNow, stopwatch.Elapsed);
		}
		catch (Exception exception)
		{
			Logger.TransactionError(Connection, _dbTransaction, TransactionId, "Rollback", exception, utcNow, stopwatch.Elapsed);
			throw;
		}
		ClearTransaction();
	}

	public virtual async Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		DateTimeOffset startTime = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			if (!(await Logger.TransactionCommittingAsync(Connection, _dbTransaction, TransactionId, startTime, cancellationToken)).IsSuppressed)
			{
				await DbTransactionExtensions.CommitAsync(_dbTransaction, cancellationToken);
			}
			await Logger.TransactionCommittedAsync(Connection, _dbTransaction, TransactionId, startTime, stopwatch.Elapsed, cancellationToken);
		}
		catch (Exception exception)
		{
			await Logger.TransactionErrorAsync(Connection, _dbTransaction, TransactionId, "Commit", exception, startTime, stopwatch.Elapsed, cancellationToken);
			throw;
		}
		await ClearTransactionAsync(cancellationToken);
	}

	public virtual async Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		DateTimeOffset startTime = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			if (!(await Logger.TransactionRollingBackAsync(Connection, _dbTransaction, TransactionId, startTime, cancellationToken)).IsSuppressed)
			{
				await DbTransactionExtensions.RollbackAsync(_dbTransaction, cancellationToken);
			}
			await Logger.TransactionRolledBackAsync(Connection, _dbTransaction, TransactionId, startTime, stopwatch.Elapsed, cancellationToken);
		}
		catch (Exception exception)
		{
			await Logger.TransactionErrorAsync(Connection, _dbTransaction, TransactionId, "Rollback", exception, startTime, stopwatch.Elapsed, cancellationToken);
			throw;
		}
		await ClearTransactionAsync(cancellationToken);
	}

	public virtual void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			if (_transactionOwned)
			{
				_dbTransaction.Dispose();
				Logger.TransactionDisposed(Connection, _dbTransaction, TransactionId, DateTimeOffset.UtcNow);
			}
			ClearTransaction();
		}
	}

	public virtual async ValueTask DisposeAsync()
	{
		if (!_disposed)
		{
			_disposed = true;
			if (_transactionOwned)
			{
				await _dbTransaction.DisposeAsyncIfAvailable();
				Logger.TransactionDisposed(Connection, _dbTransaction, TransactionId, DateTimeOffset.UtcNow);
			}
			await ClearTransactionAsync();
		}
	}

	protected virtual void ClearTransaction()
	{
		Connection.UseTransaction(null);
		if (!_connectionClosed)
		{
			_connectionClosed = true;
			Connection.Close();
		}
	}

	protected virtual async Task ClearTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		await Connection.UseTransactionAsync(null, cancellationToken);
		if (!_connectionClosed)
		{
			_connectionClosed = true;
			await Connection.CloseAsync();
		}
	}
}
