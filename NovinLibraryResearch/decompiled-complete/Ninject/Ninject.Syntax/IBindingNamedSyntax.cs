using Ninject.Infrastructure;

namespace Ninject.Syntax;

public interface IBindingNamedSyntax<T> : IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	IBindingWithOrOnSyntax<T> Named(string name);
}
