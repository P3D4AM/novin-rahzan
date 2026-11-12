using System;
using System.Collections.Generic;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;

namespace Ninject.Planning.Bindings.Resolvers;

public class StandardBindingResolver : NinjectComponent, IBindingResolver, INinjectComponent, IDisposable
{
	public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, Type service)
	{
		return bindings[service].ToEnumerable();
	}
}
