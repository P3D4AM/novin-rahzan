using Ninject.Planning.Bindings;

namespace Ninject.Infrastructure;

public interface IHaveBindingConfiguration
{
	IBindingConfiguration BindingConfiguration { get; }
}
