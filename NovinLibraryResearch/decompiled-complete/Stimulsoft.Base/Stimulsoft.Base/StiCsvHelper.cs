using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace Stimulsoft.Base;

public class StiCsvHelper
{
	public static int[] CodePageCodes = new int[12]
	{
		0, 1, 65000, 65001, 1200, 1250, 1251, 1252, 1253, 1254,
		1255, 1256
	};

	public static string[] CodePageNames = new string[12]
	{
		"Default", "System", "UTF7", "UTF8", "Unicode", "1250", "1251", "1252", "1253", "1254",
		"1255", "1256"
	};

	public static DataTable GetTable(string path, int maxDataRows = 0)
	{
		return GetTable(path, 0, null, maxDataRows);
	}

	public static DataTable GetTable(string path, int codePage, string separator, int maxDataRows = 0)
	{
		return GetTable(File.ReadAllBytes(path), codePage, separator, maxDataRows);
	}

	public static DataTable GetTable(byte[] data, int maxDataRows = 0)
	{
		return GetTable(data, 0, null, maxDataRows);
	}

	public static DataSet GetDataSet(byte[] data, string tableName, int codePage, string separator, int maxDataRows = 0)
	{
		DataTable table = GetTable(data, codePage, separator, maxDataRows);
		if (table == null)
		{
			return null;
		}
		table.TableName = (string.IsNullOrWhiteSpace(tableName) ? "CSV-Table" : tableName);
		DataSet dataSet = new DataSet
		{
			EnforceConstraints = false
		};
		dataSet.Tables.Add(table);
		return dataSet;
	}

	public static DataTable GetTable(byte[] data, int codePage, string separator, int maxDataRows = 0)
	{
		return GetTable(data, codePage, separator, loadData: true, maxDataRows);
	}

	public static DataTable GetTable(byte[] data, int codePage, string separator, bool loadData, int maxDataRows = 0)
	{
		using MemoryStream stream = new MemoryStream(data);
		string text = ((!string.IsNullOrEmpty(separator)) ? separator : CultureInfo.CurrentCulture.TextInfo.ListSeparator);
		char c = text[0];
		Encoding encoding = null;
		if (codePage != 0)
		{
			encoding = ((codePage == 1) ? Encoding.Default : Encoding.GetEncoding(codePage));
		}
		using StreamReader streamReader = ((encoding == null) ? new StreamReader(stream) : new StreamReader(stream, encoding));
		List<string> list = null;
		string inputString = streamReader.ReadLine();
		if (inputString != null && inputString.Length > 0)
		{
			list = SplitToColumns(ref inputString, streamReader, c);
			if (list.Count == 1)
			{
				list = SplitToColumns(ref inputString, streamReader, '\t');
				if (list.Count > 1)
				{
					c = '\t';
				}
				else
				{
					list = SplitToColumns(ref inputString, streamReader, ';');
					if (list.Count > 1)
					{
						c = ';';
					}
					else
					{
						list = SplitToColumns(ref inputString, streamReader, ',');
						if (list.Count > 1)
						{
							c = ',';
						}
					}
				}
			}
		}
		Hashtable hashtable = new Hashtable();
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				string text2 = list[i].Trim();
				if (text2.Length == 0)
				{
					text2 = $"Column{i + 1}";
				}
				text2 = StiDataNameValidator.Correct(text2);
				string text3 = string.Empty;
				int num = 0;
				while (hashtable.Contains(text2 + text3))
				{
					num++;
					text3 = num.ToString();
				}
				text2 = (list[i] = text2 + text3);
				hashtable.Add(text2, text2);
			}
		}
		DataTable dataTable = new DataTable("CSV-Table");
		if (list != null)
		{
			foreach (string item in list)
			{
				Type typeFromHandle = typeof(string);
				DataColumn column = new DataColumn(item, typeFromHandle);
				dataTable.Columns.Add(column);
			}
		}
		if (loadData && list != null)
		{
			while ((inputString = streamReader.ReadLine()) != null)
			{
				if (inputString.Length == 0)
				{
					inputString = new string(c, list.Count - 1);
				}
				List<string> list2 = SplitToColumns(ref inputString, streamReader, c);
				DataRow dataRow = dataTable.NewRow();
				int num2 = Math.Min(dataRow.ItemArray.Length, list2.Count);
				for (int j = 0; j < num2; j++)
				{
					dataRow[j] = list2[j];
				}
				dataTable.Rows.Add(dataRow);
				if (maxDataRows != 0 && maxDataRows < dataTable.Rows.Count)
				{
					break;
				}
			}
		}
		return dataTable;
	}

	private static List<string> SplitToColumns(ref string inputString, StreamReader sr, char delimiter)
	{
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		bool flag = false;
		while (num < inputString.Length)
		{
			char c = inputString[num];
			num++;
			if (c == '"')
			{
				flag = !flag;
			}
			if (c == delimiter && !flag)
			{
				list.Add(stringBuilder.ToString());
				stringBuilder = new StringBuilder();
				continue;
			}
			stringBuilder.Append(c);
			if (num == inputString.Length && flag)
			{
				string text = sr.ReadLine();
				if (text != null)
				{
					inputString = inputString + "\n" + text;
				}
			}
		}
		list.Add(stringBuilder.ToString());
		for (int i = 0; i < list.Count; i++)
		{
			string text2 = list[i];
			if (text2.Length > 1 && text2.StartsWith("\"", StringComparison.InvariantCulture) && text2.EndsWith("\"", StringComparison.InvariantCulture))
			{
				list[i] = text2.Substring(1, text2.Length - 2).Replace("\"\"", "\"");
			}
		}
		return list;
	}
}
