using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Identity;

internal class ImdsManagedIdentitySource : ManagedIdentitySource
{
	private static readonly Uri s_imdsEndpoint = new Uri("http://169.254.169.254/metadata/identity/oauth2/token");

	private static readonly IPAddress s_imdsHostIp = IPAddress.Parse("169.254.169.254");

	private const int s_imdsPort = 80;

	private const int ImdsAvailableTimeoutMs = 1000;

	private const string ImdsApiVersion = "2018-02-01";

	internal const string IdentityUnavailableError = "ManagedIdentityCredential authentication unavailable. The requested identity has not been assigned to this resource.";

	private readonly string _clientId;

	private string _identityUnavailableErrorMessage;

	public static async ValueTask<ManagedIdentitySource> TryCreateAsync(ManagedIdentityClientOptions options, bool async, CancellationToken cancellationToken)
	{
		AzureIdentityEventSource.Singleton.ProbeImdsEndpoint(s_imdsEndpoint);
		bool flag;
		try
		{
			using TcpClient client = new TcpClient();
			Task task = client.ConnectAsync(s_imdsHostIp, 80);
			if (async)
			{
				using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { cancellationToken });
				cts.CancelAfter(1000);
				await task.AwaitWithCancellation(cts.Token);
				flag = client.Connected;
			}
			else
			{
				flag = task.Wait(1000, cancellationToken) && client.Connected;
			}
		}
		catch
		{
			flag = false;
		}
		if (flag)
		{
			AzureIdentityEventSource.Singleton.ImdsEndpointFound(s_imdsEndpoint);
		}
		else
		{
			AzureIdentityEventSource.Singleton.ImdsEndpointUnavailable(s_imdsEndpoint);
		}
		return flag ? new ImdsManagedIdentitySource(options.Pipeline, options.ClientId) : null;
	}

	internal ImdsManagedIdentitySource(CredentialPipeline pipeline, string clientId)
		: base(pipeline)
	{
		_clientId = clientId;
	}

	protected override Request CreateRequest(string[] scopes)
	{
		if (_identityUnavailableErrorMessage != null)
		{
			throw new CredentialUnavailableException(_identityUnavailableErrorMessage);
		}
		string value = ScopeUtilities.ScopesToResource(scopes);
		Request request = base.Pipeline.HttpPipeline.CreateRequest();
		request.Method = RequestMethod.Get;
		request.Headers.Add("Metadata", "true");
		request.Uri.Reset(s_imdsEndpoint);
		request.Uri.AppendQuery("api-version", "2018-02-01");
		request.Uri.AppendQuery("resource", value);
		if (!string.IsNullOrEmpty(_clientId))
		{
			request.Uri.AppendQuery("client_id", _clientId);
		}
		return request;
	}

	protected override async ValueTask<AccessToken> HandleResponseAsync(bool async, TokenRequestContext context, Response response, CancellationToken cancellationToken)
	{
		if (response.Status == 400)
		{
			string text = _identityUnavailableErrorMessage;
			if (text == null)
			{
				text = await base.Pipeline.Diagnostics.CreateRequestFailedMessageAsync(response, "ManagedIdentityCredential authentication unavailable. The requested identity has not been assigned to this resource.", null, null, async).ConfigureAwait(continueOnCapturedContext: false);
			}
			string text2 = text;
			Interlocked.CompareExchange(ref _identityUnavailableErrorMessage, text2, null);
			throw new CredentialUnavailableException(text2);
		}
		return await base.HandleResponseAsync(async, context, response, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}
}
