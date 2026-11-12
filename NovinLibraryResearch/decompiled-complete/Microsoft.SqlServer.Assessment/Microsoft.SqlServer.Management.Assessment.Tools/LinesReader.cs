using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal class LinesReader : TextReader
{
	private IEnumerator<string> Line { get; }

	private IEnumerator<char> Char { get; set; }

	private bool Eof { get; set; }

	private int Eol { get; set; }

	public LinesReader(IEnumerable<string> lines)
	{
		if (lines == null)
		{
			throw new ArgumentNullException("lines");
		}
		Line = lines.GetEnumerator();
		Eof = false;
		Eol = 2;
		MoveNext();
	}

	private void MoveNext()
	{
		switch (Eol)
		{
		case 1:
			Eol = 2;
			break;
		case 2:
			Eof = !Line.MoveNext();
			if (!Eof)
			{
				if (string.IsNullOrEmpty(Line.Current))
				{
					Eol = 1;
					break;
				}
				Char = Line.Current.GetEnumerator();
				Char.MoveNext();
			}
			Eol = 0;
			break;
		default:
			if (!Char.MoveNext())
			{
				Eol = 1;
			}
			break;
		}
	}

	public override int Peek()
	{
		if (Eof)
		{
			return -1;
		}
		return Eol switch
		{
			1 => 13, 
			2 => 10, 
			_ => Char.Current, 
		};
	}

	public override int Read()
	{
		if (Eof)
		{
			return -1;
		}
		char result = Eol switch
		{
			1 => '\r', 
			2 => '\n', 
			_ => Char.Current, 
		};
		MoveNext();
		return result;
	}
}
