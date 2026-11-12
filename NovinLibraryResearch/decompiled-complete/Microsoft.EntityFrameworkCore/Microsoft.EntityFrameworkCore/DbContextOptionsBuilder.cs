using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore;

public class DbContextOptionsBuilder : IDbContextOptionsBuilderInfrastructure
{
	private DbContextOptions _options;

	public virtual DbContextOptions Options => _options;

	public virtual bool IsConfigured => _options.Extensions.Any((IDbContextOptionsExtension e) => e.Info.IsDatabaseProvider);

	public DbContextOptionsBuilder()
		: this(new DbContextOptions<DbContext>())
	{
	}

	public DbContextOptionsBuilder([JetBrains.Annotations.NotNull] DbContextOptions options)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(options, "options");
		_options = options;
	}

	public virtual DbContextOptionsBuilder UseModel([JetBrains.Annotations.NotNull] IModel model)
	{
		return WithOption((CoreOptionsExtension e) => e.WithModel(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model")));
	}

	public virtual DbContextOptionsBuilder UseLoggerFactory([JetBrains.Annotations.CanBeNull] ILoggerFactory loggerFactory)
	{
		return WithOption((CoreOptionsExtension e) => e.WithLoggerFactory(loggerFactory));
	}

	public virtual DbContextOptionsBuilder EnableDetailedErrors(bool detailedErrorsEnabled = true)
	{
		return WithOption((CoreOptionsExtension e) => e.WithDetailedErrorsEnabled(detailedErrorsEnabled));
	}

	public virtual DbContextOptionsBuilder UseMemoryCache([JetBrains.Annotations.CanBeNull] IMemoryCache memoryCache)
	{
		return WithOption((CoreOptionsExtension e) => e.WithMemoryCache(memoryCache));
	}

	public virtual DbContextOptionsBuilder UseInternalServiceProvider([JetBrains.Annotations.CanBeNull] IServiceProvider serviceProvider)
	{
		return WithOption((CoreOptionsExtension e) => e.WithInternalServiceProvider(serviceProvider));
	}

	public virtual DbContextOptionsBuilder UseApplicationServiceProvider([JetBrains.Annotations.CanBeNull] IServiceProvider serviceProvider)
	{
		return WithOption((CoreOptionsExtension e) => e.WithApplicationServiceProvider(serviceProvider));
	}

	public virtual DbContextOptionsBuilder EnableSensitiveDataLogging(bool sensitiveDataLoggingEnabled = true)
	{
		return WithOption((CoreOptionsExtension e) => e.WithSensitiveDataLoggingEnabled(sensitiveDataLoggingEnabled));
	}

	public virtual DbContextOptionsBuilder EnableServiceProviderCaching(bool cacheServiceProvider = true)
	{
		return WithOption((CoreOptionsExtension e) => e.WithServiceProviderCachingEnabled(cacheServiceProvider));
	}

	public virtual DbContextOptionsBuilder UseQueryTrackingBehavior(QueryTrackingBehavior queryTrackingBehavior)
	{
		return WithOption((CoreOptionsExtension e) => e.WithQueryTrackingBehavior(queryTrackingBehavior));
	}

	public virtual DbContextOptionsBuilder ConfigureWarnings([JetBrains.Annotations.NotNull] Action<WarningsConfigurationBuilder> warningsConfigurationBuilderAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(warningsConfigurationBuilderAction, "warningsConfigurationBuilderAction");
		warningsConfigurationBuilderAction(new WarningsConfigurationBuilder(this));
		return this;
	}

	public virtual DbContextOptionsBuilder ReplaceService<TService, TImplementation>() where TImplementation : TService
	{
		return WithOption((CoreOptionsExtension e) => e.WithReplacedService(typeof(TService), typeof(TImplementation)));
	}

	public virtual DbContextOptionsBuilder AddInterceptors([JetBrains.Annotations.NotNull] IEnumerable<IInterceptor> interceptors)
	{
		return WithOption((CoreOptionsExtension e) => e.WithInterceptors(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(interceptors, "interceptors")));
	}

	public virtual DbContextOptionsBuilder AddInterceptors([JetBrains.Annotations.NotNull] params IInterceptor[] interceptors)
	{
		return AddInterceptors((IEnumerable<IInterceptor>)interceptors);
	}

	void IDbContextOptionsBuilderInfrastructure.AddOrUpdateExtension<TExtension>(TExtension extension)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(extension, "extension");
		_options = _options.WithExtension(extension);
	}

	private DbContextOptionsBuilder WithOption(Func<CoreOptionsExtension, CoreOptionsExtension> withFunc)
	{
		((IDbContextOptionsBuilderInfrastructure)this).AddOrUpdateExtension(withFunc(Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension()));
		return this;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
public class DbContextOptionsBuilder<TContext> : DbContextOptionsBuilder where TContext : DbContext
{
	public new virtual DbContextOptions<TContext> Options => (DbContextOptions<TContext>)base.Options;

	public DbContextOptionsBuilder()
		: this(new DbContextOptions<TContext>())
	{
	}

	public DbContextOptionsBuilder([JetBrains.Annotations.NotNull] DbContextOptions<TContext> options)
		: base(options)
	{
	}

	public new virtual DbContextOptionsBuilder<TContext> UseModel([JetBrains.Annotations.NotNull] IModel model)
	{
		return (DbContextOptionsBuilder<TContext>)base.UseModel(model);
	}

	public new virtual DbContextOptionsBuilder<TContext> UseLoggerFactory([JetBrains.Annotations.CanBeNull] ILoggerFactory loggerFactory)
	{
		return (DbContextOptionsBuilder<TContext>)base.UseLoggerFactory(loggerFactory);
	}

	public new virtual DbContextOptionsBuilder<TContext> EnableDetailedErrors(bool detailedErrorsEnabled = true)
	{
		return (DbContextOptionsBuilder<TContext>)base.EnableDetailedErrors(detailedErrorsEnabled);
	}

	public new virtual DbContextOptionsBuilder<TContext> UseMemoryCache([JetBrains.Annotations.CanBeNull] IMemoryCache memoryCache)
	{
		return (DbContextOptionsBuilder<TContext>)base.UseMemoryCache(memoryCache);
	}

	public new virtual DbContextOptionsBuilder<TContext> UseInternalServiceProvider([JetBrains.Annotations.CanBeNull] IServiceProvider serviceProvider)
	{
		return (DbContextOptionsBuilder<TContext>)base.UseInternalServiceProvider(serviceProvider);
	}

	public new virtual DbContextOptionsBuilder<TContext> UseApplicationServiceProvider([JetBrains.Annotations.CanBeNull] IServiceProvider serviceProvider)
	{
		return (DbContextOptionsBuilder<TContext>)base.UseApplicationServiceProvider(serviceProvider);
	}

	public new virtual DbContextOptionsBuilder<TContext> EnableSensitiveDataLogging(bool sensitiveDataLoggingEnabled = true)
	{
		return (DbContextOptionsBuilder<TContext>)base.EnableSensitiveDataLogging(sensitiveDataLoggingEnabled);
	}

	public new virtual DbContextOptionsBuilder<TContext> EnableServiceProviderCaching(bool cacheServiceProvider = true)
	{
		return (DbContextOptionsBuilder<TContext>)base.EnableServiceProviderCaching(cacheServiceProvider);
	}

	public new virtual DbContextOptionsBuilder<TContext> UseQueryTrackingBehavior(QueryTrackingBehavior queryTrackingBehavior)
	{
		return (DbContextOptionsBuilder<TContext>)base.UseQueryTrackingBehavior(queryTrackingBehavior);
	}

	public new virtual DbContextOptionsBuilder<TContext> ConfigureWarnings([JetBrains.Annotations.NotNull] Action<WarningsConfigurationBuilder> warningsConfigurationBuilderAction)
	{
		return (DbContextOptionsBuilder<TContext>)base.ConfigureWarnings(warningsConfigurationBuilderAction);
	}

	public new virtual DbContextOptionsBuilder<TContext> ReplaceService<TService, TImplementation>() where TImplementation : TService
	{
		return (DbContextOptionsBuilder<TContext>)base.ReplaceService<TService, TImplementation>();
	}
}
