using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Core.Pipeline;

public class HttpClientTransport : HttpPipelineTransport
{
	private sealed class PipelineRequest : Request
	{
		private sealed class PipelineContentAdapter : HttpContent
		{
			public RequestContent? PipelineContent { get; set; }

			public CancellationToken CancellationToken { get; set; }

			protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
			{
				await PipelineContent.WriteToAsync(stream, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}

			protected override bool TryComputeLength(out long length)
			{
				return PipelineContent.TryComputeLength(out length);
			}
		}

		private bool _wasSent;

		private readonly HttpRequestMessage _requestMessage;

		private PipelineContentAdapter? _requestContent;

		private string? _clientRequestId;

		private static readonly HttpMethod s_patch = new HttpMethod("PATCH");

		public override RequestMethod Method
		{
			get
			{
				return RequestMethod.Parse(_requestMessage.Method.Method);
			}
			set
			{
				_requestMessage.Method = ToHttpClientMethod(value);
			}
		}

		public override RequestContent? Content { get; set; }

		public override string ClientRequestId
		{
			get
			{
				return _clientRequestId ?? (_clientRequestId = Guid.NewGuid().ToString());
			}
			set
			{
				Argument.AssertNotNull(value, "value");
				_clientRequestId = value;
			}
		}

		public PipelineRequest()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			_requestMessage = new HttpRequestMessage();
		}

		protected internal override void AddHeader(string name, string value)
		{
			if (((HttpHeaders)_requestMessage.Headers).TryAddWithoutValidation(name, value) || ((HttpHeaders)((HttpContent)EnsureContentInitialized()).Headers).TryAddWithoutValidation(name, value))
			{
				return;
			}
			throw new InvalidOperationException("Unable to add header to request or content");
		}

		protected internal override bool TryGetHeader(string name, [NotNullWhen(true)] out string? value)
		{
			return HttpClientTransport.TryGetHeader((HttpHeaders)(object)_requestMessage.Headers, (HttpContent?)(object)_requestContent, name, out value);
		}

		protected internal override bool TryGetHeaderValues(string name, [NotNullWhen(true)] out IEnumerable<string>? values)
		{
			return HttpClientTransport.TryGetHeader((HttpHeaders)(object)_requestMessage.Headers, (HttpContent?)(object)_requestContent, name, out values);
		}

		protected internal override bool ContainsHeader(string name)
		{
			return HttpClientTransport.ContainsHeader((HttpHeaders)(object)_requestMessage.Headers, (HttpContent?)(object)_requestContent, name);
		}

		protected internal override bool RemoveHeader(string name)
		{
			return HttpClientTransport.RemoveHeader((HttpHeaders)(object)_requestMessage.Headers, (HttpContent?)(object)_requestContent, name);
		}

		protected internal override IEnumerable<HttpHeader> EnumerateHeaders()
		{
			return GetHeaders((HttpHeaders)(object)_requestMessage.Headers, (HttpContent?)(object)_requestContent);
		}

		public HttpRequestMessage BuildRequestMessage(CancellationToken cancellation)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			HttpRequestMessage val;
			if (_wasSent)
			{
				val = new HttpRequestMessage(_requestMessage.Method, Uri.ToUri());
				CopyHeaders((HttpHeaders)(object)_requestMessage.Headers, (HttpHeaders)(object)val.Headers);
			}
			else
			{
				val = _requestMessage;
			}
			val.RequestUri = Uri.ToUri();
			if (Content != null)
			{
				PipelineContentAdapter pipelineContentAdapter;
				if (_wasSent)
				{
					pipelineContentAdapter = new PipelineContentAdapter();
					CopyHeaders((HttpHeaders)(object)((HttpContent)_requestContent).Headers, (HttpHeaders)(object)((HttpContent)pipelineContentAdapter).Headers);
				}
				else
				{
					pipelineContentAdapter = EnsureContentInitialized();
				}
				pipelineContentAdapter.CancellationToken = cancellation;
				pipelineContentAdapter.PipelineContent = Content;
				val.Content = (HttpContent)(object)pipelineContentAdapter;
			}
			_wasSent = true;
			return val;
		}

		public override void Dispose()
		{
			Content?.Dispose();
			_requestMessage.Dispose();
		}

		public override string ToString()
		{
			return ((object)_requestMessage).ToString();
		}

		private static HttpMethod ToHttpClientMethod(RequestMethod requestMethod)
		{
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			string method = requestMethod.Method;
			if (method.Length == 3)
			{
				if (string.Equals(method, "GET", StringComparison.OrdinalIgnoreCase))
				{
					return HttpMethod.Get;
				}
				if (string.Equals(method, "PUT", StringComparison.OrdinalIgnoreCase))
				{
					return HttpMethod.Put;
				}
			}
			else if (method.Length == 4)
			{
				if (string.Equals(method, "POST", StringComparison.OrdinalIgnoreCase))
				{
					return HttpMethod.Post;
				}
				if (string.Equals(method, "HEAD", StringComparison.OrdinalIgnoreCase))
				{
					return HttpMethod.Head;
				}
			}
			else
			{
				if (string.Equals(method, "PATCH", StringComparison.OrdinalIgnoreCase))
				{
					return s_patch;
				}
				if (string.Equals(method, "DELETE", StringComparison.OrdinalIgnoreCase))
				{
					return HttpMethod.Delete;
				}
			}
			return new HttpMethod(method);
		}

		private PipelineContentAdapter EnsureContentInitialized()
		{
			if (_requestContent == null)
			{
				_requestContent = new PipelineContentAdapter();
			}
			return _requestContent;
		}
	}

	private sealed class PipelineResponse : Response
	{
		private readonly HttpResponseMessage _responseMessage;

		private readonly HttpContent _responseContent;

		private Stream? _contentStream;

		public override int Status => (int)_responseMessage.StatusCode;

		public override string ReasonPhrase => _responseMessage.ReasonPhrase;

		public override Stream? ContentStream
		{
			get
			{
				return _contentStream;
			}
			set
			{
				_responseMessage.Content = null;
				_contentStream = value;
			}
		}

		public override string ClientRequestId { get; set; }

		public PipelineResponse(string requestId, HttpResponseMessage responseMessage, Stream? contentStream)
		{
			ClientRequestId = requestId ?? throw new ArgumentNullException("requestId");
			_responseMessage = responseMessage ?? throw new ArgumentNullException("responseMessage");
			_contentStream = contentStream;
			_responseContent = _responseMessage.Content;
		}

		protected internal override bool TryGetHeader(string name, [NotNullWhen(true)] out string? value)
		{
			return HttpClientTransport.TryGetHeader((HttpHeaders)(object)_responseMessage.Headers, _responseContent, name, out value);
		}

		protected internal override bool TryGetHeaderValues(string name, [NotNullWhen(true)] out IEnumerable<string>? values)
		{
			return HttpClientTransport.TryGetHeader((HttpHeaders)(object)_responseMessage.Headers, _responseContent, name, out values);
		}

		protected internal override bool ContainsHeader(string name)
		{
			return HttpClientTransport.ContainsHeader((HttpHeaders)(object)_responseMessage.Headers, _responseContent, name);
		}

		protected internal override IEnumerable<HttpHeader> EnumerateHeaders()
		{
			return GetHeaders((HttpHeaders)(object)_responseMessage.Headers, _responseContent);
		}

		public override void Dispose()
		{
			HttpResponseMessage responseMessage = _responseMessage;
			if (responseMessage != null)
			{
				responseMessage.Dispose();
			}
		}

		public override string ToString()
		{
			return ((object)_responseMessage).ToString();
		}
	}

	private readonly HttpClient _client;

	public static readonly HttpClientTransport Shared = new HttpClientTransport();

	public HttpClientTransport()
		: this(CreateDefaultClient())
	{
	}

	public HttpClientTransport(HttpMessageHandler messageHandler)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		HttpClient val = new HttpClient(messageHandler);
		if ((int)val == 0)
		{
			throw new ArgumentNullException("messageHandler");
		}
		_client = val;
	}

	public HttpClientTransport(HttpClient client)
	{
		_client = client ?? throw new ArgumentNullException("client");
	}

	public sealed override Request CreateRequest()
	{
		return new PipelineRequest();
	}

	public override void Process(HttpMessage message)
	{
		ProcessAsync(message).GetAwaiter().GetResult();
	}

	public sealed override async ValueTask ProcessAsync(HttpMessage message)
	{
		HttpRequestMessage httpRequest = BuildRequestMessage(message);
		try
		{
			Stream contentStream = null;
			HttpResponseMessage responseMessage;
			try
			{
				responseMessage = await _client.SendAsync(httpRequest, (HttpCompletionOption)1, message.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (responseMessage.Content != null)
				{
					contentStream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			catch (HttpRequestException val)
			{
				HttpRequestException val2 = val;
				throw new RequestFailedException(((Exception)(object)val2).Message, (Exception?)(object)val2);
			}
			message.Response = new PipelineResponse(message.Request.ClientRequestId, responseMessage, contentStream);
		}
		finally
		{
			((IDisposable)httpRequest)?.Dispose();
		}
	}

	private static HttpClient CreateDefaultClient()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		HttpClientHandler val = new HttpClientHandler();
		if (HttpEnvironmentProxy.TryCreate(out var proxy))
		{
			val.Proxy = proxy;
		}
		ServicePointHelpers.SetLimits(val);
		return new HttpClient((HttpMessageHandler)(object)val);
	}

	private static HttpRequestMessage BuildRequestMessage(HttpMessage message)
	{
		return ((message.Request as PipelineRequest) ?? throw new InvalidOperationException("the request is not compatible with the transport")).BuildRequestMessage(message.CancellationToken);
	}

	internal static bool TryGetHeader(HttpHeaders headers, HttpContent? content, string name, [NotNullWhen(true)] out string? value)
	{
		if (TryGetHeader(headers, content, name, out IEnumerable<string> values))
		{
			value = JoinHeaderValues(values);
			return true;
		}
		value = null;
		return false;
	}

	internal static bool TryGetHeader(HttpHeaders headers, HttpContent? content, string name, [NotNullWhen(true)] out IEnumerable<string>? values)
	{
		if (!headers.TryGetValues(name, ref values))
		{
			if (content == null)
			{
				return false;
			}
			return ((HttpHeaders)content.Headers).TryGetValues(name, ref values);
		}
		return true;
	}

	internal static IEnumerable<HttpHeader> GetHeaders(HttpHeaders headers, HttpContent? content)
	{
		foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
		{
			yield return new HttpHeader(header.Key, JoinHeaderValues(header.Value));
		}
		if (content == null)
		{
			yield break;
		}
		foreach (KeyValuePair<string, IEnumerable<string>> item in (HttpHeaders)content.Headers)
		{
			yield return new HttpHeader(item.Key, JoinHeaderValues(item.Value));
		}
	}

	internal static bool RemoveHeader(HttpHeaders headers, HttpContent? content, string name)
	{
		IEnumerable<string> enumerable = default(IEnumerable<string>);
		if (headers.TryGetValues(name, ref enumerable) && headers.Remove(name))
		{
			return true;
		}
		if (content != null && ((HttpHeaders)content.Headers).TryGetValues(name, ref enumerable))
		{
			return ((HttpHeaders)content.Headers).Remove(name);
		}
		return false;
	}

	internal static bool ContainsHeader(HttpHeaders headers, HttpContent? content, string name)
	{
		IEnumerable<string> enumerable = default(IEnumerable<string>);
		if (headers.TryGetValues(name, ref enumerable))
		{
			return true;
		}
		if (content == null)
		{
			return false;
		}
		return ((HttpHeaders)content.Headers).TryGetValues(name, ref enumerable);
	}

	internal static void CopyHeaders(HttpHeaders from, HttpHeaders to)
	{
		foreach (KeyValuePair<string, IEnumerable<string>> item in from)
		{
			if (!to.TryAddWithoutValidation(item.Key, item.Value))
			{
				throw new InvalidOperationException($"Unable to add header {item} to header collection.");
			}
		}
	}

	private static string JoinHeaderValues(IEnumerable<string> values)
	{
		return string.Join(",", values);
	}
}
