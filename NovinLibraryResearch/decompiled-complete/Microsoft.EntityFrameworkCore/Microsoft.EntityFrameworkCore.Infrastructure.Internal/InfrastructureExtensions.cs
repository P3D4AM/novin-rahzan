using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Infrastructure.Internal;

public static class InfrastructureExtensions
{
	public static TService GetService<TService>([JetBrains.Annotations.CanBeNull] IInfrastructure<IServiceProvider> accessor)
	{
		object obj = null;
		if (accessor != null)
		{
			IServiceProvider instance = accessor.Instance;
			obj = instance.GetService(typeof(TService)) ?? ServiceProviderServiceExtensions.GetService<IDbContextOptions>(instance)?.Extensions.OfType<CoreOptionsExtension>().FirstOrDefault()?.ApplicationServiceProvider?.GetService(typeof(TService));
			if (obj == null)
			{
				throw new InvalidOperationException(CoreStrings.NoProviderConfiguredFailedToResolveService(typeof(TService).DisplayName()));
			}
		}
		return (TService)obj;
	}
}
