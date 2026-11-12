using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public abstract class RelationalConnection : IRelationalConnection, IRelationalTransactionManager, IDbContextTransactionManager, IResettableService, IDisposable, IAsyncDisposable, ITransactionEnlistmentManager
{
	private readonly string _connectionString;

	private readonly bool _connectionOwned;

	private int _openedCount;

	private bool _openedInternally;

	private int? _commandTimeout;

	private Transaction _ambientTransaction;

	private DbConnection _connection;

	private Transaction _enlistedTransaction;

	public virtual Guid ConnectionId { get; } = Guid.NewGuid();


	public virtual DbContext Context { get; }

	protected virtual RelationalConnectionDependencies Dependencies { get; }

	public virtual string ConnectionString => _connectionString ?? DbConnection.ConnectionString;

	public virtual DbConnection DbConnection => _connection ?? (_connection = CreateDbConnection());

	public virtual IDbContextTransaction CurrentTransaction
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		protected set;
	}

	public virtual Transaction EnlistedTransaction
	{
		get
		{
			if (_enlistedTransaction != null)
			{
				try
				{
					if (_enlistedTransaction.TransactionInformation.Status != 0)
					{
						_enlistedTransaction = null;
					}
				}
				catch (ObjectDisposedException)
				{
					_enlistedTransaction = null;
				}
			}
			return _enlistedTransaction;
		}
		[param: JetBrains.Annotations.CanBeNull]
		protected set
		{
			_enlistedTransaction = value;
		}
	}

	protected virtual bool SupportsAmbientTransactions => false;

	public virtual int? CommandTimeout
	{
		get
		{
			return _commandTimeout;
		}
		set
		{
			if (value.HasValue && value < 0)
			{
				throw new ArgumentException(RelationalStrings.InvalidCommandTimeout);
			}
			_commandTimeout = value;
		}
	}

	public virtual bool IsMultipleActiveResultSetsEnabled => false;

	public virtual SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(1);


	protected RelationalConnection([JetBrains.Annotations.NotNull] RelationalConnectionDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Context = dependencies.CurrentContext.Context;
		Dependencies = dependencies;
		RelationalOptionsExtension relationalOptionsExtension = RelationalOptionsExtension.Extract(dependencies.ContextOptions);
		_commandTimeout = relationalOptionsExtension.CommandTimeout;
		if (relationalOptionsExtension.Connection != null)
		{
			if (!string.IsNullOrWhiteSpace(relationalOptionsExtension.ConnectionString))
			{
				throw new InvalidOperationException(RelationalStrings.ConnectionAndConnectionString);
			}
			_connection = relationalOptionsExtension.Connection;
			_connectionOwned = false;
		}
		else
		{
			if (string.IsNullOrWhiteSpace(relationalOptionsExtension.ConnectionString))
			{
				throw new InvalidOperationException(RelationalStrings.NoConnectionOrConnectionString);
			}
			_connectionString = dependencies.ConnectionStringResolver.ResolveConnectionString(relationalOptionsExtension.ConnectionString);
			_connectionOwned = true;
		}
	}

	protected abstract DbConnection CreateDbConnection();

	public virtual void EnlistTransaction(Transaction transaction)
	{
		if (!SupportsAmbientTransactions)
		{
			Dependencies.TransactionLogger.AmbientTransactionWarning(this, DateTimeOffset.UtcNow);
			return;
		}
		if (transaction != null)
		{
			Dependencies.TransactionLogger.ExplicitTransactionEnlisted(this, transaction);
		}
		DbConnection.EnlistTransaction(transaction);
		EnlistedTransaction = transaction;
	}

	[JetBrains.Annotations.NotNull]
	public virtual IDbContextTransaction BeginTransaction()
	{
		return BeginTransaction(System.Data.IsolationLevel.Unspecified);
	}

	[JetBrains.Annotations.NotNull]
	public virtual async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return await BeginTransactionAsync(System.Data.IsolationLevel.Unspecified, cancellationToken);
	}

	public virtual IDbContextTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
	{
		Open();
		EnsureNoTransactions();
		Guid transactionId = Guid.NewGuid();
		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		InterceptionResult<DbTransaction> interceptionResult = Dependencies.TransactionLogger.TransactionStarting(this, isolationLevel, transactionId, utcNow);
		DbTransaction transaction = (interceptionResult.HasResult ? interceptionResult.Result : DbConnection.BeginTransaction(isolationLevel));
		transaction = Dependencies.TransactionLogger.TransactionStarted(this, transaction, transactionId, utcNow, stopwatch.Elapsed);
		return CreateRelationalTransaction(transaction, transactionId, transactionOwned: true);
	}

	public virtual async Task<IDbContextTransaction> BeginTransactionAsync(System.Data.IsolationLevel isolationLevel, CancellationToken cancellationToken = default(CancellationToken))
	{
		await OpenAsync(cancellationToken);
		EnsureNoTransactions();
		Guid transactionId = Guid.NewGuid();
		DateTimeOffset startTime = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		InterceptionResult<DbTransaction> interceptionResult = await Dependencies.TransactionLogger.TransactionStartingAsync(this, isolationLevel, transactionId, startTime, cancellationToken);
		DbTransaction dbTransaction = ((!interceptionResult.HasResult) ? (await DbConnectionExtensions.BeginTransactionAsync(DbConnection, isolationLevel, cancellationToken)) : interceptionResult.Result);
		DbTransaction transaction = dbTransaction;
		return CreateRelationalTransaction(await Dependencies.TransactionLogger.TransactionStartedAsync(this, transaction, transactionId, startTime, stopwatch.Elapsed, cancellationToken), transactionId, transactionOwned: true);
	}

	private void EnsureNoTransactions()
	{
		if (CurrentTransaction != null)
		{
			throw new InvalidOperationException(RelationalStrings.TransactionAlreadyStarted);
		}
		if (Transaction.Current != null)
		{
			throw new InvalidOperationException(RelationalStrings.ConflictingAmbientTransaction);
		}
		if (EnlistedTransaction != null)
		{
			throw new InvalidOperationException(RelationalStrings.ConflictingEnlistedTransaction);
		}
	}

	private IDbContextTransaction CreateRelationalTransaction(DbTransaction transaction, Guid transactionId, bool transactionOwned)
	{
		return CurrentTransaction = Dependencies.RelationalTransactionFactory.Create(this, transaction, transactionId, Dependencies.TransactionLogger, transactionOwned);
	}

	public virtual IDbContextTransaction UseTransaction(DbTransaction transaction)
	{
		if (ShouldUseTransaction(transaction))
		{
			Open();
			Guid transactionId = Guid.NewGuid();
			transaction = Dependencies.TransactionLogger.TransactionUsed(this, transaction, transactionId, DateTimeOffset.UtcNow);
			CurrentTransaction = CreateRelationalTransaction(transaction, transactionId, transactionOwned: false);
		}
		return CurrentTransaction;
	}

	public virtual async Task<IDbContextTransaction> UseTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (ShouldUseTransaction(transaction))
		{
			await OpenAsync(cancellationToken);
			Guid transactionId = Guid.NewGuid();
			transaction = await Dependencies.TransactionLogger.TransactionUsedAsync(this, transaction, transactionId, DateTimeOffset.UtcNow, cancellationToken);
			CurrentTransaction = CreateRelationalTransaction(transaction, transactionId, transactionOwned: false);
		}
		return CurrentTransaction;
	}

	private bool ShouldUseTransaction(DbTransaction transaction)
	{
		if (transaction == null)
		{
			if (CurrentTransaction != null)
			{
				CurrentTransaction = null;
			}
			return false;
		}
		EnsureNoTransactions();
		return true;
	}

	public virtual void CommitTransaction()
	{
		if (CurrentTransaction == null)
		{
			throw new InvalidOperationException(RelationalStrings.NoActiveTransaction);
		}
		CurrentTransaction.Commit();
	}

	public virtual void RollbackTransaction()
	{
		if (CurrentTransaction == null)
		{
			throw new InvalidOperationException(RelationalStrings.NoActiveTransaction);
		}
		CurrentTransaction.Rollback();
	}

	public virtual bool Open(bool errorsExpected = false)
	{
		if (DbConnection.State == ConnectionState.Broken)
		{
			DbConnection.Close();
		}
		bool result = false;
		if (DbConnection.State != ConnectionState.Open)
		{
			CurrentTransaction?.Dispose();
			ClearTransactions(clearAmbient: false);
			OpenDbConnection(errorsExpected);
			result = true;
		}
		_openedCount++;
		HandleAmbientTransactions();
		return result;
	}

	public virtual async Task<bool> OpenAsync(CancellationToken cancellationToken, bool errorsExpected = false)
	{
		if (DbConnection.State == ConnectionState.Broken)
		{
			await DbConnectionExtensions.CloseAsync(DbConnection);
		}
		bool result = false;
		if (DbConnection.State != ConnectionState.Open)
		{
			if (CurrentTransaction != null)
			{
				await CurrentTransaction.DisposeAsync();
			}
			ClearTransactions(clearAmbient: false);
			await OpenDbConnectionAsync(errorsExpected, cancellationToken);
			result = true;
		}
		_openedCount++;
		HandleAmbientTransactions();
		return result;
	}

	private void ClearTransactions(bool clearAmbient)
	{
		CurrentTransaction = null;
		EnlistedTransaction = null;
		if (clearAmbient && _ambientTransaction != null)
		{
			_ambientTransaction.TransactionCompleted -= HandleTransactionCompleted;
			_ambientTransaction = null;
		}
		if (_openedCount < 0)
		{
			_openedCount = 0;
		}
	}

	private void OpenDbConnection(bool errorsExpected)
	{
		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		InterceptionResult interceptionResult = Dependencies.ConnectionLogger.ConnectionOpening(this, utcNow);
		try
		{
			if (!interceptionResult.IsSuppressed)
			{
				DbConnection.Open();
			}
			Dependencies.ConnectionLogger.ConnectionOpened(this, utcNow, stopwatch.Elapsed);
		}
		catch (Exception exception)
		{
			Dependencies.ConnectionLogger.ConnectionError(this, exception, utcNow, stopwatch.Elapsed, errorsExpected);
			throw;
		}
		if (_openedCount == 0)
		{
			_openedInternally = true;
		}
	}

	private async Task OpenDbConnectionAsync(bool errorsExpected, CancellationToken cancellationToken)
	{
		DateTimeOffset startTime = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		InterceptionResult interceptionResult = await Dependencies.ConnectionLogger.ConnectionOpeningAsync(this, startTime, cancellationToken);
		try
		{
			if (!interceptionResult.IsSuppressed)
			{
				await DbConnection.OpenAsync(cancellationToken);
			}
			await Dependencies.ConnectionLogger.ConnectionOpenedAsync(this, startTime, stopwatch.Elapsed, cancellationToken);
		}
		catch (Exception exception)
		{
			await Dependencies.ConnectionLogger.ConnectionErrorAsync(this, exception, startTime, stopwatch.Elapsed, errorsExpected, cancellationToken);
			throw;
		}
		if (_openedCount == 0)
		{
			_openedInternally = true;
		}
	}

	private void HandleAmbientTransactions()
	{
		Transaction current = Transaction.Current;
		if (current != null && !SupportsAmbientTransactions)
		{
			Dependencies.TransactionLogger.AmbientTransactionWarning(this, DateTimeOffset.UtcNow);
		}
		else if (!object.Equals(current, _ambientTransaction))
		{
			if (_ambientTransaction != null)
			{
				throw new InvalidOperationException(RelationalStrings.PendingAmbientTransaction);
			}
			if (current != null)
			{
				Dependencies.TransactionLogger.AmbientTransactionEnlisted(this, current);
			}
			DbConnection.EnlistTransaction(current);
			Transaction ambientTransaction = _ambientTransaction;
			if (ambientTransaction != null)
			{
				ambientTransaction.TransactionCompleted -= HandleTransactionCompleted;
			}
			if (current != null)
			{
				current.TransactionCompleted += HandleTransactionCompleted;
			}
			_ambientTransaction = current;
		}
	}

	private void HandleTransactionCompleted(object sender, TransactionEventArgs e)
	{
		Transaction ambientTransaction = _ambientTransaction;
		if (ambientTransaction != null)
		{
			ambientTransaction.TransactionCompleted -= HandleTransactionCompleted;
			_ambientTransaction = null;
		}
	}

	public virtual bool Close()
	{
		bool result = false;
		if (ShouldClose())
		{
			CurrentTransaction?.Dispose();
			ClearTransactions(clearAmbient: false);
			if (DbConnection.State != 0)
			{
				DateTimeOffset utcNow = DateTimeOffset.UtcNow;
				Stopwatch stopwatch = Stopwatch.StartNew();
				InterceptionResult interceptionResult = Dependencies.ConnectionLogger.ConnectionClosing(this, utcNow);
				try
				{
					if (!interceptionResult.IsSuppressed)
					{
						DbConnection.Close();
					}
					result = true;
					Dependencies.ConnectionLogger.ConnectionClosed(this, utcNow, stopwatch.Elapsed);
				}
				catch (Exception exception)
				{
					Dependencies.ConnectionLogger.ConnectionError(this, exception, utcNow, stopwatch.Elapsed, logErrorAsDebug: false);
					throw;
				}
			}
			_openedInternally = false;
		}
		return result;
	}

	public virtual async Task<bool> CloseAsync()
	{
		bool wasClosed = false;
		if (ShouldClose())
		{
			if (CurrentTransaction != null)
			{
				await CurrentTransaction.DisposeAsync();
			}
			ClearTransactions(clearAmbient: false);
			if (DbConnection.State != 0)
			{
				DateTimeOffset startTime = DateTimeOffset.UtcNow;
				Stopwatch stopwatch = Stopwatch.StartNew();
				InterceptionResult interceptionResult = await Dependencies.ConnectionLogger.ConnectionClosingAsync(this, startTime);
				try
				{
					if (!interceptionResult.IsSuppressed)
					{
						await DbConnectionExtensions.CloseAsync(DbConnection);
					}
					wasClosed = true;
					await Dependencies.ConnectionLogger.ConnectionClosedAsync(this, startTime, stopwatch.Elapsed);
				}
				catch (Exception exception)
				{
					await Dependencies.ConnectionLogger.ConnectionErrorAsync(this, exception, startTime, stopwatch.Elapsed, logErrorAsDebug: false);
					throw;
				}
			}
			_openedInternally = false;
		}
		return wasClosed;
	}

	private bool ShouldClose()
	{
		if (_openedCount == 0 || (_openedCount > 0 && --_openedCount == 0))
		{
			return _openedInternally;
		}
		return false;
	}

	void IResettableService.ResetState()
	{
		Dispose();
	}

	Task IResettableService.ResetStateAsync(CancellationToken cancellationToken)
	{
		return DisposeAsync().AsTask();
	}

	public virtual void Dispose()
	{
		CurrentTransaction?.Dispose();
		ClearTransactions(clearAmbient: true);
		if (_connectionOwned && _connection != null)
		{
			DbConnection.Dispose();
			_connection = null;
			_openedCount = 0;
		}
	}

	public virtual async ValueTask DisposeAsync()
	{
		if (CurrentTransaction != null)
		{
			await CurrentTransaction.DisposeAsync();
		}
		ClearTransactions(clearAmbient: true);
		if (_connectionOwned && _connection != null)
		{
			await DbConnection.DisposeAsyncIfAvailable();
			_connection = null;
			_openedCount = 0;
		}
	}
}
