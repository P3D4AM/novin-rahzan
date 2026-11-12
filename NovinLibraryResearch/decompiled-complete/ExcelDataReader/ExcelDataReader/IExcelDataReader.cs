using System;
using System.Data;

namespace ExcelDataReader;

public interface IExcelDataReader : IDataReader, IDisposable, IDataRecord
{
	string Name { get; }

	string CodeName { get; }

	string VisibleState { get; }

	HeaderFooter HeaderFooter { get; }

	CellRange[] MergeCells { get; }

	int ResultsCount { get; }

	int RowCount { get; }

	double RowHeight { get; }

	void Reset();

	string GetNumberFormatString(int i);

	int GetNumberFormatIndex(int i);

	double GetColumnWidth(int i);
}
