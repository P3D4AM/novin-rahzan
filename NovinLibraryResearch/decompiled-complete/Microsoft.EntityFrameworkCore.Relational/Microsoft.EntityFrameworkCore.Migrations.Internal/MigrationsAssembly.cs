using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal;

public class MigrationsAssembly : IMigrationsAssembly
{
	private readonly IMigrationsIdGenerator _idGenerator;

	private readonly IDiagnosticsLogger<DbLoggerCategory.Migrations> _logger;

	private IReadOnlyDictionary<string, TypeInfo> _migrations;

	private ModelSnapshot _modelSnapshot;

	private readonly Type _contextType;

	public virtual IReadOnlyDictionary<string, TypeInfo> Migrations
	{
		get
		{
			return _migrations ?? (_migrations = Create());
			IReadOnlyDictionary<string, TypeInfo> Create()
			{
				SortedList<string, TypeInfo> sortedList = new SortedList<string, TypeInfo>();
				foreach (var (text, typeInfo) in from t in Assembly.GetConstructibleTypes()
					where t.IsSubclassOf(typeof(Migration)) && t.GetCustomAttribute<DbContextAttribute>()?.ContextType == _contextType
					let id = t.GetCustomAttribute<MigrationAttribute>()?.Id
					orderby id
					select (id: id, t: t))
				{
					if (text == null)
					{
						_logger.MigrationAttributeMissingWarning(typeInfo);
					}
					else
					{
						sortedList.Add(text, typeInfo);
					}
				}
				return sortedList;
			}
		}
	}

	public virtual ModelSnapshot ModelSnapshot => _modelSnapshot ?? (_modelSnapshot = (from t in Assembly.GetConstructibleTypes()
		where t.IsSubclassOf(typeof(ModelSnapshot)) && t.GetCustomAttribute<DbContextAttribute>()?.ContextType == _contextType
		select (ModelSnapshot)Activator.CreateInstance(t.AsType())).FirstOrDefault());

	public virtual Assembly Assembly { get; }

	public MigrationsAssembly([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IDbContextOptions options, [JetBrains.Annotations.NotNull] IMigrationsIdGenerator idGenerator, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Migrations> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(options, "options");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(idGenerator, "idGenerator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logger, "logger");
		_contextType = currentContext.Context.GetType();
		string text = RelationalOptionsExtension.Extract(options)?.MigrationsAssembly;
		Assembly = ((text == null) ? _contextType.GetTypeInfo().Assembly : Assembly.Load(new AssemblyName(text)));
		_idGenerator = idGenerator;
		_logger = logger;
	}

	public virtual string FindMigrationId(string nameOrId)
	{
		return Migrations.Keys.Where(_idGenerator.IsValidId(nameOrId) ? ((Func<string, bool>)((string id) => string.Equals(id, nameOrId, StringComparison.OrdinalIgnoreCase))) : ((Func<string, bool>)((string id) => string.Equals(_idGenerator.GetName(id), nameOrId, StringComparison.OrdinalIgnoreCase)))).FirstOrDefault();
	}

	public virtual Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(activeProvider, "activeProvider");
		Migration obj = (Migration)Activator.CreateInstance(migrationClass.AsType());
		obj.ActiveProvider = activeProvider;
		return obj;
	}
}
