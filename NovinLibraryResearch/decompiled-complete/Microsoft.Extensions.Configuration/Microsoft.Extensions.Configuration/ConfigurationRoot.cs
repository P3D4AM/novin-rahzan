using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration;

public class ConfigurationRoot : IConfigurationRoot, IConfiguration, IDisposable
{
	private readonly IList<IConfigurationProvider> _providers;

	private readonly IList<IDisposable> _changeTokenRegistrations;

	private ConfigurationReloadToken _changeToken = new ConfigurationReloadToken();

	public IEnumerable<IConfigurationProvider> Providers => _providers;

	public string this[string key]
	{
		get
		{
			return GetConfiguration(_providers, key);
		}
		set
		{
			SetConfiguration(_providers, key, value);
		}
	}

	public ConfigurationRoot(IList<IConfigurationProvider> providers)
	{
		if (providers == null)
		{
			throw new ArgumentNullException("providers");
		}
		_providers = providers;
		_changeTokenRegistrations = new List<IDisposable>(providers.Count);
		foreach (IConfigurationProvider p in providers)
		{
			p.Load();
			_changeTokenRegistrations.Add(ChangeToken.OnChange(() => p.GetReloadToken(), delegate
			{
				RaiseChanged();
			}));
		}
	}

	public IEnumerable<IConfigurationSection> GetChildren()
	{
		return this.GetChildrenImplementation(null);
	}

	public IChangeToken GetReloadToken()
	{
		return _changeToken;
	}

	public IConfigurationSection GetSection(string key)
	{
		return new ConfigurationSection(this, key);
	}

	public void Reload()
	{
		foreach (IConfigurationProvider provider in _providers)
		{
			provider.Load();
		}
		RaiseChanged();
	}

	private void RaiseChanged()
	{
		ConfigurationReloadToken configurationReloadToken = Interlocked.Exchange(ref _changeToken, new ConfigurationReloadToken());
		configurationReloadToken.OnReload();
	}

	public void Dispose()
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

	internal static string GetConfiguration(IList<IConfigurationProvider> providers, string key)
	{
		for (int num = providers.Count - 1; num >= 0; num--)
		{
			IConfigurationProvider configurationProvider = providers[num];
			if (configurationProvider.TryGet(key, out string value))
			{
				return value;
			}
		}
		return null;
	}

	internal static void SetConfiguration(IList<IConfigurationProvider> providers, string key, string value)
	{
		if (providers.Count == 0)
		{
			throw new InvalidOperationException(System.SR.Error_NoSources);
		}
		foreach (IConfigurationProvider provider in providers)
		{
			provider.Set(key, value);
		}
	}
}
