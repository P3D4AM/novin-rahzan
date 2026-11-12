using System;
using System.IO;

namespace SharpCompress.Compressors.ADC;

public sealed class ADCStream : Stream
{
	private readonly Stream _stream;

	private bool _isDisposed;

	private long _position;

	private byte[] _outBuffer;

	private int _outPosition;

	public override bool CanRead => _stream.CanRead;

	public override bool CanSeek => false;

	public override bool CanWrite => false;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override long Position
	{
		get
		{
			return _position;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public ADCStream(Stream stream, CompressionMode compressionMode = CompressionMode.Decompress)
	{
		if (compressionMode == CompressionMode.Compress)
		{
			throw new NotSupportedException();
		}
		_stream = stream;
	}

	public override void Flush()
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			base.Dispose(disposing);
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (count == 0)
		{
			return 0;
		}
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (offset < buffer.GetLowerBound(0))
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (offset + count > buffer.GetLength(0))
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (_outBuffer == null)
		{
			ADCBase.Decompress(_stream, out _outBuffer);
			_outPosition = 0;
		}
		int num = offset;
		int num2 = count;
		int num3 = 0;
		while (_outPosition + num2 >= _outBuffer.Length)
		{
			int num4 = _outBuffer.Length - _outPosition;
			Array.Copy(_outBuffer, _outPosition, buffer, num, num4);
			num += num4;
			num3 += num4;
			_position += num4;
			num2 -= num4;
			int num5 = ADCBase.Decompress(_stream, out _outBuffer);
			_outPosition = 0;
			if (num5 == 0 || _outBuffer == null || _outBuffer.Length == 0)
			{
				return num3;
			}
		}
		Array.Copy(_outBuffer, _outPosition, buffer, num, num2);
		_outPosition += num2;
		_position += num2;
		return num3 + num2;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}
}
