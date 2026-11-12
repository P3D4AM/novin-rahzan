namespace Azure.Core.Pipeline;

internal class ClientRequestIdPolicy : HttpPipelineSynchronousPolicy
{
	internal const string ClientRequestIdHeader = "x-ms-client-request-id";

	internal const string EchoClientRequestId = "x-ms-return-client-request-id";

	public static ClientRequestIdPolicy Shared { get; } = new ClientRequestIdPolicy();


	protected ClientRequestIdPolicy()
	{
	}

	public override void OnSendingRequest(HttpMessage message)
	{
		message.Request.Headers.SetValue("x-ms-client-request-id", message.Request.ClientRequestId);
		message.Request.Headers.SetValue("x-ms-return-client-request-id", "true");
	}
}
