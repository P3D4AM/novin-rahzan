using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace SharpCompress.Compressors.Deflate;

public class GZipStream : Stream
{
	internal static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	private string? _comment;

	private string? _fileName;

	private DateTime? _lastModified;

	internal ZlibBaseStream BaseStream;

	private bool _disposed;

	private bool _firstReadDone;

	private int _headerByteCount;

	private readonly Encoding _encoding;

	public virtual FlushType FlushMode
	{
		get
		{
			return BaseStream._flushMode;
		}
		set
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			BaseStream._flushMode = value;
		}
	}

	public int BufferSize
	{
		get
		{
			return BaseStream._bufferSize;
		}
		set
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (BaseStream._workingBuffer != null)
			{
				throw new ZlibException("The working buffer is already set.");
			}
			if (value < 1024)
			{
				throw new ZlibException($"Don't be silly. {value} bytes?? Use a bigger buffer, at least {1024}.");
			}
			BaseStream._bufferSize = value;
		}
	}

	internal virtual long TotalIn => BaseStream._z.TotalBytesIn;

	internal virtual long TotalOut => BaseStream._z.TotalBytesOut;

	public override bool CanRead
	{
		get
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			return BaseStream._stream.CanRead;
		}
	}

	public override bool CanSeek => false;

	public override bool CanWrite
	{
		get
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			return BaseStream._stream.CanWrite;
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
			if (BaseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
			{
				return BaseStream._z.TotalBytesOut + _headerByteCount;
			}
			if (BaseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
			{
				return BaseStream._z.TotalBytesIn + BaseStream._gzipHeaderByteCount;
			}
			return 0L;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public string? Comment
	{
		get
		{
			return _comment;
		}
		set
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			_comment = value;
		}
	}

	public DateTime? LastModified
	{
		get
		{
			return _lastModified;
		}
		set
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			_lastModified = value;
		}
	}

	public string? FileName
	{
		get
		{
			return _fileName;
		}
		set
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			_fileName = value;
			if (_fileName != null)
			{
				if (StringExtensions.Contains(_fileName, '/'))
				{
					_fileName = _fileName.Replace('/', '\\');
				}
				if (StringExtensions.EndsWith(_fileName, '\\'))
				{
					throw new InvalidOperationException("Illegal filename");
				}
				if (StringExtensions.Contains(_fileName, '\\'))
				{
					_fileName = Path.GetFileName(_fileName);
				}
			}
		}
	}

	public int Crc32 { get; private set; }

	public GZipStream(Stream stream, CompressionMode mode)
		: this(stream, mode, CompressionLevel.Default, Encoding.UTF8)
	{
	}

	public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level)
		: this(stream, mode, level, Encoding.UTF8)
	{
	}

	public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, Encoding encoding)
	{
		BaseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, encoding);
		_encoding = encoding;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_disposed)
			{
				if (disposing && BaseStream != null)
				{
					BaseStream.Dispose();
					Crc32 = BaseStream.Crc32;
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
			throw new ObjectDisposedException("GZipStream");
		}
		BaseStream.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("GZipStream");
		}
		int result = BaseStream.Read(buffer, offset, count);
		if (!_firstReadDone)
		{
			_firstReadDone = true;
			FileName = BaseStream._GzipFileName;
			Comment = BaseStream._GzipComment;
			LastModified = BaseStream._GzipMtime;
		}
		return result;
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
			throw new ObjectDisposedException("GZipStream");
		}
		if (BaseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
		{
			if (!BaseStream._wantCompress)
			{
				throw new InvalidOperationException();
			}
			_headerByteCount = EmitHeader();
		}
		BaseStream.Write(buffer, offset, count);
	}

	private int EmitHeader()
	{
		byte[] array = ((Comment == null) ? null : _encoding.GetBytes(Comment));
		byte[] array2 = ((FileName == null) ? null : _encoding.GetBytes(FileName));
		int num = ((array != null) ? (array.Length + 1) : 0);
		int num2 = ((array2 != null) ? (array2.Length + 1) : 0);
		byte[] array3 = new byte[10 + num + num2];
		int num3 = 0;
		array3[num3++] = 31;
		array3[num3++] = 139;
		array3[num3++] = 8;
		byte b = 0;
		if (Comment != null)
		{
			b = (byte)(b ^ 0x10u);
		}
		if (FileName != null)
		{
			b = (byte)(b ^ 8u);
		}
		array3[num3++] = b;
		if (!LastModified.HasValue)
		{
			LastModified = DateTime.Now;
		}
		int value = (int)(LastModified.Value - UNIX_EPOCH).TotalSeconds;
		BinaryPrimitives.WriteInt32LittleEndian(MemoryExtensions.AsSpan(array3, num3), value);
		num3 += 4;
		array3[num3++] = 0;
		array3[num3++] = byte.MaxValue;
		if (num2 != 0)
		{
			Array.Copy(array2, 0, array3, num3, num2 - 1);
			num3 += num2 - 1;
			array3[num3++] = 0;
		}
		if (num != 0)
		{
			Array.Copy(array, 0, array3, num3, num - 1);
			num3 += num - 1;
			array3[num3++] = 0;
		}
		BaseStream._stream.Write(array3, 0, array3.Length);
		return array3.Length;
	}
}
