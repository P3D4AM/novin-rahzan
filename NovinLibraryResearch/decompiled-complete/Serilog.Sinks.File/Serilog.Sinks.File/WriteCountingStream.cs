using System;
using System.IO;

namespace Serilog.Sinks.File;

internal sealed class WriteCountingStream : Stream
{
	private readonly Stream _stream;

	public long CountedLength { get; private set; }

	public override bool CanRead => false;

	public override bool CanSeek => _stream.CanSeek;

	public override bool CanWrite => true;

	public override long Length => _stream.Length;

	public override long Position
	{
		get
		{
			return _stream.Position;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public WriteCountingStream(Stream stream)
	{
		_stream = stream ?? throw new ArgumentNullException("stream");
		CountedLength = stream.Length;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_stream.Dispose();
		}
		base.Dispose(disposing);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		_stream.Write(buffer, offset, count);
		CountedLength += count;
	}

	public override void Flush()
	{
		_stream.Flush();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new InvalidOperationException("Seek operations are not available through `WriteCountingStream`.");
	}

	public override void SetLength(long value)
	{
		_stream.SetLength(value);
		if (value < CountedLength)
		{
			CountedLength = _stream.Length;
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}
}
