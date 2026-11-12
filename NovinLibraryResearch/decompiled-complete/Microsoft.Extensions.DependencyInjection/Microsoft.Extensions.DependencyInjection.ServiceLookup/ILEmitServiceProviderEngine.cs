using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal sealed class ILEmitServiceProviderEngine : ServiceProviderEngine
{
	private readonly ILEmitResolverBuilder _expressionResolverBuilder;

	[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Creates DynamicMethods")]
	public ILEmitServiceProviderEngine(ServiceProvider serviceProvider)
	{
		_expressionResolverBuilder = new ILEmitResolverBuilder(serviceProvider);
	}

	public override Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite)
	{
		return _expressionResolverBuilder.Build(callSite);
	}
}
