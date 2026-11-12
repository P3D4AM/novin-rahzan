using System;
using Ninject.Activation;
using Ninject.Infrastructure;

namespace Ninject.Syntax;

public interface IBindingOnSyntax<T> : IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	IBindingOnSyntax<T> OnActivation(Action<T> action);

	IBindingOnSyntax<T> OnActivation<TImplementation>(Action<TImplementation> action);

	IBindingOnSyntax<T> OnActivation(Action<IContext, T> action);

	IBindingOnSyntax<T> OnActivation<TImplementation>(Action<IContext, TImplementation> action);

	IBindingOnSyntax<T> OnDeactivation(Action<T> action);

	IBindingOnSyntax<T> OnDeactivation<TImplementation>(Action<TImplementation> action);

	IBindingOnSyntax<T> OnDeactivation(Action<IContext, T> action);

	IBindingOnSyntax<T> OnDeactivation<TImplementation>(Action<IContext, TImplementation> action);
}
