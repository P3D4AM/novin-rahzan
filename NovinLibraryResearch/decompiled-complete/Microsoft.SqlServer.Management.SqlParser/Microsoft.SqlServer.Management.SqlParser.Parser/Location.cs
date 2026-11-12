using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

public struct Location : IComparable<Location>
{
	public readonly int LineNumber;

	public readonly int ColumnNumber;

	public readonly int Offset;

	public Location(int lineNumber, int columnNumber, int offset = -1)
	{
		LineNumber = lineNumber;
		ColumnNumber = columnNumber;
		Offset = offset;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.CurrentCulture, "({0},{1})", LineNumber, ColumnNumber);
	}

	public int CompareTo(Location other)
	{
		if (LineNumber == other.LineNumber)
		{
			return ColumnNumber - other.ColumnNumber;
		}
		return LineNumber - other.LineNumber;
	}
}
