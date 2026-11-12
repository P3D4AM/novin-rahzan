using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExcelDataReader.Core.CsvFormat;

internal class CsvWorkbook : IWorkbook<CsvWorksheet>
{
	public int ResultsCount => 1;

	public Stream Stream { get; }

	public Encoding Encoding { get; }

	public char[] AutodetectSeparators { get; }

	public int AnalyzeInitialCsvRows { get; }

	public CsvWorkbook(Stream stream, Encoding encoding, char[] autodetectSeparators, int analyzeInitialCsvRows)
	{
		Stream = stream;
		Encoding = encoding;
		AutodetectSeparators = autodetectSeparators;
		AnalyzeInitialCsvRows = analyzeInitialCsvRows;
	}

	public IEnumerable<CsvWorksheet> ReadWorksheets()
	{
		yield return new CsvWorksheet(Stream, Encoding, AutodetectSeparators, AnalyzeInitialCsvRows);
	}
}
