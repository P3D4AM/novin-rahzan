using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Disposal;
using Ninject.Infrastructure.Introspection;
using Ninject.Infrastructure.Language;

namespace Ninject.Components;

public class ComponentContainer : DisposableObject, IComponentContainer, IDisposable
{
	private readonly Multimap<Type, Type> mappings = new Multimap<Type, Type>();

	private readonly Dictionary<Type, INinjectComponent> instances = new Dictionary<Type, INinjectComponent>();

	private readonly HashSet<KeyValuePair<Type, Type>> transients = new HashSet<KeyValuePair<Type, Type>>();

	public IKernel Kernel { get; set; }

	public override void Dispose(bool disposing)
	{
		if (disposing && !base.IsDisposed)
		{
			foreach (INinjectComponent value in instances.Values)
			{
				value.Dispose();
			}
			mappings.Clear();
			instances.Clear();
		}
		base.Dispose(disposing);
	}

	public void Add<TComponent, TImplementation>() where TComponent : INinjectComponent where TImplementation : TComponent, INinjectComponent
	{
		mappings.Add(typeof(TComponent), typeof(TImplementation));
	}

	public void AddTransient<TComponent, TImplementation>() where TComponent : INinjectComponent where TImplementation : TComponent, INinjectComponent
	{
		Add<TComponent, TImplementation>();
		transients.Add(new KeyValuePair<Type, Type>(typeof(TComponent), typeof(TImplementation)));
	}

	public void RemoveAll<T>() where T : INinjectComponent
	{
		RemoveAll(typeof(T));
	}

	public void Remove<T, TImplementation>() where T : INinjectComponent where TImplementation : T
	{
		Type typeFromHandle = typeof(TImplementation);
		if (instances.ContainsKey(typeFromHandle))
		{
			instances[typeFromHandle].Dispose();
		}
		instances.Remove(typeFromHandle);
		mappings.Remove(typeof(T), typeof(TImplementation));
	}

	public void RemoveAll(Type component)
	{
		Ensure.ArgumentNotNull(component, "component");
		foreach (Type item in mappings[component])
		{
			if (instances.ContainsKey(item))
			{
				instances[item].Dispose();
			}
			instances.Remove(item);
		}
		mappings.RemoveAll(component);
	}

	public T Get<T>() where T : INinjectComponent
	{
		return (T)Get(typeof(T));
	}

	public IEnumerable<T> GetAll<T>() where T : INinjectComponent
	{
		return GetAll(typeof(T)).Cast<T>();
	}

	public object Get(Type component)
	{
		Ensure.ArgumentNotNull(component, "component");
		if (component == typeof(IKernel))
		{
			return Kernel;
		}
		if (component.IsGenericType)
		{
			Type genericTypeDefinition = component.GetGenericTypeDefinition();
			Type type = component.GenericTypeArguments[0];
			if (genericTypeDefinition.IsInterface && typeof(IEnumerable<>).IsAssignableFrom(genericTypeDefinition))
			{
				return GetAll(type).CastSlow(type);
			}
		}
		Type type2 = mappings[component].FirstOrDefault();
		if (type2 == null)
		{
			throw new InvalidOperationException(ExceptionFormatter.NoSuchComponentRegistered(component));
		}
		return ResolveInstance(component, type2);
	}

	public IEnumerable<object> GetAll(Type component)
	{
		Ensure.ArgumentNotNull(component, "component");
		return mappings[component].Select((Type implementation) => ResolveInstance(component, implementation));
	}

	private static ConstructorInfo SelectConstructor(Type component, Type implementation)
	{
		ConstructorInfo? constructorInfo = (from c in implementation.GetConstructors()
			orderby c.GetParameters().Length descending
			select c).FirstOrDefault();
		if (constructorInfo == null)
		{
			throw new InvalidOperationException(ExceptionFormatter.NoConstructorsAvailableForComponent(component, implementation));
		}
		return constructorInfo;
	}

	private object ResolveInstance(Type component, Type implementation)
	{
		lock (instances)
		{
			return instances.ContainsKey(implementation) ? instances[implementation] : CreateNewInstance(component, implementation);
		}
	}

	private object CreateNewInstance(Type component, Type implementation)
	{
		ConstructorInfo constructorInfo = SelectConstructor(component, implementation);
		object[] parameters = (from parameter in constructorInfo.GetParameters()
			select Get(parameter.ParameterType)).ToArray();
		try
		{
			INinjectComponent ninjectComponent = constructorInfo.Invoke(parameters) as INinjectComponent;
			ninjectComponent.Settings = Kernel.Settings;
			if (!transients.Contains(new KeyValuePair<Type, Type>(component, implementation)))
			{
				instances.Add(implementation, ninjectComponent);
			}
			return ninjectComponent;
		}
		catch (TargetInvocationException exception)
		{
			exception.RethrowInnerException();
			return null;
		}
	}
}
