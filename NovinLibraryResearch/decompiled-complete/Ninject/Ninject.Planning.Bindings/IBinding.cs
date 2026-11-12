using System;

namespace Ninject.Planning.Bindings;

public interface IBinding : IBindingConfiguration
{
	IBindingConfiguration BindingConfiguration { get; }

	Type Service { get; }
}
