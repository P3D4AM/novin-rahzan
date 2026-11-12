using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalDatabaseFacadeExtensions
{
	public static void Migrate([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade").GetRelationalService<IMigrator>().Migrate();
	}

	public static IEnumerable<string> GetMigrations([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade").GetRelationalService<IMigrationsAssembly>().Migrations.Keys;
	}

	public static IEnumerable<string> GetAppliedMigrations([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade)
	{
		return from hr in Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade").GetRelationalService<IHistoryRepository>().GetAppliedMigrations()
			select hr.MigrationId;
	}

	public static async Task<IEnumerable<string>> GetAppliedMigrationsAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, CancellationToken cancellationToken = default(CancellationToken))
	{
		return (await Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade").GetRelationalService<IHistoryRepository>().GetAppliedMigrationsAsync(cancellationToken)).Select((HistoryRow hr) => hr.MigrationId);
	}

	public static IEnumerable<string> GetPendingMigrations([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade)
	{
		return databaseFacade.GetMigrations().Except(databaseFacade.GetAppliedMigrations());
	}

	public static async Task<IEnumerable<string>> GetPendingMigrationsAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, CancellationToken cancellationToken = default(CancellationToken))
	{
		IEnumerable<string> migrations = databaseFacade.GetMigrations();
		return migrations.Except(await databaseFacade.GetAppliedMigrationsAsync(cancellationToken));
	}

	public static Task MigrateAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade").GetRelationalService<IMigrator>().MigrateAsync(null, cancellationToken);
	}

	[Obsolete("For the execution of SQL queries using plain strings, use ExecuteSqlRaw instead. For the execution of SQL queries using interpolated string syntax to create parameters, use ExecuteSqlInterpolated instead.")]
	public static int ExecuteSqlCommand([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, RawSqlString sql, [JetBrains.Annotations.NotNull] params object[] parameters)
	{
		return databaseFacade.ExecuteSqlCommand(sql, (IEnumerable<object>)parameters);
	}

	[Obsolete("For the execution of SQL queries using plain strings, use ExecuteSqlRaw instead. For the execution of SQL queries using interpolated string syntax to create parameters, use ExecuteSqlInterpolated instead.")]
	public static int ExecuteSqlCommand([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, [JetBrains.Annotations.NotNull] FormattableString sql)
	{
		return databaseFacade.ExecuteSqlCommand(sql.Format, sql.GetArguments());
	}

	[Obsolete("For the execution of SQL queries using plain strings, use ExecuteSqlRaw instead. For the execution of SQL queries using interpolated string syntax to create parameters, use ExecuteSqlInterpolated instead.")]
	public static int ExecuteSqlCommand([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, RawSqlString sql, [JetBrains.Annotations.NotNull] IEnumerable<object> parameters)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sql, "sql");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameters, "parameters");
		IConcurrencyDetector concurrencyDetector = GetFacadeDependencies(databaseFacade).ConcurrencyDetector;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger = GetFacadeDependencies(databaseFacade).CommandLogger;
		using (concurrencyDetector.EnterCriticalSection())
		{
			RawSqlCommand rawSqlCommand = GetFacadeDependencies(databaseFacade).RawSqlCommandBuilder.Build(sql.Format, parameters);
			return rawSqlCommand.RelationalCommand.ExecuteNonQuery(new RelationalCommandParameterObject(GetFacadeDependencies(databaseFacade).RelationalConnection, rawSqlCommand.ParameterValues, null, ((IDatabaseFacadeDependenciesAccessor)databaseFacade).Context, commandLogger));
		}
	}

	[Obsolete("For the async execution of SQL queries using plain strings, use ExecuteSqlRawAsync instead. For the async execution of SQL queries using interpolated string syntax to create parameters, use ExecuteSqlInterpolatedAsync instead.")]
	public static Task<int> ExecuteSqlCommandAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, [JetBrains.Annotations.NotNull] FormattableString sql, CancellationToken cancellationToken = default(CancellationToken))
	{
		return databaseFacade.ExecuteSqlCommandAsync(sql.Format, sql.GetArguments(), cancellationToken);
	}

	[Obsolete("For the async execution of SQL queries using plain strings, use ExecuteSqlRawAsync instead. For the async execution of SQL queries using interpolated string syntax to create parameters, use ExecuteSqlInterpolatedAsync instead.")]
	public static Task<int> ExecuteSqlCommandAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, RawSqlString sql, CancellationToken cancellationToken = default(CancellationToken))
	{
		return databaseFacade.ExecuteSqlCommandAsync(sql, Enumerable.Empty<object>(), cancellationToken);
	}

	[Obsolete("For the async execution of SQL queries using plain strings, use ExecuteSqlRawAsync instead. For the async execution of SQL queries using interpolated string syntax to create parameters, use ExecuteSqlInterpolatedAsync instead.")]
	public static Task<int> ExecuteSqlCommandAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, RawSqlString sql, [JetBrains.Annotations.NotNull] params object[] parameters)
	{
		return databaseFacade.ExecuteSqlCommandAsync(sql, parameters, default(CancellationToken));
	}

	[Obsolete("For the async execution of SQL queries using plain strings, use ExecuteSqlRawAsync instead. For the async execution of SQL queries using interpolated string syntax to create parameters, use ExecuteSqlInterpolatedAsync instead.")]
	public static async Task<int> ExecuteSqlCommandAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, RawSqlString sql, [JetBrains.Annotations.NotNull] IEnumerable<object> parameters, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sql, "sql");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameters, "parameters");
		IRelationalDatabaseFacadeDependencies facadeDependencies = GetFacadeDependencies(databaseFacade);
		IConcurrencyDetector concurrencyDetector = facadeDependencies.ConcurrencyDetector;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger = facadeDependencies.CommandLogger;
		using (concurrencyDetector.EnterCriticalSection())
		{
			RawSqlCommand rawSqlCommand = GetFacadeDependencies(databaseFacade).RawSqlCommandBuilder.Build(sql.Format, parameters);
			return await rawSqlCommand.RelationalCommand.ExecuteNonQueryAsync(new RelationalCommandParameterObject(facadeDependencies.RelationalConnection, rawSqlCommand.ParameterValues, null, ((IDatabaseFacadeDependenciesAccessor)databaseFacade).Context, commandLogger), cancellationToken);
		}
	}

	public static int ExecuteSqlRaw([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, [JetBrains.Annotations.NotNull] string sql, [JetBrains.Annotations.NotNull] params object[] parameters)
	{
		return databaseFacade.ExecuteSqlRaw(sql, (IEnumerable<object>)parameters);
	}

	public static int ExecuteSqlInterpolated([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, [JetBrains.Annotations.NotNull] FormattableString sql)
	{
		return databaseFacade.ExecuteSqlRaw(sql.Format, sql.GetArguments());
	}

	public static int ExecuteSqlRaw([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, [JetBrains.Annotations.NotNull] string sql, [JetBrains.Annotations.NotNull] IEnumerable<object> parameters)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sql, "sql");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameters, "parameters");
		IRelationalDatabaseFacadeDependencies facadeDependencies = GetFacadeDependencies(databaseFacade);
		IConcurrencyDetector concurrencyDetector = facadeDependencies.ConcurrencyDetector;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger = facadeDependencies.CommandLogger;
		using (concurrencyDetector.EnterCriticalSection())
		{
			RawSqlCommand rawSqlCommand = GetFacadeDependencies(databaseFacade).RawSqlCommandBuilder.Build(sql, parameters);
			return rawSqlCommand.RelationalCommand.ExecuteNonQuery(new RelationalCommandParameterObject(facadeDependencies.RelationalConnection, rawSqlCommand.ParameterValues, null, ((IDatabaseFacadeDependenciesAccessor)databaseFacade).Context, commandLogger));
		}
	}

	public static Task<int> ExecuteSqlInterpolatedAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, [JetBrains.Annotations.NotNull] FormattableString sql, CancellationToken cancellationToken = default(CancellationToken))
	{
		return databaseFacade.ExecuteSqlRawAsync(sql.Format, sql.GetArguments(), cancellationToken);
	}

	public static Task<int> ExecuteSqlRawAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, [JetBrains.Annotations.NotNull] string sql, CancellationToken cancellationToken = default(CancellationToken))
	{
		return databaseFacade.ExecuteSqlRawAsync(sql, Enumerable.Empty<object>(), cancellationToken);
	}

	public static Task<int> ExecuteSqlRawAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, [JetBrains.Annotations.NotNull] string sql, [JetBrains.Annotations.NotNull] params object[] parameters)
	{
		return databaseFacade.ExecuteSqlRawAsync(sql, parameters, default(CancellationToken));
	}

	public static async Task<int> ExecuteSqlRawAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, [JetBrains.Annotations.NotNull] string sql, [JetBrains.Annotations.NotNull] IEnumerable<object> parameters, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sql, "sql");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameters, "parameters");
		IRelationalDatabaseFacadeDependencies facadeDependencies = GetFacadeDependencies(databaseFacade);
		IConcurrencyDetector concurrencyDetector = facadeDependencies.ConcurrencyDetector;
		IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger = facadeDependencies.CommandLogger;
		using (concurrencyDetector.EnterCriticalSection())
		{
			RawSqlCommand rawSqlCommand = GetFacadeDependencies(databaseFacade).RawSqlCommandBuilder.Build(sql, parameters);
			return await rawSqlCommand.RelationalCommand.ExecuteNonQueryAsync(new RelationalCommandParameterObject(facadeDependencies.RelationalConnection, rawSqlCommand.ParameterValues, null, ((IDatabaseFacadeDependenciesAccessor)databaseFacade).Context, commandLogger), cancellationToken);
		}
	}

	public static DbConnection GetDbConnection([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade)
	{
		return GetFacadeDependencies(databaseFacade).RelationalConnection.DbConnection;
	}

	public static void OpenConnection([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade)
	{
		databaseFacade.CreateExecutionStrategy().Execute(databaseFacade, (DatabaseFacade database) => GetFacadeDependencies(database).RelationalConnection.Open());
	}

	public static Task OpenConnectionAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, CancellationToken cancellationToken = default(CancellationToken))
	{
		return databaseFacade.CreateExecutionStrategy().ExecuteAsync(databaseFacade, (DatabaseFacade database, CancellationToken ct) => GetFacadeDependencies(database).RelationalConnection.OpenAsync(cancellationToken), cancellationToken);
	}

	public static void CloseConnection([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade)
	{
		GetFacadeDependencies(databaseFacade).RelationalConnection.Close();
	}

	public static Task CloseConnectionAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade)
	{
		return GetFacadeDependencies(databaseFacade).RelationalConnection.CloseAsync();
	}

	public static IDbContextTransaction BeginTransaction([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, IsolationLevel isolationLevel)
	{
		return databaseFacade.CreateExecutionStrategy().Execute(databaseFacade, delegate(DatabaseFacade database)
		{
			IDbContextTransactionManager transactionManager = database.GetTransactionManager();
			return (!(transactionManager is IRelationalTransactionManager relationalTransactionManager)) ? transactionManager.BeginTransaction() : relationalTransactionManager.BeginTransaction(isolationLevel);
		});
	}

	public static Task<IDbContextTransaction> BeginTransactionAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, IsolationLevel isolationLevel, CancellationToken cancellationToken = default(CancellationToken))
	{
		return databaseFacade.CreateExecutionStrategy().ExecuteAsync(databaseFacade, delegate(DatabaseFacade database, CancellationToken ct)
		{
			IDbContextTransactionManager transactionManager = database.GetTransactionManager();
			return (!(transactionManager is IRelationalTransactionManager relationalTransactionManager)) ? transactionManager.BeginTransactionAsync(ct) : relationalTransactionManager.BeginTransactionAsync(isolationLevel, ct);
		}, cancellationToken);
	}

	public static IDbContextTransaction UseTransaction([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, [JetBrains.Annotations.CanBeNull] DbTransaction transaction)
	{
		return ((databaseFacade.GetTransactionManager() as IRelationalTransactionManager) ?? throw new InvalidOperationException(RelationalStrings.RelationalNotInUse)).UseTransaction(transaction);
	}

	public static Task<IDbContextTransaction> UseTransactionAsync([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, [JetBrains.Annotations.CanBeNull] DbTransaction transaction, CancellationToken cancellationToken = default(CancellationToken))
	{
		return ((databaseFacade.GetTransactionManager() as IRelationalTransactionManager) ?? throw new InvalidOperationException(RelationalStrings.RelationalNotInUse)).UseTransactionAsync(transaction, cancellationToken);
	}

	public static void SetCommandTimeout([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, int? timeout)
	{
		GetFacadeDependencies(databaseFacade).RelationalConnection.CommandTimeout = timeout;
	}

	public static void SetCommandTimeout([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, TimeSpan timeout)
	{
		if (timeout < TimeSpan.Zero)
		{
			throw new ArgumentException(RelationalStrings.TimeoutTooSmall(timeout.TotalSeconds));
		}
		if (timeout.TotalSeconds > 2147483647.0)
		{
			throw new ArgumentException(RelationalStrings.TimeoutTooBig(timeout.TotalSeconds));
		}
		databaseFacade.SetCommandTimeout(Convert.ToInt32(timeout.TotalSeconds));
	}

	public static int? GetCommandTimeout([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade)
	{
		return GetFacadeDependencies(databaseFacade).RelationalConnection.CommandTimeout;
	}

	public static string GenerateCreateScript([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade)
	{
		return databaseFacade.GetRelationalService<IRelationalDatabaseCreator>().GenerateCreateScript();
	}

	private static IRelationalDatabaseFacadeDependencies GetFacadeDependencies(DatabaseFacade databaseFacade)
	{
		if (((IDatabaseFacadeDependenciesAccessor)databaseFacade).Dependencies is IRelationalDatabaseFacadeDependencies result)
		{
			return result;
		}
		throw new InvalidOperationException(RelationalStrings.RelationalNotInUse);
	}

	private static TService GetRelationalService<TService>(this IInfrastructure<IServiceProvider> databaseFacade)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade");
		return databaseFacade.Instance.GetService<TService>() ?? throw new InvalidOperationException(RelationalStrings.RelationalNotInUse);
	}

	private static IDbContextTransactionManager GetTransactionManager([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade)
	{
		return ((IDatabaseFacadeDependenciesAccessor)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade")).Dependencies.TransactionManager;
	}
}
