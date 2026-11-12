using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal sealed class ExpressionsServiceProviderEngine : ServiceProviderEngine
{
	private readonly ExpressionResolverBuilder _expressionResolverBuilder;

	[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.")]
	public ExpressionsServiceProviderEngine(ServiceProvider serviceProvider)
	{
		_expressionResolverBuilder = new ExpressionResolverBuilder(serviceProvider);
	}

	public override Func<ServiceProviderEngineScope, object> RealizeService(ServiceCallSite callSite)
	{
		return _expressionResolverBuilder.Build(callSite);
	}
}
