using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class EntityFrameworkRelationalServicesBuilder : EntityFrameworkServicesBuilder
{
	[EntityFrameworkInternal]
	public static readonly IDictionary<Type, ServiceCharacteristics> RelationalServices = new Dictionary<Type, ServiceCharacteristics>
	{
		{
			typeof(IKeyValueIndexFactorySource),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IParameterNameGeneratorFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IComparer<ModificationCommand>),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IMigrationsIdGenerator),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(ISqlGenerationHelper),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IMigrationsAnnotationProvider),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IMigrationCommandExecutor),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IRelationalTypeMappingSource),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IRelationalValueBufferFactoryFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IUpdateSqlGenerator),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IRelationalTransactionFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IRelationalCommandBuilderFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IRawSqlCommandBuilder),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IQuerySqlGeneratorFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IRelationalSqlTranslatingExpressionVisitorFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IMethodCallTranslatorProvider),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IMemberTranslatorProvider),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(ISqlExpressionFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(ICommandBatchPreparer),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IModificationCommandBatchFactory),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IMigrationsModelDiffer),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IMigrationsSqlGenerator),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IMigrator),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IMigrationsAssembly),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IBatchExecutor),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IRelationalConnection),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IRelationalDatabaseFacadeDependencies),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IRelationalDatabaseCreator),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IHistoryRepository),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(INamedConnectionStringResolver),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IInterceptorAggregator),
			new ServiceCharacteristics(ServiceLifetime.Scoped, multipleRegistrations: true)
		},
		{
			typeof(IInterceptor),
			new ServiceCharacteristics(ServiceLifetime.Scoped, multipleRegistrations: true)
		},
		{
			typeof(IRelationalTypeMappingSourcePlugin),
			new ServiceCharacteristics(ServiceLifetime.Singleton, multipleRegistrations: true)
		},
		{
			typeof(IMethodCallTranslatorPlugin),
			new ServiceCharacteristics(ServiceLifetime.Singleton, multipleRegistrations: true)
		},
		{
			typeof(IMemberTranslatorPlugin),
			new ServiceCharacteristics(ServiceLifetime.Singleton, multipleRegistrations: true)
		}
	};

	public EntityFrameworkRelationalServicesBuilder([JetBrains.Annotations.NotNull] IServiceCollection serviceCollection)
		: base(serviceCollection)
	{
	}

	[EntityFrameworkInternal]
	protected override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
	{
		if (!RelationalServices.TryGetValue(serviceType, out var value))
		{
			return base.GetServiceCharacteristics(serviceType);
		}
		return value;
	}

	public override EntityFrameworkServicesBuilder TryAddCoreServices()
	{
		TryAdd<IParameterNameGeneratorFactory, ParameterNameGeneratorFactory>();
		TryAdd<IComparer<ModificationCommand>, ModificationCommandComparer>();
		TryAdd<IMigrationsIdGenerator, MigrationsIdGenerator>();
		TryAdd<IKeyValueIndexFactorySource, KeyValueIndexFactorySource>();
		TryAdd<IModelCustomizer, RelationalModelCustomizer>();
		TryAdd<IMigrationsAnnotationProvider, MigrationsAnnotationProvider>();
		TryAdd<IModelValidator, RelationalModelValidator>();
		TryAdd<IMigrator, Migrator>();
		TryAdd<IMigrationCommandExecutor, MigrationCommandExecutor>();
		TryAdd<IMigrationsAssembly, MigrationsAssembly>();
		TryAdd<IDatabase, RelationalDatabase>();
		TryAdd<IBatchExecutor, BatchExecutor>();
		TryAdd<IValueGeneratorSelector, RelationalValueGeneratorSelector>();
		TryAdd<IRelationalCommandBuilderFactory, RelationalCommandBuilderFactory>();
		TryAdd<IRawSqlCommandBuilder, RawSqlCommandBuilder>();
		TryAdd<ICommandBatchPreparer, CommandBatchPreparer>();
		TryAdd<IMigrationsModelDiffer, MigrationsModelDiffer>();
		TryAdd<IMigrationsSqlGenerator, MigrationsSqlGenerator>();
		TryAdd<IExecutionStrategyFactory, RelationalExecutionStrategyFactory>();
		TryAdd((Func<IServiceProvider, ITypeMappingSource>)((IServiceProvider p) => p.GetService<IRelationalTypeMappingSource>()));
		TryAdd<IRelationalValueBufferFactoryFactory, TypedRelationalValueBufferFactoryFactory>();
		TryAdd((Func<IServiceProvider, IDatabaseCreator>)((IServiceProvider p) => p.GetService<IRelationalDatabaseCreator>()));
		TryAdd((Func<IServiceProvider, IDbContextTransactionManager>)((IServiceProvider p) => p.GetService<IRelationalConnection>()));
		TryAdd<IQueryContextFactory, RelationalQueryContextFactory>();
		TryAdd<ICompiledQueryCacheKeyGenerator, RelationalCompiledQueryCacheKeyGenerator>();
		TryAdd<INamedConnectionStringResolver, NamedConnectionStringResolver>();
		TryAdd<IEvaluatableExpressionFilter, RelationalEvaluatableExpressionFilter>();
		TryAdd<IRelationalTransactionFactory, RelationalTransactionFactory>();
		TryAdd((Func<IServiceProvider, IDatabaseFacadeDependencies>)((IServiceProvider p) => p.GetService<IRelationalDatabaseFacadeDependencies>()));
		TryAdd<IRelationalDatabaseFacadeDependencies, RelationalDatabaseFacadeDependencies>();
		TryAdd<IInterceptorAggregator, DbConnectionInterceptorAggregator>();
		TryAdd<IInterceptorAggregator, DbTransactionInterceptorAggregator>();
		TryAdd<IInterceptorAggregator, DbCommandInterceptorAggregator>();
		TryAdd<IQuerySqlGeneratorFactory, QuerySqlGeneratorFactory>();
		TryAdd<IShapedQueryCompilingExpressionVisitorFactory, RelationalShapedQueryCompilingExpressionVisitorFactory>();
		TryAdd<IQueryableMethodTranslatingExpressionVisitorFactory, RelationalQueryableMethodTranslatingExpressionVisitorFactory>();
		TryAdd<IMethodCallTranslatorProvider, RelationalMethodCallTranslatorProvider>();
		TryAdd<IMemberTranslatorProvider, RelationalMemberTranslatorProvider>();
		TryAdd<IQueryTranslationPostprocessorFactory, RelationalQueryTranslationPostprocessorFactory>();
		TryAdd<IRelationalSqlTranslatingExpressionVisitorFactory, RelationalSqlTranslatingExpressionVisitorFactory>();
		TryAdd<ISqlExpressionFactory, SqlExpressionFactory>();
		TryAdd<IQueryTranslationPreprocessorFactory, RelationalQueryTranslationPreprocessorFactory>();
		ServiceCollectionMap.GetInfrastructure().AddDependencySingleton<RelationalSqlGenerationHelperDependencies>().AddDependencySingleton<RelationalTypeMappingSourceDependencies>()
			.AddDependencySingleton<RelationalModelValidatorDependencies>()
			.AddDependencySingleton<UpdateSqlGeneratorDependencies>()
			.AddDependencySingleton<MigrationsAnnotationProviderDependencies>()
			.AddDependencySingleton<ParameterNameGeneratorDependencies>()
			.AddDependencySingleton<RelationalValueBufferFactoryDependencies>()
			.AddDependencySingleton<RelationalTransactionFactoryDependencies>()
			.AddDependencySingleton<RelationalCommandBuilderDependencies>()
			.AddDependencySingleton<SqlExpressionFactoryDependencies>()
			.AddDependencySingleton<RelationalMethodCallTranslatorProviderDependencies>()
			.AddDependencySingleton<RelationalMemberTranslatorProviderDependencies>()
			.AddDependencySingleton<RelationalSqlTranslatingExpressionVisitorDependencies>()
			.AddDependencySingleton<RelationalQueryableMethodTranslatingExpressionVisitorDependencies>()
			.AddDependencySingleton<QuerySqlGeneratorDependencies>()
			.AddDependencySingleton<RelationalShapedQueryCompilingExpressionVisitorDependencies>()
			.AddDependencySingleton<RelationalQueryTranslationPostprocessorDependencies>()
			.AddDependencySingleton<RelationalEvaluatableExpressionFilterDependencies>()
			.AddDependencySingleton<RelationalQueryTranslationPreprocessorDependencies>()
			.AddDependencyScoped<MigrationsSqlGeneratorDependencies>()
			.AddDependencyScoped<RelationalConventionSetBuilderDependencies>()
			.AddDependencyScoped<ModificationCommandBatchFactoryDependencies>()
			.AddDependencyScoped<CommandBatchPreparerDependencies>()
			.AddDependencyScoped<RelationalDatabaseCreatorDependencies>()
			.AddDependencyScoped<HistoryRepositoryDependencies>()
			.AddDependencyScoped<RelationalCompiledQueryCacheKeyGeneratorDependencies>()
			.AddDependencyScoped<RelationalConnectionDependencies>()
			.AddDependencyScoped<RelationalDatabaseDependencies>()
			.AddDependencyScoped<RelationalQueryContextDependencies>();
		return base.TryAddCoreServices();
	}
}
