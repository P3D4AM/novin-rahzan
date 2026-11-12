using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public static class RelationalLoggerExtensions
{
	public static InterceptionResult<DbCommand> CommandCreating([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, DbCommandMethod commandMethod, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, DateTimeOffset startTime)
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogCommandCreating(diagnostics);
		LogCommandCreating(diagnostics, eventDefinition, commandMethod);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandCorrelatedEventData eventData = BroadcastCommandCreating(diagnostics, connection.DbConnection, context, commandMethod, commandId, connectionId, async: false, startTime, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.CommandCreating(eventData, default(InterceptionResult<DbCommand>));
			}
		}
		return default(InterceptionResult<DbCommand>);
	}

	private static CommandCorrelatedEventData BroadcastCommandCreating(IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, DbConnection connection, DbContext context, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, bool async, DateTimeOffset startTime, EventDefinition<string> definition, bool diagnosticSourceEnabled)
	{
		CommandCorrelatedEventData commandCorrelatedEventData = new CommandCorrelatedEventData(definition, CommandCreating, connection, context, executeMethod, commandId, connectionId, async, startTime);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, commandCorrelatedEventData);
		}
		return commandCorrelatedEventData;
	}

	private static void LogCommandCreating(IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, EventDefinition<string> definition, DbCommandMethod commandMethod)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, commandMethod.ToString());
		}
	}

	private static string CommandCreating(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		CommandCorrelatedEventData commandCorrelatedEventData = (CommandCorrelatedEventData)payload;
		return obj.GenerateMessage(commandCorrelatedEventData.ExecuteMethod.ToString());
	}

	public static DbCommand CommandCreated([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, DbCommandMethod commandMethod, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition<string, int> eventDefinition = RelationalResources.LogCommandCreated(diagnostics);
		LogCommandCreated(diagnostics, eventDefinition, commandMethod, duration);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandEndEventData eventData = BroadcastCommandCreated(diagnostics, connection.DbConnection, command, context, commandMethod, commandId, connectionId, async: false, startTime, duration, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.CommandCreated(eventData, command);
			}
		}
		return command;
	}

	private static CommandEndEventData BroadcastCommandCreated(IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, DbConnection connection, DbCommand command, DbContext context, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, bool async, DateTimeOffset startTime, TimeSpan duration, EventDefinition<string, int> definition, bool diagnosticSourceEnabled)
	{
		CommandEndEventData commandEndEventData = new CommandEndEventData(definition, CommandCreated, connection, command, context, executeMethod, commandId, connectionId, async, logParameterValues: false, startTime, duration);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, commandEndEventData);
		}
		return commandEndEventData;
	}

	private static void LogCommandCreated(IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, EventDefinition<string, int> definition, DbCommandMethod commandMethod, TimeSpan duration)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, commandMethod.ToString(), (int)duration.TotalMilliseconds);
		}
	}

	private static string CommandCreated(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, int> obj = (EventDefinition<string, int>)definition;
		CommandEndEventData commandEndEventData = (CommandEndEventData)payload;
		return obj.GenerateMessage(commandEndEventData.ExecuteMethod.ToString(), (int)commandEndEventData.Duration.TotalMilliseconds);
	}

	public static InterceptionResult<DbDataReader> CommandReaderExecuting([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, DateTimeOffset startTime)
	{
		EventDefinition<string, CommandType, int, string, string> eventDefinition = RelationalResources.LogExecutingCommand(diagnostics);
		LogCommandExecuting(diagnostics, command, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandEventData eventData = BroadcastCommandExecuting(diagnostics, connection.DbConnection, command, context, DbCommandMethod.ExecuteReader, commandId, connectionId, async: false, startTime, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.ReaderExecuting(command, eventData, default(InterceptionResult<DbDataReader>));
			}
		}
		return default(InterceptionResult<DbDataReader>);
	}

	public static InterceptionResult<object> CommandScalarExecuting([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, DateTimeOffset startTime)
	{
		EventDefinition<string, CommandType, int, string, string> eventDefinition = RelationalResources.LogExecutingCommand(diagnostics);
		LogCommandExecuting(diagnostics, command, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandEventData eventData = BroadcastCommandExecuting(diagnostics, connection.DbConnection, command, context, DbCommandMethod.ExecuteScalar, commandId, connectionId, async: false, startTime, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.ScalarExecuting(command, eventData, default(InterceptionResult<object>));
			}
		}
		return default(InterceptionResult<object>);
	}

	public static InterceptionResult<int> CommandNonQueryExecuting([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, DateTimeOffset startTime)
	{
		EventDefinition<string, CommandType, int, string, string> eventDefinition = RelationalResources.LogExecutingCommand(diagnostics);
		LogCommandExecuting(diagnostics, command, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandEventData eventData = BroadcastCommandExecuting(diagnostics, connection.DbConnection, command, context, DbCommandMethod.ExecuteNonQuery, commandId, connectionId, async: false, startTime, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.NonQueryExecuting(command, eventData, default(InterceptionResult<int>));
			}
		}
		return default(InterceptionResult<int>);
	}

	public static Task<InterceptionResult<DbDataReader>> CommandReaderExecutingAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, DateTimeOffset startTime, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition<string, CommandType, int, string, string> eventDefinition = RelationalResources.LogExecutingCommand(diagnostics);
		LogCommandExecuting(diagnostics, command, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandEventData eventData = BroadcastCommandExecuting(diagnostics, connection.DbConnection, command, context, DbCommandMethod.ExecuteReader, commandId, connectionId, async: true, startTime, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.ReaderExecutingAsync(command, eventData, default(InterceptionResult<DbDataReader>), cancellationToken);
			}
		}
		return Task.FromResult(default(InterceptionResult<DbDataReader>));
	}

	public static Task<InterceptionResult<object>> CommandScalarExecutingAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, DateTimeOffset startTime, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition<string, CommandType, int, string, string> eventDefinition = RelationalResources.LogExecutingCommand(diagnostics);
		LogCommandExecuting(diagnostics, command, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandEventData eventData = BroadcastCommandExecuting(diagnostics, connection.DbConnection, command, context, DbCommandMethod.ExecuteScalar, commandId, connectionId, async: true, startTime, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.ScalarExecutingAsync(command, eventData, default(InterceptionResult<object>), cancellationToken);
			}
		}
		return Task.FromResult(default(InterceptionResult<object>));
	}

	public static Task<InterceptionResult<int>> CommandNonQueryExecutingAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, DateTimeOffset startTime, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition<string, CommandType, int, string, string> eventDefinition = RelationalResources.LogExecutingCommand(diagnostics);
		LogCommandExecuting(diagnostics, command, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandEventData eventData = BroadcastCommandExecuting(diagnostics, connection.DbConnection, command, context, DbCommandMethod.ExecuteNonQuery, commandId, connectionId, async: true, startTime, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.NonQueryExecutingAsync(command, eventData, default(InterceptionResult<int>), cancellationToken);
			}
		}
		return Task.FromResult(default(InterceptionResult<int>));
	}

	private static CommandEventData BroadcastCommandExecuting(IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, DbConnection connection, DbCommand command, DbContext context, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, bool async, DateTimeOffset startTime, EventDefinition<string, CommandType, int, string, string> definition, bool diagnosticSourceEnabled)
	{
		CommandEventData commandEventData = new CommandEventData(definition, CommandExecuting, connection, command, context, executeMethod, commandId, connectionId, async, ShouldLogParameterValues(diagnostics, command), startTime);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, commandEventData);
		}
		return commandEventData;
	}

	private static void LogCommandExecuting(IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, DbCommand command, EventDefinition<string, CommandType, int, string, string> definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, command.Parameters.FormatParameters(ShouldLogParameterValues(diagnostics, command)), command.CommandType, command.CommandTimeout, Environment.NewLine, command.CommandText.TrimEnd(Array.Empty<char>()));
		}
	}

	private static string CommandExecuting(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, CommandType, int, string, string> obj = (EventDefinition<string, CommandType, int, string, string>)definition;
		CommandEventData commandEventData = (CommandEventData)payload;
		return obj.GenerateMessage(commandEventData.Command.Parameters.FormatParameters(commandEventData.LogParameterValues), commandEventData.Command.CommandType, commandEventData.Command.CommandTimeout, Environment.NewLine, commandEventData.Command.CommandText.TrimEnd(Array.Empty<char>()));
	}

	private static bool ShouldLogParameterValues(IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, DbCommand command)
	{
		if (command.Parameters.Count > 0)
		{
			return diagnostics.ShouldLogSensitiveData();
		}
		return false;
	}

	public static DbDataReader CommandReaderExecuted([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, [JetBrains.Annotations.CanBeNull] DbDataReader methodResult, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition<string, string, CommandType, int, string, string> eventDefinition = RelationalResources.LogExecutedCommand(diagnostics);
		LogCommandExecuted(diagnostics, command, duration, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandExecutedEventData eventData = BroadcastCommandExecuted(diagnostics, connection.DbConnection, command, context, DbCommandMethod.ExecuteReader, commandId, connectionId, methodResult, async: false, startTime, duration, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.ReaderExecuted(command, eventData, methodResult);
			}
		}
		return methodResult;
	}

	public static object CommandScalarExecuted([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, [JetBrains.Annotations.CanBeNull] object methodResult, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition<string, string, CommandType, int, string, string> eventDefinition = RelationalResources.LogExecutedCommand(diagnostics);
		LogCommandExecuted(diagnostics, command, duration, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandExecutedEventData eventData = BroadcastCommandExecuted(diagnostics, connection.DbConnection, command, context, DbCommandMethod.ExecuteScalar, commandId, connectionId, methodResult, async: false, startTime, duration, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.ScalarExecuted(command, eventData, methodResult);
			}
		}
		return methodResult;
	}

	public static int CommandNonQueryExecuted([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, int methodResult, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition<string, string, CommandType, int, string, string> eventDefinition = RelationalResources.LogExecutedCommand(diagnostics);
		LogCommandExecuted(diagnostics, command, duration, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandExecutedEventData eventData = BroadcastCommandExecuted(diagnostics, connection.DbConnection, command, context, DbCommandMethod.ExecuteNonQuery, commandId, connectionId, methodResult, async: false, startTime, duration, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.NonQueryExecuted(command, eventData, methodResult);
			}
		}
		return methodResult;
	}

	public static Task<DbDataReader> CommandReaderExecutedAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, [JetBrains.Annotations.CanBeNull] DbDataReader methodResult, DateTimeOffset startTime, TimeSpan duration, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition<string, string, CommandType, int, string, string> eventDefinition = RelationalResources.LogExecutedCommand(diagnostics);
		LogCommandExecuted(diagnostics, command, duration, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandExecutedEventData eventData = BroadcastCommandExecuted(diagnostics, connection.DbConnection, command, context, DbCommandMethod.ExecuteReader, commandId, connectionId, methodResult, async: true, startTime, duration, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.ReaderExecutedAsync(command, eventData, methodResult, cancellationToken);
			}
		}
		return Task.FromResult(methodResult);
	}

	public static Task<object> CommandScalarExecutedAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, [JetBrains.Annotations.CanBeNull] object methodResult, DateTimeOffset startTime, TimeSpan duration, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition<string, string, CommandType, int, string, string> eventDefinition = RelationalResources.LogExecutedCommand(diagnostics);
		LogCommandExecuted(diagnostics, command, duration, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandExecutedEventData eventData = BroadcastCommandExecuted(diagnostics, connection.DbConnection, command, context, DbCommandMethod.ExecuteScalar, commandId, connectionId, methodResult, async: true, startTime, duration, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.ScalarExecutedAsync(command, eventData, methodResult, cancellationToken);
			}
		}
		return Task.FromResult(methodResult);
	}

	public static Task<int> CommandNonQueryExecutedAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, int methodResult, DateTimeOffset startTime, TimeSpan duration, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition<string, string, CommandType, int, string, string> eventDefinition = RelationalResources.LogExecutedCommand(diagnostics);
		LogCommandExecuted(diagnostics, command, duration, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandExecutedEventData eventData = BroadcastCommandExecuted(diagnostics, connection.DbConnection, command, context, DbCommandMethod.ExecuteNonQuery, commandId, connectionId, methodResult, async: true, startTime, duration, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.NonQueryExecutedAsync(command, eventData, methodResult, cancellationToken);
			}
		}
		return Task.FromResult(methodResult);
	}

	private static CommandExecutedEventData BroadcastCommandExecuted(IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, DbConnection connection, DbCommand command, DbContext context, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, object methodResult, bool async, DateTimeOffset startTime, TimeSpan duration, EventDefinition<string, string, CommandType, int, string, string> definition, bool diagnosticSourceEnabled)
	{
		CommandExecutedEventData commandExecutedEventData = new CommandExecutedEventData(definition, CommandExecuted, connection, command, context, executeMethod, commandId, connectionId, methodResult, async, ShouldLogParameterValues(diagnostics, command), startTime, duration);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, commandExecutedEventData);
		}
		return commandExecutedEventData;
	}

	private static void LogCommandExecuted(IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, DbCommand command, TimeSpan duration, EventDefinition<string, string, CommandType, int, string, string> definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, string.Format(CultureInfo.InvariantCulture, "{0:N0}", duration.TotalMilliseconds), command.Parameters.FormatParameters(ShouldLogParameterValues(diagnostics, command)), command.CommandType, command.CommandTimeout, Environment.NewLine, command.CommandText.TrimEnd(Array.Empty<char>()));
		}
	}

	private static string CommandExecuted(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, CommandType, int, string, string> obj = (EventDefinition<string, string, CommandType, int, string, string>)definition;
		CommandExecutedEventData commandExecutedEventData = (CommandExecutedEventData)payload;
		return obj.GenerateMessage(string.Format(CultureInfo.InvariantCulture, "{0:N0}", commandExecutedEventData.Duration.TotalMilliseconds), commandExecutedEventData.Command.Parameters.FormatParameters(commandExecutedEventData.LogParameterValues), commandExecutedEventData.Command.CommandType, commandExecutedEventData.Command.CommandTimeout, Environment.NewLine, commandExecutedEventData.Command.CommandText.TrimEnd(Array.Empty<char>()));
	}

	public static void CommandError([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, [JetBrains.Annotations.NotNull] Exception exception, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition<string, string, CommandType, int, string, string> eventDefinition = RelationalResources.LogCommandFailed(diagnostics);
		LogCommandError(diagnostics, command, duration, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandErrorEventData eventData = BroadcastCommandError(diagnostics, connection.DbConnection, command, context, executeMethod, commandId, connectionId, exception, async: false, startTime, duration, eventDefinition, flag);
			dbCommandInterceptor?.CommandFailed(command, eventData);
		}
	}

	private static void LogCommandError(IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, DbCommand command, TimeSpan duration, EventDefinition<string, string, CommandType, int, string, string> definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, string.Format(CultureInfo.InvariantCulture, "{0:N0}", duration.TotalMilliseconds), command.Parameters.FormatParameters(ShouldLogParameterValues(diagnostics, command)), command.CommandType, command.CommandTimeout, Environment.NewLine, command.CommandText.TrimEnd(Array.Empty<char>()));
		}
	}

	public static Task CommandErrorAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, [JetBrains.Annotations.NotNull] Exception exception, DateTimeOffset startTime, TimeSpan duration, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition<string, string, CommandType, int, string, string> eventDefinition = RelationalResources.LogCommandFailed(diagnostics);
		LogCommandError(diagnostics, command, duration, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			CommandErrorEventData eventData = BroadcastCommandError(diagnostics, connection.DbConnection, command, context, executeMethod, commandId, connectionId, exception, async: true, startTime, duration, eventDefinition, flag);
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.CommandFailedAsync(command, eventData, cancellationToken);
			}
		}
		return Task.CompletedTask;
	}

	private static CommandErrorEventData BroadcastCommandError(IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, DbConnection connection, DbCommand command, DbContext context, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, Exception exception, bool async, DateTimeOffset startTime, TimeSpan duration, EventDefinition<string, string, CommandType, int, string, string> definition, bool diagnosticSourceEnabled)
	{
		CommandErrorEventData commandErrorEventData = new CommandErrorEventData(definition, CommandError, connection, command, context, executeMethod, commandId, connectionId, exception, async, ShouldLogParameterValues(diagnostics, command), startTime, duration);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, commandErrorEventData);
		}
		return commandErrorEventData;
	}

	private static string CommandError(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string, CommandType, int, string, string> obj = (EventDefinition<string, string, CommandType, int, string, string>)definition;
		CommandErrorEventData commandErrorEventData = (CommandErrorEventData)payload;
		return obj.GenerateMessage(string.Format(CultureInfo.InvariantCulture, "{0:N0}", commandErrorEventData.Duration.TotalMilliseconds), commandErrorEventData.Command.Parameters.FormatParameters(commandErrorEventData.LogParameterValues), commandErrorEventData.Command.CommandType, commandErrorEventData.Command.CommandTimeout, Environment.NewLine, commandErrorEventData.Command.CommandText.TrimEnd(Array.Empty<char>()));
	}

	public static InterceptionResult ConnectionOpening([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, DateTimeOffset startTime)
	{
		EventDefinition<string, string> eventDefinition = RelationalResources.LogOpeningConnection(diagnostics);
		LogConnectionOpening(diagnostics, connection, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbConnectionInterceptor dbConnectionInterceptor = diagnostics.Interceptors?.Aggregate<IDbConnectionInterceptor>();
		if (dbConnectionInterceptor != null || flag)
		{
			ConnectionEventData eventData = BroadcastConnectionOpening(diagnostics, connection, startTime, eventDefinition, async: false, flag);
			if (dbConnectionInterceptor != null)
			{
				return dbConnectionInterceptor.ConnectionOpening(connection.DbConnection, eventData, default(InterceptionResult));
			}
		}
		return default(InterceptionResult);
	}

	public static Task<InterceptionResult> ConnectionOpeningAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, DateTimeOffset startTime, CancellationToken cancellationToken)
	{
		EventDefinition<string, string> eventDefinition = RelationalResources.LogOpeningConnection(diagnostics);
		LogConnectionOpening(diagnostics, connection, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbConnectionInterceptor dbConnectionInterceptor = diagnostics.Interceptors?.Aggregate<IDbConnectionInterceptor>();
		if (dbConnectionInterceptor != null || flag)
		{
			ConnectionEventData eventData = BroadcastConnectionOpening(diagnostics, connection, startTime, eventDefinition, async: true, flag);
			if (dbConnectionInterceptor != null)
			{
				return dbConnectionInterceptor.ConnectionOpeningAsync(connection.DbConnection, eventData, default(InterceptionResult), cancellationToken);
			}
		}
		return Task.FromResult(default(InterceptionResult));
	}

	private static void LogConnectionOpening(IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, IRelationalConnection connection, EventDefinition<string, string> definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, connection.DbConnection.Database, connection.DbConnection.DataSource);
		}
	}

	private static ConnectionEventData BroadcastConnectionOpening(IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, IRelationalConnection connection, DateTimeOffset startTime, EventDefinition<string, string> definition, bool async, bool diagnosticSourceEnabled)
	{
		ConnectionEventData connectionEventData = new ConnectionEventData(definition, ConnectionOpening, connection.DbConnection, connection.Context, connection.ConnectionId, async, startTime);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, connectionEventData);
		}
		return connectionEventData;
	}

	private static string ConnectionOpening(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		ConnectionEventData connectionEventData = (ConnectionEventData)payload;
		return obj.GenerateMessage(connectionEventData.Connection.Database, connectionEventData.Connection.DataSource);
	}

	public static void ConnectionOpened([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition<string, string> eventDefinition = RelationalResources.LogOpenedConnection(diagnostics);
		LogConnectionOpened(diagnostics, connection, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbConnectionInterceptor dbConnectionInterceptor = diagnostics.Interceptors?.Aggregate<IDbConnectionInterceptor>();
		if (dbConnectionInterceptor != null || flag)
		{
			ConnectionEndEventData eventData = BroadcastConnectionOpened(diagnostics, connection, async: false, startTime, duration, eventDefinition, flag);
			dbConnectionInterceptor?.ConnectionOpened(connection.DbConnection, eventData);
		}
	}

	public static Task ConnectionOpenedAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, DateTimeOffset startTime, TimeSpan duration, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition<string, string> eventDefinition = RelationalResources.LogOpenedConnection(diagnostics);
		LogConnectionOpened(diagnostics, connection, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbConnectionInterceptor dbConnectionInterceptor = diagnostics.Interceptors?.Aggregate<IDbConnectionInterceptor>();
		if (dbConnectionInterceptor != null || flag)
		{
			ConnectionEndEventData eventData = BroadcastConnectionOpened(diagnostics, connection, async: true, startTime, duration, eventDefinition, flag);
			if (dbConnectionInterceptor != null)
			{
				return dbConnectionInterceptor.ConnectionOpenedAsync(connection.DbConnection, eventData, cancellationToken);
			}
		}
		return Task.CompletedTask;
	}

	private static ConnectionEndEventData BroadcastConnectionOpened(IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, IRelationalConnection connection, bool async, DateTimeOffset startTime, TimeSpan duration, EventDefinition<string, string> definition, bool diagnosticSourceEnabled)
	{
		ConnectionEndEventData connectionEndEventData = new ConnectionEndEventData(definition, ConnectionOpened, connection.DbConnection, connection.Context, connection.ConnectionId, async, startTime, duration);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, connectionEndEventData);
		}
		return connectionEndEventData;
	}

	private static void LogConnectionOpened(IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, IRelationalConnection connection, EventDefinition<string, string> definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, connection.DbConnection.Database, connection.DbConnection.DataSource);
		}
	}

	private static string ConnectionOpened(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		ConnectionEndEventData connectionEndEventData = (ConnectionEndEventData)payload;
		return obj.GenerateMessage(connectionEndEventData.Connection.Database, connectionEndEventData.Connection.DataSource);
	}

	public static InterceptionResult ConnectionClosing([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, DateTimeOffset startTime)
	{
		EventDefinition<string, string> eventDefinition = RelationalResources.LogClosingConnection(diagnostics);
		LogConnectionClosing(diagnostics, connection, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbConnectionInterceptor dbConnectionInterceptor = diagnostics.Interceptors?.Aggregate<IDbConnectionInterceptor>();
		if (dbConnectionInterceptor != null || flag)
		{
			ConnectionEventData eventData = BroadcastConnectionClosing(diagnostics, connection, startTime, async: false, eventDefinition, flag);
			if (dbConnectionInterceptor != null)
			{
				return dbConnectionInterceptor.ConnectionClosing(connection.DbConnection, eventData, default(InterceptionResult));
			}
		}
		return default(InterceptionResult);
	}

	public static Task<InterceptionResult> ConnectionClosingAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, DateTimeOffset startTime)
	{
		EventDefinition<string, string> eventDefinition = RelationalResources.LogClosingConnection(diagnostics);
		LogConnectionClosing(diagnostics, connection, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbConnectionInterceptor dbConnectionInterceptor = diagnostics.Interceptors?.Aggregate<IDbConnectionInterceptor>();
		if (dbConnectionInterceptor != null || flag)
		{
			ConnectionEventData eventData = BroadcastConnectionClosing(diagnostics, connection, startTime, async: true, eventDefinition, flag);
			if (dbConnectionInterceptor != null)
			{
				return dbConnectionInterceptor.ConnectionClosingAsync(connection.DbConnection, eventData, default(InterceptionResult));
			}
		}
		return Task.FromResult(default(InterceptionResult));
	}

	private static ConnectionEventData BroadcastConnectionClosing(IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, IRelationalConnection connection, DateTimeOffset startTime, bool async, EventDefinition<string, string> definition, bool diagnosticSourceEnabled)
	{
		ConnectionEventData connectionEventData = new ConnectionEventData(definition, ConnectionClosing, connection.DbConnection, connection.Context, connection.ConnectionId, async, startTime);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, connectionEventData);
		}
		return connectionEventData;
	}

	private static void LogConnectionClosing(IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, IRelationalConnection connection, EventDefinition<string, string> definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, connection.DbConnection.Database, connection.DbConnection.DataSource);
		}
	}

	private static string ConnectionClosing(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		ConnectionEventData connectionEventData = (ConnectionEventData)payload;
		return obj.GenerateMessage(connectionEventData.Connection.Database, connectionEventData.Connection.DataSource);
	}

	public static void ConnectionClosed([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition<string, string> eventDefinition = RelationalResources.LogClosedConnection(diagnostics);
		LogConnectionClosed(diagnostics, connection, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbConnectionInterceptor dbConnectionInterceptor = diagnostics.Interceptors?.Aggregate<IDbConnectionInterceptor>();
		if (dbConnectionInterceptor != null || flag)
		{
			ConnectionEndEventData eventData = BroadcastCollectionClosed(diagnostics, connection, startTime, duration, async: false, eventDefinition, flag);
			dbConnectionInterceptor?.ConnectionClosed(connection.DbConnection, eventData);
		}
	}

	public static Task ConnectionClosedAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition<string, string> eventDefinition = RelationalResources.LogClosedConnection(diagnostics);
		LogConnectionClosed(diagnostics, connection, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbConnectionInterceptor dbConnectionInterceptor = diagnostics.Interceptors?.Aggregate<IDbConnectionInterceptor>();
		if (dbConnectionInterceptor != null || flag)
		{
			ConnectionEndEventData eventData = BroadcastCollectionClosed(diagnostics, connection, startTime, duration, async: true, eventDefinition, flag);
			if (dbConnectionInterceptor != null)
			{
				return dbConnectionInterceptor.ConnectionClosedAsync(connection.DbConnection, eventData);
			}
		}
		return Task.CompletedTask;
	}

	private static ConnectionEndEventData BroadcastCollectionClosed(IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, IRelationalConnection connection, DateTimeOffset startTime, TimeSpan duration, bool async, EventDefinition<string, string> definition, bool diagnosticSourceEnabled)
	{
		ConnectionEndEventData connectionEndEventData = new ConnectionEndEventData(definition, ConnectionClosed, connection.DbConnection, connection.Context, connection.ConnectionId, async, startTime, duration);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, connectionEndEventData);
		}
		return connectionEndEventData;
	}

	private static void LogConnectionClosed(IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, IRelationalConnection connection, EventDefinition<string, string> definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, connection.DbConnection.Database, connection.DbConnection.DataSource);
		}
	}

	private static string ConnectionClosed(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		ConnectionEndEventData connectionEndEventData = (ConnectionEndEventData)payload;
		return obj.GenerateMessage(connectionEndEventData.Connection.Database, connectionEndEventData.Connection.DataSource);
	}

	public static void ConnectionError([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] Exception exception, DateTimeOffset startTime, TimeSpan duration, bool logErrorAsDebug)
	{
		EventDefinition<string, string> eventDefinition = (logErrorAsDebug ? RelationalResources.LogConnectionErrorAsDebug(diagnostics) : RelationalResources.LogConnectionError(diagnostics));
		LogConnectionError(diagnostics, connection, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbConnectionInterceptor dbConnectionInterceptor = diagnostics.Interceptors?.Aggregate<IDbConnectionInterceptor>();
		if (dbConnectionInterceptor != null || flag)
		{
			ConnectionErrorEventData eventData = BroadcastConnectionError(diagnostics, connection, exception, startTime, duration, async: false, eventDefinition, flag);
			dbConnectionInterceptor?.ConnectionFailed(connection.DbConnection, eventData);
		}
	}

	public static Task ConnectionErrorAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] Exception exception, DateTimeOffset startTime, TimeSpan duration, bool logErrorAsDebug, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition<string, string> eventDefinition = (logErrorAsDebug ? RelationalResources.LogConnectionErrorAsDebug(diagnostics) : RelationalResources.LogConnectionError(diagnostics));
		LogConnectionError(diagnostics, connection, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbConnectionInterceptor dbConnectionInterceptor = diagnostics.Interceptors?.Aggregate<IDbConnectionInterceptor>();
		if (dbConnectionInterceptor != null || flag)
		{
			ConnectionErrorEventData eventData = BroadcastConnectionError(diagnostics, connection, exception, startTime, duration, async: true, eventDefinition, flag);
			if (dbConnectionInterceptor != null)
			{
				return dbConnectionInterceptor.ConnectionFailedAsync(connection.DbConnection, eventData, cancellationToken);
			}
		}
		return Task.CompletedTask;
	}

	private static ConnectionErrorEventData BroadcastConnectionError(IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, IRelationalConnection connection, Exception exception, DateTimeOffset startTime, TimeSpan duration, bool async, EventDefinition<string, string> definition, bool diagnosticSourceEnabled)
	{
		ConnectionErrorEventData connectionErrorEventData = new ConnectionErrorEventData(definition, ConnectionError, connection.DbConnection, connection.Context, connection.ConnectionId, exception, async, startTime, duration);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, connectionErrorEventData);
		}
		return connectionErrorEventData;
	}

	private static void LogConnectionError(IDiagnosticsLogger<DbLoggerCategory.Database.Connection> diagnostics, IRelationalConnection connection, EventDefinition<string, string> definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, connection.DbConnection.Database, connection.DbConnection.DataSource);
		}
	}

	private static string ConnectionError(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		ConnectionErrorEventData connectionErrorEventData = (ConnectionErrorEventData)payload;
		return obj.GenerateMessage(connectionErrorEventData.Connection.Database, connectionErrorEventData.Connection.DataSource);
	}

	public static InterceptionResult<DbTransaction> TransactionStarting([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, System.Data.IsolationLevel isolationLevel, Guid transactionId, DateTimeOffset startTime)
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogBeginningTransaction(diagnostics);
		LogTransactionStarting(diagnostics, isolationLevel, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionStartingEventData eventData = BroadcastTransactionStarting(diagnostics, connection, isolationLevel, transactionId, async: false, startTime, eventDefinition, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionStarting(connection.DbConnection, eventData, default(InterceptionResult<DbTransaction>));
			}
		}
		return default(InterceptionResult<DbTransaction>);
	}

	public static Task<InterceptionResult<DbTransaction>> TransactionStartingAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, System.Data.IsolationLevel isolationLevel, Guid transactionId, DateTimeOffset startTime, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogBeginningTransaction(diagnostics);
		LogTransactionStarting(diagnostics, isolationLevel, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionStartingEventData eventData = BroadcastTransactionStarting(diagnostics, connection, isolationLevel, transactionId, async: true, startTime, eventDefinition, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionStartingAsync(connection.DbConnection, eventData, default(InterceptionResult<DbTransaction>), cancellationToken);
			}
		}
		return Task.FromResult(default(InterceptionResult<DbTransaction>));
	}

	private static TransactionStartingEventData BroadcastTransactionStarting(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, IRelationalConnection connection, System.Data.IsolationLevel isolationLevel, Guid transactionId, bool async, DateTimeOffset startTime, EventDefinition<string> definition, bool diagnosticSourceEnabled)
	{
		TransactionStartingEventData transactionStartingEventData = new TransactionStartingEventData(definition, TransactionStarting, connection.Context, isolationLevel, transactionId, connection.ConnectionId, async, startTime);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, transactionStartingEventData);
		}
		return transactionStartingEventData;
	}

	private static void LogTransactionStarting(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, System.Data.IsolationLevel isolationLevel, EventDefinition<string> definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, isolationLevel.ToString("G"));
		}
	}

	private static string TransactionStarting(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		TransactionStartingEventData transactionStartingEventData = (TransactionStartingEventData)payload;
		return obj.GenerateMessage(transactionStartingEventData.IsolationLevel.ToString("G"));
	}

	public static DbTransaction TransactionStarted([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogBeganTransaction(diagnostics);
		LogTransactionStarted(diagnostics, transaction, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionEndEventData eventData = BroadcastTransactionStarted(diagnostics, connection, transaction, transactionId, async: false, startTime, duration, eventDefinition, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionStarted(connection.DbConnection, eventData, transaction);
			}
		}
		return transaction;
	}

	public static Task<DbTransaction> TransactionStartedAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, TimeSpan duration, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogBeganTransaction(diagnostics);
		LogTransactionStarted(diagnostics, transaction, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionEndEventData eventData = BroadcastTransactionStarted(diagnostics, connection, transaction, transactionId, async: true, startTime, duration, eventDefinition, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionStartedAsync(connection.DbConnection, eventData, transaction, cancellationToken);
			}
		}
		return Task.FromResult(transaction);
	}

	private static TransactionEndEventData BroadcastTransactionStarted(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, IRelationalConnection connection, DbTransaction transaction, Guid transactionId, bool async, DateTimeOffset startTime, TimeSpan duration, EventDefinition<string> definition, bool diagnosticSourceEnabled)
	{
		TransactionEndEventData transactionEndEventData = new TransactionEndEventData(definition, TransactionStarted, transaction, connection.Context, transactionId, connection.ConnectionId, async, startTime, duration);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, transactionEndEventData);
		}
		return transactionEndEventData;
	}

	private static void LogTransactionStarted(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, DbTransaction transaction, EventDefinition<string> definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, transaction.IsolationLevel.ToString("G"));
		}
	}

	private static string TransactionStarted(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		TransactionEndEventData transactionEndEventData = (TransactionEndEventData)payload;
		return obj.GenerateMessage(transactionEndEventData.Transaction.IsolationLevel.ToString("G"));
	}

	public static DbTransaction TransactionUsed([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime)
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogUsingTransaction(diagnostics);
		LogTransactionUsed(diagnostics, transaction, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionEventData eventData = BroadcasstTransactionUsed(diagnostics, connection, transaction, transactionId, async: false, startTime, eventDefinition, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionUsed(connection.DbConnection, eventData, transaction);
			}
		}
		return transaction;
	}

	public static Task<DbTransaction> TransactionUsedAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogUsingTransaction(diagnostics);
		LogTransactionUsed(diagnostics, transaction, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionEventData eventData = BroadcasstTransactionUsed(diagnostics, connection, transaction, transactionId, async: true, startTime, eventDefinition, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionUsedAsync(connection.DbConnection, eventData, transaction, cancellationToken);
			}
		}
		return Task.FromResult(transaction);
	}

	private static TransactionEventData BroadcasstTransactionUsed(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, IRelationalConnection connection, DbTransaction transaction, Guid transactionId, bool async, DateTimeOffset startTime, EventDefinition<string> definition, bool diagnosticSourceEnabled)
	{
		TransactionEventData transactionEventData = new TransactionEventData(definition, TransactionUsed, transaction, connection.Context, transactionId, connection.ConnectionId, async, startTime);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, transactionEventData);
		}
		return transactionEventData;
	}

	private static void LogTransactionUsed(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, DbTransaction transaction, EventDefinition<string> definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, transaction.IsolationLevel.ToString("G"));
		}
	}

	private static string TransactionUsed(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		TransactionEventData transactionEventData = (TransactionEventData)payload;
		return obj.GenerateMessage(transactionEventData.Transaction.IsolationLevel.ToString("G"));
	}

	public static InterceptionResult TransactionCommitting([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime)
	{
		EventDefinition eventDefinition = RelationalResources.LogCommittingTransaction(diagnostics);
		LogTransactionCommitting(diagnostics, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionEventData eventData = BroadcastTransactionCommitting(diagnostics, connection, transaction, transactionId, startTime, eventDefinition, async: false, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionCommitting(transaction, eventData, default(InterceptionResult));
			}
		}
		return default(InterceptionResult);
	}

	public static Task<InterceptionResult> TransactionCommittingAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition eventDefinition = RelationalResources.LogCommittingTransaction(diagnostics);
		LogTransactionCommitting(diagnostics, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionEventData eventData = BroadcastTransactionCommitting(diagnostics, connection, transaction, transactionId, startTime, eventDefinition, async: true, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionCommittingAsync(transaction, eventData, default(InterceptionResult), cancellationToken);
			}
		}
		return Task.FromResult(default(InterceptionResult));
	}

	private static TransactionEventData BroadcastTransactionCommitting(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, IRelationalConnection connection, DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, EventDefinition definition, bool async, bool diagnosticSourceEnabled)
	{
		TransactionEventData transactionEventData = new TransactionEventData(definition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage(), transaction, connection.Context, transactionId, connection.ConnectionId, async, startTime);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, transactionEventData);
		}
		return transactionEventData;
	}

	private static void LogTransactionCommitting(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, EventDefinition definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior);
		}
	}

	public static void TransactionCommitted([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition eventDefinition = RelationalResources.LogCommittedTransaction(diagnostics);
		LogTransactionCommitted(diagnostics, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionEndEventData eventData = BroadcastTransactionCommitted(diagnostics, connection, transaction, transactionId, startTime, duration, eventDefinition, async: false, flag);
			dbTransactionInterceptor?.TransactionCommitted(transaction, eventData);
		}
	}

	public static Task TransactionCommittedAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, TimeSpan duration, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition eventDefinition = RelationalResources.LogCommittedTransaction(diagnostics);
		LogTransactionCommitted(diagnostics, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionEndEventData eventData = BroadcastTransactionCommitted(diagnostics, connection, transaction, transactionId, startTime, duration, eventDefinition, async: true, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionCommittedAsync(transaction, eventData, cancellationToken);
			}
		}
		return Task.CompletedTask;
	}

	private static TransactionEndEventData BroadcastTransactionCommitted(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, IRelationalConnection connection, DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, TimeSpan duration, EventDefinition definition, bool async, bool diagnosticSourceEnabled)
	{
		TransactionEndEventData transactionEndEventData = new TransactionEndEventData(definition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage(), transaction, connection.Context, transactionId, connection.ConnectionId, async, startTime, duration);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, transactionEndEventData);
		}
		return transactionEndEventData;
	}

	private static void LogTransactionCommitted(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, EventDefinition definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior);
		}
	}

	public static void TransactionRolledBack([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition eventDefinition = RelationalResources.LogRolledBackTransaction(diagnostics);
		LogTransactionRolledBack(diagnostics, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionEndEventData eventData = BroadcastTransactionRolledBack(diagnostics, connection, transaction, transactionId, startTime, duration, eventDefinition, async: false, flag);
			dbTransactionInterceptor?.TransactionRolledBack(transaction, eventData);
		}
	}

	public static Task TransactionRolledBackAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, TimeSpan duration, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition eventDefinition = RelationalResources.LogRolledBackTransaction(diagnostics);
		LogTransactionRolledBack(diagnostics, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionEndEventData eventData = BroadcastTransactionRolledBack(diagnostics, connection, transaction, transactionId, startTime, duration, eventDefinition, async: true, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionRolledBackAsync(transaction, eventData, cancellationToken);
			}
		}
		return Task.CompletedTask;
	}

	private static TransactionEndEventData BroadcastTransactionRolledBack(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, IRelationalConnection connection, DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, TimeSpan duration, EventDefinition definition, bool async, bool diagnosticSourceEnabled)
	{
		TransactionEndEventData transactionEndEventData = new TransactionEndEventData(definition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage(), transaction, connection.Context, transactionId, connection.ConnectionId, async, startTime, duration);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, transactionEndEventData);
		}
		return transactionEndEventData;
	}

	private static void LogTransactionRolledBack(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, EventDefinition definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior);
		}
	}

	public static InterceptionResult TransactionRollingBack([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime)
	{
		EventDefinition eventDefinition = RelationalResources.LogRollingBackTransaction(diagnostics);
		LogTransactionRollingBack(diagnostics, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionEventData eventData = BroadcastTransactionRollingBack(diagnostics, connection, transaction, transactionId, startTime, eventDefinition, async: false, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionRollingBack(transaction, eventData, default(InterceptionResult));
			}
		}
		return default(InterceptionResult);
	}

	public static Task<InterceptionResult> TransactionRollingBackAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition eventDefinition = RelationalResources.LogRollingBackTransaction(diagnostics);
		LogTransactionRollingBack(diagnostics, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionEventData eventData = BroadcastTransactionRollingBack(diagnostics, connection, transaction, transactionId, startTime, eventDefinition, async: true, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionRollingBackAsync(transaction, eventData, default(InterceptionResult), cancellationToken);
			}
		}
		return Task.FromResult(default(InterceptionResult));
	}

	private static TransactionEventData BroadcastTransactionRollingBack(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, IRelationalConnection connection, DbTransaction transaction, Guid transactionId, DateTimeOffset startTime, EventDefinition definition, bool async, bool diagnosticSourceEnabled)
	{
		TransactionEventData transactionEventData = new TransactionEventData(definition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage(), transaction, connection.Context, transactionId, connection.ConnectionId, async, startTime);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, transactionEventData);
		}
		return transactionEventData;
	}

	private static void LogTransactionRollingBack(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, EventDefinition definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior);
		}
	}

	public static void TransactionDisposed([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, DateTimeOffset startTime)
	{
		EventDefinition eventDefinition = RelationalResources.LogDisposingTransaction(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TransactionEventData(eventDefinition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage(), transaction, connection.Context, transactionId, connection.ConnectionId, async: false, startTime));
		}
	}

	public static void TransactionError([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, [JetBrains.Annotations.NotNull] string action, [JetBrains.Annotations.NotNull] Exception exception, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition eventDefinition = RelationalResources.LogTransactionError(diagnostics);
		LogTransactionError(diagnostics, exception, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionErrorEventData eventData = BroadcastTransactionError(diagnostics, connection, transaction, transactionId, action, exception, startTime, duration, eventDefinition, async: false, flag);
			dbTransactionInterceptor?.TransactionFailed(transaction, eventData);
		}
	}

	public static Task TransactionErrorAsync([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbTransaction transaction, Guid transactionId, [JetBrains.Annotations.NotNull] string action, [JetBrains.Annotations.NotNull] Exception exception, DateTimeOffset startTime, TimeSpan duration, CancellationToken cancellationToken = default(CancellationToken))
	{
		EventDefinition eventDefinition = RelationalResources.LogTransactionError(diagnostics);
		LogTransactionError(diagnostics, exception, eventDefinition);
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbTransactionInterceptor dbTransactionInterceptor = diagnostics.Interceptors?.Aggregate<IDbTransactionInterceptor>();
		if (dbTransactionInterceptor != null || flag)
		{
			TransactionErrorEventData eventData = BroadcastTransactionError(diagnostics, connection, transaction, transactionId, action, exception, startTime, duration, eventDefinition, async: true, flag);
			if (dbTransactionInterceptor != null)
			{
				return dbTransactionInterceptor.TransactionFailedAsync(transaction, eventData, cancellationToken);
			}
		}
		return Task.CompletedTask;
	}

	private static TransactionErrorEventData BroadcastTransactionError(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, IRelationalConnection connection, DbTransaction transaction, Guid transactionId, string action, Exception exception, DateTimeOffset startTime, TimeSpan duration, EventDefinition definition, bool async, bool diagnosticSourceEnabled)
	{
		TransactionErrorEventData transactionErrorEventData = new TransactionErrorEventData(definition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage(), transaction, connection.Context, transactionId, connection.ConnectionId, async, action, exception, startTime, duration);
		if (diagnosticSourceEnabled)
		{
			diagnostics.DiagnosticSource.Write(definition.EventId.Name, transactionErrorEventData);
		}
		return transactionErrorEventData;
	}

	private static void LogTransactionError(IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, Exception exception, EventDefinition definition)
	{
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, exception);
		}
	}

	public static void AmbientTransactionWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, DateTimeOffset startTime)
	{
		EventDefinition eventDefinition = RelationalResources.LogAmbientTransaction(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new ConnectionEventData(eventDefinition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage(), connection.DbConnection, connection.Context, connection.ConnectionId, async: false, startTime));
		}
	}

	public static void AmbientTransactionEnlisted([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] Transaction transaction)
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogAmbientTransactionEnlisted(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, transaction.IsolationLevel.ToString("G"));
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TransactionEnlistedEventData(eventDefinition, AmbientTransactionEnlisted, transaction, connection.DbConnection, connection.ConnectionId));
		}
	}

	private static string AmbientTransactionEnlisted(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		TransactionEnlistedEventData transactionEnlistedEventData = (TransactionEnlistedEventData)payload;
		return obj.GenerateMessage(transactionEnlistedEventData.Transaction.IsolationLevel.ToString("G"));
	}

	public static void ExplicitTransactionEnlisted([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] Transaction transaction)
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogExplicitTransactionEnlisted(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, transaction.IsolationLevel.ToString("G"));
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new TransactionEnlistedEventData(eventDefinition, ExplicitTransactionEnlisted, transaction, connection.DbConnection, connection.ConnectionId));
		}
	}

	private static string ExplicitTransactionEnlisted(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		TransactionEnlistedEventData transactionEnlistedEventData = (TransactionEnlistedEventData)payload;
		return obj.GenerateMessage(transactionEnlistedEventData.Transaction.IsolationLevel.ToString("G"));
	}

	public static InterceptionResult DataReaderDisposing([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Database.Command> diagnostics, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] DbDataReader dataReader, Guid commandId, int recordsAffected, int readCount, DateTimeOffset startTime, TimeSpan duration)
	{
		EventDefinition eventDefinition = RelationalResources.LogDisposingDataReader(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior);
		}
		bool flag = diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name);
		IDbCommandInterceptor dbCommandInterceptor = diagnostics.Interceptors?.Aggregate<IDbCommandInterceptor>();
		if (dbCommandInterceptor != null || flag)
		{
			DataReaderDisposingEventData dataReaderDisposingEventData = new DataReaderDisposingEventData(eventDefinition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage(), command, dataReader, connection.Context, commandId, connection.ConnectionId, recordsAffected, readCount, startTime, duration);
			if (flag)
			{
				diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, dataReaderDisposingEventData);
			}
			if (dbCommandInterceptor != null)
			{
				return dbCommandInterceptor.DataReaderDisposing(command, dataReaderDisposingEventData, default(InterceptionResult));
			}
		}
		return default(InterceptionResult);
	}

	public static void MigrateUsingConnection([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Migrations> diagnostics, [JetBrains.Annotations.NotNull] IMigrator migrator, [JetBrains.Annotations.NotNull] IRelationalConnection connection)
	{
		EventDefinition<string, string> eventDefinition = RelationalResources.LogMigrating(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			DbConnection dbConnection = connection.DbConnection;
			eventDefinition.Log(diagnostics, logBehavior, dbConnection.Database, dbConnection.DataSource);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new MigratorConnectionEventData(eventDefinition, MigrateUsingConnection, migrator, connection.DbConnection, connection.ConnectionId));
		}
	}

	private static string MigrateUsingConnection(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		MigratorConnectionEventData migratorConnectionEventData = (MigratorConnectionEventData)payload;
		return obj.GenerateMessage(migratorConnectionEventData.Connection.Database, migratorConnectionEventData.Connection.DataSource);
	}

	public static void MigrationReverting([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Migrations> diagnostics, [JetBrains.Annotations.NotNull] IMigrator migrator, [JetBrains.Annotations.NotNull] Migration migration)
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogRevertingMigration(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, migration.GetId());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new MigrationEventData(eventDefinition, MigrationReverting, migrator, migration));
		}
	}

	private static string MigrationReverting(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		MigrationEventData migrationEventData = (MigrationEventData)payload;
		return obj.GenerateMessage(migrationEventData.Migration.GetId());
	}

	public static void MigrationApplying([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Migrations> diagnostics, [JetBrains.Annotations.NotNull] IMigrator migrator, [JetBrains.Annotations.NotNull] Migration migration)
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogApplyingMigration(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, migration.GetId());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new MigrationEventData(eventDefinition, MigrationApplying, migrator, migration));
		}
	}

	private static string MigrationApplying(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		MigrationEventData migrationEventData = (MigrationEventData)payload;
		return obj.GenerateMessage(migrationEventData.Migration.GetId());
	}

	public static void MigrationGeneratingDownScript([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Migrations> diagnostics, [JetBrains.Annotations.NotNull] IMigrator migrator, [JetBrains.Annotations.NotNull] Migration migration, [JetBrains.Annotations.CanBeNull] string fromMigration, [JetBrains.Annotations.CanBeNull] string toMigration, bool idempotent)
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogGeneratingDown(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, migration.GetId());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new MigrationScriptingEventData(eventDefinition, MigrationGeneratingDownScript, migrator, migration, fromMigration, toMigration, idempotent));
		}
	}

	private static string MigrationGeneratingDownScript(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		MigrationScriptingEventData migrationScriptingEventData = (MigrationScriptingEventData)payload;
		return obj.GenerateMessage(migrationScriptingEventData.Migration.GetId());
	}

	public static void MigrationGeneratingUpScript([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Migrations> diagnostics, [JetBrains.Annotations.NotNull] IMigrator migrator, [JetBrains.Annotations.NotNull] Migration migration, [JetBrains.Annotations.CanBeNull] string fromMigration, [JetBrains.Annotations.CanBeNull] string toMigration, bool idempotent)
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogGeneratingUp(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, migration.GetId());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new MigrationScriptingEventData(eventDefinition, MigrationGeneratingUpScript, migrator, migration, fromMigration, toMigration, idempotent));
		}
	}

	private static string MigrationGeneratingUpScript(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		MigrationScriptingEventData migrationScriptingEventData = (MigrationScriptingEventData)payload;
		return obj.GenerateMessage(migrationScriptingEventData.Migration.GetId());
	}

	public static void MigrationsNotApplied([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Migrations> diagnostics, [JetBrains.Annotations.NotNull] IMigrator migrator)
	{
		EventDefinition eventDefinition = RelationalResources.LogNoMigrationsApplied(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new MigratorEventData(eventDefinition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage(), migrator));
		}
	}

	public static void MigrationsNotFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Migrations> diagnostics, [JetBrains.Annotations.NotNull] IMigrator migrator, [JetBrains.Annotations.NotNull] IMigrationsAssembly migrationsAssembly)
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogNoMigrationsFound(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, migrationsAssembly.Assembly.GetName().Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new MigrationAssemblyEventData(eventDefinition, MigrationsNotFound, migrator, migrationsAssembly));
		}
	}

	private static string MigrationsNotFound(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		MigrationAssemblyEventData migrationAssemblyEventData = (MigrationAssemblyEventData)payload;
		return obj.GenerateMessage(migrationAssemblyEventData.MigrationsAssembly.Assembly.GetName().Name);
	}

	public static void MigrationAttributeMissingWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Migrations> diagnostics, [JetBrains.Annotations.NotNull] TypeInfo migrationType)
	{
		EventDefinition<string> eventDefinition = RelationalResources.LogMigrationAttributeMissingWarning(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, migrationType.Name);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new MigrationTypeEventData(eventDefinition, MigrationAttributeMissingWarning, migrationType));
		}
	}

	private static string MigrationAttributeMissingWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string> obj = (EventDefinition<string>)definition;
		MigrationTypeEventData migrationTypeEventData = (MigrationTypeEventData)payload;
		return obj.GenerateMessage(migrationTypeEventData.MigrationType.Name);
	}

	public static void QueryPossibleUnintendedUseOfEqualsWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Query> diagnostics, [JetBrains.Annotations.NotNull] MethodCallExpression methodCallExpression)
	{
		EventDefinition<object> eventDefinition = RelationalResources.LogPossibleUnintendedUseOfEquals(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, methodCallExpression);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new ExpressionEventData(eventDefinition, QueryPossibleUnintendedUseOfEqualsWarning, methodCallExpression));
		}
	}

	private static string QueryPossibleUnintendedUseOfEqualsWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<object> obj = (EventDefinition<object>)definition;
		ExpressionEventData expressionEventData = (ExpressionEventData)payload;
		return obj.GenerateMessage(expressionEventData.Expression);
	}

	public static void QueryPossibleExceptionWithAggregateOperatorWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Query> diagnostics)
	{
		EventDefinition eventDefinition = RelationalResources.LogQueryPossibleExceptionWithAggregateOperatorWarning(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new EventData(eventDefinition, (EventDefinitionBase d, EventData p) => ((EventDefinition)d).GenerateMessage()));
		}
	}

	public static void ModelValidationKeyDefaultValueWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics, [JetBrains.Annotations.NotNull] IProperty property)
	{
		EventDefinition<string, string> eventDefinition = RelationalResources.LogKeyHasDefaultValue(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, property.Name, property.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new PropertyEventData(eventDefinition, ModelValidationKeyDefaultValueWarning, property));
		}
	}

	private static string ModelValidationKeyDefaultValueWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		PropertyEventData propertyEventData = (PropertyEventData)payload;
		return obj.GenerateMessage(propertyEventData.Property.Name, propertyEventData.Property.DeclaringEntityType.DisplayName());
	}

	public static void BoolWithDefaultWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics, [JetBrains.Annotations.NotNull] IProperty property)
	{
		EventDefinition<string, string> eventDefinition = RelationalResources.LogBoolWithDefaultWarning(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, property.Name, property.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new PropertyEventData(eventDefinition, BoolWithDefaultWarning, property));
		}
	}

	private static string BoolWithDefaultWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		PropertyEventData propertyEventData = (PropertyEventData)payload;
		return obj.GenerateMessage(propertyEventData.Property.Name, propertyEventData.Property.DeclaringEntityType.DisplayName());
	}

	public static void BatchReadyForExecution([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Update> diagnostics, [JetBrains.Annotations.NotNull] IEnumerable<IUpdateEntry> entries, int commandCount)
	{
		EventDefinition<int> eventDefinition = RelationalResources.LogBatchReadyForExecution(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, commandCount);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new BatchEventData(eventDefinition, BatchReadyForExecution, entries, commandCount));
		}
	}

	private static string BatchReadyForExecution(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<int> obj = (EventDefinition<int>)definition;
		BatchEventData batchEventData = (BatchEventData)payload;
		return obj.GenerateMessage(batchEventData.CommandCount);
	}

	public static void BatchSmallerThanMinBatchSize([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Update> diagnostics, [JetBrains.Annotations.NotNull] IEnumerable<IUpdateEntry> entries, int commandCount, int minBatchSize)
	{
		EventDefinition<int, int> eventDefinition = RelationalResources.LogBatchSmallerThanMinBatchSize(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, commandCount, minBatchSize);
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new MinBatchSizeEventData(eventDefinition, BatchSmallerThanMinBatchSize, entries, commandCount, minBatchSize));
		}
	}

	private static string BatchSmallerThanMinBatchSize(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<int, int> obj = (EventDefinition<int, int>)definition;
		MinBatchSizeEventData minBatchSizeEventData = (MinBatchSizeEventData)payload;
		return obj.GenerateMessage(minBatchSizeEventData.CommandCount, minBatchSizeEventData.MinBatchSize);
	}
}
