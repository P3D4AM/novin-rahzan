using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class RelationalCommand : IRelationalCommand
{
	protected virtual RelationalCommandBuilderDependencies Dependencies { get; }

	public virtual string CommandText { get; }

	public virtual IReadOnlyList<IRelationalParameter> Parameters { get; }

	public RelationalCommand([JetBrains.Annotations.NotNull] RelationalCommandBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] string commandText, [JetBrains.Annotations.NotNull] IReadOnlyList<IRelationalParameter> parameters)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandText, "commandText");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameters, "parameters");
		Dependencies = dependencies;
		CommandText = commandText;
		Parameters = parameters;
	}

	public virtual int ExecuteNonQuery(RelationalCommandParameterObject parameterObject)
	{
		IRelationalConnection connection = parameterObject.Connection;
		DbContext context = parameterObject.Context;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger = parameterObject.Logger;
		IRelationalConnection relationalConnection = connection;
		DbContext context2 = context;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnosticsLogger = logger;
		Guid commandId = Guid.NewGuid();
		DbCommand dbCommand = CreateCommand(parameterObject, commandId, DbCommandMethod.ExecuteNonQuery);
		relationalConnection.Open();
		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			InterceptionResult<int> interceptionResult = diagnosticsLogger?.CommandNonQueryExecuting(relationalConnection, dbCommand, context2, commandId, relationalConnection.ConnectionId, utcNow) ?? default(InterceptionResult<int>);
			int num = (interceptionResult.HasResult ? interceptionResult.Result : dbCommand.ExecuteNonQuery());
			return diagnosticsLogger?.CommandNonQueryExecuted(relationalConnection, dbCommand, context2, commandId, relationalConnection.ConnectionId, num, utcNow, stopwatch.Elapsed) ?? num;
		}
		catch (Exception exception)
		{
			diagnosticsLogger?.CommandError(relationalConnection, dbCommand, context2, DbCommandMethod.ExecuteNonQuery, commandId, relationalConnection.ConnectionId, exception, utcNow, stopwatch.Elapsed);
			throw;
		}
		finally
		{
			CleanupCommand(dbCommand, relationalConnection);
		}
	}

	private static void CleanupCommand(DbCommand command, IRelationalConnection connection)
	{
		command.Parameters.Clear();
		command.Dispose();
		connection.Close();
	}

	private static async Task CleanupCommandAsync(DbCommand command, IRelationalConnection connection)
	{
		command.Parameters.Clear();
		await command.DisposeAsyncIfAvailable();
		await connection.CloseAsync();
	}

	public virtual async Task<int> ExecuteNonQueryAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		IRelationalConnection connection2 = parameterObject.Connection;
		DbContext context2 = parameterObject.Context;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger2 = parameterObject.Logger;
		IRelationalConnection connection = connection2;
		DbContext context = context2;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger = logger2;
		Guid commandId = Guid.NewGuid();
		DbCommand command = CreateCommand(parameterObject, commandId, DbCommandMethod.ExecuteNonQuery);
		await connection.OpenAsync(cancellationToken);
		DateTimeOffset startTime = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		int result;
		try
		{
			InterceptionResult<int> interceptionResult = ((logger != null) ? (await logger.CommandNonQueryExecutingAsync(connection, command, context, commandId, connection.ConnectionId, startTime, cancellationToken)) : default(InterceptionResult<int>));
			InterceptionResult<int> interceptionResult2 = interceptionResult;
			int num = ((!interceptionResult2.HasResult) ? (await command.ExecuteNonQueryAsync(cancellationToken)) : interceptionResult2.Result);
			int num2 = num;
			if (logger != null)
			{
				num2 = await logger.CommandNonQueryExecutedAsync(connection, command, context, commandId, connection.ConnectionId, num2, startTime, stopwatch.Elapsed, cancellationToken);
			}
			result = num2;
		}
		catch (Exception exception)
		{
			if (logger != null)
			{
				await logger.CommandErrorAsync(connection, command, context, DbCommandMethod.ExecuteNonQuery, commandId, connection.ConnectionId, exception, startTime, stopwatch.Elapsed, cancellationToken);
			}
			throw;
		}
		finally
		{
			await CleanupCommandAsync(command, connection);
		}
		return result;
	}

	public virtual object ExecuteScalar(RelationalCommandParameterObject parameterObject)
	{
		IRelationalConnection connection = parameterObject.Connection;
		DbContext context = parameterObject.Context;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger = parameterObject.Logger;
		IRelationalConnection relationalConnection = connection;
		DbContext context2 = context;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnosticsLogger = logger;
		Guid commandId = Guid.NewGuid();
		DbCommand dbCommand = CreateCommand(parameterObject, commandId, DbCommandMethod.ExecuteScalar);
		relationalConnection.Open();
		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			InterceptionResult<object> interceptionResult = diagnosticsLogger?.CommandScalarExecuting(relationalConnection, dbCommand, context2, commandId, relationalConnection.ConnectionId, utcNow) ?? default(InterceptionResult<object>);
			object obj = (interceptionResult.HasResult ? interceptionResult.Result : dbCommand.ExecuteScalar());
			return diagnosticsLogger?.CommandScalarExecuted(relationalConnection, dbCommand, context2, commandId, relationalConnection.ConnectionId, obj, utcNow, stopwatch.Elapsed) ?? obj;
		}
		catch (Exception exception)
		{
			diagnosticsLogger?.CommandError(relationalConnection, dbCommand, context2, DbCommandMethod.ExecuteScalar, commandId, relationalConnection.ConnectionId, exception, utcNow, stopwatch.Elapsed);
			throw;
		}
		finally
		{
			CleanupCommand(dbCommand, relationalConnection);
		}
	}

	public virtual async Task<object> ExecuteScalarAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		IRelationalConnection connection2 = parameterObject.Connection;
		DbContext context2 = parameterObject.Context;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger2 = parameterObject.Logger;
		IRelationalConnection connection = connection2;
		DbContext context = context2;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger = logger2;
		Guid commandId = Guid.NewGuid();
		DbCommand command = CreateCommand(parameterObject, commandId, DbCommandMethod.ExecuteScalar);
		await connection.OpenAsync(cancellationToken);
		DateTimeOffset startTime = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		object result;
		try
		{
			InterceptionResult<object> interceptionResult = ((logger != null) ? (await logger.CommandScalarExecutingAsync(connection, command, context, commandId, connection.ConnectionId, startTime, cancellationToken)) : default(InterceptionResult<object>));
			InterceptionResult<object> interceptionResult2 = interceptionResult;
			object obj = ((!interceptionResult2.HasResult) ? (await command.ExecuteScalarAsync(cancellationToken)) : interceptionResult2.Result);
			object obj2 = obj;
			if (logger != null)
			{
				obj2 = await logger.CommandScalarExecutedAsync(connection, command, context, commandId, connection.ConnectionId, obj2, startTime, stopwatch.Elapsed, cancellationToken);
			}
			result = obj2;
		}
		catch (Exception exception)
		{
			if (logger != null)
			{
				await logger.CommandErrorAsync(connection, command, context, DbCommandMethod.ExecuteScalar, commandId, connection.ConnectionId, exception, startTime, stopwatch.Elapsed, cancellationToken);
			}
			throw;
		}
		finally
		{
			await CleanupCommandAsync(command, connection);
		}
		return result;
	}

	public virtual RelationalDataReader ExecuteReader(RelationalCommandParameterObject parameterObject)
	{
		IRelationalConnection connection = parameterObject.Connection;
		DbContext context = parameterObject.Context;
		IReadOnlyList<ReaderColumn> readerColumns = parameterObject.ReaderColumns;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger = parameterObject.Logger;
		Guid commandId = Guid.NewGuid();
		DbCommand dbCommand = CreateCommand(parameterObject, commandId, DbCommandMethod.ExecuteReader);
		connection.Open();
		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		bool flag = false;
		try
		{
			InterceptionResult<DbDataReader> interceptionResult = logger?.CommandReaderExecuting(connection, dbCommand, context, commandId, connection.ConnectionId, utcNow) ?? default(InterceptionResult<DbDataReader>);
			DbDataReader dbDataReader = (interceptionResult.HasResult ? interceptionResult.Result : dbCommand.ExecuteReader());
			if (logger != null)
			{
				dbDataReader = logger.CommandReaderExecuted(connection, dbCommand, context, commandId, connection.ConnectionId, dbDataReader, utcNow, stopwatch.Elapsed);
			}
			if (readerColumns != null)
			{
				dbDataReader = new BufferedDataReader(dbDataReader).Initialize(readerColumns);
			}
			RelationalDataReader result = new RelationalDataReader(connection, dbCommand, dbDataReader, commandId, logger);
			flag = true;
			return result;
		}
		catch (Exception exception)
		{
			logger?.CommandError(connection, dbCommand, context, DbCommandMethod.ExecuteReader, commandId, connection.ConnectionId, exception, utcNow, stopwatch.Elapsed);
			throw;
		}
		finally
		{
			if (!flag)
			{
				CleanupCommand(dbCommand, connection);
			}
		}
	}

	public virtual async Task<RelationalDataReader> ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		IRelationalConnection connection = parameterObject.Connection;
		DbContext context = parameterObject.Context;
		IReadOnlyList<ReaderColumn> readerColumns = parameterObject.ReaderColumns;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger = parameterObject.Logger;
		Guid commandId = Guid.NewGuid();
		DbCommand command = CreateCommand(parameterObject, commandId, DbCommandMethod.ExecuteReader);
		await connection.OpenAsync(cancellationToken);
		DateTimeOffset startTime = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		bool readerOpen = false;
		RelationalDataReader result;
		try
		{
			InterceptionResult<DbDataReader> interceptionResult = ((logger != null) ? (await logger.CommandReaderExecutingAsync(connection, command, context, commandId, connection.ConnectionId, startTime, cancellationToken)) : default(InterceptionResult<DbDataReader>));
			InterceptionResult<DbDataReader> interceptionResult2 = interceptionResult;
			DbDataReader dbDataReader = ((!interceptionResult2.HasResult) ? (await command.ExecuteReaderAsync(cancellationToken)) : interceptionResult2.Result);
			DbDataReader dbDataReader2 = dbDataReader;
			if (logger != null)
			{
				dbDataReader2 = await logger.CommandReaderExecutedAsync(connection, command, context, commandId, connection.ConnectionId, dbDataReader2, startTime, stopwatch.Elapsed, cancellationToken);
			}
			if (readerColumns != null)
			{
				dbDataReader2 = await new BufferedDataReader(dbDataReader2).InitializeAsync(readerColumns, cancellationToken);
			}
			RelationalDataReader relationalDataReader = new RelationalDataReader(connection, command, dbDataReader2, commandId, logger);
			readerOpen = true;
			result = relationalDataReader;
		}
		catch (Exception exception)
		{
			if (logger != null)
			{
				await logger.CommandErrorAsync(connection, command, context, DbCommandMethod.ExecuteReader, commandId, connection.ConnectionId, exception, startTime, stopwatch.Elapsed, cancellationToken);
			}
			throw;
		}
		finally
		{
			if (!readerOpen)
			{
				await CleanupCommandAsync(command, connection);
			}
		}
		return result;
	}

	protected virtual DbCommand CreateCommand(RelationalCommandParameterObject parameterObject, Guid commandId, DbCommandMethod commandMethod)
	{
		IRelationalConnection connection = parameterObject.Connection;
		DbContext context = parameterObject.Context;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger = parameterObject.Logger;
		IRelationalConnection relationalConnection = connection;
		DbContext context2 = context;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnosticsLogger = logger;
		Guid connectionId = relationalConnection.ConnectionId;
		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		Stopwatch stopwatch = Stopwatch.StartNew();
		InterceptionResult<DbCommand> interceptionResult = diagnosticsLogger?.CommandCreating(relationalConnection, commandMethod, context2, commandId, connectionId, utcNow) ?? default(InterceptionResult<DbCommand>);
		DbCommand dbCommand = (interceptionResult.HasResult ? interceptionResult.Result : relationalConnection.DbConnection.CreateCommand());
		if (diagnosticsLogger != null)
		{
			dbCommand = diagnosticsLogger.CommandCreated(relationalConnection, dbCommand, commandMethod, context2, commandId, connectionId, utcNow, stopwatch.Elapsed);
		}
		dbCommand.CommandText = CommandText;
		if (relationalConnection.CurrentTransaction != null)
		{
			dbCommand.Transaction = relationalConnection.CurrentTransaction.GetDbTransaction();
		}
		if (relationalConnection.CommandTimeout.HasValue)
		{
			dbCommand.CommandTimeout = relationalConnection.CommandTimeout.Value;
		}
		if (Parameters != null && Parameters.Count > 0)
		{
			IReadOnlyDictionary<string, object> parameterValues = parameterObject.ParameterValues;
			if (parameterValues == null)
			{
				throw new InvalidOperationException(RelationalStrings.MissingParameterValue(Parameters[0].InvariantName));
			}
			for (int i = 0; i < Parameters.Count; i++)
			{
				Parameters[i].AddDbParameter(dbCommand, parameterValues);
			}
		}
		return dbCommand;
	}
}
