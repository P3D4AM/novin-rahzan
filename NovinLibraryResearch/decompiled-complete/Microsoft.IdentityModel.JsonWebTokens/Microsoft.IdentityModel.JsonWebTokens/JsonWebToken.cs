using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.JsonWebTokens;

public class JsonWebToken : SecurityToken
{
	public string Actor => Payload.Value<string>("actort") ?? string.Empty;

	public string Alg => Header.Value<string>("alg") ?? string.Empty;

	public IEnumerable<string> Audiences
	{
		get
		{
			JToken value = Payload.GetValue("aud", StringComparison.Ordinal);
			if (value != null)
			{
				if (value.Type == JTokenType.String)
				{
					return new List<string> { value.ToObject<string>() };
				}
				if (value.Type == JTokenType.Array)
				{
					return value.ToObject<List<string>>();
				}
			}
			return Enumerable.Empty<string>();
		}
	}

	public string AuthenticationTag { get; private set; }

	public string Ciphertext { get; private set; }

	public virtual IEnumerable<Claim> Claims
	{
		get
		{
			if (InnerToken != null)
			{
				return InnerToken.Claims;
			}
			if (!Payload.HasValues)
			{
				return Enumerable.Empty<Claim>();
			}
			List<Claim> list = new List<Claim>();
			string text = Issuer ?? "LOCAL AUTHORITY";
			foreach (KeyValuePair<string, JToken> item in Payload)
			{
				if (item.Value == null)
				{
					list.Add(new Claim(item.Key, string.Empty, "JSON_NULL", text, text));
					continue;
				}
				if (item.Value.Type == JTokenType.String)
				{
					string value = item.Value.ToObject<string>();
					list.Add(new Claim(item.Key, value, "http://www.w3.org/2001/XMLSchema#string", text, text));
					continue;
				}
				JToken value2 = item.Value;
				if (value2 != null)
				{
					AddClaimsFromJToken(list, item.Key, value2, text);
				}
			}
			return list;
		}
	}

	public string Cty => Header.Value<string>("cty") ?? string.Empty;

	public string Enc => Header.Value<string>("enc") ?? string.Empty;

	public string EncryptedKey { get; private set; }

	internal JObject Header { get; private set; } = new JObject();


	public override string Id => Payload.Value<string>("jti") ?? string.Empty;

	public string InitializationVector { get; private set; }

	public JsonWebToken InnerToken { get; internal set; }

	public DateTime IssuedAt => JwtTokenUtilities.GetDateTime("iat", Payload);

	public override string Issuer => Payload.Value<string>("iss") ?? string.Empty;

	public string Kid => Header.Value<string>("kid") ?? string.Empty;

	internal JObject Payload { get; private set; } = new JObject();


	public string EncodedHeader { get; internal set; }

	public string EncodedPayload { get; internal set; }

	public string EncodedSignature { get; internal set; }

	public string EncodedToken { get; private set; }

	public override SecurityKey SecurityKey { get; }

	public override SecurityKey SigningKey { get; set; }

	public string Subject => Payload.Value<string>("sub") ?? string.Empty;

	public string Typ => Header.Value<string>("typ") ?? string.Empty;

	public override DateTime ValidFrom => JwtTokenUtilities.GetDateTime("nbf", Payload);

	public override DateTime ValidTo => JwtTokenUtilities.GetDateTime("exp", Payload);

	public string X5t => Header.Value<string>("x5t") ?? string.Empty;

	public string Zip => Header.Value<string>("zip") ?? string.Empty;

	public JsonWebToken(string jwtEncodedString)
	{
		if (string.IsNullOrEmpty(jwtEncodedString))
		{
			throw new ArgumentNullException("jwtEncodedString");
		}
		string[] array = jwtEncodedString.Split(new char[1] { '.' }, 6);
		if (array.Length == 3 || array.Length == 5)
		{
			Decode(array, jwtEncodedString);
			return;
		}
		throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX14100: JWT is not well formed: '{0}'.\nThe token needs to be in JWS or JWE Compact Serialization Format. (JWS): 'EncodedHeader.EndcodedPayload.EncodedSignature'. (JWE): 'EncodedProtectedHeader.EncodedEncryptedKey.EncodedInitializationVector.EncodedCiphertext.EncodedAuthenticationTag'.", jwtEncodedString)));
	}

	public JsonWebToken(string header, string payload)
	{
		if (string.IsNullOrEmpty(header))
		{
			throw LogHelper.LogArgumentNullException("header");
		}
		if (string.IsNullOrEmpty(payload))
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		try
		{
			Header = JObject.Parse(header);
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX14301: Unable to parse the header into a JSON object. \nHeader: '{0}'.", header), innerException));
		}
		try
		{
			Payload = JObject.Parse(payload);
		}
		catch (Exception innerException2)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX14302: Unable to parse the payload into a JSON object. \nPayload: '{0}'.", payload), innerException2));
		}
	}

	private void Decode(string[] tokenParts, string rawData)
	{
		LogHelper.LogInformation("IDX14106: Decoding token: '{0}' into header, payload and signature.", rawData);
		try
		{
			Header = JObject.Parse(Base64UrlEncoder.Decode(tokenParts[0]));
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX14102: Unable to decode the header '{0}' as Base64Url encoded string. jwtEncodedString: '{1}'.", tokenParts[0], rawData), innerException));
		}
		if (tokenParts.Length == 5)
		{
			DecodeJwe(tokenParts);
		}
		else
		{
			DecodeJws(tokenParts);
		}
		EncodedToken = rawData;
	}

	private static void AddClaimsFromJToken(List<Claim> claims, string claimType, JToken jtoken, string issuer)
	{
		if (jtoken.Type == JTokenType.Object)
		{
			claims.Add(new Claim(claimType, jtoken.ToString(Formatting.None), "JSON", issuer, issuer));
			return;
		}
		if (jtoken.Type == JTokenType.Array)
		{
			foreach (JToken item in jtoken as JArray)
			{
				switch (item.Type)
				{
				case JTokenType.Object:
					claims.Add(new Claim(claimType, item.ToString(Formatting.None), "JSON", issuer, issuer));
					break;
				case JTokenType.Array:
					claims.Add(new Claim(claimType, item.ToString(Formatting.None), "JSON_ARRAY", issuer, issuer));
					break;
				default:
					AddDefaultClaimFromJToken(claims, claimType, item, issuer);
					break;
				}
			}
			return;
		}
		AddDefaultClaimFromJToken(claims, claimType, jtoken, issuer);
	}

	private static void AddDefaultClaimFromJToken(List<Claim> claims, string claimType, JToken jtoken, string issuer)
	{
		if (jtoken is JValue jValue)
		{
			if (jValue.Type == JTokenType.String)
			{
				claims.Add(new Claim(claimType, jValue.Value.ToString(), "http://www.w3.org/2001/XMLSchema#string", issuer, issuer));
			}
			else if (jValue.Value is DateTime dateTime)
			{
				claims.Add(new Claim(claimType, dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), "http://www.w3.org/2001/XMLSchema#dateTime", issuer, issuer));
			}
			else
			{
				claims.Add(new Claim(claimType, jtoken.ToString(Formatting.None), GetClaimValueType(jValue.Value), issuer, issuer));
			}
		}
		else
		{
			claims.Add(new Claim(claimType, jtoken.ToString(Formatting.None), GetClaimValueType(jtoken), issuer, issuer));
		}
	}

	private void DecodeJwe(string[] tokenParts)
	{
		EncodedHeader = tokenParts[0];
		EncryptedKey = tokenParts[1];
		InitializationVector = tokenParts[2];
		if (string.IsNullOrWhiteSpace(tokenParts[3]))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException("IDX14306: JWE Ciphertext cannot be an empty string."));
		}
		Ciphertext = tokenParts[3];
		AuthenticationTag = tokenParts[4];
	}

	private void DecodeJws(string[] tokenParts)
	{
		if (!string.IsNullOrEmpty(Cty))
		{
			LogHelper.LogVerbose(LogHelper.FormatInvariant("IDX14105: Header.Cty != null, assuming JWS. Cty: '{0}'.", Payload.Value<string>("cty")));
		}
		try
		{
			Payload = JObject.Parse(Base64UrlEncoder.Decode(tokenParts[1]));
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX14101: Unable to decode the payload '{0}' as Base64Url encoded string. jwtEncodedString: '{1}'.", tokenParts[1], EncodedToken), innerException));
		}
		EncodedHeader = tokenParts[0];
		EncodedPayload = tokenParts[1];
		EncodedSignature = tokenParts[2];
	}

	private static string GetClaimValueType(object obj)
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

	public Claim GetClaim(string key)
	{
		string text = Issuer ?? "LOCAL AUTHORITY";
		if (!Payload.TryGetValue(key, out var value))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX14304: Claim with name '{0}' does not exist in the payload.", key)));
		}
		if (value.Type == JTokenType.Null)
		{
			return new Claim(key, string.Empty, "JSON_NULL", text, text);
		}
		if (value.Type == JTokenType.Object)
		{
			return new Claim(key, value.ToString(Formatting.None), "JSON", text, text);
		}
		if (value.Type == JTokenType.Array)
		{
			return new Claim(key, value.ToString(Formatting.None), "JSON_ARRAY", text, text);
		}
		if (value is JValue jValue)
		{
			if (jValue.Type == JTokenType.String)
			{
				return new Claim(key, jValue.Value.ToString(), "http://www.w3.org/2001/XMLSchema#string", text, text);
			}
			if (jValue.Value is DateTime dateTime)
			{
				return new Claim(key, dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), "http://www.w3.org/2001/XMLSchema#dateTime", text, text);
			}
			return new Claim(key, value.ToString(Formatting.None), GetClaimValueType(jValue.Value), text, text);
		}
		return new Claim(key, value.ToString(Formatting.None), GetClaimValueType(value), text, text);
	}

	public T GetPayloadValue<T>(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (typeof(T).Equals(typeof(Claim)))
		{
			return (T)(object)GetClaim(key);
		}
		if (!Payload.TryGetValue(key, out var value))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX14304: Claim with name '{0}' does not exist in the payload.", key)));
		}
		try
		{
			return value.ToObject<T>();
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX14305: Unable to convert the '{0}' claim to the following type: '{1}'. Claim type was: '{2}'.", key, typeof(T), value.Type), innerException));
		}
	}

	public bool TryGetClaim(string key, out Claim value)
	{
		string text = Issuer ?? "LOCAL AUTHORITY";
		if (!Payload.TryGetValue(key, out var value2))
		{
			value = null;
			return false;
		}
		if (value2.Type == JTokenType.Null)
		{
			value = new Claim(key, string.Empty, "JSON_NULL", text, text);
		}
		else if (value2.Type == JTokenType.Object)
		{
			value = new Claim(key, value2.ToString(Formatting.None), "JSON", text, text);
		}
		else if (value2.Type == JTokenType.Array)
		{
			value = new Claim(key, value2.ToString(Formatting.None), "JSON_ARRAY", text, text);
		}
		else if (value2 is JValue jValue)
		{
			if (jValue.Type == JTokenType.String)
			{
				value = new Claim(key, jValue.Value.ToString(), "http://www.w3.org/2001/XMLSchema#string", text, text);
			}
			else if (jValue.Value is DateTime dateTime)
			{
				value = new Claim(key, dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), "http://www.w3.org/2001/XMLSchema#dateTime", text);
			}
			else
			{
				value = new Claim(key, value2.ToString(Formatting.None), GetClaimValueType(jValue.Value), text, text);
			}
		}
		else
		{
			value = new Claim(key, value2.ToString(Formatting.None), GetClaimValueType(value2), text, text);
		}
		return true;
	}

	public bool TryGetPayloadValue<T>(string key, out T value)
	{
		if (string.IsNullOrEmpty(key))
		{
			value = default(T);
			return false;
		}
		if (typeof(T).Equals(typeof(Claim)))
		{
			Claim value2;
			bool result = TryGetClaim(key, out value2);
			value = (T)(object)value2;
			return result;
		}
		if (!Payload.TryGetValue(key, out var value3))
		{
			value = default(T);
			return false;
		}
		try
		{
			value = value3.ToObject<T>();
		}
		catch (Exception)
		{
			value = default(T);
			return false;
		}
		return true;
	}

	public T GetHeaderValue<T>(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (!Header.TryGetValue(key, out var value))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX14303: Claim with name '{0}' does not exist in the header.", key)));
		}
		try
		{
			return value.ToObject<T>();
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX14305: Unable to convert the '{0}' claim to the following type: '{1}'. Claim type was: '{2}'.", key, typeof(T), value.Type), innerException));
		}
	}

	public bool TryGetHeaderValue<T>(string key, out T value)
	{
		if (string.IsNullOrEmpty(key))
		{
			value = default(T);
			return false;
		}
		if (!Header.TryGetValue(key, out var value2))
		{
			value = default(T);
			return false;
		}
		try
		{
			value = value2.ToObject<T>();
		}
		catch (Exception)
		{
			value = default(T);
			return false;
		}
		return true;
	}
}
