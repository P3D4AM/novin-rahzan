using System;
using Azure.Core;

namespace Azure.Identity;

internal class AppServiceV2017ManagedIdentitySource : ManagedIdentitySource
{
	private const string AppServiceMsiApiVersion = "2017-09-01";

	private const string MsiEndpointInvalidUriError = "The environment variable MSI_ENDPOINT contains an invalid Uri.";

	private readonly Uri _endpoint;

	private readonly string _secret;

	private readonly string _clientId;

	public static ManagedIdentitySource TryCreate(ManagedIdentityClientOptions options)
	{
		string msiEndpoint = EnvironmentVariables.MsiEndpoint;
		string msiSecret = EnvironmentVariables.MsiSecret;
		if (string.IsNullOrEmpty(msiEndpoint) || string.IsNullOrEmpty(msiSecret))
		{
			return null;
		}
		Uri endpoint;
		try
		{
			endpoint = new Uri(msiEndpoint);
		}
		catch (FormatException innerException)
		{
			throw new AuthenticationFailedException("The environment variable MSI_ENDPOINT contains an invalid Uri.", innerException);
		}
		return new AppServiceV2017ManagedIdentitySource(options.Pipeline, endpoint, msiSecret, options.ClientId);
	}

	private AppServiceV2017ManagedIdentitySource(CredentialPipeline pipeline, Uri endpoint, string secret, string clientId)
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
		request.Headers.Add("secret", _secret);
		request.Uri.Reset(_endpoint);
		request.Uri.AppendQuery("api-version", "2017-09-01");
		request.Uri.AppendQuery("resource", value);
		if (!string.IsNullOrEmpty(_clientId))
		{
			request.Uri.AppendQuery("clientid", _clientId);
		}
		return request;
	}
}
