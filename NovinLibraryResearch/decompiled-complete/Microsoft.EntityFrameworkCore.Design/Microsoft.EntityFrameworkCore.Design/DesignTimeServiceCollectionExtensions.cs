using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Design;

public static class DesignTimeServiceCollectionExtensions
{
	public static IServiceCollection AddEntityFrameworkDesignTimeServices([JetBrains.Annotations.NotNull] this IServiceCollection services, [JetBrains.Annotations.CanBeNull] IOperationReporter reporter = null, [JetBrains.Annotations.CanBeNull] Func<IServiceProvider> applicationServiceProviderAccessor = null)
	{
		if (reporter == null)
		{
			reporter = new OperationReporter(null);
		}
		return services.AddSingleton<AnnotationCodeGeneratorDependencies>().AddSingleton<TypeMappingSourceDependencies>().AddSingleton<CSharpMigrationOperationGeneratorDependencies>()
			.AddSingleton<CSharpMigrationsGeneratorDependencies>()
			.AddSingleton<CSharpSnapshotGeneratorDependencies>()
			.AddSingleton<MigrationsCodeGeneratorDependencies>()
			.AddSingleton<ModelCodeGeneratorDependencies>()
			.AddSingleton<ProviderCodeGeneratorDependencies>()
			.AddSingleton<TypeMappingSourceDependencies>()
			.AddSingleton<RelationalTypeMappingSourceDependencies>()
			.AddSingleton<ValueConverterSelectorDependencies>()
			.AddSingleton<ICandidateNamingService, CandidateNamingService>()
			.AddSingleton<ICSharpDbContextGenerator, CSharpDbContextGenerator>()
			.AddSingleton<ICSharpEntityTypeGenerator, CSharpEntityTypeGenerator>()
			.AddSingleton<ICSharpHelper, CSharpHelper>()
			.AddSingleton<ICSharpMigrationOperationGenerator, CSharpMigrationOperationGenerator>()
			.AddSingleton<ICSharpSnapshotGenerator, CSharpSnapshotGenerator>()
			.AddSingleton<ICSharpUtilities, CSharpUtilities>()
			.AddSingleton(typeof(IDiagnosticsLogger<>), typeof(DiagnosticsLogger<>))
			.AddSingleton<IInterceptors, Interceptors>()
			.AddSingleton((DiagnosticSource)new DiagnosticListener("Microsoft.EntityFrameworkCore"))
			.AddSingleton<ILoggingOptions, LoggingOptions>()
			.AddSingleton<IMigrationsCodeGenerator, CSharpMigrationsGenerator>()
			.AddSingleton<IMigrationsCodeGeneratorSelector, MigrationsCodeGeneratorSelector>()
			.AddSingleton<IModelCodeGenerator, CSharpModelGenerator>()
			.AddSingleton<IModelCodeGeneratorSelector, ModelCodeGeneratorSelector>()
			.AddSingleton((INamedConnectionStringResolver)new DesignTimeConnectionStringResolver(applicationServiceProviderAccessor))
			.AddSingleton(reporter)
			.AddSingleton<IPluralizer, NullPluralizer>()
			.AddSingleton<IReverseEngineerScaffolder, ReverseEngineerScaffolder>()
			.AddSingleton<IScaffoldingModelFactory, RelationalScaffoldingModelFactory>()
			.AddSingleton<IScaffoldingTypeMapper, ScaffoldingTypeMapper>()
			.AddSingleton<IValueConverterSelector, ValueConverterSelector>()
			.AddTransient<MigrationsScaffolderDependencies>()
			.AddTransient<IMigrationsScaffolder, MigrationsScaffolder>()
			.AddTransient<ISnapshotModelProcessor, SnapshotModelProcessor>()
			.AddLogging(delegate(ILoggingBuilder b)
			{
				b.SetMinimumLevel(LogLevel.Debug).AddProvider(new OperationLoggerProvider(reporter));
			});
	}

	public static IServiceCollection AddDbContextDesignTimeServices([JetBrains.Annotations.NotNull] this IServiceCollection services, [JetBrains.Annotations.NotNull] DbContext context)
	{
		return services.AddTransient((IServiceProvider _) => context.GetService<ICurrentDbContext>()).AddTransient((IServiceProvider _) => context.GetService<IDatabaseProvider>()).AddTransient((IServiceProvider _) => context.GetService<IDbContextOptions>())
			.AddTransient((IServiceProvider _) => context.GetService<IHistoryRepository>())
			.AddTransient((IServiceProvider _) => context.GetService<IMigrationsAssembly>())
			.AddTransient((IServiceProvider _) => context.GetService<IMigrationsIdGenerator>())
			.AddTransient((IServiceProvider _) => context.GetService<IMigrationsModelDiffer>())
			.AddTransient((IServiceProvider _) => context.GetService<IMigrator>())
			.AddTransient((IServiceProvider _) => context.GetService<IRelationalTypeMappingSource>())
			.AddTransient((IServiceProvider _) => context.GetService<IModel>());
	}
}
