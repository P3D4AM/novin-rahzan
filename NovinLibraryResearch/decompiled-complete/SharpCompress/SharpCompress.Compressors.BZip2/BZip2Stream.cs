using System.IO;

namespace SharpCompress.Compressors.BZip2;

public sealed class BZip2Stream : Stream
{
	private readonly Stream stream;

	private bool isDisposed;

	public CompressionMode Mode { get; }

	public override bool CanRead => stream.CanRead;

	public override bool CanSeek => stream.CanSeek;

	public override bool CanWrite => stream.CanWrite;

	public override long Length => stream.Length;

	public override long Position
	{
		get
		{
			return stream.Position;
		}
		set
		{
			stream.Position = value;
		}
	}

	public BZip2Stream(Stream stream, CompressionMode compressionMode, bool decompressConcatenated)
	{
		Mode = compressionMode;
		if (Mode == CompressionMode.Compress)
		{
			this.stream = new CBZip2OutputStream(stream);
		}
		else
		{
			this.stream = new CBZip2InputStream(stream, decompressConcatenated);
		}
	}

	public void Finish()
	{
		(stream as CBZip2OutputStream)?.Finish();
	}

	protected override void Dispose(bool disposing)
	{
		if (!isDisposed)
		{
			isDisposed = true;
			if (disposing)
			{
				stream.Dispose();
			}
		}
	}

	public override void Flush()
	{
		stream.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return stream.Read(buffer, offset, count);
	}

	public override int ReadByte()
	{
		return stream.ReadByte();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return stream.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		stream.SetLength(value);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		stream.Write(buffer, offset, count);
	}

	public override void WriteByte(byte value)
	{
		stream.WriteByte(value);
	}

	public static bool IsBZip2(Stream stream)
	{
		byte[] array = new BinaryReader(stream).ReadBytes(2);
		if (array.Length < 2 || array[0] != 66 || array[1] != 90)
		{
			return false;
		}
		return true;
	}
}
