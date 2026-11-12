using System.IO;
using System.Text;
using ExcelDataReader.Core.CsvFormat;

namespace ExcelDataReader;

internal class ExcelCsvReader : ExcelDataReader<CsvWorkbook, CsvWorksheet>
{
	public ExcelCsvReader(Stream stream, Encoding fallbackEncoding, char[] autodetectSeparators, int analyzeInitialCsvRows)
	{
		base.Workbook = new CsvWorkbook(stream, fallbackEncoding, autodetectSeparators, analyzeInitialCsvRows);
		Reset();
	}

	public override void Close()
	{
		base.Close();
		base.Workbook?.Stream?.Dispose();
		base.Workbook = null;
	}
}
