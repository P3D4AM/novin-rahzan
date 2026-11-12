using System;
using System.IO;
using Microsoft.Data.Common;
using Microsoft.Data.SqlClient.Server;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlSequentialStreamSmi : Stream
{
	private SmiEventSink_Default _sink;

	private ITypedGettersV3 _getters;

	private int _columnIndex;

	private long _position;

	private long _length;

	public override bool CanRead
	{
		get
		{
			if (_sink != null)
			{
				return _getters != null;
			}
			return false;
		}
	}

	public override bool CanSeek => false;

	public override bool CanWrite => false;

	public override long Length
	{
		get
		{
			throw ADP.NotSupported();
		}
	}

	public override long Position
	{
		get
		{
			throw ADP.NotSupported();
		}
		set
		{
			throw ADP.NotSupported();
		}
	}

	internal int ColumnIndex => _columnIndex;

	internal SqlSequentialStreamSmi(SmiEventSink_Default sink, ITypedGettersV3 getters, int columnIndex, long length)
	{
		_sink = sink;
		_getters = getters;
		_columnIndex = columnIndex;
		_length = length;
		_position = 0L;
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		SqlSequentialStream.ValidateReadParameters(buffer, offset, count);
		if (!CanRead)
		{
			throw ADP.ObjectDisposed(this);
		}
		try
		{
			int num = (int)Math.Min(count, _length - _position);
			int num2 = 0;
			if (num > 0)
			{
				num2 = ValueUtilsSmi.GetBytes_Unchecked(_sink, _getters, _columnIndex, _position, buffer, offset, num);
				_position += num2;
			}
			return num2;
		}
		catch (SqlException internalException)
		{
			throw ADP.ErrorReadingFromStream(internalException);
		}
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw ADP.NotSupported();
	}

	public override void SetLength(long value)
	{
		throw ADP.NotSupported();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw ADP.NotSupported();
	}

	internal void SetClosed()
	{
		_sink = null;
		_getters = null;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			SetClosed();
		}
		base.Dispose(disposing);
	}
}
