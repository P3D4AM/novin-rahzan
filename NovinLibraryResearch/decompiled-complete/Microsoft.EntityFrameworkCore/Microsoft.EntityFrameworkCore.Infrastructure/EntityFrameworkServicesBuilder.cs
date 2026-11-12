using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class EntityFrameworkServicesBuilder
{
	[EntityFrameworkInternal]
	public readonly struct ServiceCharacteristics
	{
		[EntityFrameworkInternal]
		public ServiceLifetime Lifetime { get; }

		[EntityFrameworkInternal]
		public bool MultipleRegistrations { get; }

		[EntityFrameworkInternal]
		public ServiceCharacteristics(ServiceLifetime lifetime, bool multipleRegistrations = false)
		{
			Lifetime = lifetime;
			MultipleRegistrations = multipleRegistrations;
		}
	}

	[EntityFrameworkInternal]
	public static readonly IDictionary<Type, ServiceCharacteristics> CoreServices = new Dictionary<Type, ServiceCharacteristics>
	{
		{
			typeof(LoggingDefinitions),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IDatabaseProvider),
			new ServiceCharacteristics(ServiceLifetime.Singleton, multipleRegistrations: true)
		},
		{
			typeof(IDbSetFinder),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IDbSetInitializer),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IDbSetSource),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IEntityFinderSource),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IEntityMaterializerSource),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(ITypeMappingSource),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IModelCustomizer),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IModelCacheKeyFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IModelSource),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IInternalEntityEntryFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IInternalEntityEntrySubscriber),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IEntityEntryGraphIterator),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IValueGeneratorCache),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(ISingletonOptionsInitializer),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(ILoggingOptions),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(ICoreSingletonOptions),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IModelValidator),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(ICompiledQueryCache),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IValueConverterSelector),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IConstructorBindingFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IRegisteredServices),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IPropertyParameterBindingFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IParameterBindingFactories),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IMemberClassifier),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IMemoryCache),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IEvaluatableExpressionFilter),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IQueryTranslationPreprocessorFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IQueryableMethodTranslatingExpressionVisitorFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IQueryTranslationPostprocessorFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IShapedQueryCompilingExpressionVisitorFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton)
		},
		{
			typeof(IProviderConventionSetBuilder),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IConventionSetBuilder),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IDiagnosticsLogger<>),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IInterceptors),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(ILoggerFactory),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IEntityGraphAttacher),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IKeyPropagator),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(INavigationFixer),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(ILocalViewListener),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IStateManager),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IConcurrencyDetector),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IInternalEntityEntryNotifier),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IValueGenerationManager),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IChangeTrackerFactory),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IChangeDetector),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IDbContextServices),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IValueGeneratorSelector),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IExecutionStrategyFactory),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IAsyncQueryProvider),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IQueryCompiler),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(ICompiledQueryCacheKeyGenerator),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IModel),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IUpdateAdapterFactory),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(ICurrentDbContext),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IDbContextDependencies),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IDatabaseFacadeDependencies),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IDbContextOptions),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IDatabase),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IDatabaseCreator),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IDbContextTransactionManager),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IQueryContextFactory),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(IQueryCompilationContextFactory),
			new ServiceCharacteristics(ServiceLifetime.Scoped)
		},
		{
			typeof(ILazyLoader),
			new ServiceCharacteristics(ServiceLifetime.Transient)
		},
		{
			typeof(IParameterBindingFactory),
			new ServiceCharacteristics(ServiceLifetime.Singleton, multipleRegistrations: true)
		},
		{
			typeof(ITypeMappingSourcePlugin),
			new ServiceCharacteristics(ServiceLifetime.Singleton, multipleRegistrations: true)
		},
		{
			typeof(ISingletonOptions),
			new ServiceCharacteristics(ServiceLifetime.Singleton, multipleRegistrations: true)
		},
		{
			typeof(IConventionSetPlugin),
			new ServiceCharacteristics(ServiceLifetime.Scoped, multipleRegistrations: true)
		},
		{
			typeof(IResettableService),
			new ServiceCharacteristics(ServiceLifetime.Scoped, multipleRegistrations: true)
		}
	};

	protected virtual ServiceCollectionMap ServiceCollectionMap { get; }

	public EntityFrameworkServicesBuilder([JetBrains.Annotations.NotNull] IServiceCollection serviceCollection)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceCollection, "serviceCollection");
		ServiceCollectionMap = new ServiceCollectionMap(serviceCollection);
	}

	[EntityFrameworkInternal]
	protected virtual ServiceCharacteristics GetServiceCharacteristics([JetBrains.Annotations.NotNull] Type serviceType)
	{
		if (!CoreServices.TryGetValue(serviceType, out var value))
		{
			throw new InvalidOperationException(CoreStrings.NotAnEFService(serviceType.Name));
		}
		return value;
	}

	public virtual EntityFrameworkServicesBuilder TryAddProviderSpecificServices([JetBrains.Annotations.NotNull] Action<ServiceCollectionMap> serviceMap)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceMap, "serviceMap");
		serviceMap(ServiceCollectionMap);
		return this;
	}

	public virtual EntityFrameworkServicesBuilder TryAddCoreServices()
	{
		TryAdd<IDbSetFinder, DbSetFinder>();
		TryAdd<IDbSetInitializer, DbSetInitializer>();
		TryAdd<IDbSetSource, DbSetSource>();
		TryAdd<IEntityFinderSource, EntityFinderSource>();
		TryAdd<IEntityMaterializerSource, EntityMaterializerSource>();
		TryAdd<IProviderConventionSetBuilder, ProviderConventionSetBuilder>();
		TryAdd<IConventionSetBuilder, RuntimeConventionSetBuilder>();
		TryAdd<IModelCustomizer, ModelCustomizer>();
		TryAdd<IModelCacheKeyFactory, ModelCacheKeyFactory>();
		TryAdd((Func<IServiceProvider, ILoggerFactory>)((IServiceProvider p) => ScopedLoggerFactory.Create(p, null)));
		TryAdd<IModelSource, ModelSource>();
		TryAdd<IInternalEntityEntryFactory, InternalEntityEntryFactory>();
		TryAdd<IInternalEntityEntrySubscriber, InternalEntityEntrySubscriber>();
		TryAdd<IEntityEntryGraphIterator, EntityEntryGraphIterator>();
		TryAdd<IEntityGraphAttacher, EntityGraphAttacher>();
		TryAdd<IValueGeneratorCache, ValueGeneratorCache>();
		TryAdd<IKeyPropagator, KeyPropagator>();
		TryAdd<INavigationFixer, NavigationFixer>();
		TryAdd<ILocalViewListener, LocalViewListener>();
		TryAdd<IStateManager, StateManager>();
		TryAdd<IConcurrencyDetector, ConcurrencyDetector>();
		TryAdd<IInternalEntityEntryNotifier, InternalEntityEntryNotifier>();
		TryAdd<IValueGenerationManager, ValueGenerationManager>();
		TryAdd<IChangeTrackerFactory, ChangeTrackerFactory>();
		TryAdd<IChangeDetector, ChangeDetector>();
		TryAdd<IDbContextServices, DbContextServices>();
		TryAdd<IDbContextDependencies, DbContextDependencies>();
		TryAdd<IDatabaseFacadeDependencies, DatabaseFacadeDependencies>();
		TryAdd<IValueGeneratorSelector, ValueGeneratorSelector>();
		TryAdd<IModelValidator, ModelValidator>();
		TryAdd<IExecutionStrategyFactory, ExecutionStrategyFactory>();
		TryAdd<ICompiledQueryCache, CompiledQueryCache>();
		TryAdd<IAsyncQueryProvider, EntityQueryProvider>();
		TryAdd<IQueryCompiler, QueryCompiler>();
		TryAdd<ICompiledQueryCacheKeyGenerator, CompiledQueryCacheKeyGenerator>();
		TryAdd<ISingletonOptionsInitializer, SingletonOptionsInitializer>();
		TryAdd(typeof(IDiagnosticsLogger<>), typeof(DiagnosticsLogger<>));
		TryAdd<IInterceptors, Interceptors>();
		TryAdd<ILoggingOptions, LoggingOptions>();
		TryAdd<ICoreSingletonOptions, CoreSingletonOptions>();
		TryAdd<ISingletonOptions, ILoggingOptions>((IServiceProvider p) => ServiceProviderServiceExtensions.GetService<ILoggingOptions>(p));
		TryAdd<ISingletonOptions, ICoreSingletonOptions>((IServiceProvider p) => ServiceProviderServiceExtensions.GetService<ICoreSingletonOptions>(p));
		TryAdd((IServiceProvider p) => GetContextServices(p).Model);
		TryAdd((IServiceProvider p) => GetContextServices(p).CurrentContext);
		TryAdd((IServiceProvider p) => GetContextServices(p).ContextOptions);
		TryAdd<IResettableService, IStateManager>((IServiceProvider p) => ServiceProviderServiceExtensions.GetService<IStateManager>(p));
		TryAdd<IResettableService, IDbContextTransactionManager>((IServiceProvider p) => ServiceProviderServiceExtensions.GetService<IDbContextTransactionManager>(p));
		TryAdd<IEvaluatableExpressionFilter, EvaluatableExpressionFilter>();
		TryAdd<IValueConverterSelector, ValueConverterSelector>();
		TryAdd<IConstructorBindingFactory, ConstructorBindingFactory>();
		TryAdd<ILazyLoader, LazyLoader>();
		TryAdd<IParameterBindingFactories, ParameterBindingFactories>();
		TryAdd<IMemberClassifier, MemberClassifier>();
		TryAdd<IPropertyParameterBindingFactory, PropertyParameterBindingFactory>();
		TryAdd<IParameterBindingFactory, LazyLoaderParameterBindingFactory>();
		TryAdd<IParameterBindingFactory, ContextParameterBindingFactory>();
		TryAdd<IParameterBindingFactory, EntityTypeParameterBindingFactory>();
		TryAdd((Func<IServiceProvider, IMemoryCache>)((IServiceProvider p) => new MemoryCache(new MemoryCacheOptions
		{
			SizeLimit = 10240L
		})));
		TryAdd<IUpdateAdapterFactory, UpdateAdapterFactory>();
		TryAdd<IQueryCompilationContextFactory, QueryCompilationContextFactory>();
		TryAdd<IQueryTranslationPreprocessorFactory, QueryTranslationPreprocessorFactory>();
		TryAdd<IQueryTranslationPostprocessorFactory, QueryTranslationPostprocessorFactory>();
		ServiceCollectionMap.TryAddSingleton((Func<IServiceProvider, DiagnosticSource>)((IServiceProvider p) => new DiagnosticListener("Microsoft.EntityFrameworkCore")));
		ServiceCollectionMap.GetInfrastructure().AddDependencySingleton<LazyLoaderParameterBindingFactoryDependencies>().AddDependencySingleton<DatabaseProviderDependencies>()
			.AddDependencySingleton<ModelSourceDependencies>()
			.AddDependencySingleton<ValueGeneratorCacheDependencies>()
			.AddDependencySingleton<ModelValidatorDependencies>()
			.AddDependencySingleton<TypeMappingSourceDependencies>()
			.AddDependencySingleton<ModelCustomizerDependencies>()
			.AddDependencySingleton<ModelCacheKeyFactoryDependencies>()
			.AddDependencySingleton<ValueConverterSelectorDependencies>()
			.AddDependencySingleton<EntityMaterializerSourceDependencies>()
			.AddDependencySingleton<ShapedQueryCompilingExpressionVisitorDependencies>()
			.AddDependencySingleton<QueryableMethodTranslatingExpressionVisitorDependencies>()
			.AddDependencySingleton<QueryTranslationPreprocessorDependencies>()
			.AddDependencySingleton<QueryTranslationPostprocessorDependencies>()
			.AddDependencySingleton<EvaluatableExpressionFilterDependencies>()
			.AddDependencyScoped<ProviderConventionSetBuilderDependencies>()
			.AddDependencyScoped<QueryCompilationContextDependencies>()
			.AddDependencyScoped<StateManagerDependencies>()
			.AddDependencyScoped<ExecutionStrategyDependencies>()
			.AddDependencyScoped<CompiledQueryCacheKeyGeneratorDependencies>()
			.AddDependencyScoped<QueryContextDependencies>()
			.AddDependencyScoped<ValueGeneratorSelectorDependencies>()
			.AddDependencyScoped<DatabaseDependencies>();
		ServiceCollectionMap.TryAddSingleton((IRegisteredServices)new RegisteredServices(ServiceCollectionMap.ServiceCollection.Select((ServiceDescriptor s) => s.ServiceType)));
		return this;
	}

	private static IDbContextServices GetContextServices(IServiceProvider serviceProvider)
	{
		return ServiceProviderServiceExtensions.GetRequiredService<IDbContextServices>(serviceProvider);
	}

	public virtual EntityFrameworkServicesBuilder TryAdd<TService, TImplementation>() where TService : class where TImplementation : class, TService
	{
		return TryAdd(typeof(TService), typeof(TImplementation));
	}

	public virtual EntityFrameworkServicesBuilder TryAdd([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Type implementationType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceType, "serviceType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(implementationType, "implementationType");
		ServiceCharacteristics serviceCharacteristics = GetServiceCharacteristics(serviceType);
		if (serviceCharacteristics.MultipleRegistrations)
		{
			ServiceCollectionMap.TryAddEnumerable(serviceType, implementationType, serviceCharacteristics.Lifetime);
		}
		else
		{
			ServiceCollectionMap.TryAdd(serviceType, implementationType, serviceCharacteristics.Lifetime);
		}
		return this;
	}

	public virtual EntityFrameworkServicesBuilder TryAdd<TService>([JetBrains.Annotations.NotNull] Func<IServiceProvider, TService> factory) where TService : class
	{
		return TryAdd(typeof(TService), typeof(TService), factory);
	}

	public virtual EntityFrameworkServicesBuilder TryAdd<TService, TImplementation>([JetBrains.Annotations.NotNull] Func<IServiceProvider, TImplementation> factory) where TService : class where TImplementation : class, TService
	{
		return TryAdd(typeof(TService), typeof(TImplementation), factory);
	}

	public virtual EntityFrameworkServicesBuilder TryAdd([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Type implementationType, [JetBrains.Annotations.NotNull] Func<IServiceProvider, object> factory)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceType, "serviceType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(implementationType, "implementationType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(factory, "factory");
		ServiceCharacteristics serviceCharacteristics = GetServiceCharacteristics(serviceType);
		if (serviceCharacteristics.MultipleRegistrations)
		{
			if (implementationType == serviceType || implementationType == typeof(object))
			{
				throw new InvalidOperationException(CoreStrings.ImplementationTypeRequired(serviceType.Name));
			}
			ServiceCollectionMap.TryAddEnumerable(serviceType, implementationType, factory, serviceCharacteristics.Lifetime);
		}
		else
		{
			ServiceCollectionMap.TryAdd(serviceType, factory, serviceCharacteristics.Lifetime);
		}
		return this;
	}

	public virtual EntityFrameworkServicesBuilder TryAdd<TService>([JetBrains.Annotations.CanBeNull] TService implementation) where TService : class
	{
		return TryAdd(typeof(TService), implementation);
	}

	public virtual EntityFrameworkServicesBuilder TryAdd([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.CanBeNull] object implementation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceType, "serviceType");
		ServiceCharacteristics serviceCharacteristics = GetServiceCharacteristics(serviceType);
		if (serviceCharacteristics.Lifetime != 0)
		{
			throw new InvalidOperationException(CoreStrings.SingletonRequired(serviceCharacteristics.Lifetime, serviceType.Name));
		}
		if (serviceCharacteristics.MultipleRegistrations)
		{
			if (implementation == null)
			{
				throw new InvalidOperationException(CoreStrings.ImplementationTypeRequired(serviceType.Name));
			}
			ServiceCollectionMap.TryAddSingletonEnumerable(serviceType, implementation);
		}
		else
		{
			ServiceCollectionMap.TryAddSingleton(serviceType, implementation);
		}
		return this;
	}
}
