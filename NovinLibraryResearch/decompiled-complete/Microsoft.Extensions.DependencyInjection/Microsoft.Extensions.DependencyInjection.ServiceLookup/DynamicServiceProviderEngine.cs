using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.")]
internal sealed class DynamicServiceProviderEngine : CompiledServiceProviderEngine
{
	private readonly ServiceProvider _serviceProvider;

	public DynamicServiceProviderEngine(ServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public override Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite)
	{
		ServiceCallSite callSite2 = callSite;
		int callCount = 0;
		return delegate(ServiceProviderEngineScope scope)
		{
			object result = CallSiteRuntimeResolver.Instance.Resolve(callSite2, scope);
			if (Interlocked.Increment(ref callCount) == 2)
			{
				ThreadPool.UnsafeQueueUserWorkItem(delegate
				{
					try
					{
						_serviceProvider.ReplaceServiceAccessor(callSite2, base.RealizeService(callSite2));
					}
					catch (Exception exception)
					{
						DependencyInjectionEventSource.Log.ServiceRealizationFailed(exception, _serviceProvider.GetHashCode());
					}
				}, null);
			}
			return result;
		};
	}
}
