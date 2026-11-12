using System;
using System.Linq;
using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Introspection;
using Ninject.Infrastructure.Language;
using Ninject.Parameters;
using Ninject.Planning.Targets;
using Ninject.Syntax;

namespace Ninject.Planning.Bindings;

public class BindingConfigurationBuilder<T> : IBindingConfigurationSyntax<T>, IBindingWhenInNamedWithOrOnSyntax<T>, IBindingWhenSyntax<T>, IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax, IBindingInSyntax<T>, IBindingNamedSyntax<T>, IBindingWithSyntax<T>, IBindingOnSyntax<T>, IBindingInNamedWithOrOnSyntax<T>, IBindingNamedWithOrOnSyntax<T>, IBindingWithOrOnSyntax<T>
{
	private readonly string serviceNames;

	public IBindingConfiguration BindingConfiguration { get; private set; }

	public IKernel Kernel { get; private set; }

	public BindingConfigurationBuilder(IBindingConfiguration bindingConfiguration, string serviceNames, IKernel kernel)
	{
		Ensure.ArgumentNotNull(bindingConfiguration, "bindingConfiguration");
		Ensure.ArgumentNotNull(kernel, "kernel");
		BindingConfiguration = bindingConfiguration;
		Kernel = kernel;
		this.serviceNames = serviceNames;
	}

	public IBindingInNamedWithOrOnSyntax<T> When(Func<IRequest, bool> condition)
	{
		BindingConfiguration.Condition = condition;
		return this;
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenInjectedInto<TParent>()
	{
		return WhenInjectedInto(typeof(TParent));
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenInjectedInto(Type parent)
	{
		if (parent.IsGenericTypeDefinition)
		{
			if (parent.IsInterface)
			{
				BindingConfiguration.Condition = (IRequest r) => r.Target != null && r.Target.Member.ReflectedType.GetInterfaces().Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == parent);
			}
			else
			{
				BindingConfiguration.Condition = (IRequest r) => r.Target != null && r.Target.Member.ReflectedType.GetAllBaseTypes().Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == parent);
			}
		}
		else
		{
			BindingConfiguration.Condition = (IRequest r) => r.Target != null && parent.IsAssignableFrom(r.Target.Member.ReflectedType);
		}
		return this;
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenInjectedInto(params Type[] parents)
	{
		BindingConfiguration.Condition = delegate(IRequest r)
		{
			Type[] array = parents;
			foreach (Type parent in array)
			{
				bool flag = false;
				if ((!parent.IsGenericTypeDefinition) ? (r.Target != null && parent.IsAssignableFrom(r.Target.Member.ReflectedType)) : ((!parent.IsInterface) ? (r.Target != null && r.Target.Member.ReflectedType.GetAllBaseTypes().Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == parent)) : (r.Target != null && r.Target.Member.ReflectedType.GetInterfaces().Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == parent))))
				{
					return true;
				}
			}
			return false;
		};
		return this;
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenInjectedExactlyInto<TParent>()
	{
		return WhenInjectedExactlyInto(typeof(TParent));
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenInjectedExactlyInto(Type parent)
	{
		if (parent.IsGenericTypeDefinition)
		{
			BindingConfiguration.Condition = (IRequest r) => r.Target != null && r.Target.Member.ReflectedType.IsGenericType && parent == r.Target.Member.ReflectedType.GetGenericTypeDefinition();
		}
		else
		{
			BindingConfiguration.Condition = (IRequest r) => r.Target != null && r.Target.Member.ReflectedType == parent;
		}
		return this;
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenInjectedExactlyInto(params Type[] parents)
	{
		BindingConfiguration.Condition = delegate(IRequest r)
		{
			Type[] array = parents;
			foreach (Type type in array)
			{
				bool flag = false;
				if ((!type.IsGenericTypeDefinition) ? (r.Target != null && r.Target.Member.ReflectedType == type) : (r.Target != null && r.Target.Member.ReflectedType.IsGenericType && type == r.Target.Member.ReflectedType.GetGenericTypeDefinition()))
				{
					return true;
				}
			}
			return false;
		};
		return this;
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenClassHas<TAttribute>() where TAttribute : Attribute
	{
		return WhenClassHas(typeof(TAttribute));
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenMemberHas<TAttribute>() where TAttribute : Attribute
	{
		return WhenMemberHas(typeof(TAttribute));
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenTargetHas<TAttribute>() where TAttribute : Attribute
	{
		return WhenTargetHas(typeof(TAttribute));
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenClassHas(Type attributeType)
	{
		if (!typeof(Attribute).IsAssignableFrom(attributeType))
		{
			throw new InvalidOperationException(ExceptionFormatter.InvalidAttributeTypeUsedInBindingCondition(serviceNames, "WhenClassHas", attributeType));
		}
		BindingConfiguration.Condition = (IRequest r) => r.Target != null && r.Target.Member.ReflectedType.HasAttribute(attributeType);
		return this;
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenMemberHas(Type attributeType)
	{
		if (!typeof(Attribute).IsAssignableFrom(attributeType))
		{
			throw new InvalidOperationException(ExceptionFormatter.InvalidAttributeTypeUsedInBindingCondition(serviceNames, "WhenMemberHas", attributeType));
		}
		BindingConfiguration.Condition = (IRequest r) => r.Target != null && r.Target.Member.HasAttribute(attributeType);
		return this;
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenTargetHas(Type attributeType)
	{
		if (!typeof(Attribute).IsAssignableFrom(attributeType))
		{
			throw new InvalidOperationException(ExceptionFormatter.InvalidAttributeTypeUsedInBindingCondition(serviceNames, "WhenTargetHas", attributeType));
		}
		BindingConfiguration.Condition = (IRequest r) => r.Target != null && r.Target.HasAttribute(attributeType);
		return this;
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenParentNamed(string name)
	{
		string.Intern(name);
		BindingConfiguration.Condition = (IRequest r) => r.ParentContext != null && string.Equals(r.ParentContext.Binding.Metadata.Name, name, StringComparison.Ordinal);
		return this;
	}

	[Obsolete("Use WhenAnyAncestorNamed(string name)")]
	public IBindingInNamedWithOrOnSyntax<T> WhenAnyAnchestorNamed(string name)
	{
		return WhenAnyAncestorNamed(name);
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenAnyAncestorNamed(string name)
	{
		return WhenAnyAncestorMatches((IContext ctx) => ctx.Binding.Metadata.Name == name);
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenNoAncestorNamed(string name)
	{
		return WhenNoAncestorMatches((IContext ctx) => ctx.Binding.Metadata.Name == name);
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenAnyAncestorMatches(Predicate<IContext> predicate)
	{
		BindingConfiguration.Condition = (IRequest r) => DoesAnyAncestorMatch(r, predicate);
		return this;
	}

	public IBindingInNamedWithOrOnSyntax<T> WhenNoAncestorMatches(Predicate<IContext> predicate)
	{
		BindingConfiguration.Condition = (IRequest r) => !DoesAnyAncestorMatch(r, predicate);
		return this;
	}

	public IBindingWithOrOnSyntax<T> Named(string name)
	{
		string.Intern(name);
		BindingConfiguration.Metadata.Name = name;
		return this;
	}

	public IBindingNamedWithOrOnSyntax<T> InSingletonScope()
	{
		BindingConfiguration.ScopeCallback = StandardScopeCallbacks.Singleton;
		return this;
	}

	public IBindingNamedWithOrOnSyntax<T> InTransientScope()
	{
		BindingConfiguration.ScopeCallback = StandardScopeCallbacks.Transient;
		return this;
	}

	public IBindingNamedWithOrOnSyntax<T> InThreadScope()
	{
		BindingConfiguration.ScopeCallback = StandardScopeCallbacks.Thread;
		return this;
	}

	public IBindingNamedWithOrOnSyntax<T> InScope(Func<IContext, object> scope)
	{
		BindingConfiguration.ScopeCallback = scope;
		return this;
	}

	public IBindingWithOrOnSyntax<T> WithConstructorArgument(string name, object value)
	{
		BindingConfiguration.Parameters.Add(new ConstructorArgument(name, value));
		return this;
	}

	public IBindingWithOrOnSyntax<T> WithConstructorArgument(string name, Func<IContext, object> callback)
	{
		BindingConfiguration.Parameters.Add(new ConstructorArgument(name, callback));
		return this;
	}

	public IBindingWithOrOnSyntax<T> WithConstructorArgument(string name, Func<IContext, ITarget, object> callback)
	{
		BindingConfiguration.Parameters.Add(new ConstructorArgument(name, callback));
		return this;
	}

	public IBindingWithOrOnSyntax<T> WithConstructorArgument<TValue>(TValue value)
	{
		return WithConstructorArgument(typeof(TValue), (IContext context, ITarget target) => value);
	}

	public IBindingWithOrOnSyntax<T> WithConstructorArgument(Type type, object value)
	{
		return WithConstructorArgument(type, (IContext context, ITarget target) => value);
	}

	public IBindingWithOrOnSyntax<T> WithConstructorArgument(Type type, Func<IContext, object> callback)
	{
		return WithConstructorArgument(type, (IContext context, ITarget target) => callback(context));
	}

	public IBindingWithOrOnSyntax<T> WithConstructorArgument<TValue>(Func<IContext, TValue> callback)
	{
		return WithConstructorArgument(typeof(TValue), (IContext context, ITarget target) => callback(context));
	}

	public IBindingWithOrOnSyntax<T> WithConstructorArgument(Type type, Func<IContext, ITarget, object> callback)
	{
		BindingConfiguration.Parameters.Add(new TypeMatchingConstructorArgument(type, callback));
		return this;
	}

	public IBindingWithOrOnSyntax<T> WithConstructorArgument<TValue>(Func<IContext, ITarget, TValue> callback)
	{
		WithConstructorArgument(typeof(TValue), callback);
		return this;
	}

	public IBindingWithOrOnSyntax<T> WithPropertyValue(string name, object value)
	{
		BindingConfiguration.Parameters.Add(new PropertyValue(name, value));
		return this;
	}

	public IBindingWithOrOnSyntax<T> WithPropertyValue(string name, Func<IContext, object> callback)
	{
		BindingConfiguration.Parameters.Add(new PropertyValue(name, callback));
		return this;
	}

	public IBindingWithOrOnSyntax<T> WithPropertyValue(string name, Func<IContext, ITarget, object> callback)
	{
		BindingConfiguration.Parameters.Add(new PropertyValue(name, callback));
		return this;
	}

	public IBindingWithOrOnSyntax<T> WithParameter(IParameter parameter)
	{
		BindingConfiguration.Parameters.Add(parameter);
		return this;
	}

	public IBindingWithOrOnSyntax<T> WithMetadata(string key, object value)
	{
		BindingConfiguration.Metadata.Set(key, value);
		return this;
	}

	public IBindingOnSyntax<T> OnActivation(Action<T> action)
	{
		return this.OnActivation<T>(action);
	}

	public IBindingOnSyntax<T> OnActivation<TImplementation>(Action<TImplementation> action)
	{
		BindingConfiguration.ActivationActions.Add(delegate(IContext context, object instance)
		{
			action((TImplementation)instance);
		});
		return this;
	}

	public IBindingOnSyntax<T> OnActivation(Action<IContext, T> action)
	{
		return this.OnActivation<T>(action);
	}

	public IBindingOnSyntax<T> OnActivation<TImplementation>(Action<IContext, TImplementation> action)
	{
		BindingConfiguration.ActivationActions.Add(delegate(IContext context, object instance)
		{
			action(context, (TImplementation)instance);
		});
		return this;
	}

	public IBindingOnSyntax<T> OnDeactivation(Action<T> action)
	{
		return this.OnDeactivation<T>(action);
	}

	public IBindingOnSyntax<T> OnDeactivation<TImplementation>(Action<TImplementation> action)
	{
		BindingConfiguration.DeactivationActions.Add(delegate(IContext context, object instance)
		{
			action((TImplementation)instance);
		});
		return this;
	}

	public IBindingOnSyntax<T> OnDeactivation(Action<IContext, T> action)
	{
		return this.OnDeactivation<T>(action);
	}

	public IBindingOnSyntax<T> OnDeactivation<TImplementation>(Action<IContext, TImplementation> action)
	{
		BindingConfiguration.DeactivationActions.Add(delegate(IContext context, object instance)
		{
			action(context, (TImplementation)instance);
		});
		return this;
	}

	private static bool DoesAnyAncestorMatch(IRequest request, Predicate<IContext> predicate)
	{
		IContext parentContext = request.ParentContext;
		if (parentContext == null)
		{
			return false;
		}
		if (!predicate(parentContext))
		{
			return DoesAnyAncestorMatch(parentContext.Request, predicate);
		}
		return true;
	}

	Type IFluentSyntax.GetType()
	{
		return GetType();
	}
}
