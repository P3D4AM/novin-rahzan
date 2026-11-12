using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration;

public abstract class ConfigurationProvider : IConfigurationProvider
{
	private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

	protected IDictionary<string, string> Data { get; set; }

	protected ConfigurationProvider()
	{
		Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	}

	public virtual bool TryGet(string key, out string value)
	{
		return Data.TryGetValue(key, out value);
	}

	public virtual void Set(string key, string value)
	{
		Data[key] = value;
	}

	public virtual void Load()
	{
	}

	public virtual IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
	{
		List<string> list = new List<string>();
		if (parentPath == null)
		{
			foreach (KeyValuePair<string, string> datum in Data)
			{
				list.Add(Segment(datum.Key, 0));
			}
		}
		else
		{
			foreach (KeyValuePair<string, string> datum2 in Data)
			{
				if (datum2.Key.Length > parentPath.Length && datum2.Key.StartsWith(parentPath, StringComparison.OrdinalIgnoreCase) && datum2.Key[parentPath.Length] == ':')
				{
					list.Add(Segment(datum2.Key, parentPath.Length + 1));
				}
			}
		}
		list.AddRange(earlierKeys);
		list.Sort(ConfigurationKeyComparer.Comparison);
		return list;
	}

	private static string Segment(string key, int prefixLength)
	{
		int num = key.IndexOf(ConfigurationPath.KeyDelimiter, prefixLength, StringComparison.OrdinalIgnoreCase);
		if (num >= 0)
		{
			return key.Substring(prefixLength, num - prefixLength);
		}
		return key.Substring(prefixLength);
	}

	public IChangeToken GetReloadToken()
	{
		return _reloadToken;
	}

	protected void OnReload()
	{
		ConfigurationReloadToken configurationReloadToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
		configurationReloadToken.OnReload();
	}

	public override string ToString()
	{
		return GetType().Name ?? "";
	}
}
