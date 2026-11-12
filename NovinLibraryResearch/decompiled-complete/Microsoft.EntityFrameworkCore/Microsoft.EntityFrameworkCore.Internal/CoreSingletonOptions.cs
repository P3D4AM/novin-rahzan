using System;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Internal;

public class CoreSingletonOptions : ICoreSingletonOptions, ISingletonOptions
{
	public virtual bool AreDetailedErrorsEnabled { get; private set; }

	public virtual void Initialize(IDbContextOptions options)
	{
		CoreOptionsExtension coreOptionsExtension = options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();
		AreDetailedErrorsEnabled = coreOptionsExtension.DetailedErrorsEnabled;
	}

	public virtual void Validate(IDbContextOptions options)
	{
		CoreOptionsExtension coreOptionsExtension = options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();
		if (AreDetailedErrorsEnabled != coreOptionsExtension.DetailedErrorsEnabled)
		{
			throw new InvalidOperationException(CoreStrings.SingletonOptionChanged("EnableDetailedErrors", "UseInternalServiceProvider"));
		}
	}
}
