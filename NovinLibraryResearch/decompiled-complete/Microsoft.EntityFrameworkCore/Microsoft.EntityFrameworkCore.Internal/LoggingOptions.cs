using System;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Internal;

public class LoggingOptions : ILoggingOptions, ISingletonOptions
{
	public virtual bool IsSensitiveDataLoggingEnabled { get; private set; }

	public virtual bool IsSensitiveDataLoggingWarned { get; set; }

	public virtual WarningsConfiguration WarningsConfiguration { get; private set; }

	public virtual void Initialize(IDbContextOptions options)
	{
		CoreOptionsExtension coreOptionsExtension = options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();
		IsSensitiveDataLoggingEnabled = coreOptionsExtension.IsSensitiveDataLoggingEnabled;
		WarningsConfiguration = coreOptionsExtension.WarningsConfiguration;
	}

	public virtual void Validate(IDbContextOptions options)
	{
		CoreOptionsExtension coreOptionsExtension = options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();
		if (IsSensitiveDataLoggingEnabled != coreOptionsExtension.IsSensitiveDataLoggingEnabled)
		{
			throw new InvalidOperationException(CoreStrings.SingletonOptionChanged("EnableSensitiveDataLogging", "UseInternalServiceProvider"));
		}
		if (WarningsConfiguration?.GetServiceProviderHashCode() != coreOptionsExtension.WarningsConfiguration?.GetServiceProviderHashCode())
		{
			throw new InvalidOperationException(CoreStrings.SingletonOptionChanged("ConfigureWarnings", "UseInternalServiceProvider"));
		}
	}
}
