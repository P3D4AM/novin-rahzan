using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Internal;

internal sealed class MemoryBufferWriter : Stream, IBufferWriter<byte>
{
	private readonly struct CompletedBuffer
	{
		public byte[] Buffer { get; }

		public int Length { get; }

		public ReadOnlySpan<byte> Span => MemoryExtensions.AsSpan(Buffer, 0, Length);

		public CompletedBuffer(byte[] buffer, int length)
		{
			Buffer = buffer;
			Length = length;
		}

		public void Return()
		{
			ArrayPool<byte>.Shared.Return(Buffer);
		}
	}

	[ThreadStatic]
	private static MemoryBufferWriter _cachedInstance;

	private readonly int _minimumSegmentSize;

	private int _bytesWritten;

	private List<CompletedBuffer> _completedSegments;

	private byte[] _currentSegment;

	private int _position;

	public override long Length => _bytesWritten;

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

	public override long Position
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public MemoryBufferWriter(int minimumSegmentSize = 4096)
	{
		_minimumSegmentSize = minimumSegmentSize;
	}

	public static MemoryBufferWriter Get()
	{
		MemoryBufferWriter memoryBufferWriter = _cachedInstance;
		if (memoryBufferWriter == null)
		{
			memoryBufferWriter = new MemoryBufferWriter();
		}
		else
		{
			_cachedInstance = null;
		}
		return memoryBufferWriter;
	}

	public static void Return(MemoryBufferWriter writer)
	{
		_cachedInstance = writer;
		writer.Reset();
	}

	public void Reset()
	{
		if (_completedSegments != null)
		{
			for (int i = 0; i < _completedSegments.Count; i++)
			{
				_completedSegments[i].Return();
			}
			_completedSegments.Clear();
		}
		if (_currentSegment != null)
		{
			ArrayPool<byte>.Shared.Return(_currentSegment);
			_currentSegment = null;
		}
		_bytesWritten = 0;
		_position = 0;
	}

	public void Advance(int count)
	{
		_bytesWritten += count;
		_position += count;
	}

	public Memory<byte> GetMemory(int sizeHint = 0)
	{
		EnsureCapacity(sizeHint);
		return MemoryExtensions.AsMemory(_currentSegment, _position, _currentSegment.Length - _position);
	}

	public Span<byte> GetSpan(int sizeHint = 0)
	{
		EnsureCapacity(sizeHint);
		return MemoryExtensions.AsSpan(_currentSegment, _position, _currentSegment.Length - _position);
	}

	public void CopyTo(IBufferWriter<byte> destination)
	{
		if (_completedSegments != null)
		{
			int count = _completedSegments.Count;
			for (int i = 0; i < count; i++)
			{
				destination.Write(_completedSegments[i].Span);
			}
		}
		destination.Write(MemoryExtensions.AsSpan(_currentSegment, 0, _position));
	}

	public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
	{
		if (_completedSegments == null && _currentSegment != null)
		{
			return destination.WriteAsync(_currentSegment, 0, _position, cancellationToken);
		}
		return CopyToSlowAsync(destination, cancellationToken);
	}

	[MemberNotNull("_currentSegment")]
	private void EnsureCapacity(int sizeHint)
	{
		int valueOrDefault = (_currentSegment?.Length - _position).GetValueOrDefault();
		if ((sizeHint != 0 || valueOrDefault <= 0) && (sizeHint <= 0 || valueOrDefault < sizeHint))
		{
			AddSegment(sizeHint);
		}
	}

	[MemberNotNull("_currentSegment")]
	private void AddSegment(int sizeHint = 0)
	{
		if (_currentSegment != null)
		{
			if (_completedSegments == null)
			{
				_completedSegments = new List<CompletedBuffer>();
			}
			_completedSegments.Add(new CompletedBuffer(_currentSegment, _position));
		}
		_currentSegment = ArrayPool<byte>.Shared.Rent(Math.Max(_minimumSegmentSize, sizeHint));
		_position = 0;
	}

	private async Task CopyToSlowAsync(Stream destination, CancellationToken cancellationToken)
	{
		if (_completedSegments != null)
		{
			int count = _completedSegments.Count;
			for (int i = 0; i < count; i++)
			{
				CompletedBuffer completedBuffer = _completedSegments[i];
				await destination.WriteAsync(completedBuffer.Buffer, 0, completedBuffer.Length, cancellationToken);
			}
		}
		if (_currentSegment != null)
		{
			await destination.WriteAsync(_currentSegment, 0, _position, cancellationToken);
		}
	}

	public byte[] ToArray()
	{
		if (_currentSegment == null)
		{
			return Array.Empty<byte>();
		}
		byte[] array = new byte[_bytesWritten];
		int num = 0;
		if (_completedSegments != null)
		{
			int count = _completedSegments.Count;
			for (int i = 0; i < count; i++)
			{
				CompletedBuffer completedBuffer = _completedSegments[i];
				completedBuffer.Span.CopyTo(MemoryExtensions.AsSpan(array, num));
				num += completedBuffer.Span.Length;
			}
		}
		MemoryExtensions.AsSpan(_currentSegment, 0, _position).CopyTo(MemoryExtensions.AsSpan(array, num));
		return array;
	}

	public void CopyTo(Span<byte> span)
	{
		if (_currentSegment == null)
		{
			return;
		}
		int num = 0;
		if (_completedSegments != null)
		{
			int count = _completedSegments.Count;
			for (int i = 0; i < count; i++)
			{
				CompletedBuffer completedBuffer = _completedSegments[i];
				completedBuffer.Span.CopyTo(span.Slice(num));
				num += completedBuffer.Span.Length;
			}
		}
		MemoryExtensions.AsSpan(_currentSegment, 0, _position).CopyTo(span.Slice(num));
	}

	public override void Flush()
	{
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void WriteByte(byte value)
	{
		if (_currentSegment != null && (uint)_position < (uint)_currentSegment.Length)
		{
			_currentSegment[_position] = value;
		}
		else
		{
			AddSegment();
			_currentSegment[0] = value;
		}
		_position++;
		_bytesWritten++;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		int position = _position;
		if (_currentSegment != null && position < _currentSegment.Length - count)
		{
			Buffer.BlockCopy(buffer, offset, _currentSegment, position, count);
			_position = position + count;
			_bytesWritten += count;
		}
		else
		{
			this.Write(MemoryExtensions.AsSpan(buffer, offset, count));
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			Reset();
		}
	}
}
