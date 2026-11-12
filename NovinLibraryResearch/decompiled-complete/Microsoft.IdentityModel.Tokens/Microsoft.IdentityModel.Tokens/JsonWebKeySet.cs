using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

[JsonObject]
public class JsonWebKeySet
{
	[DefaultValue(true)]
	public static bool DefaultSkipUnresolvedJsonWebKeys = true;

	[JsonExtensionData]
	public virtual IDictionary<string, object> AdditionalData { get; } = new Dictionary<string, object>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "keys", Required = Required.Default)]
	public IList<JsonWebKey> Keys { get; private set; } = new List<JsonWebKey>();


	[DefaultValue(true)]
	public bool SkipUnresolvedJsonWebKeys { get; set; } = DefaultSkipUnresolvedJsonWebKeys;


	public static JsonWebKeySet Create(string json)
	{
		if (string.IsNullOrEmpty(json))
		{
			throw LogHelper.LogArgumentNullException("json");
		}
		return new JsonWebKeySet(json);
	}

	public JsonWebKeySet()
	{
	}

	public JsonWebKeySet(string json)
	{
		if (string.IsNullOrEmpty(json))
		{
			throw LogHelper.LogArgumentNullException("json");
		}
		try
		{
			LogHelper.LogVerbose("IDX10806: Deserializing json: '{0}' into '{1}'.", json, this);
			JsonConvert.PopulateObject(json, this);
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10805: Error deserializing json: '{0}' into '{1}'.", json, GetType()), innerException));
		}
	}

	public IList<SecurityKey> GetSigningKeys()
	{
		List<SecurityKey> list = new List<SecurityKey>();
		foreach (JsonWebKey key4 in Keys)
		{
			if (!string.IsNullOrEmpty(key4.Use) && !key4.Use.Equals("sig", StringComparison.Ordinal))
			{
				LogHelper.LogInformation(LogHelper.FormatInvariant("IDX10808: The 'use' parameter of a JsonWebKey: '{0}' was expected to be 'sig' or empty, but was '{1}'.", key4, key4.Use));
				if (!SkipUnresolvedJsonWebKeys)
				{
					list.Add(key4);
				}
			}
			else if ("RSA".Equals(key4.Kty, StringComparison.Ordinal))
			{
				bool flag = true;
				if ((key4.X5c == null || key4.X5c.Count == 0) && string.IsNullOrEmpty(key4.E) && string.IsNullOrEmpty(key4.N))
				{
					flag = false;
				}
				else
				{
					if (key4.X5c != null && key4.X5c.Count != 0)
					{
						if (JsonWebKeyConverter.TryConvertToX509SecurityKey(key4, out var key))
						{
							list.Add(key);
						}
						else
						{
							flag = false;
						}
					}
					if (!string.IsNullOrEmpty(key4.E) && !string.IsNullOrEmpty(key4.N))
					{
						if (JsonWebKeyConverter.TryCreateToRsaSecurityKey(key4, out var key2))
						{
							list.Add(key2);
						}
						else
						{
							flag = false;
						}
					}
				}
				if (!flag && !SkipUnresolvedJsonWebKeys)
				{
					list.Add(key4);
				}
			}
			else if ("EC".Equals(key4.Kty, StringComparison.Ordinal))
			{
				if (JsonWebKeyConverter.TryConvertToECDsaSecurityKey(key4, out var key3))
				{
					list.Add(key3);
				}
				else if (!SkipUnresolvedJsonWebKeys)
				{
					list.Add(key4);
				}
			}
			else
			{
				LogHelper.LogInformation(LogHelper.FormatInvariant("IDX10810: Unable to convert the JsonWebKey: '{0}' to a X509SecurityKey, RsaSecurityKey or ECDSASecurityKey.", key4));
				if (!SkipUnresolvedJsonWebKeys)
				{
					list.Add(key4);
				}
			}
		}
		return list;
	}
}
