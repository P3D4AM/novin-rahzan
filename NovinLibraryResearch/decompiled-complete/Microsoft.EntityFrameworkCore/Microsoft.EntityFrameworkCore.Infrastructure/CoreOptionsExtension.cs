using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class CoreOptionsExtension : IDbContextOptionsExtension
{
	private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
	{
		private long? _serviceProviderHash;

		private string _logFragment;

		private new CoreOptionsExtension Extension => (CoreOptionsExtension)base.Extension;

		public override bool IsDatabaseProvider => false;

		public override string LogFragment
		{
			get
			{
				if (_logFragment == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (Extension._queryTrackingBehavior != 0)
					{
						stringBuilder.Append(Extension._queryTrackingBehavior).Append(' ');
					}
					if (Extension._sensitiveDataLoggingEnabled)
					{
						stringBuilder.Append("SensitiveDataLoggingEnabled ");
					}
					if (Extension._detailedErrorsEnabled)
					{
						stringBuilder.Append("DetailedErrorsEnabled ");
					}
					if (Extension._maxPoolSize.HasValue)
					{
						stringBuilder.Append("MaxPoolSize=").Append(Extension._maxPoolSize).Append(' ');
					}
					_logFragment = stringBuilder.ToString();
				}
				return _logFragment;
			}
		}

		public ExtensionInfo(CoreOptionsExtension extension)
			: base(extension)
		{
		}

		public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(debugInfo, "debugInfo");
			debugInfo["Core:UseMemoryCache"] = ((long)(Extension.GetMemoryCache()?.GetHashCode() ?? 0)).ToString(CultureInfo.InvariantCulture);
			debugInfo["Core:EnableSensitiveDataLogging"] = Extension._sensitiveDataLoggingEnabled.GetHashCode().ToString(CultureInfo.InvariantCulture);
			debugInfo["Core:EnableDetailedErrors"] = Extension._detailedErrorsEnabled.GetHashCode().ToString(CultureInfo.InvariantCulture);
			debugInfo["Core:ConfigureWarnings"] = Extension._warningsConfiguration.GetServiceProviderHashCode().ToString(CultureInfo.InvariantCulture);
			if (Extension._replacedServices == null)
			{
				return;
			}
			foreach (KeyValuePair<Type, Type> replacedService in Extension._replacedServices)
			{
				debugInfo["Core:ReplaceService:" + replacedService.Key.DisplayName()] = replacedService.Value.GetHashCode().ToString(CultureInfo.InvariantCulture);
			}
		}

		public override long GetServiceProviderHashCode()
		{
			if (!_serviceProviderHash.HasValue)
			{
				long num = Extension.GetMemoryCache()?.GetHashCode() ?? 0;
				num = (num * 3) ^ Extension._sensitiveDataLoggingEnabled.GetHashCode();
				num = (num * 3) ^ Extension._detailedErrorsEnabled.GetHashCode();
				num = (num * 1073742113) ^ Extension._warningsConfiguration.GetServiceProviderHashCode();
				if (Extension._replacedServices != null)
				{
					num = Extension._replacedServices.Aggregate(num, (long t, KeyValuePair<Type, Type> e) => (t * 397) ^ e.Value.GetHashCode());
				}
				_serviceProviderHash = num;
			}
			return _serviceProviderHash.Value;
		}
	}

	private IServiceProvider _internalServiceProvider;

	private IServiceProvider _applicationServiceProvider;

	private IModel _model;

	private ILoggerFactory _loggerFactory;

	private IMemoryCache _memoryCache;

	private bool _sensitiveDataLoggingEnabled;

	private bool _detailedErrorsEnabled;

	private QueryTrackingBehavior _queryTrackingBehavior;

	private IDictionary<Type, Type> _replacedServices;

	private int? _maxPoolSize;

	private bool _serviceProviderCachingEnabled = true;

	private DbContextOptionsExtensionInfo _info;

	private IEnumerable<IInterceptor> _interceptors;

	private WarningsConfiguration _warningsConfiguration = new WarningsConfiguration().TryWithExplicit(CoreEventId.ManyServiceProvidersCreatedWarning, WarningBehavior.Throw).TryWithExplicit(CoreEventId.LazyLoadOnDisposedContextWarning, WarningBehavior.Throw).TryWithExplicit(CoreEventId.DetachedLazyLoadingWarning, WarningBehavior.Throw);

	public virtual DbContextOptionsExtensionInfo Info => _info ?? (_info = new ExtensionInfo(this));

	public virtual bool IsSensitiveDataLoggingEnabled => _sensitiveDataLoggingEnabled;

	public virtual bool DetailedErrorsEnabled => _detailedErrorsEnabled;

	public virtual IModel Model => _model;

	public virtual ILoggerFactory LoggerFactory => _loggerFactory;

	public virtual IMemoryCache MemoryCache => _memoryCache;

	public virtual IServiceProvider InternalServiceProvider => _internalServiceProvider;

	public virtual IServiceProvider ApplicationServiceProvider => _applicationServiceProvider;

	public virtual WarningsConfiguration WarningsConfiguration => _warningsConfiguration;

	public virtual QueryTrackingBehavior QueryTrackingBehavior => _queryTrackingBehavior;

	public virtual bool ServiceProviderCachingEnabled => _serviceProviderCachingEnabled;

	public virtual IReadOnlyDictionary<Type, Type> ReplacedServices => (IReadOnlyDictionary<Type, Type>)_replacedServices;

	public virtual int? MaxPoolSize => _maxPoolSize;

	public virtual IEnumerable<IInterceptor> Interceptors => _interceptors;

	public CoreOptionsExtension()
	{
	}

	protected CoreOptionsExtension([JetBrains.Annotations.NotNull] CoreOptionsExtension copyFrom)
	{
		_internalServiceProvider = copyFrom.InternalServiceProvider;
		_applicationServiceProvider = copyFrom.ApplicationServiceProvider;
		_model = copyFrom.Model;
		_loggerFactory = copyFrom.LoggerFactory;
		_memoryCache = copyFrom.MemoryCache;
		_sensitiveDataLoggingEnabled = copyFrom.IsSensitiveDataLoggingEnabled;
		_detailedErrorsEnabled = copyFrom.DetailedErrorsEnabled;
		_warningsConfiguration = copyFrom.WarningsConfiguration;
		_queryTrackingBehavior = copyFrom.QueryTrackingBehavior;
		_maxPoolSize = copyFrom.MaxPoolSize;
		_serviceProviderCachingEnabled = copyFrom.ServiceProviderCachingEnabled;
		_interceptors = copyFrom.Interceptors?.ToList();
		if (copyFrom._replacedServices != null)
		{
			_replacedServices = new Dictionary<Type, Type>(copyFrom._replacedServices);
		}
	}

	protected virtual CoreOptionsExtension Clone()
	{
		return new CoreOptionsExtension(this);
	}

	public virtual CoreOptionsExtension WithInternalServiceProvider([JetBrains.Annotations.CanBeNull] IServiceProvider internalServiceProvider)
	{
		CoreOptionsExtension coreOptionsExtension = Clone();
		coreOptionsExtension._internalServiceProvider = internalServiceProvider;
		return coreOptionsExtension;
	}

	public virtual CoreOptionsExtension WithApplicationServiceProvider([JetBrains.Annotations.CanBeNull] IServiceProvider applicationServiceProvider)
	{
		CoreOptionsExtension coreOptionsExtension = Clone();
		coreOptionsExtension._applicationServiceProvider = applicationServiceProvider;
		return coreOptionsExtension;
	}

	public virtual CoreOptionsExtension WithModel([JetBrains.Annotations.CanBeNull] IModel model)
	{
		CoreOptionsExtension coreOptionsExtension = Clone();
		coreOptionsExtension._model = model;
		return coreOptionsExtension;
	}

	public virtual CoreOptionsExtension WithMemoryCache([JetBrains.Annotations.CanBeNull] IMemoryCache memoryCache)
	{
		CoreOptionsExtension coreOptionsExtension = Clone();
		coreOptionsExtension._memoryCache = memoryCache;
		return coreOptionsExtension;
	}

	public virtual CoreOptionsExtension WithLoggerFactory([JetBrains.Annotations.CanBeNull] ILoggerFactory loggerFactory)
	{
		CoreOptionsExtension coreOptionsExtension = Clone();
		coreOptionsExtension._loggerFactory = loggerFactory;
		return coreOptionsExtension;
	}

	public virtual CoreOptionsExtension WithSensitiveDataLoggingEnabled(bool sensitiveDataLoggingEnabled)
	{
		CoreOptionsExtension coreOptionsExtension = Clone();
		coreOptionsExtension._sensitiveDataLoggingEnabled = sensitiveDataLoggingEnabled;
		return coreOptionsExtension;
	}

	public virtual CoreOptionsExtension WithDetailedErrorsEnabled(bool detailedErrorsEnabled)
	{
		CoreOptionsExtension coreOptionsExtension = Clone();
		coreOptionsExtension._detailedErrorsEnabled = detailedErrorsEnabled;
		return coreOptionsExtension;
	}

	public virtual CoreOptionsExtension WithQueryTrackingBehavior(QueryTrackingBehavior queryTrackingBehavior)
	{
		CoreOptionsExtension coreOptionsExtension = Clone();
		coreOptionsExtension._queryTrackingBehavior = queryTrackingBehavior;
		return coreOptionsExtension;
	}

	public virtual CoreOptionsExtension WithReplacedService([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Type implementationType)
	{
		CoreOptionsExtension coreOptionsExtension = Clone();
		if (coreOptionsExtension._replacedServices == null)
		{
			coreOptionsExtension._replacedServices = new Dictionary<Type, Type>();
		}
		coreOptionsExtension._replacedServices[serviceType] = implementationType;
		return coreOptionsExtension;
	}

	public virtual CoreOptionsExtension WithMaxPoolSize(int? maxPoolSize)
	{
		CoreOptionsExtension coreOptionsExtension = Clone();
		coreOptionsExtension._maxPoolSize = maxPoolSize;
		return coreOptionsExtension;
	}

	public virtual CoreOptionsExtension WithWarningsConfiguration([JetBrains.Annotations.CanBeNull] WarningsConfiguration warningsConfiguration)
	{
		CoreOptionsExtension coreOptionsExtension = Clone();
		coreOptionsExtension._warningsConfiguration = warningsConfiguration;
		return coreOptionsExtension;
	}

	public virtual CoreOptionsExtension WithServiceProviderCachingEnabled(bool serviceProviderCachingEnabled)
	{
		CoreOptionsExtension coreOptionsExtension = Clone();
		coreOptionsExtension._serviceProviderCachingEnabled = serviceProviderCachingEnabled;
		return coreOptionsExtension;
	}

	public virtual CoreOptionsExtension WithInterceptors([JetBrains.Annotations.NotNull] IEnumerable<IInterceptor> interceptors)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(interceptors, "interceptors");
		CoreOptionsExtension coreOptionsExtension = Clone();
		coreOptionsExtension._interceptors = ((_interceptors == null) ? interceptors : _interceptors.Concat(interceptors));
		return coreOptionsExtension;
	}

	public virtual void ApplyServices(IServiceCollection services)
	{
		IMemoryCache memoryCache = GetMemoryCache();
		if (memoryCache != null)
		{
			services.AddSingleton(memoryCache);
		}
	}

	private IMemoryCache GetMemoryCache()
	{
		return MemoryCache;
	}

	public virtual void Validate(IDbContextOptions options)
	{
		if (_internalServiceProvider != null)
		{
			if (ReplacedServices != null)
			{
				throw new InvalidOperationException(CoreStrings.InvalidReplaceService("ReplaceService", "UseInternalServiceProvider"));
			}
			if (LoggerFactory != null)
			{
				throw new InvalidOperationException(CoreStrings.InvalidUseService("UseLoggerFactory", "UseInternalServiceProvider", "ILoggerFactory"));
			}
			if (MemoryCache != null)
			{
				throw new InvalidOperationException(CoreStrings.InvalidUseService("UseMemoryCache", "UseInternalServiceProvider", "IMemoryCache"));
			}
		}
	}
}
