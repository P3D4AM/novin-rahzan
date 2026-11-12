using System;
using System.IO;
using SharpCompress.Readers;

namespace SharpCompress.Common;

public class EntryStream : Stream
{
	private readonly IReader _reader;

	private readonly Stream _stream;

	private bool _completed;

	private bool _isDisposed;

	public override bool CanRead => true;

	public override bool CanSeek => false;

	public override bool CanWrite => false;

	public override long Length => _stream.Length;

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

	internal EntryStream(IReader reader, Stream stream)
	{
		_reader = reader;
		_stream = stream;
	}

	public void SkipEntry()
	{
		this.Skip();
		_completed = true;
	}

	protected override void Dispose(bool disposing)
	{
		if (!_completed && !_reader.Cancelled)
		{
			SkipEntry();
		}
		if (!_isDisposed)
		{
			_isDisposed = true;
			base.Dispose(disposing);
			_stream.Dispose();
		}
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		int num = _stream.Read(buffer, offset, count);
		if (num <= 0)
		{
			_completed = true;
		}
		return num;
	}

	public override int ReadByte()
	{
		int num = _stream.ReadByte();
		if (num == -1)
		{
			_completed = true;
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
