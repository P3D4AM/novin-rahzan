using System;
using System.IO;
using System.Text;

namespace SharpCompress.Compressors.Deflate;

public class ZlibStream : Stream
{
	private readonly ZlibBaseStream _baseStream;

	private bool _disposed;

	public virtual FlushType FlushMode
	{
		get
		{
			return _baseStream._flushMode;
		}
		set
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			_baseStream._flushMode = value;
		}
	}

	public int BufferSize
	{
		get
		{
			return _baseStream._bufferSize;
		}
		set
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			if (_baseStream._workingBuffer != null)
			{
				throw new ZlibException("The working buffer is already set.");
			}
			if (value < 1024)
			{
				throw new ZlibException($"Don't be silly. {value} bytes?? Use a bigger buffer, at least {1024}.");
			}
			_baseStream._bufferSize = value;
		}
	}

	public virtual long TotalIn => _baseStream._z.TotalBytesIn;

	public virtual long TotalOut => _baseStream._z.TotalBytesOut;

	public override bool CanRead
	{
		get
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return _baseStream._stream.CanRead;
		}
	}

	public override bool CanSeek => false;

	public override bool CanWrite
	{
		get
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return _baseStream._stream.CanWrite;
		}
	}

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
			if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
			{
				return _baseStream._z.TotalBytesOut;
			}
			if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
			{
				return _baseStream._z.TotalBytesIn;
			}
			return 0L;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public ZlibStream(Stream stream, CompressionMode mode)
		: this(stream, mode, CompressionLevel.Default, Encoding.UTF8)
	{
	}

	public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level)
		: this(stream, mode, level, Encoding.UTF8)
	{
	}

	public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, Encoding encoding)
	{
		_baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, encoding);
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_baseStream?.Dispose();
				}
				_disposed = true;
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override void Flush()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("ZlibStream");
		}
		_baseStream.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("ZlibStream");
		}
		return _baseStream.Read(buffer, offset, count);
	}

	public override int ReadByte()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("ZlibStream");
		}
		return _baseStream.ReadByte();
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
		if (_disposed)
		{
			throw new ObjectDisposedException("ZlibStream");
		}
		_baseStream.Write(buffer, offset, count);
	}

	public override void WriteByte(byte value)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("ZlibStream");
		}
		_baseStream.WriteByte(value);
	}
}
