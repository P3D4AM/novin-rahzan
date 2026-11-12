using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.WebUtilities;

public class HttpResponseStreamWriter : TextWriter
{
	private const int MinBufferSize = 128;

	internal const int DefaultBufferSize = 16384;

	private Stream _stream;

	private readonly Encoder _encoder;

	private readonly ArrayPool<byte> _bytePool;

	private readonly ArrayPool<char> _charPool;

	private readonly int _charBufferSize;

	private byte[] _byteBuffer;

	private char[] _charBuffer;

	private int _charBufferCount;

	private bool _disposed;

	public override Encoding Encoding { get; }

	public HttpResponseStreamWriter(Stream stream, Encoding encoding)
		: this(stream, encoding, 16384, ArrayPool<byte>.Shared, ArrayPool<char>.Shared)
	{
	}

	public HttpResponseStreamWriter(Stream stream, Encoding encoding, int bufferSize)
		: this(stream, encoding, bufferSize, ArrayPool<byte>.Shared, ArrayPool<char>.Shared)
	{
	}

	public HttpResponseStreamWriter(Stream stream, Encoding encoding, int bufferSize, ArrayPool<byte> bytePool, ArrayPool<char> charPool)
	{
		_stream = stream ?? throw new ArgumentNullException("stream");
		Encoding = encoding ?? throw new ArgumentNullException("encoding");
		_bytePool = bytePool ?? throw new ArgumentNullException("bytePool");
		_charPool = charPool ?? throw new ArgumentNullException("charPool");
		if (bufferSize <= 0)
		{
			throw new ArgumentOutOfRangeException("bufferSize");
		}
		if (!_stream.CanWrite)
		{
			throw new ArgumentException(Resources.HttpResponseStreamWriter_StreamNotWritable, "stream");
		}
		_charBufferSize = bufferSize;
		_encoder = encoding.GetEncoder();
		_charBuffer = charPool.Rent(bufferSize);
		try
		{
			int maxByteCount = encoding.GetMaxByteCount(bufferSize);
			_byteBuffer = bytePool.Rent(maxByteCount);
		}
		catch
		{
			charPool.Return(_charBuffer);
			if (_byteBuffer != null)
			{
				bytePool.Return(_byteBuffer);
			}
			throw;
		}
	}

	public override void Write(char value)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("HttpResponseStreamWriter");
		}
		if (_charBufferCount == _charBufferSize)
		{
			FlushInternal(flushEncoder: false);
		}
		_charBuffer[_charBufferCount] = value;
		_charBufferCount++;
	}

	public override void Write(char[] values, int index, int count)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("HttpResponseStreamWriter");
		}
		if (values == null)
		{
			return;
		}
		while (count > 0)
		{
			if (_charBufferCount == _charBufferSize)
			{
				FlushInternal(flushEncoder: false);
			}
			CopyToCharBuffer(values, ref index, ref count);
		}
	}

	public override void Write(string value)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("HttpResponseStreamWriter");
		}
		if (value == null)
		{
			return;
		}
		int count = value.Length;
		int index = 0;
		while (count > 0)
		{
			if (_charBufferCount == _charBufferSize)
			{
				FlushInternal(flushEncoder: false);
			}
			CopyToCharBuffer(value, ref index, ref count);
		}
	}

	public override async Task WriteAsync(char value)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("HttpResponseStreamWriter");
		}
		if (_charBufferCount == _charBufferSize)
		{
			await FlushInternalAsync(flushEncoder: false);
		}
		_charBuffer[_charBufferCount] = value;
		_charBufferCount++;
	}

	public override async Task WriteAsync(char[] values, int index, int count)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("HttpResponseStreamWriter");
		}
		if (values == null)
		{
			return;
		}
		while (count > 0)
		{
			if (_charBufferCount == _charBufferSize)
			{
				await FlushInternalAsync(flushEncoder: false);
			}
			CopyToCharBuffer(values, ref index, ref count);
		}
	}

	public override async Task WriteAsync(string value)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("HttpResponseStreamWriter");
		}
		if (value == null)
		{
			return;
		}
		int count = value.Length;
		int index = 0;
		while (count > 0)
		{
			if (_charBufferCount == _charBufferSize)
			{
				await FlushInternalAsync(flushEncoder: false);
			}
			CopyToCharBuffer(value, ref index, ref count);
		}
	}

	public override void Flush()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("HttpResponseStreamWriter");
		}
		FlushInternal(flushEncoder: true);
	}

	public override Task FlushAsync()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("HttpResponseStreamWriter");
		}
		return FlushInternalAsync(flushEncoder: true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !_disposed)
		{
			_disposed = true;
			try
			{
				FlushInternal(flushEncoder: true);
			}
			finally
			{
				_bytePool.Return(_byteBuffer);
				_charPool.Return(_charBuffer);
			}
		}
		base.Dispose(disposing);
	}

	private void FlushInternal(bool flushEncoder)
	{
		if (_charBufferCount != 0)
		{
			int bytes = _encoder.GetBytes(_charBuffer, 0, _charBufferCount, _byteBuffer, 0, flushEncoder);
			_charBufferCount = 0;
			if (bytes > 0)
			{
				_stream.Write(_byteBuffer, 0, bytes);
			}
		}
	}

	private async Task FlushInternalAsync(bool flushEncoder)
	{
		if (_charBufferCount != 0)
		{
			int bytes = _encoder.GetBytes(_charBuffer, 0, _charBufferCount, _byteBuffer, 0, flushEncoder);
			_charBufferCount = 0;
			if (bytes > 0)
			{
				await _stream.WriteAsync(_byteBuffer, 0, bytes);
			}
		}
	}

	private void CopyToCharBuffer(string value, ref int index, ref int count)
	{
		int num = Math.Min(_charBufferSize - _charBufferCount, count);
		value.CopyTo(index, _charBuffer, _charBufferCount, num);
		_charBufferCount += num;
		index += num;
		count -= num;
	}

	private void CopyToCharBuffer(char[] values, ref int index, ref int count)
	{
		int num = Math.Min(_charBufferSize - _charBufferCount, count);
		Buffer.BlockCopy(values, index * 2, _charBuffer, _charBufferCount * 2, num * 2);
		_charBufferCount += num;
		index += num;
		count -= num;
	}
}
