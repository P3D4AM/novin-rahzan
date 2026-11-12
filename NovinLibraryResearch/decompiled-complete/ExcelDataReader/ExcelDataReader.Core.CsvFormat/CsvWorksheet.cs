using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelDataReader.Core.NumberFormat;

namespace ExcelDataReader.Core.CsvFormat;

internal class CsvWorksheet : IWorksheet
{
	public string Name => string.Empty;

	public string CodeName => null;

	public string VisibleState => null;

	public HeaderFooter HeaderFooter => null;

	public CellRange[] MergeCells => null;

	public int FieldCount { get; }

	public int RowCount
	{
		get
		{
			if (AnalyzedPartial)
			{
				throw new InvalidOperationException("Cannot use RowCount with AnalyzeInitialCsvRows > 0");
			}
			return AnalyzedRowCount;
		}
	}

	public Stream Stream { get; }

	public Encoding Encoding { get; }

	public char Separator { get; }

	public Col[] ColumnWidths => null;

	private int BomLength { get; set; }

	private bool AnalyzedPartial { get; }

	private int AnalyzedRowCount { get; }

	public CsvWorksheet(Stream stream, Encoding fallbackEncoding, char[] autodetectSeparators, int analyzeInitialCsvRows)
	{
		Stream = stream;
		Stream.Seek(0L, SeekOrigin.Begin);
		try
		{
			CsvAnalyzer.Analyze(Stream, autodetectSeparators, Encoding.UTF8, analyzeInitialCsvRows, out var fieldCount, out var autodetectSeparator, out var autodetectEncoding, out var bomLength, out var rowCount);
			FieldCount = fieldCount;
			AnalyzedRowCount = rowCount;
			AnalyzedPartial = analyzeInitialCsvRows > 0;
			Encoding = autodetectEncoding;
			Separator = autodetectSeparator;
			BomLength = bomLength;
		}
		catch (DecoderFallbackException)
		{
			Stream.Seek(0L, SeekOrigin.Begin);
			CsvAnalyzer.Analyze(Stream, autodetectSeparators, fallbackEncoding, analyzeInitialCsvRows, out var fieldCount2, out var autodetectSeparator2, out var autodetectEncoding2, out var bomLength2, out var rowCount2);
			FieldCount = fieldCount2;
			AnalyzedRowCount = rowCount2;
			AnalyzedPartial = analyzeInitialCsvRows > 0;
			Encoding = autodetectEncoding2;
			Separator = autodetectSeparator2;
			BomLength = bomLength2;
		}
	}

	public NumberFormatString GetNumberFormatString(int index)
	{
		return null;
	}

	public IEnumerable<Row> ReadRows()
	{
		int bufferSize = 1024;
		byte[] buffer = new byte[bufferSize];
		int rowIndex = 0;
		CsvParser csv = new CsvParser(Separator, Encoding);
		int skipBomBytes = BomLength;
		Stream.Seek(0L, SeekOrigin.Begin);
		while (Stream.Position < Stream.Length)
		{
			int num = Stream.Read(buffer, 0, bufferSize);
			csv.ParseBuffer(buffer, skipBomBytes, num - skipBomBytes, out var bufferRows);
			skipBomBytes = 0;
			foreach (Row readerRow in GetReaderRows(rowIndex, bufferRows))
			{
				yield return readerRow;
			}
			rowIndex += bufferRows.Count;
			bufferRows = null;
		}
		csv.Flush(out var rows);
		foreach (Row readerRow2 in GetReaderRows(rowIndex, rows))
		{
			yield return readerRow2;
		}
	}

	private IEnumerable<Row> GetReaderRows(int rowIndex, List<List<string>> rows)
	{
		foreach (List<string> row in rows)
		{
			List<Cell> list = new List<Cell>(row.Count);
			for (int i = 0; i < row.Count; i++)
			{
				list.Add(new Cell
				{
					ColumnIndex = i,
					Value = row[i]
				});
			}
			yield return new Row
			{
				Height = 12.75,
				Cells = list,
				RowIndex = rowIndex
			};
			rowIndex++;
		}
	}
}
