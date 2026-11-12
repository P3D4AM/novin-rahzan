using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Activation;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Planning.Targets;

namespace Ninject.Planning.Bindings.Resolvers;

public class DefaultValueBindingResolver : NinjectComponent, IMissingBindingResolver, INinjectComponent, IDisposable
{
	private class DefaultParameterValueProvider : IProvider
	{
		public Type Type { get; private set; }

		public DefaultParameterValueProvider(Type type)
		{
			Type = type;
		}

		public object Create(IContext context)
		{
			return context.Request.Target?.DefaultValue;
		}
	}

	public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, IRequest request)
	{
		Type service = request.Service;
		if (!HasDefaultValue(request.Target))
		{
			return Enumerable.Empty<IBinding>();
		}
		return new Binding[1]
		{
			new Binding(service)
			{
				Condition = (IRequest r) => HasDefaultValue(r.Target),
				ProviderCallback = (IContext _) => new DefaultParameterValueProvider(service)
			}
		};
	}

	private static bool HasDefaultValue(ITarget target)
	{
		return target?.HasDefaultValue ?? false;
	}
}
