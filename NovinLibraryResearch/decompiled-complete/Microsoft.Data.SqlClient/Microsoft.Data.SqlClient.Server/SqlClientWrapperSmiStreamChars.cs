using System.IO;
using Microsoft.Data.Common;
using Microsoft.Data.SqlTypes;

namespace Microsoft.Data.SqlClient.Server;

internal class SqlClientWrapperSmiStreamChars : SqlStreamChars
{
	private SmiEventSink_Default _sink;

	private SmiStream _stream;

	public override bool IsNull => _stream == null;

	public override bool CanRead => _stream.CanRead;

	public override bool CanSeek => _stream.CanSeek;

	public override bool CanWrite => _stream.CanWrite;

	public override long Length
	{
		get
		{
			long length = _stream.GetLength(_sink);
			_sink.ProcessMessagesAndThrow();
			if (length > 0)
			{
				return length / 2;
			}
			return length;
		}
	}

	public override long Position
	{
		get
		{
			long result = _stream.GetPosition(_sink) / 2;
			_sink.ProcessMessagesAndThrow();
			return result;
		}
		set
		{
			if (value < 0)
			{
				throw ADP.ArgumentOutOfRange("Position");
			}
			_stream.SetPosition(_sink, value * 2);
			_sink.ProcessMessagesAndThrow();
		}
	}

	internal SqlClientWrapperSmiStreamChars(SmiEventSink_Default sink, SmiStream stream)
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
		long result = _stream.Seek(_sink, offset * 2, origin);
		_sink.ProcessMessagesAndThrow();
		return result;
	}

	public override void SetLength(long value)
	{
		if (value < 0)
		{
			throw ADP.ArgumentOutOfRange("value");
		}
		_stream.SetLength(_sink, value * 2);
		_sink.ProcessMessagesAndThrow();
	}

	public override int Read(char[] buffer, int offset, int count)
	{
		int result = _stream.Read(_sink, buffer, offset * 2, count);
		_sink.ProcessMessagesAndThrow();
		return result;
	}

	public override void Write(char[] buffer, int offset, int count)
	{
		_stream.Write(_sink, buffer, offset, count);
		_sink.ProcessMessagesAndThrow();
	}

	internal int Read(byte[] buffer, int offset, int count)
	{
		int result = _stream.Read(_sink, buffer, offset, count);
		_sink.ProcessMessagesAndThrow();
		return result;
	}

	internal void Write(byte[] buffer, int offset, int count)
	{
		_stream.Write(_sink, buffer, offset, count);
		_sink.ProcessMessagesAndThrow();
	}
}
