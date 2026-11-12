using System;
using Azure.Core;

namespace Azure.Identity;

internal class AppServiceV2019ManagedIdentitySource : ManagedIdentitySource
{
	private const string AppServiceMsiApiVersion = "2019-08-01";

	private const string IdentityEndpointInvalidUriError = "The environment variable IDENTITY_ENDPOINT contains an invalid Uri.";

	private readonly Uri _endpoint;

	private readonly string _secret;

	private readonly string _clientId;

	public static ManagedIdentitySource TryCreate(CredentialPipeline pipeline, string clientId)
	{
		string identityEndpoint = EnvironmentVariables.IdentityEndpoint;
		string identityHeader = EnvironmentVariables.IdentityHeader;
		if (string.IsNullOrEmpty(identityEndpoint) || string.IsNullOrEmpty(identityHeader))
		{
			return null;
		}
		Uri endpoint;
		try
		{
			endpoint = new Uri(identityEndpoint);
		}
		catch (FormatException innerException)
		{
			throw new AuthenticationFailedException("The environment variable IDENTITY_ENDPOINT contains an invalid Uri.", innerException);
		}
		return new AppServiceV2019ManagedIdentitySource(pipeline, endpoint, identityHeader, clientId);
	}

	private AppServiceV2019ManagedIdentitySource(CredentialPipeline pipeline, Uri endpoint, string secret, string clientId)
		: base(pipeline)
	{
		_endpoint = endpoint;
		_secret = secret;
		_clientId = clientId;
	}

	protected override Request CreateRequest(string[] scopes)
	{
		string value = ScopeUtilities.ScopesToResource(scopes);
		Request request = base.Pipeline.HttpPipeline.CreateRequest();
		request.Method = RequestMethod.Get;
		request.Headers.Add("X-IDENTITY-HEADER", _secret);
		request.Uri.Reset(_endpoint);
		request.Uri.AppendQuery("api-version", "2019-08-01");
		request.Uri.AppendQuery("resource", value);
		if (!string.IsNullOrEmpty(_clientId))
		{
			request.Uri.AppendQuery("client_id", _clientId);
		}
		return request;
	}
}
