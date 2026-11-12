using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration;

public sealed class ConfigurationManager : IConfigurationBuilder, IConfigurationRoot, IConfiguration, IDisposable
{
	private class ConfigurationSources : IList<IConfigurationSource>, ICollection<IConfigurationSource>, IEnumerable<IConfigurationSource>, IEnumerable
	{
		private readonly List<IConfigurationSource> _sources = new List<IConfigurationSource>();

		private readonly ConfigurationManager _config;

		public IConfigurationSource this[int index]
		{
			get
			{
				return _sources[index];
			}
			set
			{
				_sources[index] = value;
				_config.ReloadSources();
			}
		}

		public int Count => _sources.Count;

		public bool IsReadOnly => false;

		public ConfigurationSources(ConfigurationManager config)
		{
			_config = config;
		}

		public void Add(IConfigurationSource source)
		{
			_sources.Add(source);
			_config.AddSource(source);
		}

		public void Clear()
		{
			_sources.Clear();
			_config.ReloadSources();
		}

		public bool Contains(IConfigurationSource source)
		{
			return _sources.Contains(source);
		}

		public void CopyTo(IConfigurationSource[] array, int arrayIndex)
		{
			_sources.CopyTo(array, arrayIndex);
		}

		public IEnumerator<IConfigurationSource> GetEnumerator()
		{
			return _sources.GetEnumerator();
		}

		public int IndexOf(IConfigurationSource source)
		{
			return _sources.IndexOf(source);
		}

		public void Insert(int index, IConfigurationSource source)
		{
			_sources.Insert(index, source);
			_config.ReloadSources();
		}

		public bool Remove(IConfigurationSource source)
		{
			bool result = _sources.Remove(source);
			_config.ReloadSources();
			return result;
		}

		public void RemoveAt(int index)
		{
			_sources.RemoveAt(index);
			_config.ReloadSources();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private class ConfigurationBuilderProperties : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

		private readonly ConfigurationManager _config;

		public object this[string key]
		{
			get
			{
				return _properties[key];
			}
			set
			{
				_properties[key] = value;
				_config.ReloadSources();
			}
		}

		public ICollection<string> Keys => _properties.Keys;

		public ICollection<object> Values => _properties.Values;

		public int Count => _properties.Count;

		public bool IsReadOnly => false;

		public ConfigurationBuilderProperties(ConfigurationManager config)
		{
			_config = config;
		}

		public void Add(string key, object value)
		{
			_properties.Add(key, value);
			_config.ReloadSources();
		}

		public void Add(KeyValuePair<string, object> item)
		{
			((ICollection<KeyValuePair<string, object>>)_properties).Add(item);
			_config.ReloadSources();
		}

		public void Clear()
		{
			_properties.Clear();
			_config.ReloadSources();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return _properties.Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return _properties.ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, object>>)_properties).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return _properties.GetEnumerator();
		}

		public bool Remove(string key)
		{
			bool result = _properties.Remove(key);
			_config.ReloadSources();
			return result;
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			bool result = ((ICollection<KeyValuePair<string, object>>)_properties).Remove(item);
			_config.ReloadSources();
			return result;
		}

		public bool TryGetValue(string key, out object value)
		{
			return _properties.TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _properties.GetEnumerator();
		}
	}

	private readonly ConfigurationSources _sources;

	private readonly ConfigurationBuilderProperties _properties;

	private readonly object _providerLock = new object();

	private readonly List<IConfigurationProvider> _providers = new List<IConfigurationProvider>();

	private readonly List<IDisposable> _changeTokenRegistrations = new List<IDisposable>();

	private ConfigurationReloadToken _changeToken = new ConfigurationReloadToken();

	public string this[string key]
	{
		get
		{
			lock (_providerLock)
			{
				return ConfigurationRoot.GetConfiguration(_providers, key);
			}
		}
		set
		{
			lock (_providerLock)
			{
				ConfigurationRoot.SetConfiguration(_providers, key, value);
			}
		}
	}

	IDictionary<string, object> IConfigurationBuilder.Properties => _properties;

	IList<IConfigurationSource> IConfigurationBuilder.Sources => _sources;

	IEnumerable<IConfigurationProvider> IConfigurationRoot.Providers
	{
		get
		{
			lock (_providerLock)
			{
				return new List<IConfigurationProvider>(_providers);
			}
		}
	}

	public ConfigurationManager()
	{
		_sources = new ConfigurationSources(this);
		_properties = new ConfigurationBuilderProperties(this);
		this.AddInMemoryCollection();
		AddSource(_sources[0]);
	}

	public IConfigurationSection GetSection(string key)
	{
		return new ConfigurationSection(this, key);
	}

	public IEnumerable<IConfigurationSection> GetChildren()
	{
		lock (_providerLock)
		{
			return this.GetChildrenImplementation(null).ToList();
		}
	}

	public void Dispose()
	{
		lock (_providerLock)
		{
			DisposeRegistrationsAndProvidersUnsynchronized();
		}
	}

	IConfigurationBuilder IConfigurationBuilder.Add(IConfigurationSource source)
	{
		_sources.Add(source ?? throw new ArgumentNullException("source"));
		return this;
	}

	IConfigurationRoot IConfigurationBuilder.Build()
	{
		return this;
	}

	IChangeToken IConfiguration.GetReloadToken()
	{
		return _changeToken;
	}

	void IConfigurationRoot.Reload()
	{
		lock (_providerLock)
		{
			foreach (IConfigurationProvider provider in _providers)
			{
				provider.Load();
			}
		}
		RaiseChanged();
	}

	private void RaiseChanged()
	{
		ConfigurationReloadToken configurationReloadToken = Interlocked.Exchange(ref _changeToken, new ConfigurationReloadToken());
		configurationReloadToken.OnReload();
	}

	private void AddSource(IConfigurationSource source)
	{
		lock (_providerLock)
		{
			IConfigurationProvider provider = source.Build(this);
			_providers.Add(provider);
			provider.Load();
			_changeTokenRegistrations.Add(ChangeToken.OnChange(() => provider.GetReloadToken(), delegate
			{
				RaiseChanged();
			}));
		}
		RaiseChanged();
	}

	private void ReloadSources()
	{
		lock (_providerLock)
		{
			DisposeRegistrationsAndProvidersUnsynchronized();
			_changeTokenRegistrations.Clear();
			_providers.Clear();
			foreach (IConfigurationSource source in _sources)
			{
				_providers.Add(source.Build(this));
			}
			foreach (IConfigurationProvider p in _providers)
			{
				p.Load();
				_changeTokenRegistrations.Add(ChangeToken.OnChange(() => p.GetReloadToken(), delegate
				{
					RaiseChanged();
				}));
			}
		}
		RaiseChanged();
	}

	private void DisposeRegistrationsAndProvidersUnsynchronized()
	{
		foreach (IDisposable changeTokenRegistration in _changeTokenRegistrations)
		{
			changeTokenRegistration.Dispose();
		}
		foreach (IConfigurationProvider provider in _providers)
		{
			(provider as IDisposable)?.Dispose();
		}
	}
}
