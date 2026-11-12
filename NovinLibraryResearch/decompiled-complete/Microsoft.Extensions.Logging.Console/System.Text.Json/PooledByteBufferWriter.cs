using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Text.Json;

internal sealed class PooledByteBufferWriter : IBufferWriter<byte>, IDisposable
{
	private byte[] _rentedBuffer;

	private int _index;

	private const int MinimumBufferSize = 256;

	public ReadOnlyMemory<byte> WrittenMemory => MemoryExtensions.AsMemory(_rentedBuffer, 0, _index);

	public int WrittenCount => _index;

	public int Capacity => _rentedBuffer.Length;

	public int FreeCapacity => _rentedBuffer.Length - _index;

	public PooledByteBufferWriter(int initialCapacity)
	{
		_rentedBuffer = ArrayPool<byte>.Shared.Rent(initialCapacity);
		_index = 0;
	}

	public void Clear()
	{
		ClearHelper();
	}

	private void ClearHelper()
	{
		MemoryExtensions.AsSpan(_rentedBuffer, 0, _index).Clear();
		_index = 0;
	}

	public void Dispose()
	{
		if (_rentedBuffer != null)
		{
			ClearHelper();
			byte[] rentedBuffer = _rentedBuffer;
			_rentedBuffer = null;
			ArrayPool<byte>.Shared.Return(rentedBuffer);
		}
	}

	public void Advance(int count)
	{
		_index += count;
	}

	public Memory<byte> GetMemory(int sizeHint = 0)
	{
		CheckAndResizeBuffer(sizeHint);
		return MemoryExtensions.AsMemory(_rentedBuffer, _index);
	}

	public Span<byte> GetSpan(int sizeHint = 0)
	{
		CheckAndResizeBuffer(sizeHint);
		return MemoryExtensions.AsSpan(_rentedBuffer, _index);
	}

	internal Task WriteToStreamAsync(Stream destination, CancellationToken cancellationToken)
	{
		return destination.WriteAsync(_rentedBuffer, 0, _index, cancellationToken);
	}

	internal void WriteToStream(Stream destination)
	{
		destination.Write(_rentedBuffer, 0, _index);
	}

	private void CheckAndResizeBuffer(int sizeHint)
	{
		if (sizeHint == 0)
		{
			sizeHint = 256;
		}
		int num = _rentedBuffer.Length - _index;
		if (sizeHint <= num)
		{
			return;
		}
		int num2 = _rentedBuffer.Length;
		int num3 = Math.Max(sizeHint, num2);
		int num4 = num2 + num3;
		if ((uint)num4 > 2147483647u)
		{
			num4 = num2 + sizeHint;
			if ((uint)num4 > 2147483647u)
			{
				System.Text.Json.ThrowHelper.ThrowOutOfMemoryException_BufferMaximumSizeExceeded((uint)num4);
			}
		}
		byte[] rentedBuffer = _rentedBuffer;
		_rentedBuffer = ArrayPool<byte>.Shared.Rent(num4);
		Span<byte> span = MemoryExtensions.AsSpan(rentedBuffer, 0, _index);
		span.CopyTo(_rentedBuffer);
		span.Clear();
		ArrayPool<byte>.Shared.Return(rentedBuffer);
	}
}
