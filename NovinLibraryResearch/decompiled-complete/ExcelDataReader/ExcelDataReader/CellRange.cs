using ExcelDataReader.Core;

namespace ExcelDataReader;

public sealed class CellRange
{
	public int FromColumn { get; private set; }

	public int FromRow { get; private set; }

	public int ToColumn { get; private set; }

	public int ToRow { get; private set; }

	internal CellRange(string from, string to)
	{
		ReferenceHelper.ParseReference(from, out var column, out var row);
		FromColumn = column - 1;
		FromRow = row - 1;
		ReferenceHelper.ParseReference(to, out var column2, out var row2);
		ToColumn = column2 - 1;
		ToRow = row2 - 1;
	}

	internal CellRange(int fromColumn, int fromRow, int toColumn, int toRow)
	{
		FromColumn = fromColumn;
		FromRow = fromRow;
		ToColumn = toColumn;
		ToRow = toRow;
	}
}
