using System;
using System.Buffers.Binary;
using System.IO;
using SharpCompress.Crypto;
using SharpCompress.IO;

namespace SharpCompress.Compressors.LZMA;

public sealed class LZipStream : Stream
{
	private readonly Stream _stream;

	private readonly CountingWritableSubStream? _countingWritableSubStream;

	private bool _disposed;

	private bool _finished;

	private long _writeCount;

	private static readonly byte[] headerBytes = new byte[6] { 76, 90, 73, 80, 1, 113 };

	public CompressionMode Mode { get; }

	public override bool CanRead => Mode == CompressionMode.Decompress;

	public override bool CanSeek => false;

	public override bool CanWrite => Mode == CompressionMode.Compress;

	public override long Length
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override long Position
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public LZipStream(Stream stream, CompressionMode mode)
	{
		Mode = mode;
		if (mode == CompressionMode.Decompress)
		{
			int num = ValidateAndReadSize(stream);
			if (num == 0)
			{
				throw new IOException("Not an LZip stream");
			}
			byte[] properties = GetProperties(num);
			_stream = new LzmaStream(properties, stream);
		}
		else
		{
			int dictionary = 106496;
			WriteHeaderSize(stream);
			_countingWritableSubStream = new CountingWritableSubStream(stream);
			_stream = new Crc32Stream(new LzmaStream(new LzmaEncoderProperties(eos: true, dictionary), isLzma2: false, _countingWritableSubStream));
		}
	}

	public void Finish()
	{
		if (!_finished)
		{
			if (Mode == CompressionMode.Compress)
			{
				Crc32Stream crc32Stream = (Crc32Stream)_stream;
				crc32Stream.WrappedStream.Dispose();
				crc32Stream.Dispose();
				ulong count = _countingWritableSubStream.Count;
				Span<byte> span = stackalloc byte[8];
				BinaryPrimitives.WriteUInt32LittleEndian(span, crc32Stream.Crc);
				_countingWritableSubStream.Write(span.Slice(0, 4));
				BinaryPrimitives.WriteInt64LittleEndian(span, _writeCount);
				_countingWritableSubStream.Write(span);
				BinaryPrimitives.WriteUInt64LittleEndian(span, count + 6 + 20);
				_countingWritableSubStream.Write(span);
			}
			_finished = true;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			_disposed = true;
			if (disposing)
			{
				Finish();
				_stream.Dispose();
			}
		}
	}

	public override void Flush()
	{
		_stream.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return _stream.Read(buffer, offset, count);
	}

	public override int ReadByte()
	{
		return _stream.ReadByte();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotImplementedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		_stream.Write(buffer, offset, count);
		_writeCount += count;
	}

	public override void WriteByte(byte value)
	{
		_stream.WriteByte(value);
		_writeCount++;
	}

	public static bool IsLZipFile(Stream stream)
	{
		return ValidateAndReadSize(stream) != 0;
	}

	public static int ValidateAndReadSize(Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		Span<byte> buffer = stackalloc byte[6];
		if (stream.Read(buffer) != 6)
		{
			return 0;
		}
		if (buffer[0] != 76 || buffer[1] != 90 || buffer[2] != 73 || buffer[3] != 80 || buffer[4] != 1)
		{
			return 0;
		}
		int num = buffer[5] & 0x1F;
		int num2 = (buffer[5] & 0xE0) >> 5;
		return (1 << num) - num2 * (1 << num - 4);
	}

	public static void WriteHeaderSize(Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		stream.Write(headerBytes, 0, 6);
	}

	private static byte[] GetProperties(int dictionarySize)
	{
		return new byte[5]
		{
			93,
			(byte)((uint)dictionarySize & 0xFFu),
			(byte)((uint)(dictionarySize >> 8) & 0xFFu),
			(byte)((uint)(dictionarySize >> 16) & 0xFFu),
			(byte)((uint)(dictionarySize >> 24) & 0xFFu)
		};
	}
}
