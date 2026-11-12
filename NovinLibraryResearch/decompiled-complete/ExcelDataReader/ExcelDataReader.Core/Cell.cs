namespace ExcelDataReader.Core;

internal class Cell
{
	public int ColumnIndex { get; set; }

	public int NumberFormatIndex { get; set; }

	public object Value { get; set; }
}
