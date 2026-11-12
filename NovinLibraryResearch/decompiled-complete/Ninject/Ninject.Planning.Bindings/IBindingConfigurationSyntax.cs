using Ninject.Infrastructure;
using Ninject.Syntax;

namespace Ninject.Planning.Bindings;

public interface IBindingConfigurationSyntax<T> : IBindingWhenInNamedWithOrOnSyntax<T>, IBindingWhenSyntax<T>, IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax, IBindingInSyntax<T>, IBindingNamedSyntax<T>, IBindingWithSyntax<T>, IBindingOnSyntax<T>, IBindingInNamedWithOrOnSyntax<T>, IBindingNamedWithOrOnSyntax<T>, IBindingWithOrOnSyntax<T>
{
}
