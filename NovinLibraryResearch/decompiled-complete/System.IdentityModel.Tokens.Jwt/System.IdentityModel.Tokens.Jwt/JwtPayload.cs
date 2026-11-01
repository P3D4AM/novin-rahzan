using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace System.IdentityModel.Tokens.Jwt;

public class JwtPayload : Dictionary<string, object>
{
	public string Actort => GetStandardClaim("actort");

	public string Acr => GetStandardClaim("acr");

	public IList<string> Amr => GetIListClaims("amr");

	public int? AuthTime => GetIntClaim("auth_time");

	public IList<string> Aud => GetIListClaims("aud");

	public string Azp => GetStandardClaim("azp");

	public string CHash => GetStandardClaim("c_hash");

	public int? Exp => GetIntClaim("exp");

	public string Jti => GetStandardClaim("jti");

	public int? Iat => GetIntClaim("iat");

	public string Iss => GetStandardClaim("iss");

	public int? Nbf => GetIntClaim("nbf");

	public string Nonce => GetStandardClaim("nonce");

	public string Sub => GetStandardClaim("sub");

	public DateTime ValidFrom => GetDateTime("nbf");

	public DateTime ValidTo => GetDateTime("exp");

	public DateTime IssuedAt => GetDateTime("iat");

	public virtual IEnumerable<Claim> Claims
	{
		get
		{
			List<Claim> list = new List<Claim>();
			string text = Iss ?? "LOCAL AUTHORITY";
			using Enumerator enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, object> current = enumerator.Current;
				if (current.Value == null)
				{
					list.Add(new Claim(current.Key, string.Empty, "JSON_NULL", text, text));
					continue;
				}
				if (current.Value is string value)
				{
					list.Add(new Claim(current.Key, value, "http://www.w3.org/2001/XMLSchema#string", text, text));
					continue;
				}
				object value2 = current.Value;
				JToken val = (JToken)((value2 is JToken) ? value2 : null);
				if (val != null)
				{
					AddClaimsFromJToken(list, current.Key, val, text);
				}
				else if (current.Value is IEnumerable<object> enumerable)
				{
					foreach (object item in enumerable)
					{
						if (item is string value3)
						{
							list.Add(new Claim(current.Key, value3, "http://www.w3.org/2001/XMLSchema#string", text, text));
							continue;
						}
						val = (JToken)((item is JToken) ? item : null);
						if (val != null)
						{
							AddDefaultClaimFromJToken(list, current.Key, val, text);
						}
						else if (item is DateTime dateTime)
						{
							list.Add(new Claim(current.Key, dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), "http://www.w3.org/2001/XMLSchema#dateTime", text, text));
						}
						else
						{
							list.Add(new Claim(current.Key, JsonConvert.SerializeObject(item), GetClaimValueType(item), text, text));
						}
					}
				}
				else if (current.Value is IDictionary<string, object> dictionary)
				{
					foreach (KeyValuePair<string, object> item2 in dictionary)
					{
						list.Add(new Claim(current.Key, "{" + item2.Key + ":" + JsonConvert.SerializeObject(item2.Value) + "}", GetClaimValueType(item2.Value), text, text));
					}
				}
				else if (current.Value is DateTime dateTime2)
				{
					list.Add(new Claim(current.Key, dateTime2.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), "http://www.w3.org/2001/XMLSchema#dateTime", text, text));
				}
				else
				{
					list.Add(new Claim(current.Key, JsonConvert.SerializeObject(current.Value), GetClaimValueType(current.Value), text, text));
				}
			}
			return list;
		}
	}

	public JwtPayload()
		: this(null, null, null, null, null)
	{
	}

	public JwtPayload(IEnumerable<Claim> claims)
		: this(null, null, claims, null, null)
	{
	}

	public JwtPayload(string issuer, string audience, IEnumerable<Claim> claims, DateTime? notBefore, DateTime? expires)
		: this(issuer, audience, claims, notBefore, expires, null)
	{
	}

	public JwtPayload(string issuer, string audience, IEnumerable<Claim> claims, DateTime? notBefore, DateTime? expires, DateTime? issuedAt)
		: base((IEqualityComparer<string>?)StringComparer.Ordinal)
	{
		if (claims != null)
		{
			AddClaims(claims);
		}
		AddFirstPriorityClaims(issuer, audience, notBefore, expires, issuedAt);
	}

	public JwtPayload(string issuer, string audience, IEnumerable<Claim> claims, IDictionary<string, object> claimsCollection, DateTime? notBefore, DateTime? expires, DateTime? issuedAt)
		: base((IEqualityComparer<string>?)StringComparer.Ordinal)
	{
		if (claims != null)
		{
			AddClaims(claims);
		}
		if (claimsCollection != null && claimsCollection.Any())
		{
			AddDictionaryClaims(claimsCollection);
		}
		AddFirstPriorityClaims(issuer, audience, notBefore, expires, issuedAt);
	}

	internal void AddFirstPriorityClaims(string issuer, string audience, DateTime? notBefore, DateTime? expires, DateTime? issuedAt)
	{
		if (expires.HasValue)
		{
			if (notBefore.HasValue)
			{
				if (notBefore.Value >= expires.Value)
				{
					throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX12401: Expires: '{0}' must be after NotBefore: '{1}'.", new object[2] { expires.Value, notBefore.Value })));
				}
				base["nbf"] = EpochTime.GetIntDate(notBefore.Value.ToUniversalTime());
			}
			base["exp"] = EpochTime.GetIntDate(expires.Value.ToUniversalTime());
		}
		if (issuedAt.HasValue)
		{
			base["iat"] = EpochTime.GetIntDate(issuedAt.Value.ToUniversalTime());
		}
		if (!string.IsNullOrEmpty(issuer))
		{
			base["iss"] = issuer;
		}
		if (!string.IsNullOrEmpty(audience))
		{
			AddClaim(new Claim("aud", audience, "http://www.w3.org/2001/XMLSchema#string"));
		}
	}

	private static void AddClaimsFromJToken(List<Claim> claims, string claimType, JToken jtoken, string issuer)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		if ((int)jtoken.Type == 1)
		{
			claims.Add(new Claim(claimType, jtoken.ToString((Formatting)0, Array.Empty<JsonConverter>()), "JSON", issuer, issuer));
			return;
		}
		if ((int)jtoken.Type == 2)
		{
			foreach (JToken item in (JArray)((jtoken is JArray) ? jtoken : null))
			{
				JTokenType type = item.Type;
				if ((int)type != 1)
				{
					if ((int)type == 2)
					{
						claims.Add(new Claim(claimType, item.ToString((Formatting)0, Array.Empty<JsonConverter>()), "JSON_ARRAY", issuer, issuer));
					}
					else
					{
						AddDefaultClaimFromJToken(claims, claimType, item, issuer);
					}
				}
				else
				{
					claims.Add(new Claim(claimType, item.ToString((Formatting)0, Array.Empty<JsonConverter>()), "JSON", issuer, issuer));
				}
			}
			return;
		}
		AddDefaultClaimFromJToken(claims, claimType, jtoken, issuer);
	}

	private static void AddDefaultClaimFromJToken(List<Claim> claims, string claimType, JToken jtoken, string issuer)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		JValue val = (JValue)(object)((jtoken is JValue) ? jtoken : null);
		if (val != null)
		{
			if ((int)((JToken)val).Type == 8)
			{
				claims.Add(new Claim(claimType, val.Value.ToString(), "http://www.w3.org/2001/XMLSchema#string", issuer, issuer));
			}
			else if (val.Value is DateTime dateTime)
			{
				claims.Add(new Claim(claimType, dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), "http://www.w3.org/2001/XMLSchema#dateTime", issuer, issuer));
			}
			else
			{
				claims.Add(new Claim(claimType, jtoken.ToString((Formatting)0, Array.Empty<JsonConverter>()), GetClaimValueType(val.Value), issuer, issuer));
			}
		}
		else
		{
			claims.Add(new Claim(claimType, jtoken.ToString((Formatting)0, Array.Empty<JsonConverter>()), GetClaimValueType(jtoken), issuer, issuer));
		}
	}

	public void AddClaim(Claim claim)
	{
		if (claim == null)
		{
			throw LogHelper.LogExceptionMessage((Exception)new ArgumentNullException("claim"));
		}
		AddClaims(new Claim[1] { claim });
	}

	public void AddClaims(IEnumerable<Claim> claims)
	{
		if (claims == null)
		{
			throw LogHelper.LogExceptionMessage((Exception)new ArgumentNullException("claims"));
		}
		foreach (Claim claim in claims)
		{
			if (claim == null)
			{
				continue;
			}
			string type = claim.Type;
			object obj = (claim.ValueType.Equals("http://www.w3.org/2001/XMLSchema#string", StringComparison.Ordinal) ? claim.Value : TokenUtilities.GetClaimValueUsingValueType(claim));
			if (TryGetValue(type, out var value))
			{
				IList<object> list = value as IList<object>;
				if (list == null)
				{
					list = new List<object>();
					list.Add(value);
					base[type] = list;
				}
				list.Add(obj);
			}
			else
			{
				base[type] = obj;
			}
		}
	}

	internal void AddDictionaryClaims(IDictionary<string, object> claimsCollection)
	{
		if (claimsCollection == null)
		{
			throw LogHelper.LogExceptionMessage((Exception)new ArgumentNullException("claimsCollection"));
		}
		foreach (string key in claimsCollection.Keys)
		{
			base[key] = claimsCollection[key];
		}
	}

	internal static string GetClaimValueType(object obj)
	{
		if (obj == null)
		{
			return "JSON_NULL";
		}
		Type type = obj.GetType();
		if (type == typeof(string))
		{
			return "http://www.w3.org/2001/XMLSchema#string";
		}
		if (type == typeof(int))
		{
			return "http://www.w3.org/2001/XMLSchema#integer";
		}
		if (type == typeof(bool))
		{
			return "http://www.w3.org/2001/XMLSchema#boolean";
		}
		if (type == typeof(double))
		{
			return "http://www.w3.org/2001/XMLSchema#double";
		}
		if (type == typeof(long))
		{
			long num = (long)obj;
			if (num >= int.MinValue && num <= int.MaxValue)
			{
				return "http://www.w3.org/2001/XMLSchema#integer";
			}
			return "http://www.w3.org/2001/XMLSchema#integer64";
		}
		if (type == typeof(DateTime))
		{
			return "http://www.w3.org/2001/XMLSchema#dateTime";
		}
		if (type == typeof(JObject))
		{
			return "JSON";
		}
		if (type == typeof(JArray))
		{
			return "JSON_ARRAY";
		}
		return type.ToString();
	}

	internal string GetStandardClaim(string claimType)
	{
		if (TryGetValue(claimType, out var value))
		{
			if (value == null)
			{
				return null;
			}
			if (value is string result)
			{
				return result;
			}
			return JsonExtensions.SerializeToJson(value);
		}
		return null;
	}

	internal int? GetIntClaim(string claimType)
	{
		int? result = null;
		if (TryGetValue(claimType, out var value))
		{
			if (value is IList<object> list)
			{
				foreach (object item in list)
				{
					result = null;
					if (item != null)
					{
						try
						{
							result = Convert.ToInt32(Math.Truncate(Convert.ToDouble(item, CultureInfo.InvariantCulture)));
						}
						catch (FormatException)
						{
							result = null;
						}
						catch (InvalidCastException)
						{
							result = null;
						}
						catch (OverflowException)
						{
							result = null;
						}
						if (result.HasValue)
						{
							return result;
						}
					}
				}
			}
			else
			{
				try
				{
					result = Convert.ToInt32(Math.Truncate(Convert.ToDouble(value, CultureInfo.InvariantCulture)));
				}
				catch (FormatException)
				{
					result = null;
				}
				catch (OverflowException)
				{
					result = null;
				}
			}
			return result;
		}
		return result;
	}

	internal IList<string> GetIListClaims(string claimType)
	{
		List<string> list = new List<string>();
		object value = null;
		if (!TryGetValue(claimType, out value))
		{
			return list;
		}
		if (value is string item)
		{
			list.Add(item);
			return list;
		}
		if (value is IEnumerable<object> enumerable)
		{
			foreach (object item2 in enumerable)
			{
				list.Add(item2.ToString());
			}
		}
		else
		{
			list.Add(JsonExtensions.SerializeToJson(value));
		}
		return list;
	}

	private DateTime GetDateTime(string key)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		if (!TryGetValue(key, out var value))
		{
			return DateTime.MinValue;
		}
		try
		{
			if (value is IList<object> list)
			{
				if (list.Count == 0)
				{
					return DateTime.MinValue;
				}
				value = list[0];
			}
			return EpochTime.DateTime(Convert.ToInt64(Math.Truncate(Convert.ToDouble(value, CultureInfo.InvariantCulture))));
		}
		catch (Exception ex)
		{
			if (ex is FormatException || ex is ArgumentException || ex is InvalidCastException)
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenException(LogHelper.FormatInvariant("IDX12700: Error found while parsing date time. The '{0}' claim has value '{1}' which is could not be parsed to an integer.", new object[2]
				{
					key,
					value ?? "<null>"
				}), ex));
			}
			if (ex is OverflowException)
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenException(LogHelper.FormatInvariant("IDX12701: Error found while parsing date time. The '{0}' claim has value '{1}' does not lie in the valid range.", new object[2]
				{
					key,
					value ?? "<null>"
				}), ex));
			}
			throw;
		}
	}

	public virtual string SerializeToJson()
	{
		return JsonExtensions.SerializeToJson(this);
	}

	public virtual string Base64UrlEncode()
	{
		return Base64UrlEncoder.Encode(SerializeToJson());
	}

	public static JwtPayload Base64UrlDeserialize(string base64UrlEncodedJsonString)
	{
		return JsonExtensions.DeserializeJwtPayload(Base64UrlEncoder.Decode(base64UrlEncodedJsonString));
	}

	public static JwtPayload Deserialize(string jsonString)
	{
		return JsonExtensions.DeserializeJwtPayload(jsonString);
	}
}
