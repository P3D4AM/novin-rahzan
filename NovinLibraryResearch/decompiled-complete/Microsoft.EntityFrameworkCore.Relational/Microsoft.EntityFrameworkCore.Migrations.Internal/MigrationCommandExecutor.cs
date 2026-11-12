using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal;

public class MigrationCommandExecutor : IMigrationCommandExecutor
{
	public virtual void ExecuteNonQuery(IEnumerable<MigrationCommand> migrationCommands, IRelationalConnection connection)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationCommands, "migrationCommands");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
		{
			connection.Open();
			try
			{
				IDbContextTransaction dbContextTransaction = null;
				try
				{
					foreach (MigrationCommand migrationCommand in migrationCommands)
					{
						if (dbContextTransaction == null && !migrationCommand.TransactionSuppressed)
						{
							dbContextTransaction = connection.BeginTransaction();
						}
						if (dbContextTransaction != null && migrationCommand.TransactionSuppressed)
						{
							dbContextTransaction.Commit();
							dbContextTransaction.Dispose();
							dbContextTransaction = null;
						}
						migrationCommand.ExecuteNonQuery(connection);
					}
					dbContextTransaction?.Commit();
				}
				finally
				{
					dbContextTransaction?.Dispose();
				}
			}
			finally
			{
				connection.Close();
			}
		}
	}

	public virtual async Task ExecuteNonQueryAsync(IEnumerable<MigrationCommand> migrationCommands, IRelationalConnection connection, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationCommands, "migrationCommands");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
		try
		{
			await connection.OpenAsync(cancellationToken);
			try
			{
				IDbContextTransaction transaction = null;
				try
				{
					foreach (MigrationCommand command in migrationCommands)
					{
						if (transaction == null && !command.TransactionSuppressed)
						{
							transaction = await connection.BeginTransactionAsync(cancellationToken);
						}
						if (transaction != null && command.TransactionSuppressed)
						{
							transaction.Commit();
							await transaction.DisposeAsync();
							transaction = null;
						}
						await command.ExecuteNonQueryAsync(connection, null, cancellationToken);
					}
					transaction?.Commit();
				}
				finally
				{
					if (transaction != null)
					{
						await transaction.DisposeAsync();
					}
				}
			}
			finally
			{
				await connection.CloseAsync();
			}
		}
		finally
		{
			await transactionScope.DisposeAsyncIfAvailable();
		}
	}
}
