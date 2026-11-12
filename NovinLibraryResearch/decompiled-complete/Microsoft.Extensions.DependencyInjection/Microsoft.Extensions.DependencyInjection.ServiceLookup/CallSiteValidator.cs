using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal sealed class CallSiteValidator : CallSiteVisitor<CallSiteValidator.CallSiteValidatorState, Type?>
{
	internal struct CallSiteValidatorState
	{
		public ServiceCallSite? Singleton
		{
			get; [param: DisallowNull]
			set;
		}
	}

	private readonly ConcurrentDictionary<Type, Type> _scopedServices = new ConcurrentDictionary<Type, Type>();

	public void ValidateCallSite(ServiceCallSite callSite)
	{
		Type type = VisitCallSite(callSite, default(CallSiteValidatorState));
		if (type != null)
		{
			_scopedServices[callSite.ServiceType] = type;
		}
	}

	public void ValidateResolution(Type serviceType, IServiceScope scope, IServiceScope rootScope)
	{
		if (scope == rootScope && _scopedServices.TryGetValue(serviceType, out var value))
		{
			if (serviceType == value)
			{
				throw new InvalidOperationException(System.SR.Format(System.SR.DirectScopedResolvedFromRootException, serviceType, "Scoped".ToLowerInvariant()));
			}
			throw new InvalidOperationException(System.SR.Format(System.SR.ScopedResolvedFromRootException, serviceType, value, "Scoped".ToLowerInvariant()));
		}
	}

	protected override Type? VisitConstructor(ConstructorCallSite constructorCallSite, CallSiteValidatorState state)
	{
		Type type = null;
		ServiceCallSite[] parameterCallSites = constructorCallSite.ParameterCallSites;
		foreach (ServiceCallSite callSite in parameterCallSites)
		{
			Type type2 = VisitCallSite(callSite, state);
			if ((object)type == null)
			{
				type = type2;
			}
		}
		return type;
	}

	protected override Type? VisitIEnumerable(IEnumerableCallSite enumerableCallSite, CallSiteValidatorState state)
	{
		Type type = null;
		ServiceCallSite[] serviceCallSites = enumerableCallSite.ServiceCallSites;
		foreach (ServiceCallSite callSite in serviceCallSites)
		{
			Type type2 = VisitCallSite(callSite, state);
			if ((object)type == null)
			{
				type = type2;
			}
		}
		return type;
	}

	protected override Type? VisitRootCache(ServiceCallSite singletonCallSite, CallSiteValidatorState state)
	{
		state.Singleton = singletonCallSite;
		return VisitCallSiteMain(singletonCallSite, state);
	}

	protected override Type? VisitScopeCache(ServiceCallSite scopedCallSite, CallSiteValidatorState state)
	{
		if (scopedCallSite.ServiceType == typeof(IServiceScopeFactory))
		{
			return null;
		}
		if (state.Singleton != null)
		{
			throw new InvalidOperationException(System.SR.Format(System.SR.ScopedInSingletonException, scopedCallSite.ServiceType, state.Singleton.ServiceType, "Scoped".ToLowerInvariant(), "Singleton".ToLowerInvariant()));
		}
		VisitCallSiteMain(scopedCallSite, state);
		return scopedCallSite.ServiceType;
	}

	protected override Type? VisitConstant(ConstantCallSite constantCallSite, CallSiteValidatorState state)
	{
		return null;
	}

	protected override Type? VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, CallSiteValidatorState state)
	{
		return null;
	}

	protected override Type? VisitFactory(FactoryCallSite factoryCallSite, CallSiteValidatorState state)
	{
		return null;
	}
}
