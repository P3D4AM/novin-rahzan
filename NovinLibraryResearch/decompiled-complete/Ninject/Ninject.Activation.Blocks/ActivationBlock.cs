using System;
using System.Collections.Generic;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Disposal;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Syntax;

namespace Ninject.Activation.Blocks;

public class ActivationBlock : DisposableObject, IActivationBlock, IResolutionRoot, IFluentSyntax, INotifyWhenDisposed, IDisposableObject, IDisposable
{
	public IResolutionRoot Parent { get; private set; }

	public ActivationBlock(IResolutionRoot parent)
	{
		Ensure.ArgumentNotNull(parent, "parent");
		Parent = parent;
	}

	public void Inject(object instance, params IParameter[] parameters)
	{
		Parent.Inject(instance, parameters);
	}

	public bool CanResolve(IRequest request)
	{
		Ensure.ArgumentNotNull(request, "request");
		return Parent.CanResolve(request);
	}

	public bool CanResolve(IRequest request, bool ignoreImplicitBindings)
	{
		Ensure.ArgumentNotNull(request, "request");
		return Parent.CanResolve(request, ignoreImplicitBindings);
	}

	public IEnumerable<object> Resolve(IRequest request)
	{
		Ensure.ArgumentNotNull(request, "request");
		return Parent.Resolve(request);
	}

	public virtual IRequest CreateRequest(Type service, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters, bool isOptional, bool isUnique)
	{
		Ensure.ArgumentNotNull(service, "service");
		Ensure.ArgumentNotNull(parameters, "parameters");
		return new Request(service, constraint, parameters, () => this, isOptional, isUnique);
	}

	public bool Release(object instance)
	{
		return Parent.Release(instance);
	}

	Type IFluentSyntax.GetType()
	{
		return GetType();
	}
}
