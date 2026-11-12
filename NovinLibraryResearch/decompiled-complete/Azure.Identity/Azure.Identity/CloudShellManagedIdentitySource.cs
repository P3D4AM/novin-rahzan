using System;
using System.Text;
using Azure.Core;

namespace Azure.Identity;

internal class CloudShellManagedIdentitySource : ManagedIdentitySource
{
	private readonly Uri _endpoint;

	private readonly string _clientId;

	private const string MsiEndpointInvalidUriError = "The environment variable MSI_ENDPOINT contains an invalid Uri.";

	public static ManagedIdentitySource TryCreate(ManagedIdentityClientOptions options)
	{
		string msiEndpoint = EnvironmentVariables.MsiEndpoint;
		if (string.IsNullOrEmpty(msiEndpoint))
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
		return new CloudShellManagedIdentitySource(options.Pipeline, endpoint, options.ClientId);
	}

	private CloudShellManagedIdentitySource(CredentialPipeline pipeline, Uri endpoint, string clientId)
		: base(pipeline)
	{
		_endpoint = endpoint;
		_clientId = clientId;
	}

	protected override Request CreateRequest(string[] scopes)
	{
		string stringToEscape = ScopeUtilities.ScopesToResource(scopes);
		Request request = base.Pipeline.HttpPipeline.CreateRequest();
		request.Method = RequestMethod.Post;
		request.Headers.Add(HttpHeader.Common.FormUrlEncodedContentType);
		request.Uri.Reset(_endpoint);
		request.Headers.Add("Metadata", "true");
		string text = "resource=" + Uri.EscapeDataString(stringToEscape);
		if (!string.IsNullOrEmpty(_clientId))
		{
			text = text + "&client_id=" + Uri.EscapeDataString(_clientId);
		}
		ReadOnlyMemory<byte> bytes = MemoryExtensions.AsMemory(Encoding.UTF8.GetBytes(text));
		request.Content = RequestContent.Create(bytes);
		return request;
	}
}
