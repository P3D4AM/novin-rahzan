using Ninject.Infrastructure;

namespace Ninject.Syntax;

public interface IBindingNamedWithOrOnSyntax<T> : IBindingNamedSyntax<T>, IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax, IBindingWithSyntax<T>, IBindingOnSyntax<T>
{
}
