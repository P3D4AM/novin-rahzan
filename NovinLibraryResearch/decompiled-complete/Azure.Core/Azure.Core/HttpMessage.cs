using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Azure.Core;

public sealed class HttpMessage : IDisposable
{
	private class ResponseShouldNotBeUsedStream : Stream
	{
		public Stream Original { get; }

		public override bool CanRead
		{
			get
			{
				throw CreateException();
			}
		}

		public override bool CanSeek
		{
			get
			{
				throw CreateException();
			}
		}

		public override bool CanWrite
		{
			get
			{
				throw CreateException();
			}
		}

		public override long Length
		{
			get
			{
				throw CreateException();
			}
		}

		public override long Position
		{
			get
			{
				throw CreateException();
			}
			set
			{
				throw CreateException();
			}
		}

		public ResponseShouldNotBeUsedStream(Stream original)
		{
			Original = original;
		}

		private static Exception CreateException()
		{
			return new InvalidOperationException("The operation has called ExtractResponseContent and will provide the stream as part of its response type.");
		}

		public override void Flush()
		{
			throw CreateException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw CreateException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw CreateException();
		}

		public override void SetLength(long value)
		{
			throw CreateException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw CreateException();
		}
	}

	private Dictionary<string, object>? _properties;

	private Response? _response;

	public Request Request { get; }

	public Response Response
	{
		get
		{
			if (_response == null)
			{
				throw new InvalidOperationException("Response was not set, make sure SendAsync was called");
			}
			return _response;
		}
		set
		{
			_response = value;
		}
	}

	public bool HasResponse => _response != null;

	public CancellationToken CancellationToken { get; internal set; }

	public ResponseClassifier ResponseClassifier { get; }

	public bool BufferResponse { get; set; }

	public HttpMessage(Request request, ResponseClassifier responseClassifier)
	{
		Request = request;
		ResponseClassifier = responseClassifier;
		BufferResponse = true;
	}

	public bool TryGetProperty(string name, out object? value)
	{
		value = null;
		return _properties?.TryGetValue(name, out value) ?? false;
	}

	public void SetProperty(string name, object value)
	{
		if (_properties == null)
		{
			_properties = new Dictionary<string, object>();
		}
		_properties[name] = value;
	}

	public Stream? ExtractResponseContent()
	{
		Stream stream = _response?.ContentStream;
		if (!(stream is ResponseShouldNotBeUsedStream responseShouldNotBeUsedStream))
		{
			if (stream != null)
			{
				_response.ContentStream = new ResponseShouldNotBeUsedStream(_response.ContentStream);
				return stream;
			}
			return null;
		}
		return responseShouldNotBeUsedStream.Original;
	}

	public void Dispose()
	{
		Request?.Dispose();
		_response?.Dispose();
	}
}
