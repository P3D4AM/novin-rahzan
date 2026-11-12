using System;

namespace ExcelDataReader;

public class ExcelDataSetConfiguration
{
	public bool UseColumnDataType { get; set; } = true;


	public Func<IExcelDataReader, ExcelDataTableConfiguration> ConfigureDataTable { get; set; }

	public Func<IExcelDataReader, int, bool> FilterSheet { get; set; }
}
