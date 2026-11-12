using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Buffers;

namespace Azure.Core;

public abstract class RequestContent : IDisposable
{
	private sealed class StreamContent : RequestContent
	{
		private const int CopyToBufferSize = 81920;

		private readonly Stream _stream;

		private readonly long _origin;

		public StreamContent(Stream stream)
		{
			if (!stream.CanSeek)
			{
				throw new ArgumentException("stream must be seekable", "stream");
			}
			_origin = stream.Position;
			_stream = stream;
		}

		public override void WriteTo(Stream stream, CancellationToken cancellationToken)
		{
			_stream.Seek(_origin, SeekOrigin.Begin);
			byte[] array = ArrayPool<byte>.Shared.Rent(81920);
			try
			{
				while (true)
				{
					cancellationToken.ThrowIfCancellationRequested();
					int num = _stream.Read(array, 0, array.Length);
					if (num == 0)
					{
						break;
					}
					cancellationToken.ThrowIfCancellationRequested();
					stream.Write(array, 0, num);
				}
			}
			finally
			{
				stream.Flush();
				ArrayPool<byte>.Shared.Return(array, clearArray: true);
			}
		}

		public override bool TryComputeLength(out long length)
		{
			if (_stream.CanSeek)
			{
				length = _stream.Length - _origin;
				return true;
			}
			length = 0L;
			return false;
		}

		public override async Task WriteToAsync(Stream stream, CancellationToken cancellation)
		{
			_stream.Seek(_origin, SeekOrigin.Begin);
			await _stream.CopyToAsync(stream, 81920, cancellation).ConfigureAwait(continueOnCapturedContext: false);
		}

		public override void Dispose()
		{
			_stream.Dispose();
		}
	}

	private sealed class ArrayContent : RequestContent
	{
		private readonly byte[] _bytes;

		private readonly int _contentStart;

		private readonly int _contentLength;

		public ArrayContent(byte[] bytes, int index, int length)
		{
			_bytes = bytes;
			_contentStart = index;
			_contentLength = length;
		}

		public override void Dispose()
		{
		}

		public override void WriteTo(Stream stream, CancellationToken cancellation)
		{
			stream.Write(_bytes, _contentStart, _contentLength);
		}

		public override bool TryComputeLength(out long length)
		{
			length = _contentLength;
			return true;
		}

		public override async Task WriteToAsync(Stream stream, CancellationToken cancellation)
		{
			await stream.WriteAsync(_bytes, _contentStart, _contentLength, cancellation).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private sealed class MemoryContent : RequestContent
	{
		private readonly ReadOnlyMemory<byte> _bytes;

		public MemoryContent(ReadOnlyMemory<byte> bytes)
		{
			_bytes = bytes;
		}

		public override void Dispose()
		{
		}

		public override void WriteTo(Stream stream, CancellationToken cancellation)
		{
			byte[] array = _bytes.ToArray();
			stream.Write(array, 0, array.Length);
		}

		public override bool TryComputeLength(out long length)
		{
			length = _bytes.Length;
			return true;
		}

		public override async Task WriteToAsync(Stream stream, CancellationToken cancellation)
		{
			await stream.WriteAsync(_bytes, cancellation).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private sealed class ReadOnlySequenceContent : RequestContent
	{
		private readonly ReadOnlySequence<byte> _bytes;

		public ReadOnlySequenceContent(ReadOnlySequence<byte> bytes)
		{
			_bytes = bytes;
		}

		public override void Dispose()
		{
		}

		public override void WriteTo(Stream stream, CancellationToken cancellation)
		{
			byte[] array = BuffersExtensions.ToArray(in _bytes);
			stream.Write(array, 0, array.Length);
		}

		public override bool TryComputeLength(out long length)
		{
			length = _bytes.Length;
			return true;
		}

		public override async Task WriteToAsync(Stream stream, CancellationToken cancellation)
		{
			await stream.WriteAsync(_bytes, cancellation).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	public static RequestContent Create(Stream stream)
	{
		return new StreamContent(stream);
	}

	public static RequestContent Create(byte[] bytes)
	{
		return new ArrayContent(bytes, 0, bytes.Length);
	}

	public static RequestContent Create(byte[] bytes, int index, int length)
	{
		return new ArrayContent(bytes, index, length);
	}

	public static RequestContent Create(ReadOnlyMemory<byte> bytes)
	{
		return new MemoryContent(bytes);
	}

	public static RequestContent Create(ReadOnlySequence<byte> bytes)
	{
		return new ReadOnlySequenceContent(bytes);
	}

	public abstract Task WriteToAsync(Stream stream, CancellationToken cancellation);

	public abstract void WriteTo(Stream stream, CancellationToken cancellation);

	public abstract bool TryComputeLength(out long length);

	public abstract void Dispose();
}
