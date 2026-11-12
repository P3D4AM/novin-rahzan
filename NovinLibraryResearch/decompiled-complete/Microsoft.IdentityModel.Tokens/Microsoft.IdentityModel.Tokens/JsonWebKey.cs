using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

[JsonObject]
public class JsonWebKey : SecurityKey
{
	private string _kid;

	[JsonIgnore]
	internal SecurityKey ConvertedSecurityKey { get; set; }

	[JsonExtensionData]
	public virtual IDictionary<string, object> AdditionalData { get; } = new Dictionary<string, object>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "alg", Required = Required.Default)]
	public string Alg { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "crv", Required = Required.Default)]
	public string Crv { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "d", Required = Required.Default)]
	public string D { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "dp", Required = Required.Default)]
	public string DP { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "dq", Required = Required.Default)]
	public string DQ { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "e", Required = Required.Default)]
	public string E { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "k", Required = Required.Default)]
	public string K { get; set; }

	[JsonIgnore]
	public override string KeyId
	{
		get
		{
			return _kid;
		}
		set
		{
			_kid = value;
		}
	}

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "key_ops", Required = Required.Default)]
	public IList<string> KeyOps { get; private set; } = new List<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "kid", Required = Required.Default)]
	public string Kid
	{
		get
		{
			return _kid;
		}
		set
		{
			_kid = value;
		}
	}

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "kty", Required = Required.Default)]
	public string Kty { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "n", Required = Required.Default)]
	public string N { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "oth", Required = Required.Default)]
	public IList<string> Oth { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "p", Required = Required.Default)]
	public string P { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "q", Required = Required.Default)]
	public string Q { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "qi", Required = Required.Default)]
	public string QI { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "use", Required = Required.Default)]
	public string Use { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "x", Required = Required.Default)]
	public string X { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "x5c", Required = Required.Default)]
	public IList<string> X5c { get; private set; } = new List<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "x5t", Required = Required.Default)]
	public string X5t { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "x5t#S256", Required = Required.Default)]
	public string X5tS256 { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "x5u", Required = Required.Default)]
	public string X5u { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "y", Required = Required.Default)]
	public string Y { get; set; }

	[JsonIgnore]
	public override int KeySize
	{
		get
		{
			if (Kty == "RSA" && !string.IsNullOrEmpty(N))
			{
				return Base64UrlEncoder.DecodeBytes(N).Length * 8;
			}
			if (Kty == "EC" && !string.IsNullOrEmpty(X))
			{
				return Base64UrlEncoder.DecodeBytes(X).Length * 8;
			}
			if (Kty == "oct" && !string.IsNullOrEmpty(K))
			{
				return Base64UrlEncoder.DecodeBytes(K).Length * 8;
			}
			return 0;
		}
	}

	[JsonIgnore]
	public bool HasPrivateKey
	{
		get
		{
			if (Kty == "RSA")
			{
				if (D != null && DP != null && DQ != null && P != null && Q != null)
				{
					return QI != null;
				}
				return false;
			}
			if (Kty == "EC")
			{
				return D != null;
			}
			return false;
		}
	}

	public static JsonWebKey Create(string json)
	{
		if (string.IsNullOrEmpty(json))
		{
			throw LogHelper.LogArgumentNullException("json");
		}
		return new JsonWebKey(json);
	}

	public JsonWebKey()
	{
	}

	public JsonWebKey(string json)
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

	public bool ShouldSerializeKeyOps()
	{
		return KeyOps.Count > 0;
	}

	public bool ShouldSerializeX5c()
	{
		return X5c.Count > 0;
	}

	internal RSAParameters CreateRsaParameters()
	{
		if (string.IsNullOrEmpty(N))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10700: {0} is unable to use 'rsaParameters'. {1} is null.", this, "Modulus")));
		}
		if (string.IsNullOrEmpty(E))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10700: {0} is unable to use 'rsaParameters'. {1} is null.", this), "Exponent"));
		}
		RSAParameters result = default(RSAParameters);
		result.Modulus = Base64UrlEncoder.DecodeBytes(N);
		result.Exponent = Base64UrlEncoder.DecodeBytes(E);
		result.D = (string.IsNullOrEmpty(D) ? null : Base64UrlEncoder.DecodeBytes(D));
		result.P = (string.IsNullOrEmpty(P) ? null : Base64UrlEncoder.DecodeBytes(P));
		result.Q = (string.IsNullOrEmpty(Q) ? null : Base64UrlEncoder.DecodeBytes(Q));
		result.DP = (string.IsNullOrEmpty(DP) ? null : Base64UrlEncoder.DecodeBytes(DP));
		result.DQ = (string.IsNullOrEmpty(DQ) ? null : Base64UrlEncoder.DecodeBytes(DQ));
		result.InverseQ = (string.IsNullOrEmpty(QI) ? null : Base64UrlEncoder.DecodeBytes(QI));
		return result;
	}

	public override bool CanComputeJwkThumbprint()
	{
		if (string.IsNullOrEmpty(Kty))
		{
			return false;
		}
		if (string.Equals(Kty, "EC", StringComparison.Ordinal))
		{
			return CanComputeECThumbprint();
		}
		if (string.Equals(Kty, "RSA", StringComparison.Ordinal))
		{
			return CanComputeRsaThumbprint();
		}
		if (string.Equals(Kty, "oct", StringComparison.Ordinal))
		{
			return CanComputeOctThumbprint();
		}
		return false;
	}

	public override byte[] ComputeJwkThumbprint()
	{
		if (string.IsNullOrEmpty(Kty))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10705: Cannot create a JWK thumbprint, '{0}' is null or empty.", "Kty"), "Kty"));
		}
		if (string.Equals(Kty, "EC", StringComparison.Ordinal))
		{
			return ComputeECThumbprint();
		}
		if (string.Equals(Kty, "RSA", StringComparison.Ordinal))
		{
			return ComputeRsaThumbprint();
		}
		if (string.Equals(Kty, "oct", StringComparison.Ordinal))
		{
			return ComputeOctThumbprint();
		}
		throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10706: Cannot create a JWK thumbprint, '{0}' must be one of the following: '{1}'.", "Kty", string.Join(", ", "EC", "RSA", "oct"), "Kty")));
	}

	private bool CanComputeOctThumbprint()
	{
		return !string.IsNullOrEmpty(K);
	}

	private byte[] ComputeOctThumbprint()
	{
		if (string.IsNullOrEmpty(K))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10705: Cannot create a JWK thumbprint, '{0}' is null or empty.", "K"), "K"));
		}
		return Utility.GenerateSha256Hash("{\"k\":\"" + K + "\",\"kty\":\"" + Kty + "\"}");
	}

	private bool CanComputeRsaThumbprint()
	{
		if (!string.IsNullOrEmpty(E))
		{
			return !string.IsNullOrEmpty(N);
		}
		return false;
	}

	private byte[] ComputeRsaThumbprint()
	{
		if (string.IsNullOrEmpty(E))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10705: Cannot create a JWK thumbprint, '{0}' is null or empty.", "E"), "E"));
		}
		if (string.IsNullOrEmpty(N))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10705: Cannot create a JWK thumbprint, '{0}' is null or empty.", "N"), "N"));
		}
		return Utility.GenerateSha256Hash("{\"e\":\"" + E + "\",\"kty\":\"" + Kty + "\",\"n\":\"" + N + "\"}");
	}

	private bool CanComputeECThumbprint()
	{
		if (!string.IsNullOrEmpty(Crv) && !string.IsNullOrEmpty(X))
		{
			return !string.IsNullOrEmpty(Y);
		}
		return false;
	}

	private byte[] ComputeECThumbprint()
	{
		if (string.IsNullOrEmpty(Crv))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10705: Cannot create a JWK thumbprint, '{0}' is null or empty.", "Crv"), "Crv"));
		}
		if (string.IsNullOrEmpty(X))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10705: Cannot create a JWK thumbprint, '{0}' is null or empty.", "X"), "X"));
		}
		if (string.IsNullOrEmpty(Y))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10705: Cannot create a JWK thumbprint, '{0}' is null or empty.", "Y"), "Y"));
		}
		return Utility.GenerateSha256Hash("{\"crv\":\"" + Crv + "\",\"kty\":\"" + Kty + "\",\"x\":\"" + X + "\",\"y\":\"" + Y + "\"}");
	}

	internal string RepresentAsAsymmetricPublicJwk()
	{
		JObject jObject = new JObject();
		if (!string.IsNullOrEmpty(Kid))
		{
			jObject.Add("kid", Kid);
		}
		if (string.Equals(Kty, "EC", StringComparison.Ordinal))
		{
			PopulateWithPublicEcParams(jObject);
		}
		else
		{
			if (!string.Equals(Kty, "RSA", StringComparison.Ordinal))
			{
				throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10707: Cannot create a JSON representation of an asymmetric public key, '{0}' must be one of the following: '{1}'.", "Kty", string.Join(", ", "EC", "RSA"), "Kty")));
			}
			PopulateWithPublicRsaParams(jObject);
		}
		return jObject.ToString(Formatting.None);
	}

	private void PopulateWithPublicEcParams(JObject jwk)
	{
		if (string.IsNullOrEmpty(Crv))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10708: Cannot create a JSON representation of an EC public key, '{0}' is null or empty.", "Crv"), "Crv"));
		}
		if (string.IsNullOrEmpty(X))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10708: Cannot create a JSON representation of an EC public key, '{0}' is null or empty.", "X"), "X"));
		}
		if (string.IsNullOrEmpty(Y))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10708: Cannot create a JSON representation of an EC public key, '{0}' is null or empty.", "Y"), "Y"));
		}
		jwk.Add("crv", Crv);
		jwk.Add("kty", Kty);
		jwk.Add("x", X);
		jwk.Add("y", Y);
	}

	private void PopulateWithPublicRsaParams(JObject jwk)
	{
		if (string.IsNullOrEmpty(E))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10709: Cannot create a JSON representation of an RSA public key, '{0}' is null or empty.", "E"), "E"));
		}
		if (string.IsNullOrEmpty(N))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10709: Cannot create a JSON representation of an RSA public key, '{0}' is null or empty.", "N"), "N"));
		}
		jwk.Add("e", E);
		jwk.Add("kty", Kty);
		jwk.Add("n", N);
	}

	public override string ToString()
	{
		return $"{GetType()}, Use: '{Use}',  Kid: '{Kid}', Kty: '{Kty}', InternalId: '{InternalId}'.";
	}
}
