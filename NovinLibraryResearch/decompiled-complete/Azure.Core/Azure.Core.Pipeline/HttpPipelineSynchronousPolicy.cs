using System;
using System.Threading.Tasks;

namespace Azure.Core.Pipeline;

public abstract class HttpPipelineSynchronousPolicy : HttpPipelinePolicy
{
	public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		OnSendingRequest(message);
		HttpPipelinePolicy.ProcessNext(message, pipeline);
		OnReceivedResponse(message);
	}

	public override async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		OnSendingRequest(message);
		await HttpPipelinePolicy.ProcessNextAsync(message, pipeline).ConfigureAwait(continueOnCapturedContext: false);
		OnReceivedResponse(message);
	}

	public virtual void OnSendingRequest(HttpMessage message)
	{
	}

	public virtual void OnReceivedResponse(HttpMessage message)
	{
	}
}
