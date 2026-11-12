using System;
using System.Linq.Expressions;
using Ninject.Activation;
using Ninject.Infrastructure;

namespace Ninject.Syntax;

public interface IBindingToSyntax<T1, T2, T3, T4> : IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>() where TImplementation : T1, T2, T3, T4;

	IBindingWhenInNamedWithOrOnSyntax<object> To(Type implementation);

	IBindingWhenInNamedWithOrOnSyntax<object> ToProvider<TProvider>() where TProvider : IProvider;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TProvider, TImplementation>() where TProvider : IProvider<TImplementation> where TImplementation : T1, T2, T3, T4;

	IBindingWhenInNamedWithOrOnSyntax<object> ToProvider(Type providerType);

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(IProvider<TImplementation> provider) where TImplementation : T1, T2, T3, T4;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(Func<IContext, TImplementation> method) where TImplementation : T1, T2, T3;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(TImplementation value) where TImplementation : T1, T2, T3, T4;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression) where TImplementation : T1, T2, T3, T4;
}
public interface IBindingToSyntax<T1, T2, T3> : IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>() where TImplementation : T1, T2, T3;

	IBindingWhenInNamedWithOrOnSyntax<object> To(Type implementation);

	IBindingWhenInNamedWithOrOnSyntax<object> ToProvider<TProvider>() where TProvider : IProvider;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TProvider, TImplementation>() where TProvider : IProvider<TImplementation> where TImplementation : T1, T2, T3;

	IBindingWhenInNamedWithOrOnSyntax<object> ToProvider(Type providerType);

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(IProvider<TImplementation> provider) where TImplementation : T1, T2, T3;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(Func<IContext, TImplementation> method) where TImplementation : T1, T2, T3;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(TImplementation value) where TImplementation : T1, T2, T3;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression) where TImplementation : T1, T2, T3;
}
public interface IBindingToSyntax<T1, T2> : IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>() where TImplementation : T1, T2;

	IBindingWhenInNamedWithOrOnSyntax<object> To(Type implementation);

	IBindingWhenInNamedWithOrOnSyntax<object> ToProvider<TProvider>() where TProvider : IProvider;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TProvider, TImplementation>() where TProvider : IProvider<TImplementation> where TImplementation : T1, T2;

	IBindingWhenInNamedWithOrOnSyntax<object> ToProvider(Type providerType);

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(IProvider<TImplementation> provider) where TImplementation : T1, T2;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(Func<IContext, TImplementation> method) where TImplementation : T1, T2;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(TImplementation value) where TImplementation : T1, T2;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression) where TImplementation : T1, T2;
}
public interface IBindingToSyntax<T1> : IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	IBindingWhenInNamedWithOrOnSyntax<T1> ToSelf();

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>() where TImplementation : T1;

	IBindingWhenInNamedWithOrOnSyntax<T1> To(Type implementation);

	IBindingWhenInNamedWithOrOnSyntax<T1> ToProvider<TProvider>() where TProvider : IProvider;

	IBindingWhenInNamedWithOrOnSyntax<T1> ToProvider(Type providerType);

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(IProvider<TImplementation> provider) where TImplementation : T1;

	IBindingWhenInNamedWithOrOnSyntax<T1> ToMethod(Func<IContext, T1> method);

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(Func<IContext, TImplementation> method) where TImplementation : T1;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(TImplementation value) where TImplementation : T1;

	IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression) where TImplementation : T1;
}
