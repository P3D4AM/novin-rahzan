using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration;

public class ChainedConfigurationProvider : IConfigurationProvider, IDisposable
{
	private readonly IConfiguration _config;

	private readonly bool _shouldDisposeConfig;

	public ChainedConfigurationProvider(ChainedConfigurationSource source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (source.Configuration == null)
		{
			throw new ArgumentException(System.SR.Format(System.SR.InvalidNullArgument, "source.Configuration"), "source");
		}
		_config = source.Configuration;
		_shouldDisposeConfig = source.ShouldDisposeConfiguration;
	}

	public bool TryGet(string key, out string value)
	{
		value = _config[key];
		return !string.IsNullOrEmpty(value);
	}

	public void Set(string key, string value)
	{
		_config[key] = value;
	}

	public IChangeToken GetReloadToken()
	{
		return _config.GetReloadToken();
	}

	public void Load()
	{
	}

	public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
	{
		IConfiguration configuration;
		if (parentPath != null)
		{
			IConfiguration section = _config.GetSection(parentPath);
			configuration = section;
		}
		else
		{
			configuration = _config;
		}
		IConfiguration configuration2 = configuration;
		List<string> list = new List<string>();
		foreach (IConfigurationSection child in configuration2.GetChildren())
		{
			list.Add(child.Key);
		}
		list.AddRange(earlierKeys);
		list.Sort(ConfigurationKeyComparer.Comparison);
		return list;
	}

	public void Dispose()
	{
		if (_shouldDisposeConfig)
		{
			(_config as IDisposable)?.Dispose();
		}
	}
}
