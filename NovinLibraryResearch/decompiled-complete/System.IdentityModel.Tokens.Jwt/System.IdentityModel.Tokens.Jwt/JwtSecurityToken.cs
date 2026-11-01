using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace System.IdentityModel.Tokens.Jwt;

public class JwtSecurityToken : SecurityToken
{
	private JwtPayload _payload;

	public string Actor
	{
		get
		{
			if (Payload != null)
			{
				return Payload.Actort;
			}
			return string.Empty;
		}
	}

	public IEnumerable<string> Audiences
	{
		get
		{
			if (Payload != null)
			{
				return Payload.Aud;
			}
			return new List<string>();
		}
	}

	public IEnumerable<Claim> Claims
	{
		get
		{
			if (Payload != null)
			{
				return Payload.Claims;
			}
			return new List<Claim>();
		}
	}

	public virtual string EncodedHeader => Header.Base64UrlEncode();

	public virtual string EncodedPayload
	{
		get
		{
			if (Payload != null)
			{
				return Payload.Base64UrlEncode();
			}
			return string.Empty;
		}
	}

	public JwtHeader Header { get; internal set; }

	public override string Id
	{
		get
		{
			if (Payload != null)
			{
				return Payload.Jti;
			}
			return string.Empty;
		}
	}

	public override string Issuer
	{
		get
		{
			if (Payload != null)
			{
				return Payload.Iss;
			}
			return string.Empty;
		}
	}

	public JwtPayload Payload
	{
		get
		{
			if (InnerToken != null)
			{
				return InnerToken.Payload;
			}
			return _payload;
		}
		internal set
		{
			_payload = value;
		}
	}

	public JwtSecurityToken InnerToken { get; internal set; }

	public string RawAuthenticationTag { get; private set; }

	public string RawCiphertext { get; private set; }

	public string RawData { get; private set; }

	public string RawEncryptedKey { get; private set; }

	public string RawInitializationVector { get; private set; }

	public string RawHeader { get; internal set; }

	public string RawPayload { get; internal set; }

	public string RawSignature { get; internal set; }

	public override SecurityKey SecurityKey => null;

	public string SignatureAlgorithm => Header.Alg;

	public SigningCredentials SigningCredentials => Header.SigningCredentials;

	public EncryptingCredentials EncryptingCredentials => Header.EncryptingCredentials;

	public override SecurityKey SigningKey { get; set; }

	public string Subject
	{
		get
		{
			if (Payload != null)
			{
				return Payload.Sub;
			}
			return string.Empty;
		}
	}

	public override DateTime ValidFrom
	{
		get
		{
			if (Payload != null)
			{
				return Payload.ValidFrom;
			}
			return DateTime.MinValue;
		}
	}

	public override DateTime ValidTo
	{
		get
		{
			if (Payload != null)
			{
				return Payload.ValidTo;
			}
			return DateTime.MinValue;
		}
	}

	public virtual DateTime IssuedAt
	{
		get
		{
			if (Payload != null)
			{
				return Payload.IssuedAt;
			}
			return DateTime.MinValue;
		}
	}

	public JwtSecurityToken(string jwtEncodedString)
	{
		if (string.IsNullOrWhiteSpace(jwtEncodedString))
		{
			throw LogHelper.LogArgumentNullException("jwtEncodedString");
		}
		string[] array = jwtEncodedString.Split(new char[1] { '.' }, 6);
		if (array.Length == 3)
		{
			if (!JwtTokenUtilities.RegexJws.IsMatch(jwtEncodedString))
			{
				throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX12739: JWT: '{0}' has three segments but is not in proper JWS format.", new object[1] { jwtEncodedString })));
			}
		}
		else
		{
			if (array.Length != 5)
			{
				throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX12741: JWT: '{0}' must have three segments (JWS) or five segments (JWE).", new object[1] { jwtEncodedString })));
			}
			if (!JwtTokenUtilities.RegexJwe.IsMatch(jwtEncodedString))
			{
				throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX12740: JWT: '{0}' has five segments but is not in proper JWE format.", new object[1] { jwtEncodedString })));
			}
		}
		Decode(array, jwtEncodedString);
	}

	public JwtSecurityToken(JwtHeader header, JwtPayload payload, string rawHeader, string rawPayload, string rawSignature)
	{
		if (header == null)
		{
			throw LogHelper.LogArgumentNullException("header");
		}
		if (payload == null)
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		if (string.IsNullOrWhiteSpace(rawHeader))
		{
			throw LogHelper.LogArgumentNullException("rawHeader");
		}
		if (string.IsNullOrWhiteSpace(rawPayload))
		{
			throw LogHelper.LogArgumentNullException("rawPayload");
		}
		if (rawSignature == null)
		{
			throw LogHelper.LogArgumentNullException("rawSignature");
		}
		Header = header;
		Payload = payload;
		RawData = rawHeader + "." + rawPayload + "." + rawSignature;
		RawHeader = rawHeader;
		RawPayload = rawPayload;
		RawSignature = rawSignature;
	}

	public JwtSecurityToken(JwtHeader header, JwtSecurityToken innerToken, string rawHeader, string rawEncryptedKey, string rawInitializationVector, string rawCiphertext, string rawAuthenticationTag)
	{
		if (header == null)
		{
			throw LogHelper.LogArgumentNullException("header");
		}
		if (innerToken == null)
		{
			throw LogHelper.LogArgumentNullException("innerToken");
		}
		if (rawEncryptedKey == null)
		{
			throw LogHelper.LogArgumentNullException("rawEncryptedKey");
		}
		if (string.IsNullOrEmpty(rawInitializationVector))
		{
			throw LogHelper.LogArgumentNullException("rawInitializationVector");
		}
		if (string.IsNullOrEmpty(rawCiphertext))
		{
			throw LogHelper.LogArgumentNullException("rawCiphertext");
		}
		if (string.IsNullOrEmpty(rawAuthenticationTag))
		{
			throw LogHelper.LogArgumentNullException("rawAuthenticationTag");
		}
		Header = header;
		InnerToken = innerToken;
		RawData = string.Join(".", rawHeader, rawEncryptedKey, rawInitializationVector, rawCiphertext, rawAuthenticationTag);
		RawHeader = rawHeader;
		RawEncryptedKey = rawEncryptedKey;
		RawInitializationVector = rawInitializationVector;
		RawCiphertext = rawCiphertext;
		RawAuthenticationTag = rawAuthenticationTag;
	}

	public JwtSecurityToken(JwtHeader header, JwtPayload payload)
	{
		if (header == null)
		{
			throw LogHelper.LogArgumentNullException("header");
		}
		if (payload == null)
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		Header = header;
		Payload = payload;
		RawSignature = string.Empty;
	}

	public JwtSecurityToken(string issuer = null, string audience = null, IEnumerable<Claim> claims = null, DateTime? notBefore = null, DateTime? expires = null, SigningCredentials signingCredentials = null)
	{
		if (expires.HasValue && notBefore.HasValue && notBefore >= expires)
		{
			throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX12401: Expires: '{0}' must be after NotBefore: '{1}'.", new object[2] { expires.Value, notBefore.Value })));
		}
		Payload = new JwtPayload(issuer, audience, claims, notBefore, expires);
		Header = new JwtHeader(signingCredentials);
		RawSignature = string.Empty;
	}

	public override string ToString()
	{
		if (Payload != null)
		{
			return Header.SerializeToJson() + "." + Payload.SerializeToJson();
		}
		return Header.SerializeToJson() + ".";
	}

	internal void Decode(string[] tokenParts, string rawData)
	{
		LogHelper.LogInformation("IDX12716: Decoding token: '{0}' into header, payload and signature.", new object[1] { rawData });
		try
		{
			Header = JwtHeader.Base64UrlDeserialize(tokenParts[0]);
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX12729: Unable to decode the header '{0}' as Base64Url encoded string. jwtEncodedString: '{1}'.", new object[2]
			{
				tokenParts[0],
				rawData
			}), innerException));
		}
		if (tokenParts.Length == 5)
		{
			DecodeJwe(tokenParts);
		}
		else
		{
			DecodeJws(tokenParts);
		}
		RawData = rawData;
	}

	private void DecodeJws(string[] tokenParts)
	{
		if (Header.Cty != null)
		{
			LogHelper.LogVerbose(LogHelper.FormatInvariant("IDX12738: Header.Cty != null, assuming JWS. Cty: '{0}'.", new object[1] { Header.Cty }), Array.Empty<object>());
		}
		try
		{
			Payload = JwtPayload.Base64UrlDeserialize(tokenParts[1]);
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX12723: Unable to decode the payload '{0}' as Base64Url encoded string. jwtEncodedString: '{1}'.", new object[2]
			{
				tokenParts[1],
				RawData
			}), innerException));
		}
		RawHeader = tokenParts[0];
		RawPayload = tokenParts[1];
		RawSignature = tokenParts[2];
	}

	private void DecodeJwe(string[] tokenParts)
	{
		RawHeader = tokenParts[0];
		RawEncryptedKey = tokenParts[1];
		RawInitializationVector = tokenParts[2];
		RawCiphertext = tokenParts[3];
		RawAuthenticationTag = tokenParts[4];
	}
}
