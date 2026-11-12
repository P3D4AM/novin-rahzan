using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExcelDataReader.Core.CsvFormat;

internal static class CsvAnalyzer
{
	private class SeparatorInfo
	{
		public int MaxFieldCount { get; set; }

		public int SumFieldCount { get; set; }

		public int RowCount { get; set; }

		public CsvParser Buffer { get; set; }
	}

	public static void Analyze(Stream stream, char[] separators, Encoding fallbackEncoding, int analyzeInitialCsvRows, out int fieldCount, out char autodetectSeparator, out Encoding autodetectEncoding, out int bomLength, out int rowCount)
	{
		int count = 16;
		byte[] array = new byte[1024];
		int initialBytesRead = stream.Read(array, 0, count);
		autodetectEncoding = GetEncodingFromBom(array, out bomLength);
		if (autodetectEncoding == null)
		{
			autodetectEncoding = fallbackEncoding;
		}
		if (separators == null || separators.Length == 0)
		{
			separators = new char[1];
		}
		SeparatorInfo[] array2 = new SeparatorInfo[separators.Length];
		for (int i = 0; i < separators.Length; i++)
		{
			array2[i] = new SeparatorInfo();
			array2[i].Buffer = new CsvParser(separators[i], autodetectEncoding);
		}
		AnalyzeCsvRows(stream, array, initialBytesRead, bomLength, analyzeInitialCsvRows, separators, array2);
		FlushSeparatorsBuffers(separators, array2);
		SeparatorInfo separatorInfo = array2[0];
		char c = separators[0];
		double num = double.MaxValue;
		for (int j = 0; j < separators.Length; j++)
		{
			char c2 = separators[j];
			SeparatorInfo separatorInfo2 = array2[j];
			if (separatorInfo2.RowCount != 0 && separatorInfo2.MaxFieldCount > 1)
			{
				double num2 = (double)separatorInfo2.SumFieldCount / (double)separatorInfo2.RowCount;
				double num3 = (double)separatorInfo2.MaxFieldCount - num2;
				if (num3 < num)
				{
					num = num3;
					c = c2;
					separatorInfo = separatorInfo2;
				}
			}
		}
		autodetectSeparator = c;
		fieldCount = separatorInfo.MaxFieldCount;
		rowCount = ((analyzeInitialCsvRows == 0) ? separatorInfo.RowCount : (-1));
	}

	private static void AnalyzeCsvRows(Stream inputStream, byte[] buffer, int initialBytesRead, int bomLength, int analyzeInitialCsvRows, char[] separators, SeparatorInfo[] separatorInfos)
	{
		ParseSeparatorsBuffer(buffer, bomLength, initialBytesRead - bomLength, separators, separatorInfos);
		if (IsMinNumberOfRowAnalyzed(analyzeInitialCsvRows, separatorInfos))
		{
			return;
		}
		while (inputStream.Position < inputStream.Length)
		{
			int count = inputStream.Read(buffer, 0, buffer.Length);
			ParseSeparatorsBuffer(buffer, 0, count, separators, separatorInfos);
			if (IsMinNumberOfRowAnalyzed(analyzeInitialCsvRows, separatorInfos))
			{
				break;
			}
		}
	}

	private static bool IsMinNumberOfRowAnalyzed(int analyzeInitialCsvRows, SeparatorInfo[] separatorInfos)
	{
		if (analyzeInitialCsvRows > 0)
		{
			for (int i = 0; i < separatorInfos.Length; i++)
			{
				if (separatorInfos[i].RowCount >= analyzeInitialCsvRows)
				{
					return true;
				}
			}
		}
		return false;
	}

	private static void ParseSeparatorsBuffer(byte[] bytes, int offset, int count, char[] separators, SeparatorInfo[] separatorInfos)
	{
		for (int i = 0; i < separators.Length; i++)
		{
			_ = separators[i];
			SeparatorInfo separatorInfo = separatorInfos[i];
			separatorInfo.Buffer.ParseBuffer(bytes, offset, count, out var rows);
			foreach (List<string> item in rows)
			{
				separatorInfo.MaxFieldCount = Math.Max(separatorInfo.MaxFieldCount, item.Count);
				separatorInfo.SumFieldCount += item.Count;
				separatorInfo.RowCount++;
			}
		}
	}

	private static void FlushSeparatorsBuffers(char[] separators, SeparatorInfo[] separatorInfos)
	{
		for (int i = 0; i < separators.Length; i++)
		{
			_ = separators[i];
			SeparatorInfo separatorInfo = separatorInfos[i];
			separatorInfo.Buffer.Flush(out var rows);
			foreach (List<string> item in rows)
			{
				separatorInfo.MaxFieldCount = Math.Max(separatorInfo.MaxFieldCount, item.Count);
				separatorInfo.SumFieldCount += item.Count;
				separatorInfo.RowCount++;
			}
		}
	}

	private static Encoding GetEncodingFromBom(byte[] bom, out int bomLength)
	{
		Encoding[] array = new Encoding[3]
		{
			Encoding.Unicode,
			Encoding.BigEndianUnicode,
			Encoding.UTF8
		};
		foreach (Encoding encoding in array)
		{
			if (IsEncodingPreamble(bom, encoding, out var bomLength2))
			{
				bomLength = bomLength2;
				return encoding;
			}
		}
		bomLength = 0;
		return null;
	}

	private static bool IsEncodingPreamble(byte[] bom, Encoding encoding, out int bomLength)
	{
		bomLength = 0;
		byte[] preamble = encoding.GetPreamble();
		if (preamble.Length > bom.Length)
		{
			return false;
		}
		int i;
		for (i = 0; i < preamble.Length; i++)
		{
			if (preamble[i] != bom[i])
			{
				return false;
			}
		}
		bomLength = i;
		return true;
	}
}
