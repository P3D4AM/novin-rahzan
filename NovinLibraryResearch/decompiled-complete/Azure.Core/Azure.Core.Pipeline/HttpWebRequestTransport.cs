using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Azure.Core.Pipeline;

internal class HttpWebRequestTransport : HttpPipelineTransport
{
	private sealed class HttpWebResponseImplementation : Response
	{
		private readonly HttpWebResponse _webResponse;

		private Stream? _contentStream;

		private Stream? _originalContentStream;

		public override int Status => (int)_webResponse.StatusCode;

		public override string ReasonPhrase => _webResponse.StatusDescription;

		public override Stream? ContentStream
		{
			get
			{
				return _contentStream;
			}
			set
			{
				_originalContentStream = null;
				_contentStream = value;
			}
		}

		public override string ClientRequestId { get; set; }

		public HttpWebResponseImplementation(string clientRequestId, HttpWebResponse webResponse)
		{
			_webResponse = webResponse;
			_originalContentStream = _webResponse.GetResponseStream();
			_contentStream = _originalContentStream;
			ClientRequestId = clientRequestId;
		}

		public override void Dispose()
		{
			_originalContentStream?.Dispose();
		}

		protected internal override bool TryGetHeader(string name, [NotNullWhen(true)] out string? value)
		{
			value = _webResponse.Headers.Get(name);
			return value != null;
		}

		protected internal override bool TryGetHeaderValues(string name, [NotNullWhen(true)] out IEnumerable<string>? values)
		{
			values = _webResponse.Headers.GetValues(name);
			return values != null;
		}

		protected internal override bool ContainsHeader(string name)
		{
			return _webResponse.Headers.Get(name) != null;
		}

		protected internal override IEnumerable<HttpHeader> EnumerateHeaders()
		{
			string[] allKeys = _webResponse.Headers.AllKeys;
			foreach (string name in allKeys)
			{
				yield return new HttpHeader(name, _webResponse.Headers.Get(name));
			}
		}
	}

	private sealed class HttpWebRequestImplementation : Request
	{
		private string? _clientRequestId;

		private readonly Dictionary<string, List<string>> _headers = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

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

		public override RequestContent? Content { get; set; }

		public HttpWebRequestImplementation()
		{
			Method = RequestMethod.Get;
		}

		protected internal override void AddHeader(string name, string value)
		{
			if (!_headers.TryGetValue(name, out List<string> value2))
			{
				value2 = (_headers[name] = new List<string>());
			}
			value2.Add(value);
		}

		protected internal override bool TryGetHeader(string name, [NotNullWhen(true)] out string? value)
		{
			if (_headers.TryGetValue(name, out List<string> value2))
			{
				value = JoinHeaderValue(value2);
				return true;
			}
			value = null;
			return false;
		}

		protected internal override bool TryGetHeaderValues(string name, out IEnumerable<string> values)
		{
			List<string> value;
			bool result = _headers.TryGetValue(name, out value);
			values = value;
			return result;
		}

		protected internal override bool ContainsHeader(string name)
		{
			IEnumerable<string> values;
			return TryGetHeaderValues(name, out values);
		}

		protected internal override bool RemoveHeader(string name)
		{
			return _headers.Remove(name);
		}

		protected internal override IEnumerable<HttpHeader> EnumerateHeaders()
		{
			return _headers.Select<KeyValuePair<string, List<string>>, HttpHeader>((KeyValuePair<string, List<string>> h) => new HttpHeader(h.Key, JoinHeaderValue(h.Value)));
		}

		private static string JoinHeaderValue(IEnumerable<string> values)
		{
			return string.Join(",", values);
		}

		public override void Dispose()
		{
			Content?.Dispose();
		}
	}

	public static readonly HttpWebRequestTransport Shared = new HttpWebRequestTransport();

	private readonly IWebProxy? _proxy;

	public HttpWebRequestTransport()
	{
		if (HttpEnvironmentProxy.TryCreate(out var proxy))
		{
			_proxy = proxy;
		}
	}

	public override void Process(HttpMessage message)
	{
		ProcessInternal(message, async: false).EnsureCompleted();
	}

	public override async ValueTask ProcessAsync(HttpMessage message)
	{
		await ProcessInternal(message, async: true).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async ValueTask ProcessInternal(HttpMessage message, bool async)
	{
		HttpWebRequest request = CreateRequest(message.Request);
		ServicePointHelpers.SetLimits(request.ServicePoint);
		using (message.CancellationToken.Register(delegate(object state)
		{
			((HttpWebRequest)state).Abort();
		}, request))
		{
			_ = 2;
			try
			{
				if (message.Request.Content != null)
				{
					if (request.ContentLength == -1 && message.Request.Content.TryComputeLength(out var length))
					{
						request.ContentLength = length;
					}
					Stream stream = ((!async) ? request.GetRequestStream() : (await request.GetRequestStreamAsync().ConfigureAwait(continueOnCapturedContext: false)));
					using Stream requestStream = stream;
					if (async)
					{
						await message.Request.Content.WriteToAsync(requestStream, message.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					}
					else
					{
						message.Request.Content.WriteTo(requestStream, message.CancellationToken);
					}
				}
				else
				{
					request.ContentLength = 0L;
				}
				WebResponse webResponse2;
				try
				{
					WebResponse webResponse = ((!async) ? request.GetResponse() : (await request.GetResponseAsync().ConfigureAwait(continueOnCapturedContext: false)));
					webResponse2 = webResponse;
				}
				catch (WebException ex) when (ex.Response != null)
				{
					webResponse2 = ex.Response;
				}
				message.Response = new HttpWebResponseImplementation(message.Request.ClientRequestId, (HttpWebResponse)webResponse2);
			}
			catch (WebException) when (message.CancellationToken.IsCancellationRequested)
			{
				throw new TaskCanceledException();
			}
			catch (WebException ex3)
			{
				throw new RequestFailedException(0, ex3.Message);
			}
		}
	}

	private HttpWebRequest CreateRequest(Request messageRequest)
	{
		HttpWebRequest httpWebRequest = WebRequest.CreateHttp(messageRequest.Uri.ToUri());
		httpWebRequest.Method = messageRequest.Method.Method;
		httpWebRequest.Proxy = _proxy;
		foreach (HttpHeader header in messageRequest.Headers)
		{
			if (string.Equals(header.Name, HttpHeader.Names.ContentLength, StringComparison.OrdinalIgnoreCase))
			{
				httpWebRequest.ContentLength = long.Parse(header.Value, CultureInfo.InvariantCulture);
			}
			else if (string.Equals(header.Name, HttpHeader.Names.Host, StringComparison.OrdinalIgnoreCase))
			{
				httpWebRequest.Host = header.Value;
			}
			else if (string.Equals(header.Name, HttpHeader.Names.Date, StringComparison.OrdinalIgnoreCase))
			{
				httpWebRequest.Date = DateTime.Parse(header.Value, CultureInfo.InvariantCulture);
			}
			else if (string.Equals(header.Name, HttpHeader.Names.ContentType, StringComparison.OrdinalIgnoreCase))
			{
				httpWebRequest.ContentType = header.Value;
			}
			else if (string.Equals(header.Name, HttpHeader.Names.UserAgent, StringComparison.OrdinalIgnoreCase))
			{
				httpWebRequest.UserAgent = header.Value;
			}
			else if (string.Equals(header.Name, HttpHeader.Names.Accept, StringComparison.OrdinalIgnoreCase))
			{
				httpWebRequest.Accept = header.Value;
			}
			else if (string.Equals(header.Name, HttpHeader.Names.Referer, StringComparison.OrdinalIgnoreCase))
			{
				httpWebRequest.Referer = header.Value;
			}
			else if (string.Equals(header.Name, HttpHeader.Names.Range, StringComparison.OrdinalIgnoreCase))
			{
				RangeHeaderValue obj = RangeHeaderValue.Parse(header.Value);
				if (obj.Unit != "bytes")
				{
					throw new InvalidOperationException("Only ranges with bytes unit supported.");
				}
				foreach (RangeItemHeaderValue range in obj.Ranges)
				{
					if (!range.From.HasValue)
					{
						throw new InvalidOperationException("Only ranges with Offset supported.");
					}
					if (!range.To.HasValue)
					{
						httpWebRequest.AddRange(range.From.Value);
					}
					else
					{
						httpWebRequest.AddRange(range.From.Value, range.To.Value);
					}
				}
			}
			else
			{
				httpWebRequest.Headers.Add(header.Name, header.Value);
			}
		}
		if (httpWebRequest.ContentLength != -1)
		{
			httpWebRequest.AllowWriteStreamBuffering = false;
		}
		return httpWebRequest;
	}

	public override Request CreateRequest()
	{
		return new HttpWebRequestImplementation();
	}
}
