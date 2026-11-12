using System;

namespace ExcelDataReader;

public class ExcelDataTableConfiguration
{
	public string EmptyColumnNamePrefix { get; set; } = "Column";


	public bool UseHeaderRow { get; set; }

	public Action<IExcelDataReader> ReadHeaderRow { get; set; }

	public Func<IExcelDataReader, bool> FilterRow { get; set; }

	public Func<IExcelDataReader, int, bool> FilterColumn { get; set; }
}
