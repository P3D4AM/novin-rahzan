using System;
using System.IO;

namespace SharpCompress.IO;

internal class CountingWritableSubStream : NonDisposingStream
{
	public ulong Count { get; private set; }

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

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
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	internal CountingWritableSubStream(Stream stream)
		: base(stream)
	{
	}

	public override void Flush()
	{
		base.Stream.Flush();
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

	public override void Write(byte[] buffer, int offset, int count)
	{
		base.Stream.Write(buffer, offset, count);
		Count += (uint)count;
	}

	public override void WriteByte(byte value)
	{
		base.Stream.WriteByte(value);
		ulong count = Count + 1;
		Count = count;
	}
}
