using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.AuthScheme.PoP;

internal class PoPAuthenticationScheme : IAuthenticationScheme
{
	private static readonly DateTime s_jwtBaselineTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	private readonly HttpRequestMessage _httpRequestMessage;

	private readonly IPoPCryptoProvider _popCryptoProvider;

	public string AuthorizationHeaderPrefix => "PoP";

	public string AccessTokenType => "pop";

	public string KeyId { get; }

	public PoPAuthenticationScheme(HttpRequestMessage httpRequestMessage, IPoPCryptoProvider popCryptoProvider)
	{
		_httpRequestMessage = httpRequestMessage ?? throw new ArgumentNullException("httpRequestMessage");
		_popCryptoProvider = popCryptoProvider ?? throw new ArgumentNullException("popCryptoProvider");
		byte[] arg = ComputeRsaThumbprint(_popCryptoProvider.CannonicalPublicKeyJwk);
		KeyId = Base64UrlHelpers.Encode(arg);
	}

	public IDictionary<string, string> GetTokenRequestParams()
	{
		return new Dictionary<string, string>
		{
			{ "token_type", "pop" },
			{
				"req_cnf",
				ComputeReqCnf()
			}
		};
	}

	public string FormatAccessToken(MsalAccessTokenCacheItem msalAccessTokenCacheItem)
	{
		JObject jObject = new JObject
		{
			{ "alg", "RS256" },
			{ "kid", KeyId },
			{ "typ", "pop" }
		};
		JToken content = JToken.Parse(_popCryptoProvider.CannonicalPublicKeyJwk);
		JObject jObject2 = new JObject(new JProperty("at", msalAccessTokenCacheItem.Secret), new JProperty("ts", (long)(DateTime.UtcNow - s_jwtBaselineTime).TotalSeconds), new JProperty("m", _httpRequestMessage.Method.ToString()), new JProperty("u", _httpRequestMessage.RequestUri.Host), new JProperty("p", _httpRequestMessage.RequestUri.AbsolutePath), new JProperty("nonce", CreateNonce()), new JProperty("cnf", new JObject(new JProperty("jwk", content))));
		string text = CreateJWS(jObject2.ToString(Formatting.None), jObject.ToString(Formatting.None));
		_httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(AuthorizationHeaderPrefix, text);
		return text;
	}

	private static string CreateNonce()
	{
		return Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
	}

	private string ComputeReqCnf()
	{
		return Base64UrlHelpers.Encode("{\"kid\":\"" + KeyId + "\"}");
	}

	private static byte[] ComputeRsaThumbprint(string cannonicalJwk)
	{
		using SHA256 sHA = SHA256.Create();
		return sHA.ComputeHash(Encoding.UTF8.GetBytes(cannonicalJwk));
	}

	private string CreateJWS(string payload, string header)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Base64UrlHelpers.Encode(Encoding.UTF8.GetBytes(header)));
		stringBuilder.Append(".");
		stringBuilder.Append(Base64UrlHelpers.Encode(payload));
		string s = stringBuilder.ToString();
		stringBuilder.Append(".");
		stringBuilder.Append(Base64UrlHelpers.Encode(_popCryptoProvider.Sign(Encoding.UTF8.GetBytes(s))));
		return stringBuilder.ToString();
	}
}
