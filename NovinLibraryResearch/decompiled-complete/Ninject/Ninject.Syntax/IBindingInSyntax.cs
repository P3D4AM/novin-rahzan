using System;
using Ninject.Activation;
using Ninject.Infrastructure;

namespace Ninject.Syntax;

public interface IBindingInSyntax<T> : IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	IBindingNamedWithOrOnSyntax<T> InSingletonScope();

	IBindingNamedWithOrOnSyntax<T> InTransientScope();

	IBindingNamedWithOrOnSyntax<T> InThreadScope();

	IBindingNamedWithOrOnSyntax<T> InScope(Func<IContext, object> scope);
}
