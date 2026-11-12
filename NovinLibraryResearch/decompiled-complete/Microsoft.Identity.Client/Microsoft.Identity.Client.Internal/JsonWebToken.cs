using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Internal;

internal class JsonWebToken
{
	[JsonObject]
	[Preserve(AllMembers = true)]
	internal class JWTHeader
	{
		protected ClientCredentialWrapper Credential { get; }

		[JsonProperty(PropertyName = "typ")]
		public static string Type
		{
			get
			{
				return "JWT";
			}
			set
			{
			}
		}

		[JsonProperty(PropertyName = "alg")]
		public string Algorithm
		{
			get
			{
				if (Credential != null)
				{
					return "RS256";
				}
				return "none";
			}
			set
			{
			}
		}

		public JWTHeader(ClientCredentialWrapper credential)
		{
			Credential = credential;
		}
	}

	[JsonObject]
	[Preserve(AllMembers = true)]
	internal class JWTPayload
	{
		[JsonProperty(PropertyName = "aud")]
		public string Audience { get; set; }

		[JsonProperty(PropertyName = "iss")]
		public string Issuer { get; set; }

		[JsonProperty(PropertyName = "nbf")]
		public long ValidFrom { get; set; }

		[JsonProperty(PropertyName = "exp")]
		public long ValidTo { get; set; }

		[JsonProperty(PropertyName = "sub", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string Subject { get; set; }

		[JsonProperty(PropertyName = "jti", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string JwtIdentifier { get; set; }
	}

	[JsonObject]
	[Preserve(AllMembers = true)]
	internal sealed class JWTHeaderWithCertificate : JWTHeader
	{
		[JsonProperty(PropertyName = "x5t")]
		public string X509CertificateThumbprint { get; set; }

		[JsonProperty(PropertyName = "kid", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string X509CertificateKeyId { get; set; }

		[JsonProperty(PropertyName = "x5c", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string X509CertificatePublicCertValue { get; set; }

		public JWTHeaderWithCertificate(ClientCredentialWrapper credential, bool sendCertificate)
			: base(credential)
		{
			X509CertificateThumbprint = base.Credential.Thumbprint;
			if (base.Credential?.Certificate?.Thumbprint != null)
			{
				X509CertificateKeyId = base.Credential.Certificate.Thumbprint;
			}
			X509CertificatePublicCertValue = null;
			if (sendCertificate)
			{
				X509CertificatePublicCertValue = Convert.ToBase64String(credential.Certificate.GetRawCertData());
			}
		}
	}

	private const int MaxTokenLength = 65536;

	public readonly JWTPayload Payload;

	private readonly ICryptographyManager _cryptographyManager;

	private bool _appendDefaultClaims;

	public IDictionary<string, string> ClaimsToSign { get; private set; }

	public long ValidTo => Payload.ValidTo;

	public JsonWebToken(ICryptographyManager cryptographyManager, string clientId, string audience)
	{
		_cryptographyManager = cryptographyManager;
		DateTime utcNow = DateTime.UtcNow;
		Payload = new JWTPayload
		{
			Audience = audience,
			Issuer = clientId,
			ValidFrom = ConvertToTimeT(utcNow),
			ValidTo = ConvertToTimeT(utcNow + TimeSpan.FromSeconds(600.0)),
			Subject = clientId,
			JwtIdentifier = Guid.NewGuid().ToString()
		};
	}

	public JsonWebToken(ICryptographyManager cryptographyManager, string clientId, string audience, IDictionary<string, string> claimsToSign, bool appendDefaultClaims = false)
		: this(cryptographyManager, clientId, audience)
	{
		ClaimsToSign = claimsToSign;
		_appendDefaultClaims = appendDefaultClaims;
	}

	public string Sign(ClientCredentialWrapper credential, bool sendCertificate)
	{
		string text = Encode(credential, sendCertificate);
		if (65536 < text.Length)
		{
			throw new MsalException("encoded_token_too_long");
		}
		return text + "." + UrlEncodeSegment(credential.Sign(_cryptographyManager, text));
	}

	private static string EncodeSegment(string segment)
	{
		return UrlEncodeSegment(Encoding.UTF8.GetBytes(segment));
	}

	private static string UrlEncodeSegment(byte[] segment)
	{
		return Base64UrlHelpers.Encode(segment);
	}

	private static string EncodeHeaderToJson(ClientCredentialWrapper credential, bool sendCertificate)
	{
		return JsonHelper.SerializeToJson(new JWTHeaderWithCertificate(credential, sendCertificate));
	}

	internal static long ConvertToTimeT(DateTime time)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return (long)(time - dateTime).TotalSeconds;
	}

	private string Encode(ClientCredentialWrapper credential, bool sendCertificate)
	{
		string text = EncodeSegment(EncodeHeaderToJson(credential, sendCertificate));
		string segment;
		if (ClaimsToSign != null && ClaimsToSign.Any())
		{
			if (_appendDefaultClaims)
			{
				JObject jObject = JObject.FromObject(Payload);
				jObject.Merge(JObject.FromObject(ClaimsToSign));
				segment = jObject.ToString();
			}
			else
			{
				segment = JObject.FromObject(ClaimsToSign).ToString();
			}
		}
		else
		{
			segment = JsonHelper.SerializeToJson(Payload);
		}
		string text2 = EncodeSegment(segment);
		return text + "." + text2;
	}
}
