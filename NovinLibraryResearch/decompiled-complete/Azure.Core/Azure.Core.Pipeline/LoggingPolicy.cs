using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Diagnostics;

namespace Azure.Core.Pipeline;

internal class LoggingPolicy : HttpPipelinePolicy
{
	private class LoggingStream : ReadOnlyStream
	{
		private readonly string _requestId;

		private readonly ContentEventSourceWrapper _eventSourceWrapper;

		private int _maxLoggedBytes;

		private readonly Stream _originalStream;

		private readonly bool _error;

		private readonly Encoding? _textEncoding;

		private int _blockNumber;

		public override bool CanRead => _originalStream.CanRead;

		public override bool CanSeek => _originalStream.CanSeek;

		public override long Length => _originalStream.Length;

		public override long Position
		{
			get
			{
				return _originalStream.Position;
			}
			set
			{
				_originalStream.Position = value;
			}
		}

		public LoggingStream(string requestId, ContentEventSourceWrapper eventSourceWrapper, int maxLoggedBytes, Stream originalStream, bool error, Encoding? textEncoding)
		{
			_requestId = requestId;
			_eventSourceWrapper = eventSourceWrapper;
			_maxLoggedBytes = maxLoggedBytes;
			_originalStream = originalStream;
			_error = error;
			_textEncoding = textEncoding;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _originalStream.Seek(offset, origin);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int count2;
			int result = (count2 = _originalStream.Read(buffer, offset, count));
			DecrementLength(ref count2);
			LogBuffer(buffer, offset, count2);
			return result;
		}

		private void LogBuffer(byte[] buffer, int offset, int count)
		{
			if (count != 0)
			{
				_eventSourceWrapper.Log(_requestId, _error, buffer, offset, count, _textEncoding, _blockNumber);
				_blockNumber++;
			}
		}

		public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			int count2;
			int result = (count2 = await _originalStream.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			DecrementLength(ref count2);
			LogBuffer(buffer, offset, count2);
			return result;
		}

		public override void Close()
		{
			_originalStream.Close();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			_originalStream.Dispose();
		}

		private void DecrementLength(ref int count)
		{
			int num = Math.Min(count, _maxLoggedBytes);
			count = num;
			_maxLoggedBytes -= count;
		}
	}

	private readonly struct ContentEventSourceWrapper
	{
		private enum EventType
		{
			Request,
			Response,
			ErrorResponse
		}

		private class MaxLengthStream : MemoryStream
		{
			private int _bytesLeft;

			public MaxLengthStream(int maxLength)
			{
				_bytesLeft = maxLength;
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				DecrementLength(ref count);
				if (count > 0)
				{
					base.Write(buffer, offset, count);
				}
			}

			public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
			{
				if (count <= 0)
				{
					return Task.CompletedTask;
				}
				return base.WriteAsync(buffer, offset, count, cancellationToken);
			}

			private void DecrementLength(ref int count)
			{
				int num = Math.Min(count, _bytesLeft);
				count = num;
				_bytesLeft -= count;
			}
		}

		private const int CopyBufferSize = 8192;

		private readonly AzureCoreEventSource? _eventSource;

		private readonly int _maxLength;

		private readonly CancellationToken _cancellationToken;

		public ContentEventSourceWrapper(AzureCoreEventSource eventSource, bool logContent, int maxLength, CancellationToken cancellationToken)
		{
			_eventSource = (logContent ? eventSource : null);
			_maxLength = maxLength;
			_cancellationToken = cancellationToken;
		}

		public async ValueTask LogAsync(string requestId, bool isError, Stream? stream, Encoding? textEncoding, bool async)
		{
			EventType eventType = ResponseOrError(isError);
			if (stream != null && IsEnabled(eventType))
			{
				Log(requestId, eventType, await FormatAsync(stream, async).ConfigureAwait(continueOnCapturedContext: false).EnsureCompleted(async), textEncoding);
			}
		}

		public async ValueTask LogAsync(string requestId, RequestContent? content, Encoding? textEncoding, bool async)
		{
			EventType eventType = EventType.Request;
			if (content != null && IsEnabled(eventType))
			{
				Log(requestId, eventType, await FormatAsync(content, async).ConfigureAwait(continueOnCapturedContext: false).EnsureCompleted(async), textEncoding);
			}
		}

		public void Log(string requestId, bool isError, byte[] buffer, int offset, int length, Encoding? textEncoding, int? block = null)
		{
			EventType eventType = ResponseOrError(isError);
			if (buffer != null && IsEnabled(eventType))
			{
				int num = Math.Min(length, _maxLength);
				byte[] array;
				if (length == num && offset == 0)
				{
					array = buffer;
				}
				else
				{
					array = new byte[num];
					Array.Copy(buffer, offset, array, 0, num);
				}
				Log(requestId, eventType, array, textEncoding, block);
			}
		}

		public bool IsEnabled(bool isError)
		{
			return IsEnabled(ResponseOrError(isError));
		}

		private bool IsEnabled(EventType errorResponse)
		{
			if (_eventSource != null)
			{
				if (!_eventSource.IsEnabled(EventLevel.Informational, EventKeywords.All))
				{
					if (errorResponse == EventType.ErrorResponse)
					{
						return _eventSource.IsEnabled(EventLevel.Warning, EventKeywords.All);
					}
					return false;
				}
				return true;
			}
			return false;
		}

		private async ValueTask<byte[]> FormatAsync(RequestContent requestContent, bool async)
		{
			using MaxLengthStream memoryStream = new MaxLengthStream(_maxLength);
			if (async)
			{
				await requestContent.WriteToAsync(memoryStream, _cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				requestContent.WriteTo(memoryStream, _cancellationToken);
			}
			return memoryStream.ToArray();
		}

		private async ValueTask<byte[]> FormatAsync(Stream content, bool async)
		{
			using MaxLengthStream memoryStream = new MaxLengthStream(_maxLength);
			if (async)
			{
				await content.CopyToAsync(memoryStream, 8192, _cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				content.CopyTo(memoryStream);
			}
			content.Seek(0L, SeekOrigin.Begin);
			return memoryStream.ToArray();
		}

		private void Log(string requestId, EventType eventType, byte[] bytes, Encoding? textEncoding, int? block = null)
		{
			string text = textEncoding?.GetString(bytes);
			AzureCoreEventSource eventSource = _eventSource;
			switch (eventType)
			{
			case EventType.Request:
				if (text != null)
				{
					eventSource.RequestContentText(requestId, text);
				}
				else
				{
					eventSource.RequestContent(requestId, bytes);
				}
				break;
			case EventType.Response:
				if (block.HasValue && text != null)
				{
					eventSource.ResponseContentTextBlock(requestId, block.Value, text);
				}
				else if (block.HasValue)
				{
					eventSource.ResponseContentBlock(requestId, block.Value, bytes);
				}
				else if (text != null)
				{
					eventSource.ResponseContentText(requestId, text);
				}
				else
				{
					eventSource.ResponseContent(requestId, bytes);
				}
				break;
			case EventType.ErrorResponse:
				if (block.HasValue && text != null)
				{
					eventSource.ErrorResponseContentTextBlock(requestId, block.Value, text);
				}
				else if (block.HasValue)
				{
					eventSource.ErrorResponseContentBlock(requestId, block.Value, bytes);
				}
				else if (text != null)
				{
					eventSource.ErrorResponseContentText(requestId, text);
				}
				else
				{
					eventSource.ErrorResponseContent(requestId, bytes);
				}
				break;
			}
		}

		private static EventType ResponseOrError(bool isError)
		{
			if (!isError)
			{
				return EventType.Response;
			}
			return EventType.ErrorResponse;
		}
	}

	private const double RequestTooLongTime = 3.0;

	private static readonly AzureCoreEventSource s_eventSource = AzureCoreEventSource.Singleton;

	private readonly bool _logContent;

	private readonly int _maxLength;

	private HttpMessageSanitizer _sanitizer;

	private readonly string? _assemblyName;

	public LoggingPolicy(bool logContent, int maxLength, string[] allowedHeaderNames, string[] allowedQueryParameters, string? assemblyName)
	{
		_sanitizer = new HttpMessageSanitizer(allowedQueryParameters, allowedHeaderNames);
		_logContent = logContent;
		_maxLength = maxLength;
		_assemblyName = assemblyName;
	}

	public override async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		await ProcessAsync(message, pipeline, async: true).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, bool async)
	{
		if (!s_eventSource.IsEnabled())
		{
			if (async)
			{
				await HttpPipelinePolicy.ProcessNextAsync(message, pipeline).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				HttpPipelinePolicy.ProcessNext(message, pipeline);
			}
			return;
		}
		Request request = message.Request;
		s_eventSource.Request(request.ClientRequestId, request.Method.ToString(), FormatUri(request.Uri), FormatHeaders(request.Headers), _assemblyName);
		Encoding encoding = null;
		if (request.TryGetHeader(HttpHeader.Names.ContentType, out string value))
		{
			ContentTypeUtilities.TryGetTextEncoding(value, out encoding);
		}
		ContentEventSourceWrapper logWrapper = new ContentEventSourceWrapper(s_eventSource, _logContent, _maxLength, message.CancellationToken);
		await logWrapper.LogAsync(request.ClientRequestId, request.Content, encoding, async).ConfigureAwait(continueOnCapturedContext: false).EnsureCompleted(async);
		long before = Stopwatch.GetTimestamp();
		try
		{
			if (async)
			{
				await HttpPipelinePolicy.ProcessNextAsync(message, pipeline).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				HttpPipelinePolicy.ProcessNext(message, pipeline);
			}
		}
		catch (Exception ex)
		{
			s_eventSource.ExceptionResponse(request.ClientRequestId, ex.ToString());
			throw;
		}
		long timestamp = Stopwatch.GetTimestamp();
		bool flag = message.ResponseClassifier.IsErrorResponse(message);
		Response response = message.Response;
		ContentTypeUtilities.TryGetTextEncoding(response.Headers.ContentType, out var encoding2);
		int num;
		if (response.ContentStream != null)
		{
			Stream? contentStream = response.ContentStream;
			if (contentStream != null && !contentStream.CanSeek)
			{
				num = (logWrapper.IsEnabled(flag) ? 1 : 0);
				goto IL_035f;
			}
		}
		num = 0;
		goto IL_035f;
		IL_035f:
		double elapsed = (double)(timestamp - before) / (double)Stopwatch.Frequency;
		if (flag)
		{
			s_eventSource.ErrorResponse(response.ClientRequestId, response.Status, response.ReasonPhrase, FormatHeaders(response.Headers), elapsed);
		}
		else
		{
			s_eventSource.Response(response.ClientRequestId, response.Status, response.ReasonPhrase, FormatHeaders(response.Headers), elapsed);
		}
		if (num == 0)
		{
			await logWrapper.LogAsync(response.ClientRequestId, flag, response.ContentStream, encoding2, async).ConfigureAwait(continueOnCapturedContext: false).EnsureCompleted(async);
		}
		else
		{
			response.ContentStream = new LoggingStream(response.ClientRequestId, logWrapper, _maxLength, response.ContentStream, flag, encoding2);
		}
		if (elapsed > 3.0)
		{
			s_eventSource.ResponseDelay(response.ClientRequestId, elapsed);
		}
	}

	private string FormatUri(RequestUriBuilder requestUri)
	{
		return _sanitizer.SanitizeUrl(requestUri.ToString());
	}

	public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		ProcessAsync(message, pipeline, async: false).EnsureCompleted();
	}

	private string FormatHeaders(IEnumerable<HttpHeader> headers)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (HttpHeader header in headers)
		{
			stringBuilder.Append(header.Name);
			stringBuilder.Append(':');
			string value = _sanitizer.SanitizeHeader(header.Name, header.Value);
			stringBuilder.AppendLine(value);
		}
		return stringBuilder.ToString();
	}
}
