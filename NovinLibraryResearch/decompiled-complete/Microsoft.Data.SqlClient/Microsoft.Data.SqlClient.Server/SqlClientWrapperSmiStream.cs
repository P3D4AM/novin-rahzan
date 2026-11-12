using System.IO;

namespace Microsoft.Data.SqlClient.Server;

internal class SqlClientWrapperSmiStream : Stream
{
	private SmiEventSink_Default _sink;

	private SmiStream _stream;

	public override bool CanRead => _stream.CanRead;

	public override bool CanSeek => _stream.CanSeek;

	public override bool CanWrite => _stream.CanWrite;

	public override long Length
	{
		get
		{
			long length = _stream.GetLength(_sink);
			_sink.ProcessMessagesAndThrow();
			return length;
		}
	}

	public override long Position
	{
		get
		{
			long position = _stream.GetPosition(_sink);
			_sink.ProcessMessagesAndThrow();
			return position;
		}
		set
		{
			_stream.SetPosition(_sink, value);
			_sink.ProcessMessagesAndThrow();
		}
	}

	internal SqlClientWrapperSmiStream(SmiEventSink_Default sink, SmiStream stream)
	{
		_sink = sink;
		_stream = stream;
	}

	public override void Flush()
	{
		_stream.Flush(_sink);
		_sink.ProcessMessagesAndThrow();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		long result = _stream.Seek(_sink, offset, origin);
		_sink.ProcessMessagesAndThrow();
		return result;
	}

	public override void SetLength(long value)
	{
		_stream.SetLength(_sink, value);
		_sink.ProcessMessagesAndThrow();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		int result = _stream.Read(_sink, buffer, offset, count);
		_sink.ProcessMessagesAndThrow();
		return result;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		_stream.Write(_sink, buffer, offset, count);
		_sink.ProcessMessagesAndThrow();
	}
}
