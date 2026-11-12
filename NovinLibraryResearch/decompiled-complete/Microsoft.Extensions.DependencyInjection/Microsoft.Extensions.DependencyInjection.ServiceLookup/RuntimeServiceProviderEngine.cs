using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.")]
internal sealed class RuntimeServiceProviderEngine : ServiceProviderEngine
{
	public static RuntimeServiceProviderEngine Instance { get; } = new RuntimeServiceProviderEngine();


	private RuntimeServiceProviderEngine()
	{
	}

	public override Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite)
	{
		ServiceCallSite callSite2 = callSite;
		return (ServiceProviderEngineScope scope) => CallSiteRuntimeResolver.Instance.Resolve(callSite2, scope);
	}
}
