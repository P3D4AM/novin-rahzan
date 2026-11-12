using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.Extensions.DependencyInjection;

public static class SqlServerServiceCollectionExtensions
{
	public static IServiceCollection AddEntityFrameworkSqlServer([JetBrains.Annotations.NotNull] this IServiceCollection serviceCollection)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceCollection, "serviceCollection");
		new EntityFrameworkRelationalServicesBuilder(serviceCollection).TryAdd<LoggingDefinitions, SqlServerLoggingDefinitions>().TryAdd<IDatabaseProvider, DatabaseProvider<SqlServerOptionsExtension>>().TryAdd((Func<IServiceProvider, IValueGeneratorCache>)((IServiceProvider p) => ServiceProviderServiceExtensions.GetService<ISqlServerValueGeneratorCache>(p)))
			.TryAdd<IRelationalTypeMappingSource, SqlServerTypeMappingSource>()
			.TryAdd<ISqlGenerationHelper, SqlServerSqlGenerationHelper>()
			.TryAdd<IMigrationsAnnotationProvider, SqlServerMigrationsAnnotationProvider>()
			.TryAdd<IModelValidator, SqlServerModelValidator>()
			.TryAdd<IProviderConventionSetBuilder, SqlServerConventionSetBuilder>()
			.TryAdd((Func<IServiceProvider, IUpdateSqlGenerator>)((IServiceProvider p) => ServiceProviderServiceExtensions.GetService<ISqlServerUpdateSqlGenerator>(p)))
			.TryAdd<IModificationCommandBatchFactory, SqlServerModificationCommandBatchFactory>()
			.TryAdd<IValueGeneratorSelector, SqlServerValueGeneratorSelector>()
			.TryAdd((Func<IServiceProvider, IRelationalConnection>)((IServiceProvider p) => ServiceProviderServiceExtensions.GetService<ISqlServerConnection>(p)))
			.TryAdd<IMigrationsSqlGenerator, SqlServerMigrationsSqlGenerator>()
			.TryAdd<IRelationalDatabaseCreator, SqlServerDatabaseCreator>()
			.TryAdd<IHistoryRepository, SqlServerHistoryRepository>()
			.TryAdd<ICompiledQueryCacheKeyGenerator, SqlServerCompiledQueryCacheKeyGenerator>()
			.TryAdd<IExecutionStrategyFactory, SqlServerExecutionStrategyFactory>()
			.TryAdd<ISingletonOptions, ISqlServerOptions>((IServiceProvider p) => ServiceProviderServiceExtensions.GetService<ISqlServerOptions>(p))
			.TryAdd<IMethodCallTranslatorProvider, SqlServerMethodCallTranslatorProvider>()
			.TryAdd<IMemberTranslatorProvider, SqlServerMemberTranslatorProvider>()
			.TryAdd<IQuerySqlGeneratorFactory, SqlServerQuerySqlGeneratorFactory>()
			.TryAdd<IQueryTranslationPostprocessorFactory, SqlServerQueryTranslationPostprocessorFactory>()
			.TryAdd<IRelationalSqlTranslatingExpressionVisitorFactory, SqlServerSqlTranslatingExpressionVisitorFactory>()
			.TryAddProviderSpecificServices(delegate(ServiceCollectionMap b)
			{
				b.TryAddSingleton<ISqlServerValueGeneratorCache, SqlServerValueGeneratorCache>().TryAddSingleton<ISqlServerOptions, SqlServerOptions>().TryAddSingleton<ISqlServerUpdateSqlGenerator, SqlServerUpdateSqlGenerator>()
					.TryAddSingleton<ISqlServerSequenceValueGeneratorFactory, SqlServerSequenceValueGeneratorFactory>()
					.TryAddScoped<ISqlServerConnection, SqlServerConnection>();
			})
			.TryAddCoreServices();
		return serviceCollection;
	}
}
