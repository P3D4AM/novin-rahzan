using System;
using System.Threading.Tasks;

namespace Azure.Core.Pipeline;

public abstract class HttpPipelinePolicy
{
	public abstract ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline);

	public abstract void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline);

	protected static ValueTask ProcessNextAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		return pipeline.Span[0].ProcessAsync(message, pipeline.Slice(1));
	}

	protected static void ProcessNext(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		pipeline.Span[0].Process(message, pipeline.Slice(1));
	}
}
