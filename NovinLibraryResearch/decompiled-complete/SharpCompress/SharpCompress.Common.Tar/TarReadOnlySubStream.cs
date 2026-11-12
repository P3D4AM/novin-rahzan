using System;
using System.IO;
using SharpCompress.IO;

namespace SharpCompress.Common.Tar;

internal class TarReadOnlySubStream : NonDisposingStream
{
	private bool _isDisposed;

	private long _amountRead;

	private long BytesLeftToRead { get; set; }

	public override bool CanRead => true;

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
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public TarReadOnlySubStream(Stream stream, long bytesToRead)
		: base(stream)
	{
		BytesLeftToRead = bytesToRead;
	}

	protected override void Dispose(bool disposing)
	{
		if (_isDisposed)
		{
			return;
		}
		_isDisposed = true;
		if (disposing)
		{
			base.Stream.Skip(BytesLeftToRead);
			_amountRead += BytesLeftToRead;
			long num = _amountRead % 512;
			if (num != 0L)
			{
				base.Stream.Skip(512 - num);
			}
		}
		base.Dispose(disposing);
	}

	public override void Flush()
	{
		throw new NotSupportedException();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (BytesLeftToRead < count)
		{
			count = (int)BytesLeftToRead;
		}
		int num = base.Stream.Read(buffer, offset, count);
		if (num > 0)
		{
			BytesLeftToRead -= num;
			_amountRead += num;
		}
		return num;
	}

	public override int ReadByte()
	{
		if (BytesLeftToRead <= 0)
		{
			return -1;
		}
		int num = base.Stream.ReadByte();
		if (num != -1)
		{
			long bytesLeftToRead = BytesLeftToRead - 1;
			BytesLeftToRead = bytesLeftToRead;
			_amountRead++;
		}
		return num;
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
