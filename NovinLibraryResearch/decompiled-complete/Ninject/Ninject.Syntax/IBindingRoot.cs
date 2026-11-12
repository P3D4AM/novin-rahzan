using System;
using Ninject.Planning.Bindings;

namespace Ninject.Syntax;

public interface IBindingRoot : IFluentSyntax
{
	IBindingToSyntax<T> Bind<T>();

	IBindingToSyntax<T1, T2> Bind<T1, T2>();

	IBindingToSyntax<T1, T2, T3> Bind<T1, T2, T3>();

	IBindingToSyntax<T1, T2, T3, T4> Bind<T1, T2, T3, T4>();

	IBindingToSyntax<object> Bind(params Type[] services);

	void Unbind<T>();

	void Unbind(Type service);

	IBindingToSyntax<T1> Rebind<T1>();

	IBindingToSyntax<T1, T2> Rebind<T1, T2>();

	IBindingToSyntax<T1, T2, T3> Rebind<T1, T2, T3>();

	IBindingToSyntax<T1, T2, T3, T4> Rebind<T1, T2, T3, T4>();

	IBindingToSyntax<object> Rebind(params Type[] services);

	void AddBinding(IBinding binding);

	void RemoveBinding(IBinding binding);
}
