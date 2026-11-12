using System;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Core.Pipeline;

public class HttpPipeline
{
	private readonly HttpPipelineTransport _transport;

	private readonly ReadOnlyMemory<HttpPipelinePolicy> _pipeline;

	public ResponseClassifier ResponseClassifier { get; }

	public HttpPipeline(HttpPipelineTransport transport, HttpPipelinePolicy[]? policies = null, ResponseClassifier? responseClassifier = null)
	{
		_transport = transport ?? throw new ArgumentNullException("transport");
		ResponseClassifier = responseClassifier ?? new ResponseClassifier();
		policies = policies ?? Array.Empty<HttpPipelinePolicy>();
		HttpPipelinePolicy[] array = new HttpPipelinePolicy[policies.Length + 1];
		array[policies.Length] = new HttpPipelineTransportPolicy(_transport);
		policies.CopyTo(array, 0);
		_pipeline = array;
	}

	public Request CreateRequest()
	{
		return _transport.CreateRequest();
	}

	public HttpMessage CreateMessage()
	{
		return new HttpMessage(CreateRequest(), ResponseClassifier);
	}

	public ValueTask SendAsync(HttpMessage message, CancellationToken cancellationToken)
	{
		message.CancellationToken = cancellationToken;
		return _pipeline.Span[0].ProcessAsync(message, _pipeline.Slice(1));
	}

	public void Send(HttpMessage message, CancellationToken cancellationToken)
	{
		message.CancellationToken = cancellationToken;
		_pipeline.Span[0].Process(message, _pipeline.Slice(1));
	}

	public async ValueTask<Response> SendRequestAsync(Request request, CancellationToken cancellationToken)
	{
		HttpMessage message = new HttpMessage(request, ResponseClassifier);
		await SendAsync(message, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		return message.Response;
	}

	public Response SendRequest(Request request, CancellationToken cancellationToken)
	{
		HttpMessage httpMessage = new HttpMessage(request, ResponseClassifier);
		Send(httpMessage, cancellationToken);
		return httpMessage.Response;
	}

	public static IDisposable CreateClientRequestIdScope(string? clientRequestId)
	{
		return ReadClientRequestIdPolicy.StartScope(clientRequestId);
	}
}
