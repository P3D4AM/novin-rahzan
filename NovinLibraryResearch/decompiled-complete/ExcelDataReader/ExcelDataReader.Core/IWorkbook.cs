using System.Collections.Generic;

namespace ExcelDataReader.Core;

internal interface IWorkbook<TWorksheet> where TWorksheet : IWorksheet
{
	int ResultsCount { get; }

	IEnumerable<TWorksheet> ReadWorksheets();
}
