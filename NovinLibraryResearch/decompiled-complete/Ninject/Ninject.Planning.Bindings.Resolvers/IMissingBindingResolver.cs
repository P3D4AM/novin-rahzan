using System;
using System.Collections.Generic;
using Ninject.Activation;
using Ninject.Components;
using Ninject.Infrastructure;

namespace Ninject.Planning.Bindings.Resolvers;

public interface IMissingBindingResolver : INinjectComponent, IDisposable
{
	IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, IRequest request);
}
