using System;
using System.Collections.Generic;
using Ninject.Activation;
using Ninject.Infrastructure;

namespace Ninject;

public class NinjectSettings : INinjectSettings
{
	private readonly Dictionary<string, object> values = new Dictionary<string, object>();

	public Type InjectAttribute
	{
		get
		{
			return Get("InjectAttribute", typeof(InjectAttribute));
		}
		set
		{
			Set("InjectAttribute", value);
		}
	}

	public TimeSpan CachePruningInterval
	{
		get
		{
			return Get("CachePruningInterval", TimeSpan.FromSeconds(30.0));
		}
		set
		{
			Set("CachePruningInterval", value);
		}
	}

	public Func<IContext, object> DefaultScopeCallback
	{
		get
		{
			return Get("DefaultScopeCallback", StandardScopeCallbacks.Transient);
		}
		set
		{
			Set("DefaultScopeCallback", value);
		}
	}

	public bool LoadExtensions
	{
		get
		{
			return Get("LoadExtensions", defaultValue: true);
		}
		set
		{
			Set("LoadExtensions", value);
		}
	}

	public string[] ExtensionSearchPatterns
	{
		get
		{
			return Get("ExtensionSearchPatterns", new string[2] { "Ninject.Extensions.*.dll", "Ninject.Web*.dll" });
		}
		set
		{
			Set("ExtensionSearchPatterns", value);
		}
	}

	public bool UseReflectionBasedInjection
	{
		get
		{
			return Get("UseReflectionBasedInjection", defaultValue: false);
		}
		set
		{
			Set("UseReflectionBasedInjection", value);
		}
	}

	public bool InjectNonPublic
	{
		get
		{
			return Get("InjectNonPublic", defaultValue: false);
		}
		set
		{
			Set("InjectNonPublic", value);
		}
	}

	public bool InjectParentPrivateProperties
	{
		get
		{
			return Get("InjectParentPrivateProperties", defaultValue: false);
		}
		set
		{
			Set("InjectParentPrivateProperties", value);
		}
	}

	public bool ActivationCacheDisabled
	{
		get
		{
			return Get("ActivationCacheDisabled", defaultValue: false);
		}
		set
		{
			Set("ActivationCacheDisabled", value);
		}
	}

	public bool AllowNullInjection
	{
		get
		{
			return Get("AllowNullInjection", defaultValue: false);
		}
		set
		{
			Set("AllowNullInjection", value);
		}
	}

	public bool ThrowOnGetServiceNotFound
	{
		get
		{
			return Get("ThrowOnGetServiceNotFound", defaultValue: false);
		}
		set
		{
			Set("ThrowOnGetServiceNotFound", value);
		}
	}

	public T Get<T>(string key, T defaultValue)
	{
		if (!values.TryGetValue(key, out var value))
		{
			return defaultValue;
		}
		return (T)value;
	}

	public void Set(string key, object value)
	{
		values[key] = value;
	}
}
