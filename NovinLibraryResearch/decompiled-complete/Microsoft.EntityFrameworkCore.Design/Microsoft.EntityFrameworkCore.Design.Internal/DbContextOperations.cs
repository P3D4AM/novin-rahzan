using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class DbContextOperations
{
	private readonly IOperationReporter _reporter;

	private readonly Assembly _assembly;

	private readonly Assembly _startupAssembly;

	private readonly string[] _args;

	private readonly AppServiceProviderFactory _appServicesFactory;

	public DbContextOperations([JetBrains.Annotations.NotNull] IOperationReporter reporter, [JetBrains.Annotations.NotNull] Assembly assembly, [JetBrains.Annotations.NotNull] Assembly startupAssembly, [JetBrains.Annotations.NotNull] string[] args)
		: this(reporter, assembly, startupAssembly, args, new AppServiceProviderFactory(startupAssembly, reporter))
	{
	}

	protected DbContextOperations([JetBrains.Annotations.NotNull] IOperationReporter reporter, [JetBrains.Annotations.NotNull] Assembly assembly, [JetBrains.Annotations.NotNull] Assembly startupAssembly, [JetBrains.Annotations.NotNull] string[] args, [JetBrains.Annotations.NotNull] AppServiceProviderFactory appServicesFactory)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(reporter, "reporter");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(assembly, "assembly");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(startupAssembly, "startupAssembly");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
		_reporter = reporter;
		_assembly = assembly;
		_startupAssembly = startupAssembly;
		_args = args;
		_appServicesFactory = appServicesFactory;
	}

	public virtual void DropDatabase([JetBrains.Annotations.CanBeNull] string contextType)
	{
		using DbContext dbContext = CreateContext(contextType);
		DbConnection dbConnection = dbContext.Database.GetDbConnection();
		_reporter.WriteInformation(DesignStrings.DroppingDatabase(dbConnection.Database));
		if (dbContext.Database.EnsureDeleted())
		{
			_reporter.WriteInformation(DesignStrings.DatabaseDropped(dbConnection.Database));
		}
		else
		{
			_reporter.WriteInformation(DesignStrings.NotExistDatabase(dbConnection.Database));
		}
	}

	public virtual string ScriptDbContext([JetBrains.Annotations.CanBeNull] string contextType)
	{
		using DbContext dbContext = CreateContext(contextType);
		return dbContext.Database.GenerateCreateScript();
	}

	public virtual DbContext CreateContext([JetBrains.Annotations.CanBeNull] string contextType)
	{
		return CreateContext(FindContextType(contextType).Value);
	}

	private DbContext CreateContext(Func<DbContext> factory)
	{
		DbContext dbContext = factory();
		_reporter.WriteVerbose(DesignStrings.UseContext(dbContext.GetType().ShortDisplayName()));
		dbContext.GetService<ILoggerFactory>().AddProvider(new OperationLoggerProvider(_reporter));
		return dbContext;
	}

	public virtual IEnumerable<Type> GetContextTypes()
	{
		return FindContextTypes().Keys;
	}

	public virtual Type GetContextType([JetBrains.Annotations.CanBeNull] string name)
	{
		return FindContextType(name).Key;
	}

	private IDictionary<Type, Func<DbContext>> FindContextTypes()
	{
		_reporter.WriteVerbose(DesignStrings.FindingContexts);
		Dictionary<Type, Func<DbContext>> contexts = new Dictionary<Type, Func<DbContext>>();
		_reporter.WriteVerbose(DesignStrings.FindingContextFactories);
		foreach (TypeInfo factory in from t in _startupAssembly.GetConstructibleTypes()
			where typeof(IDesignTimeDbContextFactory<DbContext>).GetTypeInfo().IsAssignableFrom(t)
			select t)
		{
			_reporter.WriteVerbose(DesignStrings.FoundContextFactory(factory.ShortDisplayName()));
			foreach (Type item in from i in factory.ImplementedInterfaces
				where i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IDesignTimeDbContextFactory<>)
				select i.GenericTypeArguments[0])
			{
				_reporter.WriteVerbose(DesignStrings.FoundDbContext(item.ShortDisplayName()));
				contexts.Add(item, () => CreateContextFromFactory(factory.AsType()));
			}
		}
		IServiceProvider appServices = _appServicesFactory.Create(_args);
		foreach (Type context2 in from o in ServiceProviderServiceExtensions.GetServices<DbContextOptions>(appServices)
			select o.ContextType into c
			where !contexts.ContainsKey(c)
			select c)
		{
			_reporter.WriteVerbose(DesignStrings.FoundDbContext(context2.ShortDisplayName()));
			contexts.Add(context2, FindContextFactory(context2) ?? ((Func<DbContext>)(() => (DbContext)ActivatorUtilities.GetServiceOrCreateInstance(appServices, context2))));
		}
		_reporter.WriteVerbose(DesignStrings.FindingReferencedContexts);
		List<TypeInfo> source = _startupAssembly.GetConstructibleTypes().Concat(_assembly.GetConstructibleTypes()).ToList();
		foreach (Type context in from c in (from t in source
				where typeof(DbContext).GetTypeInfo().IsAssignableFrom(t)
				select t.AsType()).Concat(from t in source
				where typeof(Migration).GetTypeInfo().IsAssignableFrom(t)
				select t.GetCustomAttribute<DbContextAttribute>()?.ContextType into t
				where t != null
				select t).Distinct()
			where !contexts.ContainsKey(c)
			select c)
		{
			_reporter.WriteVerbose(DesignStrings.FoundDbContext(context.ShortDisplayName()));
			contexts.Add(context, FindContextFactory(context) ?? ((Func<DbContext>)delegate
			{
				try
				{
					return (DbContext)Activator.CreateInstance(context);
				}
				catch (MissingMethodException innerException)
				{
					throw new OperationException(DesignStrings.NoParameterlessConstructor(context.Name), innerException);
				}
			}));
		}
		return contexts;
	}

	public virtual ContextInfo GetContextInfo([JetBrains.Annotations.CanBeNull] string contextType)
	{
		using DbContext dbContext = CreateContext(contextType);
		ContextInfo contextInfo = new ContextInfo();
		IDatabaseProvider service = dbContext.GetService<IDatabaseProvider>();
		contextInfo.ProviderName = service.Name;
		DbConnection dbConnection = dbContext.Database.GetDbConnection();
		contextInfo.DataSource = dbConnection.DataSource;
		contextInfo.DatabaseName = dbConnection.Database;
		IDbContextOptions service2 = dbContext.GetService<IDbContextOptions>();
		contextInfo.Options = service2.BuildOptionsFragment().Trim();
		return contextInfo;
	}

	private Func<DbContext> FindContextFactory(Type contextType)
	{
		TypeInfo factoryInterface = typeof(IDesignTimeDbContextFactory<>).MakeGenericType(contextType).GetTypeInfo();
		TypeInfo factory = contextType.GetTypeInfo().Assembly.GetConstructibleTypes().FirstOrDefault((TypeInfo t) => factoryInterface.IsAssignableFrom(t));
		if (!(factory == null))
		{
			return () => CreateContextFromFactory(factory.AsType());
		}
		return null;
	}

	private DbContext CreateContextFromFactory(Type factory)
	{
		_reporter.WriteVerbose(DesignStrings.UsingDbContextFactory(factory.ShortDisplayName()));
		return ((IDesignTimeDbContextFactory<DbContext>)Activator.CreateInstance(factory)).CreateDbContext(_args);
	}

	private KeyValuePair<Type, Func<DbContext>> FindContextType(string name)
	{
		IDictionary<Type, Func<DbContext>> dictionary = FindContextTypes();
		if (string.IsNullOrEmpty(name))
		{
			if (dictionary.Count == 0)
			{
				throw new OperationException(DesignStrings.NoContext(_assembly.GetName().Name));
			}
			if (dictionary.Count == 1)
			{
				return dictionary.First();
			}
			throw new OperationException(DesignStrings.MultipleContexts);
		}
		IDictionary<Type, Func<DbContext>> dictionary2 = FilterTypes(dictionary, name, ignoreCase: true);
		if (dictionary2.Count == 0)
		{
			throw new OperationException(DesignStrings.NoContextWithName(name));
		}
		if (dictionary2.Count == 1)
		{
			return dictionary2.First();
		}
		dictionary2 = FilterTypes(dictionary2, name);
		if (dictionary2.Count == 0)
		{
			throw new OperationException(DesignStrings.MultipleContextsWithName(name));
		}
		if (dictionary2.Count == 1)
		{
			return dictionary2.First();
		}
		dictionary2 = dictionary2.Where((KeyValuePair<Type, Func<DbContext>> t) => t.Key.Namespace == null).ToDictionary((KeyValuePair<Type, Func<DbContext>> t) => t.Key, (KeyValuePair<Type, Func<DbContext>> t) => t.Value);
		if (dictionary2.Count == 0)
		{
			throw new OperationException(DesignStrings.MultipleContextsWithQualifiedName(name));
		}
		return dictionary2.First();
	}

	private static IDictionary<Type, Func<DbContext>> FilterTypes(IDictionary<Type, Func<DbContext>> types, string name, bool ignoreCase = false)
	{
		StringComparison comparisonType = (ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
		return types.Where((KeyValuePair<Type, Func<DbContext>> t) => string.Equals(t.Key.Name, name, comparisonType) || string.Equals(t.Key.FullName, name, comparisonType) || string.Equals(t.Key.AssemblyQualifiedName, name, comparisonType)).ToDictionary((KeyValuePair<Type, Func<DbContext>> t) => t.Key, (KeyValuePair<Type, Func<DbContext>> t) => t.Value);
	}
}
