using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Identity;

internal static class HttpExtensions
{
	public static async Task<Request> ToPipelineRequestAsync(this HttpRequestMessage request, HttpPipeline pipeline)
	{
		Request pipelineRequest = pipeline.CreateRequest();
		pipelineRequest.Method = RequestMethod.Parse(request.Method.Method);
		pipelineRequest.Uri.Reset(request.RequestUri);
		Request request2 = pipelineRequest;
		request2.Content = await request.Content.ToPipelineRequestContentAsync().ConfigureAwait(continueOnCapturedContext: false);
		foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
		{
			foreach (string item in header.Value)
			{
				pipelineRequest.Headers.Add(header.Key, item);
			}
		}
		return pipelineRequest;
	}

	private static void AddHeader(HttpResponseMessage request, HttpHeader header)
	{
		if (request.Headers.TryAddWithoutValidation(header.Name, header.Value) || request.Content.Headers.TryAddWithoutValidation(header.Name, header.Value))
		{
			return;
		}
		throw new InvalidOperationException("Unable to add header to request or content");
	}

	public static HttpResponseMessage ToHttpResponseMessage(this Response response)
	{
		HttpResponseMessage httpResponseMessage = new HttpResponseMessage
		{
			StatusCode = (HttpStatusCode)response.Status,
			Content = new StreamContent(response.ContentStream)
		};
		foreach (HttpHeader header in response.Headers)
		{
			if (!httpResponseMessage.Headers.TryAddWithoutValidation(header.Name, header.Value) && !httpResponseMessage.Content.Headers.TryAddWithoutValidation(header.Name, header.Value))
			{
				throw new InvalidOperationException("Unable to add header to request or content");
			}
		}
		return httpResponseMessage;
	}

	public static async Task<RequestContent> ToPipelineRequestContentAsync(this HttpContent content)
	{
		if (content != null)
		{
			return RequestContent.Create(await content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false));
		}
		return null;
	}
}
