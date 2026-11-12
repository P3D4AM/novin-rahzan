using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Design;

public class OperationExecutor : MarshalByRefObject
{
	public class AddMigration : OperationBase
	{
		public AddMigration([JetBrains.Annotations.NotNull] OperationExecutor executor, [JetBrains.Annotations.NotNull] IOperationResultHandler resultHandler, [JetBrains.Annotations.NotNull] IDictionary args)
			: base(resultHandler)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executor, "executor");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
			string name = (string)args["name"];
			string outputDir = (string)args["outputDir"];
			string contextType = (string)args["contextType"];
			Execute(() => executor.AddMigrationImpl(name, outputDir, contextType));
		}
	}

	public class GetContextInfo : OperationBase
	{
		public GetContextInfo([JetBrains.Annotations.NotNull] OperationExecutor executor, [JetBrains.Annotations.NotNull] IOperationResultHandler resultHandler, [JetBrains.Annotations.NotNull] IDictionary args)
			: base(resultHandler)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executor, "executor");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
			string contextType = (string)args["contextType"];
			Execute(() => executor.GetContextInfoImpl(contextType));
		}
	}

	public class UpdateDatabase : OperationBase
	{
		public UpdateDatabase([JetBrains.Annotations.NotNull] OperationExecutor executor, [JetBrains.Annotations.NotNull] IOperationResultHandler resultHandler, [JetBrains.Annotations.NotNull] IDictionary args)
			: base(resultHandler)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executor, "executor");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
			string targetMigration = (string)args["targetMigration"];
			string contextType = (string)args["contextType"];
			Execute(delegate
			{
				executor.UpdateDatabaseImpl(targetMigration, contextType);
			});
		}
	}

	public class ScriptMigration : OperationBase
	{
		public ScriptMigration([JetBrains.Annotations.NotNull] OperationExecutor executor, [JetBrains.Annotations.NotNull] IOperationResultHandler resultHandler, [JetBrains.Annotations.NotNull] IDictionary args)
			: base(resultHandler)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executor, "executor");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
			string fromMigration = (string)args["fromMigration"];
			string toMigration = (string)args["toMigration"];
			bool idempotent = (bool)args["idempotent"];
			string contextType = (string)args["contextType"];
			Execute(() => executor.ScriptMigrationImpl(fromMigration, toMigration, idempotent, contextType));
		}
	}

	public class RemoveMigration : OperationBase
	{
		public RemoveMigration([JetBrains.Annotations.NotNull] OperationExecutor executor, [JetBrains.Annotations.NotNull] IOperationResultHandler resultHandler, [JetBrains.Annotations.NotNull] IDictionary args)
			: base(resultHandler)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executor, "executor");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
			string contextType = (string)args["contextType"];
			bool force = (bool)args["force"];
			Execute(() => executor.RemoveMigrationImpl(contextType, force));
		}
	}

	public class GetContextTypes : OperationBase
	{
		public GetContextTypes([JetBrains.Annotations.NotNull] OperationExecutor executor, [JetBrains.Annotations.NotNull] IOperationResultHandler resultHandler, [JetBrains.Annotations.NotNull] IDictionary args)
			: base(resultHandler)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executor, "executor");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
			Execute((Func<IEnumerable<IDictionary>>)executor.GetContextTypesImpl);
		}
	}

	public class GetMigrations : OperationBase
	{
		public GetMigrations([JetBrains.Annotations.NotNull] OperationExecutor executor, [JetBrains.Annotations.NotNull] IOperationResultHandler resultHandler, [JetBrains.Annotations.NotNull] IDictionary args)
			: base(resultHandler)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executor, "executor");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
			string contextType = (string)args["contextType"];
			Execute(() => executor.GetMigrationsImpl(contextType));
		}
	}

	public class ScaffoldContext : OperationBase
	{
		public ScaffoldContext([JetBrains.Annotations.NotNull] OperationExecutor executor, [JetBrains.Annotations.NotNull] IOperationResultHandler resultHandler, [JetBrains.Annotations.NotNull] IDictionary args)
			: base(resultHandler)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executor, "executor");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
			string connectionString = (string)args["connectionString"];
			string provider = (string)args["provider"];
			string outputDir = (string)args["outputDir"];
			string outputDbContextDir = (string)args["outputDbContextDir"];
			string dbContextClassName = (string)args["dbContextClassName"];
			IEnumerable<string> schemaFilters = (IEnumerable<string>)args["schemaFilters"];
			IEnumerable<string> tableFilters = (IEnumerable<string>)args["tableFilters"];
			bool useDataAnnotations = (bool)args["useDataAnnotations"];
			bool overwriteFiles = (bool)args["overwriteFiles"];
			bool useDatabaseNames = (bool)args["useDatabaseNames"];
			Execute(() => executor.ScaffoldContextImpl(provider, connectionString, outputDir, outputDbContextDir, dbContextClassName, schemaFilters, tableFilters, useDataAnnotations, overwriteFiles, useDatabaseNames));
		}
	}

	public class DropDatabase : OperationBase
	{
		public DropDatabase([JetBrains.Annotations.NotNull] OperationExecutor executor, [JetBrains.Annotations.NotNull] IOperationResultHandler resultHandler, [JetBrains.Annotations.NotNull] IDictionary args)
			: base(resultHandler)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executor, "executor");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
			string contextType = (string)args["contextType"];
			Execute(delegate
			{
				executor.DropDatabaseImpl(contextType);
			});
		}
	}

	public class ScriptDbContext : OperationBase
	{
		public ScriptDbContext([JetBrains.Annotations.NotNull] OperationExecutor executor, [JetBrains.Annotations.NotNull] IOperationResultHandler resultHandler, [JetBrains.Annotations.NotNull] IDictionary args)
			: base(resultHandler)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executor, "executor");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
			string contextType = (string)args["contextType"];
			Execute(() => executor.ScriptDbContextImpl(contextType));
		}
	}

	public abstract class OperationBase : MarshalByRefObject
	{
		private readonly IOperationResultHandler _resultHandler;

		protected OperationBase([JetBrains.Annotations.NotNull] IOperationResultHandler resultHandler)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(resultHandler, "resultHandler");
			_resultHandler = resultHandler;
		}

		protected virtual void Execute([JetBrains.Annotations.NotNull] Action action)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(action, "action");
			try
			{
				action();
			}
			catch (Exception ex)
			{
				_resultHandler.OnError(ex.GetType().FullName, ex.Message, ex.ToString());
			}
		}

		protected virtual void Execute<T>([JetBrains.Annotations.NotNull] Func<T> action)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(action, "action");
			Execute(delegate
			{
				_resultHandler.OnResult(action());
			});
		}

		protected virtual void Execute<T>([JetBrains.Annotations.NotNull] Func<IEnumerable<T>> action)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(action, "action");
			Execute(delegate
			{
				_resultHandler.OnResult(action().ToArray());
			});
		}
	}

	private readonly string _projectDir;

	private readonly string _targetName;

	private readonly string _startupTargetName;

	private readonly string _rootNamespace;

	private readonly string _language;

	private readonly string[] _designArgs;

	private readonly OperationReporter _reporter;

	private DbContextOperations _contextOperations;

	private DatabaseOperations _databaseOperations;

	private MigrationsOperations _migrationsOperations;

	private Assembly _assembly;

	private Assembly _startupAssembly;

	private Assembly Assembly
	{
		get
		{
			return _assembly ?? (_assembly = Create());
			Assembly Create()
			{
				try
				{
					return Assembly.Load(new AssemblyName(_targetName));
				}
				catch (Exception innerException)
				{
					throw new OperationException(DesignStrings.UnreferencedAssembly(_targetName, _startupTargetName), innerException);
				}
			}
		}
	}

	private Assembly StartupAssembly => _startupAssembly ?? (_startupAssembly = Assembly.Load(new AssemblyName(_startupTargetName)));

	private MigrationsOperations MigrationsOperations => _migrationsOperations ?? (_migrationsOperations = new MigrationsOperations(_reporter, Assembly, StartupAssembly, _projectDir, _rootNamespace, _language, _designArgs));

	private DbContextOperations ContextOperations => _contextOperations ?? (_contextOperations = new DbContextOperations(_reporter, Assembly, StartupAssembly, _designArgs));

	private DatabaseOperations DatabaseOperations => _databaseOperations ?? (_databaseOperations = new DatabaseOperations(_reporter, Assembly, StartupAssembly, _projectDir, _rootNamespace, _language, _designArgs));

	public OperationExecutor([JetBrains.Annotations.NotNull] IOperationReportHandler reportHandler, [JetBrains.Annotations.NotNull] IDictionary args)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(reportHandler, "reportHandler");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
		_reporter = new OperationReporter(reportHandler);
		_targetName = (string)args["targetName"];
		_startupTargetName = (string)args["startupTargetName"];
		_projectDir = (string)args["projectDir"];
		_rootNamespace = (string)args["rootNamespace"];
		_language = (string)args["language"];
		_designArgs = Array.Empty<string>();
		string text = (string)args["toolsVersion"];
		string version = ProductInfo.GetVersion();
		if (text != null && new SemanticVersionComparer().Compare(text, version) < 0)
		{
			_reporter.WriteWarning(DesignStrings.VersionMismatch(text, version));
		}
	}

	private IDictionary AddMigrationImpl([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string outputDir, [JetBrains.Annotations.CanBeNull] string contextType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		MigrationFiles migrationFiles = MigrationsOperations.AddMigration(name, outputDir, contextType);
		return new Hashtable
		{
			["MigrationFile"] = migrationFiles.MigrationFile,
			["MetadataFile"] = migrationFiles.MetadataFile,
			["SnapshotFile"] = migrationFiles.SnapshotFile
		};
	}

	private IDictionary GetContextInfoImpl([JetBrains.Annotations.CanBeNull] string contextType)
	{
		ContextInfo contextInfo = ContextOperations.GetContextInfo(contextType);
		return new Hashtable
		{
			["ProviderName"] = contextInfo.ProviderName,
			["DatabaseName"] = contextInfo.DatabaseName,
			["DataSource"] = contextInfo.DataSource,
			["Options"] = contextInfo.Options
		};
	}

	private void UpdateDatabaseImpl([JetBrains.Annotations.CanBeNull] string targetMigration, [JetBrains.Annotations.CanBeNull] string contextType)
	{
		MigrationsOperations.UpdateDatabase(targetMigration, contextType);
	}

	private string ScriptMigrationImpl([JetBrains.Annotations.CanBeNull] string fromMigration, [JetBrains.Annotations.CanBeNull] string toMigration, bool idempotent, [JetBrains.Annotations.CanBeNull] string contextType)
	{
		return MigrationsOperations.ScriptMigration(fromMigration, toMigration, idempotent, contextType);
	}

	private IDictionary RemoveMigrationImpl([JetBrains.Annotations.CanBeNull] string contextType, bool force)
	{
		MigrationFiles migrationFiles = MigrationsOperations.RemoveMigration(contextType, force);
		return new Hashtable
		{
			["MigrationFile"] = migrationFiles.MigrationFile,
			["MetadataFile"] = migrationFiles.MetadataFile,
			["SnapshotFile"] = migrationFiles.SnapshotFile
		};
	}

	private IEnumerable<IDictionary> GetContextTypesImpl()
	{
		List<Type> source = ContextOperations.GetContextTypes().ToList();
		List<IGrouping<string, Type>> nameGroups = (from t in source
			group t by t.Name).ToList();
		List<IGrouping<string, Type>> fullNameGroups = (from t in source
			group t by t.FullName).ToList();
		return source.Select((Type t) => new Hashtable
		{
			["AssemblyQualifiedName"] = t.AssemblyQualifiedName,
			["FullName"] = t.FullName,
			["Name"] = t.Name,
			["SafeName"] = ((nameGroups.Count((IGrouping<string, Type> g) => g.Key == t.Name) == 1) ? t.Name : ((fullNameGroups.Count((IGrouping<string, Type> g) => g.Key == t.FullName) == 1) ? t.FullName : t.AssemblyQualifiedName))
		});
	}

	private IEnumerable<IDictionary> GetMigrationsImpl([JetBrains.Annotations.CanBeNull] string contextType)
	{
		List<MigrationInfo> source = MigrationsOperations.GetMigrations(contextType).ToList();
		List<IGrouping<string, MigrationInfo>> nameGroups = (from m in source
			group m by m.Name).ToList();
		return source.Select((MigrationInfo m) => new Hashtable
		{
			["Id"] = m.Id,
			["Name"] = m.Name,
			["SafeName"] = ((nameGroups.Count((IGrouping<string, MigrationInfo> g) => g.Key == m.Name) == 1) ? m.Name : m.Id)
		});
	}

	private IDictionary ScaffoldContextImpl([JetBrains.Annotations.NotNull] string provider, [JetBrains.Annotations.NotNull] string connectionString, [JetBrains.Annotations.CanBeNull] string outputDir, [JetBrains.Annotations.CanBeNull] string outputDbContextDir, [JetBrains.Annotations.CanBeNull] string dbContextClassName, [JetBrains.Annotations.NotNull] IEnumerable<string> schemaFilters, [JetBrains.Annotations.NotNull] IEnumerable<string> tableFilters, bool useDataAnnotations, bool overwriteFiles, bool useDatabaseNames)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(provider, "provider");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connectionString, "connectionString");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(schemaFilters, "schemaFilters");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(tableFilters, "tableFilters");
		SavedModelFiles savedModelFiles = DatabaseOperations.ScaffoldContext(provider, connectionString, outputDir, outputDbContextDir, dbContextClassName, schemaFilters, tableFilters, useDataAnnotations, overwriteFiles, useDatabaseNames);
		return new Hashtable
		{
			["ContextFile"] = savedModelFiles.ContextFile,
			["EntityTypeFiles"] = savedModelFiles.AdditionalFiles.ToArray()
		};
	}

	private void DropDatabaseImpl(string contextType)
	{
		ContextOperations.DropDatabase(contextType);
	}

	private string ScriptDbContextImpl(string contextType)
	{
		return ContextOperations.ScriptDbContext(contextType);
	}
}
