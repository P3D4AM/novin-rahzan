using System;
using System.Linq;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Disposal;
using Ninject.Infrastructure.Introspection;
using Ninject.Planning.Bindings;

namespace Ninject.Syntax;

public abstract class BindingRoot : DisposableObject, IBindingRoot, IFluentSyntax
{
	protected abstract IKernel KernelInstance { get; }

	public IBindingToSyntax<T> Bind<T>()
	{
		Type typeFromHandle = typeof(T);
		Binding binding = new Binding(typeFromHandle);
		AddBinding(binding);
		return new BindingBuilder<T>(binding, KernelInstance, typeFromHandle.Format());
	}

	public IBindingToSyntax<T1, T2> Bind<T1, T2>()
	{
		Binding binding = new Binding(typeof(T1));
		AddBinding(binding);
		AddBinding(new Binding(typeof(T2), binding.BindingConfiguration));
		string[] value = new string[2]
		{
			typeof(T1).Format(),
			typeof(T2).Format()
		};
		return new BindingBuilder<T1, T2>(binding.BindingConfiguration, KernelInstance, string.Join(", ", value));
	}

	public IBindingToSyntax<T1, T2, T3> Bind<T1, T2, T3>()
	{
		Binding binding = new Binding(typeof(T1));
		AddBinding(binding);
		AddBinding(new Binding(typeof(T2), binding.BindingConfiguration));
		AddBinding(new Binding(typeof(T3), binding.BindingConfiguration));
		string[] value = new string[3]
		{
			typeof(T1).Format(),
			typeof(T2).Format(),
			typeof(T3).Format()
		};
		return new BindingBuilder<T1, T2, T3>(binding.BindingConfiguration, KernelInstance, string.Join(", ", value));
	}

	public IBindingToSyntax<T1, T2, T3, T4> Bind<T1, T2, T3, T4>()
	{
		Binding binding = new Binding(typeof(T1));
		AddBinding(binding);
		AddBinding(new Binding(typeof(T2), binding.BindingConfiguration));
		AddBinding(new Binding(typeof(T3), binding.BindingConfiguration));
		AddBinding(new Binding(typeof(T4), binding.BindingConfiguration));
		string[] value = new string[4]
		{
			typeof(T1).Format(),
			typeof(T2).Format(),
			typeof(T3).Format(),
			typeof(T4).Format()
		};
		return new BindingBuilder<T1, T2, T3, T4>(binding.BindingConfiguration, KernelInstance, string.Join(", ", value));
	}

	public IBindingToSyntax<object> Bind(params Type[] services)
	{
		Ensure.ArgumentNotNull(services, "service");
		if (services.Length == 0)
		{
			throw new ArgumentException("The services must contain at least one type", "services");
		}
		Binding binding = new Binding(services[0]);
		AddBinding(binding);
		foreach (Type item in services.Skip(1))
		{
			AddBinding(new Binding(item, binding.BindingConfiguration));
		}
		return new BindingBuilder<object>(binding, KernelInstance, string.Join(", ", services.Select((Type service) => service.Format()).ToArray()));
	}

	public void Unbind<T>()
	{
		Unbind(typeof(T));
	}

	public abstract void Unbind(Type service);

	public IBindingToSyntax<T1> Rebind<T1>()
	{
		Unbind<T1>();
		return Bind<T1>();
	}

	public IBindingToSyntax<T1, T2> Rebind<T1, T2>()
	{
		Unbind<T1>();
		Unbind<T2>();
		return Bind<T1, T2>();
	}

	public IBindingToSyntax<T1, T2, T3> Rebind<T1, T2, T3>()
	{
		Unbind<T1>();
		Unbind<T2>();
		Unbind<T3>();
		return Bind<T1, T2, T3>();
	}

	public IBindingToSyntax<T1, T2, T3, T4> Rebind<T1, T2, T3, T4>()
	{
		Unbind<T1>();
		Unbind<T2>();
		Unbind<T3>();
		Unbind<T4>();
		return Bind<T1, T2, T3, T4>();
	}

	public IBindingToSyntax<object> Rebind(params Type[] services)
	{
		foreach (Type service in services)
		{
			Unbind(service);
		}
		return Bind(services);
	}

	public abstract void AddBinding(IBinding binding);

	public abstract void RemoveBinding(IBinding binding);

	Type IFluentSyntax.GetType()
	{
		return GetType();
	}
}
