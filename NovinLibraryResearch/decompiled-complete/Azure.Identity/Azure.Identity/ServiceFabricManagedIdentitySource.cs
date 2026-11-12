using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Identity;

internal class ServiceFabricManagedIdentitySource : ManagedIdentitySource
{
	private const string ServiceFabricMsiApiVersion = "2019-07-01-preview";

	private const string IdentityEndpointInvalidUriError = "The environment variable IDENTITY_ENDPOINT contains an invalid Uri.";

	private readonly Uri _endpoint;

	private readonly string _identityHeaderValue;

	private readonly string _clientId;

	public static ManagedIdentitySource TryCreate(ManagedIdentityClientOptions options)
	{
		string identityEndpoint = EnvironmentVariables.IdentityEndpoint;
		string identityHeader = EnvironmentVariables.IdentityHeader;
		string identityServerThumbprint = EnvironmentVariables.IdentityServerThumbprint;
		if (string.IsNullOrEmpty(identityEndpoint) || string.IsNullOrEmpty(identityHeader) || string.IsNullOrEmpty(identityServerThumbprint))
		{
			return null;
		}
		if (!Uri.TryCreate(identityEndpoint, UriKind.Absolute, out Uri result))
		{
			throw new AuthenticationFailedException("The environment variable IDENTITY_ENDPOINT contains an invalid Uri.");
		}
		CredentialPipeline credentialPipeline = options.Pipeline;
		if (!options.PreserveTransport)
		{
			HttpPipeline httpPipeline = HttpPipelineBuilder.Build(new TokenCredentialOptions
			{
				Transport = GetServiceFabricMITransport()
			});
			credentialPipeline = new CredentialPipeline(credentialPipeline.AuthorityHost, httpPipeline, credentialPipeline.Diagnostics);
		}
		return new ServiceFabricManagedIdentitySource(credentialPipeline, result, identityHeader, options.ClientId);
	}

	internal static HttpClientTransport GetServiceFabricMITransport()
	{
		return new HttpClientTransport(new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback = ValidateMsiServerCertificate
		});
	}

	private ServiceFabricManagedIdentitySource(CredentialPipeline pipeline, Uri endpoint, string identityHeaderValue, string clientId)
		: base(pipeline)
	{
		_endpoint = endpoint;
		_identityHeaderValue = identityHeaderValue;
		_clientId = clientId;
	}

	protected override Request CreateRequest(string[] scopes)
	{
		string value = ScopeUtilities.ScopesToResource(scopes);
		Request request = base.Pipeline.HttpPipeline.CreateRequest();
		request.Method = RequestMethod.Get;
		request.Headers.Add("secret", _identityHeaderValue);
		request.Uri.Reset(_endpoint);
		request.Uri.AppendQuery("api-version", "2019-07-01-preview");
		request.Uri.AppendQuery("resource", value);
		if (!string.IsNullOrEmpty(_clientId))
		{
			request.Uri.AppendQuery("client_id", _clientId);
		}
		return request;
	}

	private static bool ValidateMsiServerCertificate(HttpRequestMessage message, X509Certificate2 cert, X509Chain certChain, SslPolicyErrors policyErrors)
	{
		if (policyErrors == SslPolicyErrors.None)
		{
			return true;
		}
		return string.Compare(cert.GetCertHashString(), EnvironmentVariables.IdentityServerThumbprint, StringComparison.OrdinalIgnoreCase) == 0;
	}
}
