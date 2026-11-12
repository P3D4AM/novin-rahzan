using System;
using System.IO;

namespace SharpCompress.Compressors.LZMA.Utilites;

internal class CrcBuilderStream : Stream
{
	private readonly Stream _mTarget;

	private uint _mCrc;

	private bool _mFinished;

	private bool _isDisposed;

	public long Processed { get; private set; }

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

	public CrcBuilderStream(Stream target)
	{
		_mTarget = target;
		_mCrc = uint.MaxValue;
	}

	protected override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			_mTarget.Dispose();
			base.Dispose(disposing);
		}
	}

	public uint Finish()
	{
		if (!_mFinished)
		{
			_mFinished = true;
			_mCrc = Crc.Finish(_mCrc);
		}
		return _mCrc;
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
		if (_mFinished)
		{
			throw new InvalidOperationException("CRC calculation has been finished.");
		}
		Processed += count;
		_mCrc = Crc.Update(_mCrc, buffer, offset, count);
		_mTarget.Write(buffer, offset, count);
	}
}
