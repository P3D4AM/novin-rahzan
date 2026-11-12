using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class EntityFrameworkServiceCollectionExtensions
{
	public static IServiceCollection AddDbContext<TContext>([JetBrains.Annotations.NotNull] this IServiceCollection serviceCollection, [JetBrains.Annotations.CanBeNull] Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : DbContext
	{
		return serviceCollection.AddDbContext<TContext, TContext>(optionsAction, contextLifetime, optionsLifetime);
	}

	public static IServiceCollection AddDbContext<TContextService, TContextImplementation>([JetBrains.Annotations.NotNull] this IServiceCollection serviceCollection, [JetBrains.Annotations.CanBeNull] Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContextImplementation : DbContext, TContextService
	{
		return serviceCollection.AddDbContext<TContextService, TContextImplementation>((optionsAction == null) ? null : ((Action<IServiceProvider, DbContextOptionsBuilder>)delegate(IServiceProvider p, DbContextOptionsBuilder b)
		{
			optionsAction(b);
		}), contextLifetime, optionsLifetime);
	}

	public static IServiceCollection AddDbContextPool<TContext>([JetBrains.Annotations.NotNull] this IServiceCollection serviceCollection, [JetBrains.Annotations.NotNull] Action<DbContextOptionsBuilder> optionsAction, int poolSize = 128) where TContext : DbContext
	{
		return serviceCollection.AddDbContextPool<TContext, TContext>(optionsAction, poolSize);
	}

	public static IServiceCollection AddDbContextPool<TContextService, TContextImplementation>([JetBrains.Annotations.NotNull] this IServiceCollection serviceCollection, [JetBrains.Annotations.NotNull] Action<DbContextOptionsBuilder> optionsAction, int poolSize = 128) where TContextService : class where TContextImplementation : DbContext, TContextService
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(optionsAction, "optionsAction");
		return serviceCollection.AddDbContextPool<TContextService, TContextImplementation>(delegate(IServiceProvider _, DbContextOptionsBuilder ob)
		{
			optionsAction(ob);
		}, poolSize);
	}

	public static IServiceCollection AddDbContextPool<TContext>([JetBrains.Annotations.NotNull] this IServiceCollection serviceCollection, [JetBrains.Annotations.NotNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, int poolSize = 128) where TContext : DbContext
	{
		return serviceCollection.AddDbContextPool<TContext, TContext>(optionsAction, poolSize);
	}

	public static IServiceCollection AddDbContextPool<TContextService, TContextImplementation>([JetBrains.Annotations.NotNull] this IServiceCollection serviceCollection, [JetBrains.Annotations.NotNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, int poolSize = 128) where TContextService : class where TContextImplementation : DbContext, TContextService
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceCollection, "serviceCollection");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(optionsAction, "optionsAction");
		if (poolSize <= 0)
		{
			throw new ArgumentOutOfRangeException("poolSize", CoreStrings.InvalidPoolSize);
		}
		CheckContextConstructors<TContextImplementation>();
		AddCoreServices<TContextImplementation>(serviceCollection, delegate(IServiceProvider sp, DbContextOptionsBuilder ob)
		{
			optionsAction(sp, ob);
			CoreOptionsExtension extension = (ob.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension()).WithMaxPoolSize(poolSize);
			((IDbContextOptionsBuilderInfrastructure)ob).AddOrUpdateExtension(extension);
		}, ServiceLifetime.Singleton);
		serviceCollection.TryAddSingleton((IServiceProvider sp) => new DbContextPool<TContextImplementation>(ServiceProviderServiceExtensions.GetService<DbContextOptions<TContextImplementation>>(sp)));
		serviceCollection.AddScoped<DbContextPool<TContextImplementation>.Lease>();
		serviceCollection.AddScoped((IServiceProvider sp) => (TContextService)ServiceProviderServiceExtensions.GetService<DbContextPool<TContextImplementation>.Lease>(sp).Context);
		return serviceCollection;
	}

	public static IServiceCollection AddDbContext<TContext>([JetBrains.Annotations.NotNull] this IServiceCollection serviceCollection, ServiceLifetime contextLifetime, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : DbContext
	{
		return serviceCollection.AddDbContext<TContext, TContext>(contextLifetime, optionsLifetime);
	}

	public static IServiceCollection AddDbContext<TContextService, TContextImplementation>([JetBrains.Annotations.NotNull] this IServiceCollection serviceCollection, ServiceLifetime contextLifetime, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContextService : class where TContextImplementation : DbContext, TContextService
	{
		return serviceCollection.AddDbContext<TContextService, TContextImplementation>((Action<IServiceProvider, DbContextOptionsBuilder>)null, contextLifetime, optionsLifetime);
	}

	public static IServiceCollection AddDbContext<TContext>([JetBrains.Annotations.NotNull] this IServiceCollection serviceCollection, [JetBrains.Annotations.CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : DbContext
	{
		return serviceCollection.AddDbContext<TContext, TContext>(optionsAction, contextLifetime, optionsLifetime);
	}

	public static IServiceCollection AddDbContext<TContextService, TContextImplementation>([JetBrains.Annotations.NotNull] this IServiceCollection serviceCollection, [JetBrains.Annotations.CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContextImplementation : DbContext, TContextService
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceCollection, "serviceCollection");
		if (contextLifetime == ServiceLifetime.Singleton)
		{
			optionsLifetime = ServiceLifetime.Singleton;
		}
		if (optionsAction != null)
		{
			CheckContextConstructors<TContextImplementation>();
		}
		AddCoreServices<TContextImplementation>(serviceCollection, optionsAction, optionsLifetime);
		serviceCollection.TryAdd(new ServiceDescriptor(typeof(TContextService), typeof(TContextImplementation), contextLifetime));
		return serviceCollection;
	}

	private static void AddCoreServices<TContextImplementation>(IServiceCollection serviceCollection, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, ServiceLifetime optionsLifetime) where TContextImplementation : DbContext
	{
		serviceCollection.TryAdd(new ServiceDescriptor(typeof(DbContextOptions<TContextImplementation>), (IServiceProvider p) => CreateDbContextOptions<TContextImplementation>(p, optionsAction), optionsLifetime));
		serviceCollection.Add(new ServiceDescriptor(typeof(DbContextOptions), (IServiceProvider p) => ServiceProviderServiceExtensions.GetRequiredService<DbContextOptions<TContextImplementation>>(p), optionsLifetime));
	}

	private static DbContextOptions<TContext> CreateDbContextOptions<TContext>([JetBrains.Annotations.NotNull] IServiceProvider applicationServiceProvider, [JetBrains.Annotations.CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction) where TContext : DbContext
	{
		DbContextOptionsBuilder<TContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<TContext>(new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>()));
		dbContextOptionsBuilder.UseApplicationServiceProvider(applicationServiceProvider);
		optionsAction?.Invoke(applicationServiceProvider, dbContextOptionsBuilder);
		return dbContextOptionsBuilder.Options;
	}

	private static void CheckContextConstructors<TContext>() where TContext : DbContext
	{
		List<ConstructorInfo> list = typeof(TContext).GetTypeInfo().DeclaredConstructors.ToList();
		if (list.Count == 1 && list[0].GetParameters().Length == 0)
		{
			throw new ArgumentException(CoreStrings.DbContextMissingConstructor(typeof(TContext).ShortDisplayName()));
		}
	}
}
