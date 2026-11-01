using System.Collections.Generic;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace System.IdentityModel.Tokens.Jwt;

public class JwtHeader : Dictionary<string, object>
{
	public string Alg
	{
		get
		{
			return GetStandardClaim("alg");
		}
		private set
		{
			base["alg"] = value;
		}
	}

	public string Cty
	{
		get
		{
			return GetStandardClaim("cty");
		}
		private set
		{
			base["cty"] = value;
		}
	}

	public string Enc
	{
		get
		{
			return GetStandardClaim("enc");
		}
		private set
		{
			base["enc"] = value;
		}
	}

	public EncryptingCredentials EncryptingCredentials { get; private set; }

	public string IV => GetStandardClaim("iv");

	public string Kid
	{
		get
		{
			return GetStandardClaim("kid");
		}
		private set
		{
			base["kid"] = value;
		}
	}

	public SigningCredentials SigningCredentials { get; private set; }

	public string Typ
	{
		get
		{
			return GetStandardClaim("typ");
		}
		private set
		{
			base["typ"] = value;
		}
	}

	public string X5t => GetStandardClaim("x5t");

	public string Zip => GetStandardClaim("zip");

	public JwtHeader()
		: base((IEqualityComparer<string>?)StringComparer.Ordinal)
	{
	}

	public JwtHeader(SigningCredentials signingCredentials)
		: this(signingCredentials, null)
	{
	}

	public JwtHeader(EncryptingCredentials encryptingCredentials)
		: this(encryptingCredentials, null)
	{
	}

	public JwtHeader(SigningCredentials signingCredentials, IDictionary<string, string> outboundAlgorithmMap)
		: this(signingCredentials, outboundAlgorithmMap, null)
	{
	}

	public JwtHeader(SigningCredentials signingCredentials, IDictionary<string, string> outboundAlgorithmMap, string tokenType)
		: base((IEqualityComparer<string>?)StringComparer.Ordinal)
	{
		if (signingCredentials == null)
		{
			base["alg"] = "none";
		}
		else
		{
			if (outboundAlgorithmMap != null && outboundAlgorithmMap.TryGetValue(signingCredentials.Algorithm, out var value))
			{
				Alg = value;
			}
			else
			{
				Alg = signingCredentials.Algorithm;
			}
			if (!string.IsNullOrEmpty(signingCredentials.Key.KeyId))
			{
				Kid = signingCredentials.Key.KeyId;
			}
			X509SigningCredentials val = (X509SigningCredentials)(object)((signingCredentials is X509SigningCredentials) ? signingCredentials : null);
			if (val != null)
			{
				base["x5t"] = Base64UrlEncoder.Encode(val.Certificate.GetCertHash());
			}
		}
		if (string.IsNullOrEmpty(tokenType))
		{
			Typ = "JWT";
		}
		else
		{
			Typ = tokenType;
		}
		SigningCredentials = signingCredentials;
	}

	public JwtHeader(EncryptingCredentials encryptingCredentials, IDictionary<string, string> outboundAlgorithmMap)
		: this(encryptingCredentials, outboundAlgorithmMap, null)
	{
	}

	public JwtHeader(EncryptingCredentials encryptingCredentials, IDictionary<string, string> outboundAlgorithmMap, string tokenType)
		: base((IEqualityComparer<string>?)StringComparer.Ordinal)
	{
		if (encryptingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("encryptingCredentials");
		}
		if (outboundAlgorithmMap != null && outboundAlgorithmMap.TryGetValue(encryptingCredentials.Alg, out var value))
		{
			Alg = value;
		}
		else
		{
			Alg = encryptingCredentials.Alg;
		}
		if (outboundAlgorithmMap != null && outboundAlgorithmMap.TryGetValue(encryptingCredentials.Enc, out value))
		{
			Enc = value;
		}
		else
		{
			Enc = encryptingCredentials.Enc;
		}
		if (!string.IsNullOrEmpty(encryptingCredentials.Key.KeyId))
		{
			Kid = encryptingCredentials.Key.KeyId;
		}
		if (string.IsNullOrEmpty(tokenType))
		{
			Typ = "JWT";
		}
		else
		{
			Typ = tokenType;
		}
		EncryptingCredentials = encryptingCredentials;
	}

	public static JwtHeader Base64UrlDeserialize(string base64UrlEncodedJsonString)
	{
		return JsonExtensions.DeserializeJwtHeader(Base64UrlEncoder.Decode(base64UrlEncodedJsonString));
	}

	public virtual string Base64UrlEncode()
	{
		return Base64UrlEncoder.Encode(SerializeToJson());
	}

	public static JwtHeader Deserialize(string jsonString)
	{
		return JsonExtensions.DeserializeJwtHeader(jsonString);
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

	public virtual string SerializeToJson()
	{
		return JsonExtensions.SerializeToJson(this);
	}
}
