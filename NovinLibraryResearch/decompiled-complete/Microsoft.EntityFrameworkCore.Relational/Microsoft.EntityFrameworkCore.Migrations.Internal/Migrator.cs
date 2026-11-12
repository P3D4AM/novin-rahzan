using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal;

public class Migrator : IMigrator
{
	private readonly IMigrationsAssembly _migrationsAssembly;

	private readonly IHistoryRepository _historyRepository;

	private readonly IRelationalDatabaseCreator _databaseCreator;

	private readonly IMigrationsSqlGenerator _migrationsSqlGenerator;

	private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;

	private readonly IMigrationCommandExecutor _migrationCommandExecutor;

	private readonly IRelationalConnection _connection;

	private readonly ISqlGenerationHelper _sqlGenerationHelper;

	private readonly ICurrentDbContext _currentContext;

	private readonly IDiagnosticsLogger<DbLoggerCategory.Migrations> _logger;

	private readonly IDiagnosticsLogger<DbLoggerCategory.Database.Command> _commandLogger;

	private readonly string _activeProvider;

	public Migrator([JetBrains.Annotations.NotNull] IMigrationsAssembly migrationsAssembly, [JetBrains.Annotations.NotNull] IHistoryRepository historyRepository, [JetBrains.Annotations.NotNull] IDatabaseCreator databaseCreator, [JetBrains.Annotations.NotNull] IMigrationsSqlGenerator migrationsSqlGenerator, [JetBrains.Annotations.NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder, [JetBrains.Annotations.NotNull] IMigrationCommandExecutor migrationCommandExecutor, [JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper, [JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Migrations> logger, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger, [JetBrains.Annotations.NotNull] IDatabaseProvider databaseProvider)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationsAssembly, "migrationsAssembly");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(historyRepository, "historyRepository");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseCreator, "databaseCreator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationsSqlGenerator, "migrationsSqlGenerator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(rawSqlCommandBuilder, "rawSqlCommandBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationCommandExecutor, "migrationCommandExecutor");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlGenerationHelper, "sqlGenerationHelper");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logger, "logger");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandLogger, "commandLogger");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseProvider, "databaseProvider");
		_migrationsAssembly = migrationsAssembly;
		_historyRepository = historyRepository;
		_databaseCreator = (IRelationalDatabaseCreator)databaseCreator;
		_migrationsSqlGenerator = migrationsSqlGenerator;
		_rawSqlCommandBuilder = rawSqlCommandBuilder;
		_migrationCommandExecutor = migrationCommandExecutor;
		_connection = connection;
		_sqlGenerationHelper = sqlGenerationHelper;
		_currentContext = currentContext;
		_logger = logger;
		_commandLogger = commandLogger;
		_activeProvider = databaseProvider.Name;
	}

	public virtual void Migrate(string targetMigration = null)
	{
		_logger.MigrateUsingConnection(this, _connection);
		if (!_historyRepository.Exists())
		{
			if (!_databaseCreator.Exists())
			{
				_databaseCreator.Create();
			}
			_rawSqlCommandBuilder.Build(_historyRepository.GetCreateScript()).ExecuteNonQuery(new RelationalCommandParameterObject(_connection, null, null, _currentContext.Context, _commandLogger));
		}
		foreach (Func<IReadOnlyList<MigrationCommand>> migrationCommandList in GetMigrationCommandLists(_historyRepository.GetAppliedMigrations(), targetMigration))
		{
			_migrationCommandExecutor.ExecuteNonQuery(migrationCommandList(), _connection);
		}
	}

	public virtual async Task MigrateAsync(string targetMigration = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		_logger.MigrateUsingConnection(this, _connection);
		if (!(await _historyRepository.ExistsAsync(cancellationToken)))
		{
			if (!(await _databaseCreator.ExistsAsync(cancellationToken)))
			{
				await _databaseCreator.CreateAsync(cancellationToken);
			}
			await _rawSqlCommandBuilder.Build(_historyRepository.GetCreateScript()).ExecuteNonQueryAsync(new RelationalCommandParameterObject(_connection, null, null, _currentContext.Context, _commandLogger), cancellationToken);
		}
		IEnumerable<Func<IReadOnlyList<MigrationCommand>>> migrationCommandLists = GetMigrationCommandLists(await _historyRepository.GetAppliedMigrationsAsync(cancellationToken), targetMigration);
		foreach (Func<IReadOnlyList<MigrationCommand>> item in migrationCommandLists)
		{
			await _migrationCommandExecutor.ExecuteNonQueryAsync(item(), _connection, cancellationToken);
		}
	}

	private IEnumerable<Func<IReadOnlyList<MigrationCommand>>> GetMigrationCommandLists(IReadOnlyList<HistoryRow> appliedMigrationEntries, string targetMigration = null)
	{
		PopulateMigrations(appliedMigrationEntries.Select((HistoryRow t) => t.MigrationId), targetMigration, out var migrationsToApply, out var migrationsToRevert, out var actualTargetMigration);
		for (int i = 0; i < migrationsToRevert.Count; i++)
		{
			Migration migration2 = migrationsToRevert[i];
			int index = i;
			yield return delegate
			{
				_logger.MigrationReverting(this, migration2);
				return GenerateDownSql(migration2, (index != migrationsToRevert.Count - 1) ? migrationsToRevert[index + 1] : actualTargetMigration);
			};
		}
		foreach (Migration migration in migrationsToApply)
		{
			yield return delegate
			{
				_logger.MigrationApplying(this, migration);
				return GenerateUpSql(migration);
			};
		}
		if (migrationsToRevert.Count + migrationsToApply.Count == 0)
		{
			_logger.MigrationsNotApplied(this);
		}
	}

	protected virtual void PopulateMigrations(IEnumerable<string> appliedMigrationEntries, string targetMigration, out IReadOnlyList<Migration> migrationsToApply, out IReadOnlyList<Migration> migrationsToRevert, out Migration actualTargetMigration)
	{
		Dictionary<string, TypeInfo> dictionary = new Dictionary<string, TypeInfo>();
		Dictionary<string, TypeInfo> dictionary2 = new Dictionary<string, TypeInfo>();
		HashSet<string> hashSet = new HashSet<string>(appliedMigrationEntries, StringComparer.OrdinalIgnoreCase);
		if (_migrationsAssembly.Migrations.Count == 0)
		{
			_logger.MigrationsNotFound(this, _migrationsAssembly);
		}
		foreach (KeyValuePair<string, TypeInfo> migration in _migrationsAssembly.Migrations)
		{
			if (hashSet.Contains(migration.Key))
			{
				dictionary.Add(migration.Key, migration.Value);
			}
			else
			{
				dictionary2.Add(migration.Key, migration.Value);
			}
		}
		if (string.IsNullOrEmpty(targetMigration))
		{
			migrationsToApply = (from m in dictionary2
				orderby m.Key
				select m into p
				select _migrationsAssembly.CreateMigration(p.Value, _activeProvider)).ToList();
			migrationsToRevert = Array.Empty<Migration>();
			actualTargetMigration = null;
			return;
		}
		if (targetMigration == "0")
		{
			migrationsToApply = Array.Empty<Migration>();
			migrationsToRevert = (from m in dictionary
				orderby m.Key descending
				select m into p
				select _migrationsAssembly.CreateMigration(p.Value, _activeProvider)).ToList();
			actualTargetMigration = null;
			return;
		}
		targetMigration = _migrationsAssembly.GetMigrationId(targetMigration);
		migrationsToApply = (from m in dictionary2
			where string.Compare(m.Key, targetMigration, StringComparison.OrdinalIgnoreCase) <= 0
			orderby m.Key
			select m into p
			select _migrationsAssembly.CreateMigration(p.Value, _activeProvider)).ToList();
		migrationsToRevert = (from m in dictionary
			where string.Compare(m.Key, targetMigration, StringComparison.OrdinalIgnoreCase) > 0
			orderby m.Key descending
			select m into p
			select _migrationsAssembly.CreateMigration(p.Value, _activeProvider)).ToList();
		actualTargetMigration = (from m in dictionary
			where string.Compare(m.Key, targetMigration, StringComparison.OrdinalIgnoreCase) == 0
			select m into p
			select _migrationsAssembly.CreateMigration(p.Value, _activeProvider)).SingleOrDefault();
	}

	public virtual string GenerateScript(string fromMigration = null, string toMigration = null, bool idempotent = false)
	{
		IEnumerable<string> appliedMigrationEntries;
		if (string.IsNullOrEmpty(fromMigration) || fromMigration == "0")
		{
			appliedMigrationEntries = Enumerable.Empty<string>();
		}
		else
		{
			string fromMigrationId = _migrationsAssembly.GetMigrationId(fromMigration);
			appliedMigrationEntries = from t in _migrationsAssembly.Migrations
				where string.Compare(t.Key, fromMigrationId, StringComparison.OrdinalIgnoreCase) <= 0
				select t.Key;
		}
		PopulateMigrations(appliedMigrationEntries, toMigration, out var migrationsToApply, out var migrationsToRevert, out var actualTargetMigration);
		IndentedStringBuilder indentedStringBuilder = new IndentedStringBuilder();
		if (fromMigration == "0" || string.IsNullOrEmpty(fromMigration))
		{
			indentedStringBuilder.AppendLine(_historyRepository.GetCreateIfNotExistsScript());
			indentedStringBuilder.Append(_sqlGenerationHelper.BatchTerminator);
		}
		for (int i = 0; i < migrationsToRevert.Count; i++)
		{
			Migration migration = migrationsToRevert[i];
			Migration previousMigration = ((i != migrationsToRevert.Count - 1) ? migrationsToRevert[i + 1] : actualTargetMigration);
			_logger.MigrationGeneratingDownScript(this, migration, fromMigration, toMigration, idempotent);
			foreach (MigrationCommand item in GenerateDownSql(migration, previousMigration))
			{
				if (idempotent)
				{
					indentedStringBuilder.AppendLine(_historyRepository.GetBeginIfExistsScript(migration.GetId()));
					using (indentedStringBuilder.Indent())
					{
						indentedStringBuilder.AppendLines(item.CommandText);
					}
					indentedStringBuilder.AppendLine(_historyRepository.GetEndIfScript());
				}
				else
				{
					indentedStringBuilder.AppendLine(item.CommandText);
				}
				indentedStringBuilder.Append(_sqlGenerationHelper.BatchTerminator);
			}
		}
		foreach (Migration item2 in migrationsToApply)
		{
			_logger.MigrationGeneratingUpScript(this, item2, fromMigration, toMigration, idempotent);
			foreach (MigrationCommand item3 in GenerateUpSql(item2))
			{
				if (idempotent)
				{
					indentedStringBuilder.AppendLine(_historyRepository.GetBeginIfNotExistsScript(item2.GetId()));
					using (indentedStringBuilder.Indent())
					{
						indentedStringBuilder.AppendLines(item3.CommandText);
					}
					indentedStringBuilder.AppendLine(_historyRepository.GetEndIfScript());
				}
				else
				{
					indentedStringBuilder.AppendLine(item3.CommandText);
				}
				indentedStringBuilder.Append(_sqlGenerationHelper.BatchTerminator);
			}
		}
		return indentedStringBuilder.ToString();
	}

	protected virtual IReadOnlyList<MigrationCommand> GenerateUpSql([JetBrains.Annotations.NotNull] Migration migration)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migration, "migration");
		IRelationalCommand relationalCommand = _rawSqlCommandBuilder.Build(_historyRepository.GetInsertScript(new HistoryRow(migration.GetId(), ProductInfo.GetVersion())));
		return _migrationsSqlGenerator.Generate(migration.UpOperations, migration.TargetModel).Concat(new MigrationCommand[1]
		{
			new MigrationCommand(relationalCommand, _currentContext.Context, _commandLogger)
		}).ToList();
	}

	protected virtual IReadOnlyList<MigrationCommand> GenerateDownSql([JetBrains.Annotations.NotNull] Migration migration, [JetBrains.Annotations.CanBeNull] Migration previousMigration)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migration, "migration");
		IRelationalCommand relationalCommand = _rawSqlCommandBuilder.Build(_historyRepository.GetDeleteScript(migration.GetId()));
		return _migrationsSqlGenerator.Generate(migration.DownOperations, previousMigration?.TargetModel).Concat(new MigrationCommand[1]
		{
			new MigrationCommand(relationalCommand, _currentContext.Context, _commandLogger)
		}).ToList();
	}
}
