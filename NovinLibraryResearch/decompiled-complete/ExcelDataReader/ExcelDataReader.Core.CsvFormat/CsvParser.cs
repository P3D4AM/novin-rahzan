using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelDataReader.Core.CsvFormat;

internal class CsvParser
{
	private enum CsvState
	{
		PreValue,
		Value,
		QuotedValue,
		QuotedValueQuote,
		Separator,
		Linebreak,
		EndOfFile
	}

	private CsvState State { get; set; }

	private char QuoteChar { get; }

	private int TrailingWhitespaceCount { get; set; }

	private Decoder Decoder { get; }

	private bool HasCarriageReturn { get; set; }

	private char Separator { get; }

	private char[] CharBuffer { get; set; }

	private StringBuilder ValueResult { get; set; } = new StringBuilder();


	private List<string> RowResult { get; set; } = new List<string>();


	private List<List<string>> RowsResult { get; set; } = new List<List<string>>();


	public CsvParser(char separator, Encoding encoding)
	{
		Separator = separator;
		QuoteChar = '"';
		Decoder = encoding.GetDecoder();
		Decoder.Fallback = new DecoderExceptionFallback();
		int num = 1024;
		CharBuffer = new char[num];
		State = CsvState.PreValue;
	}

	public void ParseBuffer(byte[] bytes, int offset, int count, out List<List<string>> rows)
	{
		while (count > 0)
		{
			Decoder.Convert(bytes, offset, count, CharBuffer, 0, CharBuffer.Length, flush: false, out var bytesUsed, out var charsUsed, out var _);
			offset += bytesUsed;
			count -= bytesUsed;
			for (int i = 0; i < charsUsed; i++)
			{
				ParseChar(CharBuffer[i], 1);
			}
		}
		rows = RowsResult;
		RowsResult = new List<List<string>>();
	}

	public void Flush(out List<List<string>> rows)
	{
		if (ValueResult.Length > 0 || RowResult.Count > 0)
		{
			AddValueToRow();
			AddRowToResult();
		}
		rows = RowsResult;
		RowsResult = new List<List<string>>();
	}

	private void ParseChar(char c, int bytesUsed)
	{
		bool flag = false;
		while (!flag)
		{
			flag = State switch
			{
				CsvState.PreValue => ReadPreValue(c, bytesUsed), 
				CsvState.Value => ReadValue(c, bytesUsed), 
				CsvState.QuotedValue => ReadQuotedValue(c, bytesUsed), 
				CsvState.QuotedValueQuote => ReadQuotedValueQuote(c, bytesUsed), 
				CsvState.Separator => ReadSeparator(c, bytesUsed), 
				CsvState.Linebreak => ReadLinebreak(c, bytesUsed), 
				_ => throw new InvalidOperationException("Unhandled parser state: " + State), 
			};
		}
	}

	private bool ReadPreValue(char c, int bytesUsed)
	{
		if (IsWhitespace(c))
		{
			return true;
		}
		if (c == QuoteChar)
		{
			State = CsvState.QuotedValue;
			return true;
		}
		if (c == Separator)
		{
			State = CsvState.Separator;
			return false;
		}
		if (c == '\r' || c == '\n' || (c == '\0' && bytesUsed == 0))
		{
			State = CsvState.Linebreak;
			return false;
		}
		if (c == '\0' && bytesUsed == 0)
		{
			State = CsvState.EndOfFile;
			return false;
		}
		State = CsvState.Value;
		return false;
	}

	private bool ReadValue(char c, int bytesUsed)
	{
		if (c == Separator)
		{
			State = CsvState.Separator;
			return false;
		}
		switch (c)
		{
		case '\n':
		case '\r':
			State = CsvState.Linebreak;
			return false;
		case '\0':
			if (bytesUsed == 0)
			{
				State = CsvState.EndOfFile;
				return false;
			}
			break;
		}
		if (IsWhitespace(c))
		{
			TrailingWhitespaceCount++;
		}
		else
		{
			TrailingWhitespaceCount = 0;
		}
		ValueResult.Append(c);
		return true;
	}

	private bool ReadQuotedValue(char c, int bytesUsed)
	{
		if (c == QuoteChar)
		{
			State = CsvState.QuotedValueQuote;
			return true;
		}
		ValueResult.Append(c);
		return true;
	}

	private bool ReadQuotedValueQuote(char c, int bytesUsed)
	{
		if (c == QuoteChar)
		{
			ValueResult.Append(c);
			State = CsvState.QuotedValue;
			return true;
		}
		State = CsvState.Value;
		return false;
	}

	private bool ReadSeparator(char c, int bytesUsed)
	{
		AddValueToRow();
		State = CsvState.PreValue;
		return true;
	}

	private bool ReadLinebreak(char c, int bytesUsed)
	{
		if (HasCarriageReturn)
		{
			HasCarriageReturn = false;
			AddValueToRow();
			AddRowToResult();
			State = CsvState.PreValue;
			return c == '\n';
		}
		if (c == '\r')
		{
			HasCarriageReturn = true;
			return true;
		}
		AddValueToRow();
		AddRowToResult();
		State = CsvState.PreValue;
		return true;
	}

	private void AddValueToRow()
	{
		RowResult.Add(ValueResult.ToString(0, ValueResult.Length - TrailingWhitespaceCount));
		ValueResult = new StringBuilder();
		TrailingWhitespaceCount = 0;
	}

	private void AddRowToResult()
	{
		RowsResult.Add(RowResult);
		RowResult = new List<string>();
	}

	private bool IsWhitespace(char c)
	{
		if (c == ' ')
		{
			return true;
		}
		if (Separator != '\t' && c == '\t')
		{
			return true;
		}
		return false;
	}
}
