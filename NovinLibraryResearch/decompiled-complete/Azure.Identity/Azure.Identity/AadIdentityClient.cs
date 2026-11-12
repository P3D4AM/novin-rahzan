using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Azure.Identity;

internal class AadIdentityClient
{
	private const string ClientAssertionType = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";

	private readonly CredentialPipeline _pipeline;

	protected AadIdentityClient()
	{
	}

	public AadIdentityClient(CredentialPipeline pipeline)
	{
		_pipeline = pipeline;
	}

	public virtual async Task<AccessToken> AuthenticateAsync(string tenantId, string clientId, string clientSecret, string[] scopes, CancellationToken cancellationToken = default(CancellationToken))
	{
		using Request request = CreateClientSecretAuthRequest(tenantId, clientId, clientSecret, scopes);
		return await SendAuthRequestAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual AccessToken Authenticate(string tenantId, string clientId, string clientSecret, string[] scopes, CancellationToken cancellationToken = default(CancellationToken))
	{
		using Request request = CreateClientSecretAuthRequest(tenantId, clientId, clientSecret, scopes);
		return SendAuthRequest(request, cancellationToken);
	}

	public virtual async Task<AccessToken> AuthenticateAsync(string tenantId, string clientId, X509Certificate2 clientCertificate, string[] scopes, CancellationToken cancellationToken = default(CancellationToken))
	{
		using Request request = CreateClientCertificateAuthRequest(tenantId, clientId, clientCertificate, scopes);
		return await SendAuthRequestAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual AccessToken Authenticate(string tenantId, string clientId, X509Certificate2 clientCertificate, string[] scopes, CancellationToken cancellationToken = default(CancellationToken))
	{
		using Request request = CreateClientCertificateAuthRequest(tenantId, clientId, clientCertificate, scopes);
		return SendAuthRequest(request, cancellationToken);
	}

	private async Task<AccessToken> SendAuthRequestAsync(Request request, CancellationToken cancellationToken)
	{
		Response response = await _pipeline.HttpPipeline.SendRequestAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (response.Status == 200 || response.Status == 201)
		{
			return Response.FromValue(await DeserializeAsync(response.ContentStream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false), response);
		}
		throw await _pipeline.Diagnostics.CreateRequestFailedExceptionAsync(response).ConfigureAwait(continueOnCapturedContext: false);
	}

	private AccessToken SendAuthRequest(Request request, CancellationToken cancellationToken)
	{
		Response response = _pipeline.HttpPipeline.SendRequest(request, cancellationToken);
		if (response.Status == 200 || response.Status == 201)
		{
			return Response.FromValue(Deserialize(response.ContentStream), response);
		}
		throw _pipeline.Diagnostics.CreateRequestFailedException(response);
	}

	private Request CreateClientSecretAuthRequest(string tenantId, string clientId, string clientSecret, string[] scopes)
	{
		Request request = _pipeline.HttpPipeline.CreateRequest();
		request.Method = RequestMethod.Post;
		request.Headers.Add(HttpHeader.Common.FormUrlEncodedContentType);
		request.Uri.Reset(_pipeline.AuthorityHost);
		request.Uri.AppendPath(tenantId);
		request.Uri.AppendPath("/oauth2/v2.0/token", escape: false);
		string s = "response_type=token&grant_type=client_credentials&client_id=" + Uri.EscapeDataString(clientId) + "&client_secret=" + Uri.EscapeDataString(clientSecret) + "&scope=" + Uri.EscapeDataString(string.Join(" ", scopes));
		ReadOnlyMemory<byte> bytes = MemoryExtensions.AsMemory(Encoding.UTF8.GetBytes(s));
		request.Content = RequestContent.Create(bytes);
		return request;
	}

	private Request CreateClientCertificateAuthRequest(string tenantId, string clientId, X509Certificate2 clientCertficate, string[] scopes)
	{
		Request request = _pipeline.HttpPipeline.CreateRequest();
		request.Method = RequestMethod.Post;
		request.Headers.Add(HttpHeader.Common.FormUrlEncodedContentType);
		request.Uri.Reset(_pipeline.AuthorityHost);
		request.Uri.AppendPath(tenantId);
		request.Uri.AppendPath("/oauth2/v2.0/token", escape: false);
		string stringToEscape = CreateClientAssertionJWT(clientId, request.Uri.ToString(), clientCertficate);
		string s = "response_type=token&grant_type=client_credentials&client_id=" + Uri.EscapeDataString(clientId) + "&client_assertion_type=" + Uri.EscapeDataString("urn:ietf:params:oauth:client-assertion-type:jwt-bearer") + "&client_assertion=" + Uri.EscapeDataString(stringToEscape) + "&scope=" + Uri.EscapeDataString(string.Join(" ", scopes));
		ReadOnlyMemory<byte> bytes = MemoryExtensions.AsMemory(Encoding.UTF8.GetBytes(s));
		request.Content = RequestContent.Create(bytes);
		return request;
	}

	private static string CreateClientAssertionJWT(string clientId, string audience, X509Certificate2 clientCertificate)
	{
		ArrayBufferWriter<byte> arrayBufferWriter = new ArrayBufferWriter<byte>();
		using (Utf8JsonWriter utf8JsonWriter = new Utf8JsonWriter(arrayBufferWriter))
		{
			utf8JsonWriter.WriteStartObject();
			utf8JsonWriter.WriteString("typ", "JWT");
			utf8JsonWriter.WriteString("alg", "RS256");
			utf8JsonWriter.WriteString("x5t", Base64Url.HexToBase64Url(clientCertificate.Thumbprint));
			utf8JsonWriter.WriteEndObject();
			utf8JsonWriter.Flush();
		}
		ArrayBufferWriter<byte> arrayBufferWriter2 = new ArrayBufferWriter<byte>();
		using (Utf8JsonWriter utf8JsonWriter2 = new Utf8JsonWriter(arrayBufferWriter2))
		{
			utf8JsonWriter2.WriteStartObject();
			utf8JsonWriter2.WriteString("jti", Guid.NewGuid());
			utf8JsonWriter2.WriteString("aud", audience);
			utf8JsonWriter2.WriteString("iss", clientId);
			utf8JsonWriter2.WriteString("sub", clientId);
			utf8JsonWriter2.WriteNumber("nbf", DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			utf8JsonWriter2.WriteNumber("exp", (DateTimeOffset.UtcNow + TimeSpan.FromMinutes(30.0)).ToUnixTimeSeconds());
			utf8JsonWriter2.WriteEndObject();
			utf8JsonWriter2.Flush();
		}
		string text = Base64Url.Encode(arrayBufferWriter.WrittenMemory.ToArray());
		string text2 = Base64Url.Encode(arrayBufferWriter2.WrittenMemory.ToArray());
		string text3 = text + "." + text2;
		byte[] bytes = clientCertificate.GetRSAPrivateKey().SignData(Encoding.ASCII.GetBytes(text3), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
		return text3 + "." + Base64Url.Encode(bytes);
	}

	private static async Task<AccessToken> DeserializeAsync(Stream content, CancellationToken cancellationToken)
	{
		using JsonDocument jsonDocument = await JsonDocument.ParseAsync(content, default(JsonDocumentOptions), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		return Deserialize(jsonDocument.RootElement);
	}

	private static AccessToken Deserialize(Stream content)
	{
		using JsonDocument jsonDocument = JsonDocument.Parse(content);
		return Deserialize(jsonDocument.RootElement);
	}

	private static AccessToken Deserialize(JsonElement json)
	{
		string accessToken = null;
		DateTimeOffset expiresOn = DateTimeOffset.MaxValue;
		foreach (JsonProperty item in json.EnumerateObject())
		{
			switch (item.Name)
			{
			case "access_token":
				accessToken = item.Value.GetString();
				break;
			case "expires_in":
				expiresOn = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(item.Value.GetInt64());
				break;
			}
		}
		return new AccessToken(accessToken, expiresOn);
	}
}
