using Ninject.Infrastructure;

namespace Ninject.Syntax;

public interface IBindingWhenInNamedWithOrOnSyntax<T> : IBindingWhenSyntax<T>, IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax, IBindingInSyntax<T>, IBindingNamedSyntax<T>, IBindingWithSyntax<T>, IBindingOnSyntax<T>
{
}
