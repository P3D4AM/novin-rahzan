using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal abstract class CompiledServiceProviderEngine : ServiceProviderEngine
{
	public ILEmitResolverBuilder ResolverBuilder { get; }

	[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Creates DynamicMethods")]
	public CompiledServiceProviderEngine(ServiceProvider provider)
	{
		ResolverBuilder = new ILEmitResolverBuilder(provider);
	}

	public override Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite)
	{
		return ResolverBuilder.Build(callSite);
	}
}
