using System;
using System.IO;
using System.Runtime.CompilerServices;
using SharpCompress.Common.Zip;

namespace SharpCompress.Compressors.Deflate64;

public sealed class Deflate64Stream : Stream
{
	private const int DEFAULT_BUFFER_SIZE = 8192;

	private Stream _stream;

	private CompressionMode _mode;

	private InflaterManaged _inflater;

	private byte[] _buffer;

	public override bool CanRead
	{
		get
		{
			if (_stream == null)
			{
				return false;
			}
			if (_mode == CompressionMode.Decompress)
			{
				return _stream.CanRead;
			}
			return false;
		}
	}

	public override bool CanWrite
	{
		get
		{
			if (_stream == null)
			{
				return false;
			}
			if (_mode == CompressionMode.Compress)
			{
				return _stream.CanWrite;
			}
			return false;
		}
	}

	public override bool CanSeek => false;

	public override long Length
	{
		get
		{
			throw new NotSupportedException("Deflate64: not supported");
		}
	}

	public override long Position
	{
		get
		{
			throw new NotSupportedException("Deflate64: not supported");
		}
		set
		{
			throw new NotSupportedException("Deflate64: not supported");
		}
	}

	public Deflate64Stream(Stream stream, CompressionMode mode)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (mode != CompressionMode.Decompress)
		{
			throw new NotImplementedException("Deflate64: this implementation only supports decompression");
		}
		if (!stream.CanRead)
		{
			throw new ArgumentException("Deflate64: input stream is not readable", "stream");
		}
		InitializeInflater(stream, ZipCompressionMethod.Deflate64);
	}

	private void InitializeInflater(Stream stream, ZipCompressionMethod method = ZipCompressionMethod.Deflate)
	{
		if (!stream.CanRead)
		{
			throw new ArgumentException("Deflate64: input stream is not readable", "stream");
		}
		_inflater = new InflaterManaged(method == ZipCompressionMethod.Deflate64);
		_stream = stream;
		_mode = CompressionMode.Decompress;
		_buffer = new byte[8192];
	}

	public override void Flush()
	{
		EnsureNotDisposed();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException("Deflate64: not supported");
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException("Deflate64: not supported");
	}

	public override int Read(byte[] array, int offset, int count)
	{
		EnsureDecompressionMode();
		ValidateParameters(array, offset, count);
		EnsureNotDisposed();
		int num = offset;
		int num2 = count;
		while (true)
		{
			int num3 = _inflater.Inflate(array, num, num2);
			num += num3;
			num2 -= num3;
			if (num2 == 0 || _inflater.Finished())
			{
				break;
			}
			int num4 = _stream.Read(_buffer, 0, _buffer.Length);
			if (num4 <= 0)
			{
				break;
			}
			if (num4 > _buffer.Length)
			{
				throw new InvalidDataException("Deflate64: invalid data");
			}
			_inflater.SetInput(_buffer, 0, num4);
		}
		return count - num2;
	}

	private void ValidateParameters(byte[] array, int offset, int count)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (array.Length - offset < count)
		{
			throw new ArgumentException("Deflate64: invalid offset/count combination");
		}
	}

	private void EnsureNotDisposed()
	{
		if (_stream == null)
		{
			ThrowStreamClosedException();
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ThrowStreamClosedException()
	{
		throw new ObjectDisposedException(null, "Deflate64: stream has been disposed");
	}

	private void EnsureDecompressionMode()
	{
		if (_mode != CompressionMode.Decompress)
		{
			ThrowCannotReadFromDeflateManagedStreamException();
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ThrowCannotReadFromDeflateManagedStreamException()
	{
		throw new InvalidOperationException("Deflate64: cannot read from this stream");
	}

	private void EnsureCompressionMode()
	{
		if (_mode != 0)
		{
			ThrowCannotWriteToDeflateManagedStreamException();
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ThrowCannotWriteToDeflateManagedStreamException()
	{
		throw new InvalidOperationException("Deflate64: cannot write to this stream");
	}

	public override void Write(byte[] array, int offset, int count)
	{
		ThrowCannotWriteToDeflateManagedStreamException();
	}

	private void PurgeBuffers(bool disposing)
	{
		if (disposing && _stream != null)
		{
			Flush();
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			PurgeBuffers(disposing);
		}
		finally
		{
			try
			{
				if (disposing)
				{
					_stream?.Dispose();
				}
			}
			finally
			{
				_stream = null;
				try
				{
					_inflater?.Dispose();
				}
				finally
				{
					_inflater = null;
					base.Dispose(disposing);
				}
			}
		}
	}
}
