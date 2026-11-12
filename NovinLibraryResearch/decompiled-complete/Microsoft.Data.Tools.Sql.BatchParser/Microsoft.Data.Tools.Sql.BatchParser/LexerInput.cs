using System;
using System.IO;
using System.Text;

namespace Microsoft.Data.Tools.Sql.BatchParser;

internal sealed class LexerInput : IDisposable
{
	private readonly string _filename;

	private TextReader _input;

	private int _currentLine;

	private int _currentColumn;

	private int _bufferStartOffset;

	private int _currentSbOffset;

	private StringBuilder _buffer;

	public string Filename => _filename;

	public int CurrentLine => _currentLine;

	public int CurrentColumn => _currentColumn;

	public int CurrentOffset => _bufferStartOffset + _currentSbOffset;

	public LexerInput(TextReader reader, string filename)
	{
		_input = reader;
		_filename = filename;
		_currentLine = 1;
		_currentColumn = 1;
		_bufferStartOffset = 0;
		_currentSbOffset = 0;
		_buffer = new StringBuilder();
		EnsureBytes(1);
	}

	public void Consume()
	{
		bool flag = false;
		char? c = Lookahead();
		if (!c.HasValue)
		{
			return;
		}
		if (c == '\r')
		{
			flag = true;
		}
		else if (c == '\n')
		{
			_currentLine++;
			_currentColumn = 0;
		}
		if (EnsureBytes(1) != 0)
		{
			_currentSbOffset++;
			if (flag && Lookahead() != '\n')
			{
				_currentLine++;
				_currentColumn = 0;
			}
			_currentColumn++;
		}
	}

	public void Dispose()
	{
		if (_input != null)
		{
			_input.Dispose();
			_input = null;
		}
	}

	public int EnsureBytes(int bytesToBuffer)
	{
		if (_currentSbOffset + bytesToBuffer > _buffer.Length)
		{
			if (_input == null)
			{
				return _buffer.Length - _currentSbOffset;
			}
			int num = bytesToBuffer - (_buffer.Length - _currentSbOffset) + 128;
			char[] array = new char[num];
			int num2 = _input.ReadBlock(array, 0, num);
			_buffer.Append(array, 0, num2);
			if (num2 < num)
			{
				_input.Dispose();
				_input = null;
			}
			return _buffer.Length - _currentSbOffset;
		}
		return bytesToBuffer;
	}

	public char? Lookahead()
	{
		if (EnsureBytes(1) == 0)
		{
			return null;
		}
		return _buffer[_currentSbOffset];
	}

	public char? Lookahead(int lookahead)
	{
		if (EnsureBytes(lookahead + 1) < lookahead + 1)
		{
			return null;
		}
		return _buffer[_currentSbOffset + lookahead];
	}

	public string FlushBufferedText()
	{
		string result = _buffer.ToString(0, _currentSbOffset);
		_bufferStartOffset += _currentSbOffset;
		_buffer.Remove(0, _currentSbOffset);
		_currentSbOffset = 0;
		return result;
	}
}
