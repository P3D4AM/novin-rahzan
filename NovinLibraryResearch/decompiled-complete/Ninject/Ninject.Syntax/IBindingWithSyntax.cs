using System;
using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Parameters;
using Ninject.Planning.Targets;

namespace Ninject.Syntax;

public interface IBindingWithSyntax<T> : IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	IBindingWithOrOnSyntax<T> WithConstructorArgument(string name, object value);

	IBindingWithOrOnSyntax<T> WithConstructorArgument(string name, Func<IContext, object> callback);

	IBindingWithOrOnSyntax<T> WithConstructorArgument(string name, Func<IContext, ITarget, object> callback);

	IBindingWithOrOnSyntax<T> WithConstructorArgument<TValue>(TValue value);

	IBindingWithOrOnSyntax<T> WithConstructorArgument(Type type, object value);

	IBindingWithOrOnSyntax<T> WithConstructorArgument<TValue>(Func<IContext, TValue> callback);

	IBindingWithOrOnSyntax<T> WithConstructorArgument(Type type, Func<IContext, object> callback);

	IBindingWithOrOnSyntax<T> WithConstructorArgument<TValue>(Func<IContext, ITarget, TValue> callback);

	IBindingWithOrOnSyntax<T> WithConstructorArgument(Type type, Func<IContext, ITarget, object> callback);

	IBindingWithOrOnSyntax<T> WithPropertyValue(string name, object value);

	IBindingWithOrOnSyntax<T> WithPropertyValue(string name, Func<IContext, object> callback);

	IBindingWithOrOnSyntax<T> WithPropertyValue(string name, Func<IContext, ITarget, object> callback);

	IBindingWithOrOnSyntax<T> WithParameter(IParameter parameter);

	IBindingWithOrOnSyntax<T> WithMetadata(string key, object value);
}
