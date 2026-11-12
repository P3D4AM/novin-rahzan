using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal;

internal class AccessTokenHttpMessageHandler : DelegatingHandler
{
	private readonly HttpConnection _httpConnection;

	public AccessTokenHttpMessageHandler(HttpMessageHandler inner, HttpConnection httpConnection)
		: base(inner)
	{
		_httpConnection = httpConnection;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		string text = await _httpConnection.GetAccessTokenAsync();
		if (!string.IsNullOrEmpty(text))
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", text);
		}
		return await _003C_003En__0(request, cancellationToken);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<HttpResponseMessage> _003C_003En__0(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return ((DelegatingHandler)this).SendAsync(request, cancellationToken);
	}
}
