using Ninject.Infrastructure;

namespace Ninject.Syntax;

public interface IBindingInNamedWithOrOnSyntax<T> : IBindingInSyntax<T>, IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax, IBindingNamedSyntax<T>, IBindingWithSyntax<T>, IBindingOnSyntax<T>
{
}
