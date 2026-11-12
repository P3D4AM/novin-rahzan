using System;
using System.Buffers;

namespace Azure.Core;

internal sealed class ArrayBufferWriter<T> : IBufferWriter<T>
{
	private T[] _buffer;

	private const int DefaultInitialBufferSize = 256;

	public ReadOnlyMemory<T> WrittenMemory => MemoryExtensions.AsMemory(_buffer, 0, WrittenCount);

	public ReadOnlySpan<T> WrittenSpan => MemoryExtensions.AsSpan(_buffer, 0, WrittenCount);

	public int WrittenCount { get; private set; }

	public int Capacity => _buffer.Length;

	public int FreeCapacity => _buffer.Length - WrittenCount;

	public ArrayBufferWriter()
	{
		_buffer = Array.Empty<T>();
		WrittenCount = 0;
	}

	public ArrayBufferWriter(int initialCapacity)
	{
		if (initialCapacity <= 0)
		{
			throw new ArgumentException("initialCapacity");
		}
		_buffer = new T[initialCapacity];
		WrittenCount = 0;
	}

	public void Clear()
	{
		MemoryExtensions.AsSpan(_buffer, 0, WrittenCount).Clear();
		WrittenCount = 0;
	}

	public void Advance(int count)
	{
		if (count < 0)
		{
			throw new ArgumentException("count");
		}
		if (WrittenCount > _buffer.Length - count)
		{
			ThrowInvalidOperationException_AdvancedTooFar(_buffer.Length);
		}
		WrittenCount += count;
	}

	public Memory<T> GetMemory(int sizeHint = 0)
	{
		CheckAndResizeBuffer(sizeHint);
		return MemoryExtensions.AsMemory(_buffer, WrittenCount);
	}

	public Span<T> GetSpan(int sizeHint = 0)
	{
		CheckAndResizeBuffer(sizeHint);
		return MemoryExtensions.AsSpan(_buffer, WrittenCount);
	}

	private void CheckAndResizeBuffer(int sizeHint)
	{
		if (sizeHint < 0)
		{
			throw new ArgumentException("sizeHint");
		}
		if (sizeHint == 0)
		{
			sizeHint = 1;
		}
		if (sizeHint > FreeCapacity)
		{
			int num = Math.Max(sizeHint, _buffer.Length);
			if (_buffer.Length == 0)
			{
				num = Math.Max(num, 256);
			}
			int newSize = checked(_buffer.Length + num);
			Array.Resize(ref _buffer, newSize);
		}
	}

	private static void ThrowInvalidOperationException_AdvancedTooFar(int capacity)
	{
		throw new InvalidOperationException($"Advanced past capacity of {capacity}");
	}
}
