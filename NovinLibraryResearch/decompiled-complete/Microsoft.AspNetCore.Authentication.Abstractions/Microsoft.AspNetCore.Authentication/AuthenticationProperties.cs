using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.AspNetCore.Authentication;

public class AuthenticationProperties
{
	internal const string IssuedUtcKey = ".issued";

	internal const string ExpiresUtcKey = ".expires";

	internal const string IsPersistentKey = ".persistent";

	internal const string RedirectUriKey = ".redirect";

	internal const string RefreshKey = ".refresh";

	internal const string UtcDateTimeFormat = "r";

	public IDictionary<string, string> Items { get; }

	public IDictionary<string, object> Parameters { get; }

	public bool IsPersistent
	{
		get
		{
			return GetString(".persistent") != null;
		}
		set
		{
			SetString(".persistent", value ? string.Empty : null);
		}
	}

	public string RedirectUri
	{
		get
		{
			return GetString(".redirect");
		}
		set
		{
			SetString(".redirect", value);
		}
	}

	public DateTimeOffset? IssuedUtc
	{
		get
		{
			return GetDateTimeOffset(".issued");
		}
		set
		{
			SetDateTimeOffset(".issued", value);
		}
	}

	public DateTimeOffset? ExpiresUtc
	{
		get
		{
			return GetDateTimeOffset(".expires");
		}
		set
		{
			SetDateTimeOffset(".expires", value);
		}
	}

	public bool? AllowRefresh
	{
		get
		{
			return GetBool(".refresh");
		}
		set
		{
			SetBool(".refresh", value);
		}
	}

	public AuthenticationProperties()
		: this(null, null)
	{
	}

	public AuthenticationProperties(IDictionary<string, string> items)
		: this(items, null)
	{
	}

	public AuthenticationProperties(IDictionary<string, string> items, IDictionary<string, object> parameters)
	{
		Items = items ?? new Dictionary<string, string>(StringComparer.Ordinal);
		Parameters = parameters ?? new Dictionary<string, object>(StringComparer.Ordinal);
	}

	public string GetString(string key)
	{
		if (!Items.TryGetValue(key, out var value))
		{
			return null;
		}
		return value;
	}

	public void SetString(string key, string value)
	{
		if (value != null)
		{
			Items[key] = value;
		}
		else if (Items.ContainsKey(key))
		{
			Items.Remove(key);
		}
	}

	public T GetParameter<T>(string key)
	{
		object obj;
		if (Parameters.TryGetValue(key, out var value) && (obj = value) is T)
		{
			return (T)obj;
		}
		return default(T);
	}

	public void SetParameter<T>(string key, T value)
	{
		Parameters[key] = value;
	}

	protected bool? GetBool(string key)
	{
		if (Items.TryGetValue(key, out var value) && bool.TryParse(value, out var result))
		{
			return result;
		}
		return null;
	}

	protected void SetBool(string key, bool? value)
	{
		if (value.HasValue)
		{
			Items[key] = value.Value.ToString();
		}
		else if (Items.ContainsKey(key))
		{
			Items.Remove(key);
		}
	}

	protected DateTimeOffset? GetDateTimeOffset(string key)
	{
		if (Items.TryGetValue(key, out var value) && DateTimeOffset.TryParseExact(value, "r", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var result))
		{
			return result;
		}
		return null;
	}

	protected void SetDateTimeOffset(string key, DateTimeOffset? value)
	{
		if (value.HasValue)
		{
			Items[key] = value.Value.ToString("r", CultureInfo.InvariantCulture);
		}
		else if (Items.ContainsKey(key))
		{
			Items.Remove(key);
		}
	}
}
