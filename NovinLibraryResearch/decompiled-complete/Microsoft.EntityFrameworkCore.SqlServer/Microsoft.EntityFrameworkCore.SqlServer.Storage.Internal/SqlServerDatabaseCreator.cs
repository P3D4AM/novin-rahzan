using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.SqlServer.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerDatabaseCreator : RelationalDatabaseCreator
{
	private readonly ISqlServerConnection _connection;

	private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;

	public virtual TimeSpan RetryDelay { get; set; } = TimeSpan.FromMilliseconds(500.0);


	public virtual TimeSpan RetryTimeout { get; set; } = TimeSpan.FromMinutes(1.0);


	public SqlServerDatabaseCreator([JetBrains.Annotations.NotNull] RelationalDatabaseCreatorDependencies dependencies, [JetBrains.Annotations.NotNull] ISqlServerConnection connection, [JetBrains.Annotations.NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder)
		: base(dependencies)
	{
		_connection = connection;
		_rawSqlCommandBuilder = rawSqlCommandBuilder;
	}

	public override void Create()
	{
		using (ISqlServerConnection connection = _connection.CreateMasterConnection())
		{
			Dependencies.MigrationCommandExecutor.ExecuteNonQuery(CreateCreateOperations(), connection);
			ClearPool();
		}
		Exists(retryOnNotExists: true);
	}

	public override async Task CreateAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		using (ISqlServerConnection masterConnection = _connection.CreateMasterConnection())
		{
			await Dependencies.MigrationCommandExecutor.ExecuteNonQueryAsync(CreateCreateOperations(), masterConnection, cancellationToken);
			ClearPool();
		}
		await ExistsAsync(retryOnNotExists: true, cancellationToken);
	}

	public override bool HasTables()
	{
		return Dependencies.ExecutionStrategyFactory.Create().Execute(_connection, (ISqlServerConnection connection) => (int)CreateHasTablesCommand().ExecuteScalar(new RelationalCommandParameterObject(connection, null, null, Dependencies.CurrentContext.Context, Dependencies.CommandLogger)) != 0);
	}

	public override Task<bool> HasTablesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return Dependencies.ExecutionStrategyFactory.Create().ExecuteAsync(_connection, async (ISqlServerConnection connection, CancellationToken ct) => (int)(await CreateHasTablesCommand().ExecuteScalarAsync(new RelationalCommandParameterObject(connection, null, null, Dependencies.CurrentContext.Context, Dependencies.CommandLogger), ct)) != 0, cancellationToken);
	}

	private IRelationalCommand CreateHasTablesCommand()
	{
		return _rawSqlCommandBuilder.Build("\r\nIF EXISTS\r\n    (SELECT *\r\n     FROM [sys].[objects] o\r\n     WHERE [o].[type] = 'U'\r\n     AND [o].[is_ms_shipped] = 0\r\n     AND NOT EXISTS (SELECT *\r\n         FROM [sys].[extended_properties] AS [ep]\r\n         WHERE [ep].[major_id] = [o].[object_id]\r\n             AND [ep].[minor_id] = 0\r\n             AND [ep].[class] = 1\r\n             AND [ep].[name] = N'microsoft_database_tools_support'\r\n    )\r\n)\r\nSELECT 1 ELSE SELECT 0");
	}

	private IReadOnlyList<MigrationCommand> CreateCreateOperations()
	{
		SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(_connection.DbConnection.ConnectionString);
		return Dependencies.MigrationsSqlGenerator.Generate(new SqlServerCreateDatabaseOperation[1]
		{
			new SqlServerCreateDatabaseOperation
			{
				Name = sqlConnectionStringBuilder.InitialCatalog,
				FileName = sqlConnectionStringBuilder.AttachDBFilename
			}
		});
	}

	public override bool Exists()
	{
		return Exists(retryOnNotExists: false);
	}

	private bool Exists(bool retryOnNotExists)
	{
		return Dependencies.ExecutionStrategyFactory.Create().Execute(DateTime.UtcNow + RetryTimeout, delegate(DateTime giveUp)
		{
			while (true)
			{
				bool flag = false;
				try
				{
					using (new TransactionScope(TransactionScopeOption.Suppress))
					{
						_connection.Open(errorsExpected: true);
						flag = true;
						_rawSqlCommandBuilder.Build("SELECT 1").ExecuteNonQuery(new RelationalCommandParameterObject(_connection, null, null, Dependencies.CurrentContext.Context, Dependencies.CommandLogger));
						return true;
					}
				}
				catch (SqlException exception)
				{
					if (!retryOnNotExists && IsDoesNotExist(exception))
					{
						return false;
					}
					if (DateTime.UtcNow > giveUp || !RetryOnExistsFailure(exception))
					{
						throw;
					}
					Thread.Sleep(RetryDelay);
				}
				finally
				{
					if (flag)
					{
						_connection.Close();
					}
				}
			}
		});
	}

	public override Task<bool> ExistsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return ExistsAsync(retryOnNotExists: false, cancellationToken);
	}

	private Task<bool> ExistsAsync(bool retryOnNotExists, CancellationToken cancellationToken)
	{
		return Dependencies.ExecutionStrategyFactory.Create().ExecuteAsync(DateTime.UtcNow + RetryTimeout, async delegate(DateTime giveUp, CancellationToken ct)
		{
			while (true)
			{
				bool opened = false;
				try
				{
					bool result;
					try
					{
						using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
						{
							await _connection.OpenAsync(ct, errorsExpected: true);
							opened = true;
							await _rawSqlCommandBuilder.Build("SELECT 1").ExecuteNonQueryAsync(new RelationalCommandParameterObject(_connection, null, null, Dependencies.CurrentContext.Context, Dependencies.CommandLogger), ct);
							result = true;
						}
					}
					catch (SqlException exception)
					{
						if (!retryOnNotExists && IsDoesNotExist(exception))
						{
							result = false;
							goto end_IL_01b3;
						}
						if (DateTime.UtcNow > giveUp || !RetryOnExistsFailure(exception))
						{
							throw;
						}
						await Task.Delay(RetryDelay, ct);
						goto end_IL_003e;
						end_IL_01b3:;
					}
					return result;
					end_IL_003e:;
				}
				finally
				{
					if (opened)
					{
						await _connection.CloseAsync();
					}
				}
			}
		}, cancellationToken);
	}

	private static bool IsDoesNotExist(SqlException exception)
	{
		if (exception.Number != 4060 && exception.Number != 1832)
		{
			return exception.Number == 5120;
		}
		return true;
	}

	private bool RetryOnExistsFailure(SqlException exception)
	{
		if (exception.Number == 233 || exception.Number == -2 || exception.Number == 4060 || exception.Number == 1832 || exception.Number == 5120)
		{
			ClearPool();
			return true;
		}
		return false;
	}

	public override void Delete()
	{
		ClearAllPools();
		using ISqlServerConnection connection = _connection.CreateMasterConnection();
		Dependencies.MigrationCommandExecutor.ExecuteNonQuery(CreateDropCommands(), connection);
	}

	public override async Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		ClearAllPools();
		using ISqlServerConnection masterConnection = _connection.CreateMasterConnection();
		await Dependencies.MigrationCommandExecutor.ExecuteNonQueryAsync(CreateDropCommands(), masterConnection, cancellationToken);
	}

	private IReadOnlyList<MigrationCommand> CreateDropCommands()
	{
		string database = _connection.DbConnection.Database;
		if (string.IsNullOrEmpty(database))
		{
			throw new InvalidOperationException(SqlServerStrings.NoInitialCatalog);
		}
		MigrationOperation[] operations = new MigrationOperation[1]
		{
			new SqlServerDropDatabaseOperation
			{
				Name = database
			}
		};
		return Dependencies.MigrationsSqlGenerator.Generate(operations);
	}

	private static void ClearAllPools()
	{
		SqlConnection.ClearAllPools();
	}

	private void ClearPool()
	{
		SqlConnection.ClearPool((SqlConnection)_connection.DbConnection);
	}
}
