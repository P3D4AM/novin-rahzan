using System;
using Ninject.Activation;
using Ninject.Infrastructure;

namespace Ninject.Syntax;

public interface IBindingWhenSyntax<T> : IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	IBindingInNamedWithOrOnSyntax<T> When(Func<IRequest, bool> condition);

	IBindingInNamedWithOrOnSyntax<T> WhenInjectedInto<TParent>();

	IBindingInNamedWithOrOnSyntax<T> WhenInjectedInto(Type parent);

	IBindingInNamedWithOrOnSyntax<T> WhenInjectedInto(params Type[] parents);

	IBindingInNamedWithOrOnSyntax<T> WhenInjectedExactlyInto<TParent>();

	IBindingInNamedWithOrOnSyntax<T> WhenInjectedExactlyInto(Type parent);

	IBindingInNamedWithOrOnSyntax<T> WhenInjectedExactlyInto(params Type[] parents);

	IBindingInNamedWithOrOnSyntax<T> WhenClassHas<TAttribute>() where TAttribute : Attribute;

	IBindingInNamedWithOrOnSyntax<T> WhenMemberHas<TAttribute>() where TAttribute : Attribute;

	IBindingInNamedWithOrOnSyntax<T> WhenTargetHas<TAttribute>() where TAttribute : Attribute;

	IBindingInNamedWithOrOnSyntax<T> WhenClassHas(Type attributeType);

	IBindingInNamedWithOrOnSyntax<T> WhenMemberHas(Type attributeType);

	IBindingInNamedWithOrOnSyntax<T> WhenTargetHas(Type attributeType);

	IBindingInNamedWithOrOnSyntax<T> WhenParentNamed(string name);

	[Obsolete("Use WhenAnyAncestorNamed(string name)")]
	IBindingInNamedWithOrOnSyntax<T> WhenAnyAnchestorNamed(string name);

	IBindingInNamedWithOrOnSyntax<T> WhenAnyAncestorNamed(string name);

	IBindingInNamedWithOrOnSyntax<T> WhenNoAncestorNamed(string name);

	IBindingInNamedWithOrOnSyntax<T> WhenAnyAncestorMatches(Predicate<IContext> predicate);

	IBindingInNamedWithOrOnSyntax<T> WhenNoAncestorMatches(Predicate<IContext> predicate);
}
