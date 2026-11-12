using System;
using System.Collections.Generic;
using ExcelDataReader.Core.NumberFormat;

namespace ExcelDataReader.Core;

internal class CommonWorkbook
{
	private class ExtendedFormat
	{
		public int XfId { get; set; }

		public int FormatIndex { get; set; }

		public bool ApplyNumberFormat { get; set; }
	}

	public Dictionary<int, NumberFormatString> Formats { get; } = new Dictionary<int, NumberFormatString>();


	private Dictionary<int, int> FormatMappings { get; } = new Dictionary<int, int>();


	private List<ExtendedFormat> ExtendedFormats { get; } = new List<ExtendedFormat>();


	public CommonWorkbook()
	{
		for (int i = 0; i < 163; i++)
		{
			NumberFormatString builtinNumberFormat = BuiltinNumberFormat.GetBuiltinNumberFormat(i);
			if (builtinNumberFormat != null)
			{
				Formats.Add(i, builtinNumberFormat);
			}
		}
	}

	public int GetExtendedFormatCount()
	{
		return ExtendedFormats.Count;
	}

	public int GetNumberFormatFromXF(int xfIndex)
	{
		if (xfIndex < 0 || xfIndex >= ExtendedFormats.Count)
		{
			return 0;
		}
		if (!ExtendedFormats[xfIndex].ApplyNumberFormat)
		{
			return 0;
		}
		return GetNumberFormatFromFileIndex(ExtendedFormats[xfIndex].FormatIndex);
	}

	public int GetNumberFormatFromFileIndex(int formatIndexInFile)
	{
		if (FormatMappings.TryGetValue(formatIndexInFile, out var value))
		{
			return value;
		}
		return formatIndexInFile;
	}

	public void AddNumberFormat(int formatIndexInFile, string formatString)
	{
		bool flag = false;
		int num = 163;
		foreach (KeyValuePair<int, NumberFormatString> format in Formats)
		{
			if (!flag && format.Value.FormatString == formatString)
			{
				FormatMappings[formatIndexInFile] = format.Key;
				flag = true;
			}
			num = Math.Max(num, format.Key);
		}
		if (!flag)
		{
			num++;
			Formats.Add(num, new NumberFormatString(formatString));
			FormatMappings[formatIndexInFile] = num;
		}
	}

	public void AddExtendedFormat(int xfId, int formatIndexInFile, bool applyNumberFormat)
	{
		ExtendedFormats.Add(new ExtendedFormat
		{
			XfId = xfId,
			FormatIndex = formatIndexInFile,
			ApplyNumberFormat = applyNumberFormat
		});
	}
}
