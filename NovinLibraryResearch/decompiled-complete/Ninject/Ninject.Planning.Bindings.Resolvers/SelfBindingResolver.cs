using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Activation;
using Ninject.Activation.Providers;
using Ninject.Components;
using Ninject.Infrastructure;

namespace Ninject.Planning.Bindings.Resolvers;

public class SelfBindingResolver : NinjectComponent, IMissingBindingResolver, INinjectComponent, IDisposable
{
	public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, IRequest request)
	{
		Type service = request.Service;
		if (!TypeIsSelfBindable(service))
		{
			return Enumerable.Empty<IBinding>();
		}
		return new Binding[1]
		{
			new Binding(service)
			{
				ProviderCallback = StandardProvider.GetCreationCallback(service)
			}
		};
	}

	protected virtual bool TypeIsSelfBindable(Type service)
	{
		if (!service.IsInterface && !service.IsAbstract && !service.IsValueType && service != typeof(string))
		{
			return !service.ContainsGenericParameters;
		}
		return false;
	}
}
