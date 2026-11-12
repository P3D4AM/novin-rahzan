using System;
using System.Threading;

namespace Azure.Core.Pipeline;

internal class ReadClientRequestIdPolicy : HttpPipelineSynchronousPolicy
{
	private class ClientRequestIdScope : IDisposable
	{
		private readonly ClientRequestIdScope? _parent;

		public string? ClientRequestId { get; }

		internal ClientRequestIdScope(string? clientRequestId, ClientRequestIdScope? parent)
		{
			ClientRequestId = clientRequestId;
			_parent = parent;
		}

		public void Dispose()
		{
			CurrentRequestIdScope.Value = _parent;
		}
	}

	private static readonly AsyncLocal<ClientRequestIdScope?> CurrentRequestIdScope = new AsyncLocal<ClientRequestIdScope>();

	public static ReadClientRequestIdPolicy Shared { get; } = new ReadClientRequestIdPolicy();


	protected ReadClientRequestIdPolicy()
	{
	}

	public override void OnSendingRequest(HttpMessage message)
	{
		if (message.Request.Headers.TryGetValue("x-ms-client-request-id", out string value))
		{
			message.Request.ClientRequestId = value;
		}
		else if (CurrentRequestIdScope.Value?.ClientRequestId != null)
		{
			message.Request.ClientRequestId = CurrentRequestIdScope.Value.ClientRequestId;
		}
	}

	internal static IDisposable StartScope(string? clientRequestId)
	{
		CurrentRequestIdScope.Value = new ClientRequestIdScope(clientRequestId, CurrentRequestIdScope.Value);
		return CurrentRequestIdScope.Value;
	}
}
