using System;
using System.Collections.Generic;
using Ninject.Activation;
using Ninject.Parameters;

namespace Ninject.Planning.Bindings;

public interface IBindingConfiguration
{
	IBindingMetadata Metadata { get; }

	BindingTarget Target { get; set; }

	bool IsImplicit { get; set; }

	bool IsConditional { get; }

	Func<IRequest, bool> Condition { get; set; }

	Func<IContext, IProvider> ProviderCallback { get; set; }

	Func<IContext, object> ScopeCallback { get; set; }

	ICollection<IParameter> Parameters { get; }

	ICollection<Action<IContext, object>> ActivationActions { get; }

	ICollection<Action<IContext, object>> DeactivationActions { get; }

	IProvider GetProvider(IContext context);

	object GetScope(IContext context);

	bool Matches(IRequest request);
}
