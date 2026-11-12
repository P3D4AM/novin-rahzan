using System;
using System.Collections.Generic;
using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Parameters;

namespace Ninject.Planning.Bindings;

public class Binding : IBinding, IBindingConfiguration
{
	public IBindingConfiguration BindingConfiguration { get; private set; }

	public Type Service { get; private set; }

	public IBindingMetadata Metadata => BindingConfiguration.Metadata;

	public BindingTarget Target
	{
		get
		{
			return BindingConfiguration.Target;
		}
		set
		{
			BindingConfiguration.Target = value;
		}
	}

	public bool IsImplicit
	{
		get
		{
			return BindingConfiguration.IsImplicit;
		}
		set
		{
			BindingConfiguration.IsImplicit = value;
		}
	}

	public bool IsConditional => BindingConfiguration.IsConditional;

	public Func<IRequest, bool> Condition
	{
		get
		{
			return BindingConfiguration.Condition;
		}
		set
		{
			BindingConfiguration.Condition = value;
		}
	}

	public Func<IContext, IProvider> ProviderCallback
	{
		get
		{
			return BindingConfiguration.ProviderCallback;
		}
		set
		{
			BindingConfiguration.ProviderCallback = value;
		}
	}

	public Func<IContext, object> ScopeCallback
	{
		get
		{
			return BindingConfiguration.ScopeCallback;
		}
		set
		{
			BindingConfiguration.ScopeCallback = value;
		}
	}

	public ICollection<IParameter> Parameters => BindingConfiguration.Parameters;

	public ICollection<Action<IContext, object>> ActivationActions => BindingConfiguration.ActivationActions;

	public ICollection<Action<IContext, object>> DeactivationActions => BindingConfiguration.DeactivationActions;

	public Binding(Type service)
	{
		Ensure.ArgumentNotNull(service, "service");
		Service = service;
		BindingConfiguration = new BindingConfiguration();
	}

	public Binding(Type service, IBindingConfiguration configuration)
	{
		Ensure.ArgumentNotNull(service, "service");
		Ensure.ArgumentNotNull(configuration, "configuration");
		Service = service;
		BindingConfiguration = configuration;
	}

	public IProvider GetProvider(IContext context)
	{
		return BindingConfiguration.GetProvider(context);
	}

	public object GetScope(IContext context)
	{
		return BindingConfiguration.GetScope(context);
	}

	public bool Matches(IRequest request)
	{
		return BindingConfiguration.Matches(request);
	}
}
