using System;
using System.Threading.Tasks;

namespace Azure.Core.Pipeline;

internal class HttpPipelineTransportPolicy : HttpPipelinePolicy
{
	private readonly HttpPipelineTransport _transport;

	public HttpPipelineTransportPolicy(HttpPipelineTransport transport)
	{
		_transport = transport;
	}

	public override ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		return _transport.ProcessAsync(message);
	}

	public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		_transport.Process(message);
	}
}
