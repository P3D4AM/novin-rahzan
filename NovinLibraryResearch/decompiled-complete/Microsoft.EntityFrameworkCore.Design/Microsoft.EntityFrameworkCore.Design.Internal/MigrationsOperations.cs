using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class MigrationsOperations
{
	private readonly IOperationReporter _reporter;

	private readonly Assembly _assembly;

	private readonly string _projectDir;

	private readonly string _rootNamespace;

	private readonly string _language;

	private readonly DesignTimeServicesBuilder _servicesBuilder;

	private readonly DbContextOperations _contextOperations;

	public MigrationsOperations([JetBrains.Annotations.NotNull] IOperationReporter reporter, [JetBrains.Annotations.NotNull] Assembly assembly, [JetBrains.Annotations.NotNull] Assembly startupAssembly, [JetBrains.Annotations.NotNull] string projectDir, [JetBrains.Annotations.NotNull] string rootNamespace, [JetBrains.Annotations.CanBeNull] string language, [JetBrains.Annotations.NotNull] string[] args)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(reporter, "reporter");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(assembly, "assembly");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(startupAssembly, "startupAssembly");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(projectDir, "projectDir");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(rootNamespace, "rootNamespace");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
		_reporter = reporter;
		_assembly = assembly;
		_projectDir = projectDir;
		_rootNamespace = rootNamespace;
		_language = language;
		_contextOperations = new DbContextOperations(reporter, assembly, startupAssembly, args);
		_servicesBuilder = new DesignTimeServicesBuilder(assembly, startupAssembly, reporter, args);
	}

	public virtual MigrationFiles AddMigration([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string outputDir, [JetBrains.Annotations.CanBeNull] string contextType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		if (outputDir != null)
		{
			outputDir = Path.GetFullPath(Path.Combine(_projectDir, outputDir));
		}
		string subNamespace = SubnamespaceFromOutputPath(outputDir);
		using DbContext context = _contextOperations.CreateContext(contextType);
		IServiceProvider serviceProvider = _servicesBuilder.Build(context);
		EnsureServices(serviceProvider);
		EnsureMigrationsAssembly(serviceProvider);
		IMigrationsScaffolder requiredService = ServiceProviderServiceExtensions.GetRequiredService<IMigrationsScaffolder>(serviceProvider);
		return requiredService.Save(migration: requiredService.ScaffoldMigration(name, _rootNamespace, subNamespace, _language), projectDir: _projectDir, outputDir: outputDir);
	}

	private string SubnamespaceFromOutputPath(string outputDir)
	{
		if (outputDir == null || !outputDir.StartsWith(_projectDir, StringComparison.Ordinal))
		{
			return null;
		}
		string text = outputDir.Substring(_projectDir.Length);
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		return string.Join(".", text.Split(new char[2]
		{
			Path.DirectorySeparatorChar,
			Path.AltDirectorySeparatorChar
		}, StringSplitOptions.RemoveEmptyEntries));
	}

	public virtual IEnumerable<MigrationInfo> GetMigrations([JetBrains.Annotations.CanBeNull] string contextType)
	{
		using DbContext context = _contextOperations.CreateContext(contextType);
		IServiceProvider serviceProvider = _servicesBuilder.Build(context);
		EnsureServices(serviceProvider);
		IMigrationsAssembly requiredService = ServiceProviderServiceExtensions.GetRequiredService<IMigrationsAssembly>(serviceProvider);
		IMigrationsIdGenerator idGenerator = ServiceProviderServiceExtensions.GetRequiredService<IMigrationsIdGenerator>(serviceProvider);
		return requiredService.Migrations.Keys.Select((string id) => new MigrationInfo
		{
			Id = id,
			Name = idGenerator.GetName(id)
		});
	}

	public virtual string ScriptMigration([JetBrains.Annotations.CanBeNull] string fromMigration, [JetBrains.Annotations.CanBeNull] string toMigration, bool idempotent, [JetBrains.Annotations.CanBeNull] string contextType)
	{
		using DbContext context = _contextOperations.CreateContext(contextType);
		IServiceProvider serviceProvider = _servicesBuilder.Build(context);
		EnsureServices(serviceProvider);
		return ServiceProviderServiceExtensions.GetRequiredService<IMigrator>(serviceProvider).GenerateScript(fromMigration, toMigration, idempotent);
	}

	public virtual void UpdateDatabase([JetBrains.Annotations.CanBeNull] string targetMigration, [JetBrains.Annotations.CanBeNull] string contextType)
	{
		using (DbContext context = _contextOperations.CreateContext(contextType))
		{
			IServiceProvider serviceProvider = _servicesBuilder.Build(context);
			EnsureServices(serviceProvider);
			ServiceProviderServiceExtensions.GetRequiredService<IMigrator>(serviceProvider).Migrate(targetMigration);
		}
		_reporter.WriteInformation(DesignStrings.Done);
	}

	public virtual MigrationFiles RemoveMigration([JetBrains.Annotations.CanBeNull] string contextType, bool force)
	{
		using DbContext context = _contextOperations.CreateContext(contextType);
		IServiceProvider serviceProvider = _servicesBuilder.Build(context);
		EnsureServices(serviceProvider);
		EnsureMigrationsAssembly(serviceProvider);
		MigrationFiles result = ServiceProviderServiceExtensions.GetRequiredService<IMigrationsScaffolder>(serviceProvider).RemoveMigration(_projectDir, _rootNamespace, force, _language);
		_reporter.WriteInformation(DesignStrings.Done);
		return result;
	}

	private static void EnsureServices(IServiceProvider services)
	{
		if (ServiceProviderServiceExtensions.GetService<IMigrator>(services) == null)
		{
			throw new OperationException(DesignStrings.NonRelationalProvider(ServiceProviderServiceExtensions.GetService<IDatabaseProvider>(services)?.Name ?? "Unknown"));
		}
	}

	private void EnsureMigrationsAssembly(IServiceProvider services)
	{
		AssemblyName name = _assembly.GetName();
		IDbContextOptions requiredService = ServiceProviderServiceExtensions.GetRequiredService<IDbContextOptions>(services);
		Type type = ServiceProviderServiceExtensions.GetRequiredService<ICurrentDbContext>(services).Context.GetType();
		string text = RelationalOptionsExtension.Extract(requiredService).MigrationsAssembly ?? type.GetTypeInfo().Assembly.GetName().Name;
		if (name.Name != text && name.FullName != text)
		{
			throw new OperationException(DesignStrings.MigrationsAssemblyMismatch(name.Name, text));
		}
	}
}
