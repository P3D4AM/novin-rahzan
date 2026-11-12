using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Stimulsoft.Base.Excel;

internal static class Helpers
{
	private static Regex re = new Regex("_x([0-9A-F]{4,4})_");

	public static bool IsSingleByteEncoding(Encoding encoding)
	{
		return encoding.IsSingleByte;
	}

	public static double Int64BitsToDouble(long value)
	{
		return BitConverter.ToDouble(BitConverter.GetBytes(value), 0);
	}

	public static string ConvertEscapeChars(string input)
	{
		return re.Replace(input, (Match m) => ((char)uint.Parse(m.Groups[1].Value, NumberStyles.HexNumber)).ToString());
	}

	public static object ConvertFromOATime(double value)
	{
		if (value >= 0.0 && value < 60.0)
		{
			value += 1.0;
		}
		return DateTime.FromOADate(value);
	}

	internal static void FixDataTypes(DataSet dataset)
	{
		List<DataTable> list = new List<DataTable>(dataset.Tables.Count);
		bool flag = false;
		foreach (DataTable table in dataset.Tables)
		{
			if (table.Rows.Count == 0)
			{
				list.Add(table);
				continue;
			}
			DataTable dataTable2 = null;
			for (int i = 0; i < table.Columns.Count; i++)
			{
				Type type = null;
				foreach (DataRow row2 in table.Rows)
				{
					if (row2.IsNull(i))
					{
						continue;
					}
					Type type2 = row2[i].GetType();
					if (type2 != type)
					{
						if (!(type == null))
						{
							type = null;
							break;
						}
						type = type2;
					}
				}
				if (type != null)
				{
					flag = true;
					if (dataTable2 == null)
					{
						dataTable2 = table.Clone();
					}
					dataTable2.Columns[i].DataType = type;
				}
			}
			if (dataTable2 != null)
			{
				dataTable2.BeginLoadData();
				foreach (DataRow row3 in table.Rows)
				{
					dataTable2.ImportRow(row3);
				}
				dataTable2.EndLoadData();
				list.Add(dataTable2);
			}
			else
			{
				list.Add(table);
			}
		}
		if (flag)
		{
			dataset.Tables.Clear();
			dataset.Tables.AddRange(list.ToArray());
		}
	}

	public static void AddColumnHandleDuplicate(DataTable table, string columnName)
	{
		string text = columnName;
		DataColumn dataColumn = table.Columns[columnName];
		int num = 1;
		while (dataColumn != null)
		{
			text = $"{columnName}_{num}";
			dataColumn = table.Columns[text];
			num++;
		}
		table.Columns.Add(text, typeof(object));
	}
}
