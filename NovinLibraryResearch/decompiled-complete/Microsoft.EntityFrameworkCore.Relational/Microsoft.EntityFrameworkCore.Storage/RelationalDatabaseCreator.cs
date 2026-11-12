using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public abstract class RelationalDatabaseCreator : IRelationalDatabaseCreator, IDatabaseCreator
{
	protected virtual RelationalDatabaseCreatorDependencies Dependencies { get; }

	protected RelationalDatabaseCreator([JetBrains.Annotations.NotNull] RelationalDatabaseCreatorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public abstract bool Exists();

	public virtual Task<bool> ExistsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		cancellationToken.ThrowIfCancellationRequested();
		return Task.FromResult(Exists());
	}

	public abstract void Create();

	public virtual Task CreateAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		cancellationToken.ThrowIfCancellationRequested();
		Create();
		return Task.FromResult(0);
	}

	public abstract void Delete();

	public virtual Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		cancellationToken.ThrowIfCancellationRequested();
		Delete();
		return Task.FromResult(0);
	}

	public virtual void CreateTables()
	{
		Dependencies.MigrationCommandExecutor.ExecuteNonQuery(GetCreateTablesCommands(), Dependencies.Connection);
	}

	public virtual Task CreateTablesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return Dependencies.MigrationCommandExecutor.ExecuteNonQueryAsync(GetCreateTablesCommands(), Dependencies.Connection, cancellationToken);
	}

	protected virtual IReadOnlyList<MigrationCommand> GetCreateTablesCommands()
	{
		return Dependencies.MigrationsSqlGenerator.Generate(Dependencies.ModelDiffer.GetDifferences(null, Dependencies.Model), Dependencies.Model);
	}

	public abstract bool HasTables();

	public virtual Task<bool> HasTablesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		cancellationToken.ThrowIfCancellationRequested();
		return Task.FromResult(HasTables());
	}

	public virtual bool EnsureDeleted()
	{
		if (Exists())
		{
			Delete();
			return true;
		}
		return false;
	}

	public virtual async Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (await ExistsAsync(cancellationToken))
		{
			await DeleteAsync(cancellationToken);
			return true;
		}
		return false;
	}

	public virtual bool EnsureCreated()
	{
		using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
		{
			if (!Exists())
			{
				Create();
				CreateTables();
				return true;
			}
			if (!HasTables())
			{
				CreateTables();
				return true;
			}
		}
		return false;
	}

	public virtual async Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
		bool result;
		try
		{
			if (!(await ExistsAsync(cancellationToken)))
			{
				await CreateAsync(cancellationToken);
				await CreateTablesAsync(cancellationToken);
				result = true;
				goto IL_0307;
			}
			if (!(await HasTablesAsync(cancellationToken)))
			{
				await CreateTablesAsync(cancellationToken);
				result = true;
				goto IL_0307;
			}
		}
		finally
		{
			await transactionScope.DisposeAsyncIfAvailable();
		}
		return false;
		IL_0307:
		return result;
	}

	public virtual string GenerateCreateScript()
	{
		IReadOnlyList<MigrationCommand> createTablesCommands = GetCreateTablesCommands();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (MigrationCommand item in createTablesCommands)
		{
			stringBuilder.Append(item.CommandText).AppendLine(Dependencies.SqlGenerationHelper.BatchTerminator);
		}
		return stringBuilder.ToString();
	}

	public virtual bool CanConnect()
	{
		return Exists();
	}

	public virtual Task<bool> CanConnectAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return ExistsAsync(cancellationToken);
	}
}
