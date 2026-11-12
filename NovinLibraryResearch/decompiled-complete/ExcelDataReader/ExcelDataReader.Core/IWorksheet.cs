using System.Collections.Generic;
using ExcelDataReader.Core.NumberFormat;

namespace ExcelDataReader.Core;

internal interface IWorksheet
{
	string Name { get; }

	string CodeName { get; }

	string VisibleState { get; }

	HeaderFooter HeaderFooter { get; }

	int FieldCount { get; }

	int RowCount { get; }

	CellRange[] MergeCells { get; }

	Col[] ColumnWidths { get; }

	IEnumerable<Row> ReadRows();

	NumberFormatString GetNumberFormatString(int index);
}
