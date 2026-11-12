using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Microsoft.Identity.Client;

namespace Azure.Identity;

internal class HttpPipelineClientFactory : IMsalHttpClientFactory
{
	private class PipelineHttpMessageHandler : HttpMessageHandler
	{
		private readonly HttpPipeline _pipeline;

		public PipelineHttpMessageHandler(HttpPipeline pipeline)
		{
			_pipeline = pipeline;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			Request request2 = await request.ToPipelineRequestAsync(_pipeline).ConfigureAwait(continueOnCapturedContext: false);
			return (await _pipeline.SendRequestAsync(request2, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).ToHttpResponseMessage();
		}
	}

	private readonly HttpPipeline _pipeline;

	public HttpPipelineClientFactory(HttpPipeline pipeline)
	{
		_pipeline = pipeline;
	}

	public HttpClient GetHttpClient()
	{
		return new HttpClient(new PipelineHttpMessageHandler(_pipeline));
	}
}
