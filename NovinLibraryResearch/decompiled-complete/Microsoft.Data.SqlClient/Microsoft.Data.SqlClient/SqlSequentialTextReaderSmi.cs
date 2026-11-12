using System;
using System.IO;
using Microsoft.Data.Common;
using Microsoft.Data.SqlClient.Server;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlSequentialTextReaderSmi : TextReader
{
	private SmiEventSink_Default _sink;

	private ITypedGettersV3 _getters;

	private int _columnIndex;

	private long _position;

	private long _length;

	private int _peekedChar;

	internal int ColumnIndex => _columnIndex;

	private bool IsClosed
	{
		get
		{
			if (_sink != null)
			{
				return _getters == null;
			}
			return true;
		}
	}

	private bool HasPeekedChar => _peekedChar >= 0;

	internal SqlSequentialTextReaderSmi(SmiEventSink_Default sink, ITypedGettersV3 getters, int columnIndex, long length)
	{
		_sink = sink;
		_getters = getters;
		_columnIndex = columnIndex;
		_length = length;
		_position = 0L;
		_peekedChar = -1;
	}

	public override int Peek()
	{
		if (!HasPeekedChar)
		{
			_peekedChar = Read();
		}
		return _peekedChar;
	}

	public override int Read()
	{
		if (IsClosed)
		{
			throw ADP.ObjectDisposed(this);
		}
		int result = -1;
		if (HasPeekedChar)
		{
			result = _peekedChar;
			_peekedChar = -1;
		}
		else if (_position < _length)
		{
			char[] array = new char[1];
			int chars_Unchecked = ValueUtilsSmi.GetChars_Unchecked(_sink, _getters, _columnIndex, _position, array, 0, 1);
			if (chars_Unchecked == 1)
			{
				result = array[0];
				_position++;
			}
		}
		return result;
	}

	public override int Read(char[] buffer, int index, int count)
	{
		SqlSequentialTextReader.ValidateReadParameters(buffer, index, count);
		if (IsClosed)
		{
			throw ADP.ObjectDisposed(this);
		}
		int num = 0;
		if (count > 0 && HasPeekedChar)
		{
			buffer[index + num] = (char)_peekedChar;
			num++;
			_peekedChar = -1;
		}
		int num2 = (int)Math.Min(count - num, _length - _position);
		if (num2 > 0)
		{
			int chars_Unchecked = ValueUtilsSmi.GetChars_Unchecked(_sink, _getters, _columnIndex, _position, buffer, index + num, num2);
			_position += chars_Unchecked;
			num += chars_Unchecked;
		}
		return num;
	}

	internal void SetClosed()
	{
		_sink = null;
		_getters = null;
		_peekedChar = -1;
	}
}
