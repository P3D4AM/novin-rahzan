using System;
using System.Collections.Generic;
using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Introspection;
using Ninject.Parameters;

namespace Ninject.Planning.Bindings;

public class BindingConfiguration : IBindingConfiguration
{
	public IBindingMetadata Metadata { get; private set; }

	public bool IsImplicit { get; set; }

	public bool IsConditional => Condition != null;

	public BindingTarget Target { get; set; }

	public Func<IRequest, bool> Condition { get; set; }

	public Func<IContext, IProvider> ProviderCallback { get; set; }

	public Func<IContext, object> ScopeCallback { get; set; }

	public ICollection<IParameter> Parameters { get; private set; }

	public ICollection<Action<IContext, object>> ActivationActions { get; private set; }

	public ICollection<Action<IContext, object>> DeactivationActions { get; private set; }

	public BindingConfiguration()
	{
		Metadata = new BindingMetadata();
		Parameters = new List<IParameter>();
		ActivationActions = new List<Action<IContext, object>>();
		DeactivationActions = new List<Action<IContext, object>>();
		ScopeCallback = StandardScopeCallbacks.Transient;
	}

	public IProvider GetProvider(IContext context)
	{
		Ensure.ArgumentNotNull(context, "context");
		if (ProviderCallback == null)
		{
			throw new ActivationException(ExceptionFormatter.ProviderCallbackIsNull(context));
		}
		return ProviderCallback(context);
	}

	public object GetScope(IContext context)
	{
		Ensure.ArgumentNotNull(context, "context");
		return ScopeCallback(context);
	}

	public bool Matches(IRequest request)
	{
		Ensure.ArgumentNotNull(request, "request");
		if (Condition != null)
		{
			return Condition(request);
		}
		return true;
	}
}
