using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public class BatchExecutor : IBatchExecutor
{
	public virtual ICurrentDbContext CurrentContext { get; }

	[Obsolete("This isn't used anymore")]
	protected virtual IExecutionStrategyFactory ExecutionStrategyFactory { get; }

	public BatchExecutor([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory)
	{
		CurrentContext = currentContext;
		ExecutionStrategyFactory = executionStrategyFactory;
	}

	public virtual int Execute(IEnumerable<ModificationCommandBatch> commandBatches, IRelationalConnection connection)
	{
		int num = 0;
		IDbContextTransaction dbContextTransaction = null;
		try
		{
			if (connection.CurrentTransaction == null && (connection as ITransactionEnlistmentManager)?.EnlistedTransaction == null && Transaction.Current == null && CurrentContext.Context.Database.AutoTransactionsEnabled)
			{
				dbContextTransaction = connection.BeginTransaction();
			}
			else
			{
				connection.Open();
			}
			foreach (ModificationCommandBatch commandBatch in commandBatches)
			{
				commandBatch.Execute(connection);
				num += commandBatch.ModificationCommands.Count;
			}
			dbContextTransaction?.Commit();
		}
		finally
		{
			if (dbContextTransaction != null)
			{
				dbContextTransaction.Dispose();
			}
			else
			{
				connection.Close();
			}
		}
		return num;
	}

	public virtual async Task<int> ExecuteAsync(IEnumerable<ModificationCommandBatch> commandBatches, IRelationalConnection connection, CancellationToken cancellationToken = default(CancellationToken))
	{
		int rowsAffected = 0;
		IDbContextTransaction startedTransaction = null;
		try
		{
			if (connection.CurrentTransaction != null || !((connection as ITransactionEnlistmentManager)?.EnlistedTransaction == null) || !(Transaction.Current == null) || !CurrentContext.Context.Database.AutoTransactionsEnabled)
			{
				await connection.OpenAsync(cancellationToken);
			}
			else
			{
				startedTransaction = await connection.BeginTransactionAsync(cancellationToken);
			}
			foreach (ModificationCommandBatch batch in commandBatches)
			{
				await batch.ExecuteAsync(connection, cancellationToken);
				rowsAffected += batch.ModificationCommands.Count;
			}
			startedTransaction?.Commit();
		}
		finally
		{
			if (startedTransaction == null)
			{
				await connection.CloseAsync();
			}
			else
			{
				await startedTransaction.DisposeAsync();
			}
		}
		return rowsAffected;
	}
}
