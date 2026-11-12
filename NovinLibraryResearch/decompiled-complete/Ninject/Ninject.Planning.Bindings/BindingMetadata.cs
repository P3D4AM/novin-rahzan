using System.Collections.Generic;
using Ninject.Infrastructure;

namespace Ninject.Planning.Bindings;

public class BindingMetadata : IBindingMetadata
{
	private readonly Dictionary<string, object> values = new Dictionary<string, object>();

	public string Name { get; set; }

	public bool Has(string key)
	{
		Ensure.ArgumentNotNullOrEmpty(key, "key");
		return values.ContainsKey(key);
	}

	public T Get<T>(string key)
	{
		Ensure.ArgumentNotNullOrEmpty(key, "key");
		return Get(key, default(T));
	}

	public T Get<T>(string key, T defaultValue)
	{
		Ensure.ArgumentNotNullOrEmpty(key, "key");
		if (!values.ContainsKey(key))
		{
			return defaultValue;
		}
		return (T)values[key];
	}

	public void Set(string key, object value)
	{
		Ensure.ArgumentNotNullOrEmpty(key, "key");
		values[key] = value;
	}
}
