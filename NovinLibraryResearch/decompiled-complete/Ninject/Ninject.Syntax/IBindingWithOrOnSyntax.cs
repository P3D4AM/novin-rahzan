using Ninject.Infrastructure;

namespace Ninject.Syntax;

public interface IBindingWithOrOnSyntax<T> : IBindingWithSyntax<T>, IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax, IBindingOnSyntax<T>
{
}
