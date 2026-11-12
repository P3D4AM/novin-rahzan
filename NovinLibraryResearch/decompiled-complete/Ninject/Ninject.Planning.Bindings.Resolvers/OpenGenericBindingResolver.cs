using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;

namespace Ninject.Planning.Bindings.Resolvers;

public class OpenGenericBindingResolver : NinjectComponent, IBindingResolver, INinjectComponent, IDisposable
{
	public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, Type service)
	{
		if (!service.IsGenericType || service.IsGenericTypeDefinition || !bindings.ContainsKey(service.GetGenericTypeDefinition()))
		{
			return Enumerable.Empty<IBinding>();
		}
		return bindings[service.GetGenericTypeDefinition()].ToEnumerable();
	}
}
