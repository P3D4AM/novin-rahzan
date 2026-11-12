using System;
using System.IO;

namespace SharpCompress.Compressors.LZMA.Utilites;

internal class CrcCheckStream : Stream
{
	private readonly uint _mExpectedCrc;

	private uint _mCurrentCrc;

	private bool _mClosed;

	private readonly long[] _mBytes = new long[256];

	private long _mLength;

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

	public CrcCheckStream(uint crc)
	{
		_mExpectedCrc = crc;
		_mCurrentCrc = uint.MaxValue;
	}

	protected override void Dispose(bool disposing)
	{
		if (_mCurrentCrc != _mExpectedCrc)
		{
			throw new InvalidOperationException();
		}
		try
		{
			if (disposing && !_mClosed)
			{
				_mClosed = true;
				_mCurrentCrc = Crc.Finish(_mCurrentCrc);
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new InvalidOperationException();
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
		_mLength += count;
		for (int i = 0; i < count; i++)
		{
			_mBytes[buffer[offset + i]]++;
		}
		_mCurrentCrc = Crc.Update(_mCurrentCrc, buffer, offset, count);
	}
}
