using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Azure.Identity;

internal class AzureArcManagedIdentitySource : ManagedIdentitySource
{
	private const string IdentityEndpointInvalidUriError = "The environment variable IDENTITY_ENDPOINT contains an invalid Uri.";

	private const string NoChallengeErrorMessage = "Did not receive expected WWW-Authenticate header in the response from Azure Arc Managed Identity Endpoint.";

	private const string InvalidChallangeErrorMessage = "The WWW-Authenticate header in the response from Azure Arc Managed Identity Endpoint did not match the expected format.";

	private const string UserAssignedNotSupportedErrorMessage = "User assigned identity is not supported by the Azure Arc Managed Identity Endpoint. To authenticate with the system assigned identity omit the client id when constructing the ManagedIdentityCredential, or if authenticating with the DefaultAzureCredential ensure the AZURE_CLIENT_ID environment variable is not set.";

	private const string ArcApiVersion = "2019-11-01";

	private readonly string _clientId;

	private readonly Uri _endpoint;

	public static ManagedIdentitySource TryCreate(ManagedIdentityClientOptions options)
	{
		string identityEndpoint = EnvironmentVariables.IdentityEndpoint;
		string imdsEndpoint = EnvironmentVariables.ImdsEndpoint;
		if (string.IsNullOrEmpty(identityEndpoint) || string.IsNullOrEmpty(imdsEndpoint))
		{
			return null;
		}
		if (!Uri.TryCreate(identityEndpoint, UriKind.Absolute, out Uri result))
		{
			throw new AuthenticationFailedException("The environment variable IDENTITY_ENDPOINT contains an invalid Uri.");
		}
		return new AzureArcManagedIdentitySource(options.Pipeline, result, options.ClientId);
	}

	private AzureArcManagedIdentitySource(CredentialPipeline pipeline, Uri endpoint, string clientId)
		: base(pipeline)
	{
		_endpoint = endpoint;
		_clientId = clientId;
	}

	protected override Request CreateRequest(string[] scopes)
	{
		if (!string.IsNullOrEmpty(_clientId))
		{
			throw new AuthenticationFailedException("User assigned identity is not supported by the Azure Arc Managed Identity Endpoint. To authenticate with the system assigned identity omit the client id when constructing the ManagedIdentityCredential, or if authenticating with the DefaultAzureCredential ensure the AZURE_CLIENT_ID environment variable is not set.");
		}
		string value = ScopeUtilities.ScopesToResource(scopes);
		Request request = base.Pipeline.HttpPipeline.CreateRequest();
		request.Method = RequestMethod.Get;
		request.Headers.Add("Metadata", "true");
		request.Uri.Reset(_endpoint);
		request.Uri.AppendQuery("api-version", "2019-11-01");
		request.Uri.AppendQuery("resource", value);
		return request;
	}

	protected override async ValueTask<AccessToken> HandleResponseAsync(bool async, TokenRequestContext context, Response response, CancellationToken cancellationToken)
	{
		if (response.Status == 401)
		{
			if (!response.Headers.TryGetValue("WWW-Authenticate", out string value))
			{
				throw new AuthenticationFailedException("Did not receive expected WWW-Authenticate header in the response from Azure Arc Managed Identity Endpoint.");
			}
			string[] array = value.Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length != 2)
			{
				throw new AuthenticationFailedException("The WWW-Authenticate header in the response from Azure Arc Managed Identity Endpoint did not match the expected format.");
			}
			string value2 = "Basic " + File.ReadAllText(array[1]);
			using Request request = CreateRequest(context.Scopes);
			request.Headers.Add("Authorization", value2);
			Response response2 = ((!async) ? base.Pipeline.HttpPipeline.SendRequest(request, cancellationToken) : (await base.Pipeline.HttpPipeline.SendRequestAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)));
			response = response2;
			return await base.HandleResponseAsync(async, context, response, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		return await base.HandleResponseAsync(async, context, response, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}
}
