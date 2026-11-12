using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Stimulsoft.Base.Excel;

public static class StiExcelHelper
{
	private static object lockOjbect = new object();

	private static Hashtable cache = new Hashtable();

	public static bool AllowCache { get; set; } = true;


	public static DataSet GetDataSetFromExcelDocument(byte[] content, bool firstRowIsHeader = true)
	{
		if (content == null)
		{
			return null;
		}
		using MemoryStream stream = new MemoryStream(content);
		return GetDataSetFromExcelDocument(stream, firstRowIsHeader);
	}

	private static DataSet GetDataSetFromExcelDocument(Stream stream, bool firstRowIsHeader = true)
	{
		if (stream == null || stream.Length <= 4)
		{
			return null;
		}
		string key = CalculateHashCode(stream);
		if (AllowCache)
		{
			lock (lockOjbect)
			{
				if (cache[key] is DataSet dataSet)
				{
					return dataSet.Copy();
				}
			}
		}
		byte[] array = new byte[4];
		stream.Read(array, 0, 4);
		stream.Seek(0L, SeekOrigin.Begin);
		DataSet dataSet2 = null;
		dataSet2 = ((array[0] != 80 || array[1] != 75 || array[2] != 3 || array[3] != 4) ? GetDataSetFromXlsAlternate(stream, firstRowIsHeader) : GetDataSetFromXlsx(stream, firstRowIsHeader));
		if (AllowCache)
		{
			DataSet value = dataSet2.Copy();
			lock (lockOjbect)
			{
				cache[key] = value;
			}
		}
		return dataSet2;
	}

	private static DataSet GetDataSetFromXlsAlternate(Stream stream, bool firstRowIsHeader = true, bool withoutTypes = false)
	{
		DataSet dataSet = new DataSet();
		Assembly assembly = StiAssemblyFinder.GetAssembly("LibExcel.dll");
		if (assembly == null)
		{
			throw new Exception("'LibExcel.dll' assembly not found.");
		}
		Type type = assembly.GetType("ExcelLibrary.SpreadSheet.Workbook");
		MethodInfo method = type.GetMethod("Load", new Type[1] { typeof(Stream) });
		object[] parameters = new Stream[1] { stream };
		object obj = method.Invoke(null, parameters);
		IList list = obj.GetType().GetField("Worksheets").GetValue(obj) as IList;
		foreach (object item in list)
		{
			string tableName = item.GetType().GetField("Name").GetValue(item) as string;
			DataTable dataTable = new DataTable(tableName);
			object value = item.GetType().GetField("Cells").GetValue(item);
			Type type2 = value.GetType();
			int num = (int)type2.GetField("FirstColIndex").GetValue(value);
			int num2 = (int)type2.GetField("LastColIndex").GetValue(value);
			int num3 = (int)type2.GetField("FirstRowIndex").GetValue(value);
			int num4 = (int)type2.GetField("LastRowIndex").GetValue(value);
			for (int i = num; i <= num2; i++)
			{
				dataTable.Columns.Add();
			}
			for (int j = num3; j <= num4; j++)
			{
				MethodInfo method2 = type2.GetMethod("GetRow", new Type[1] { typeof(int) });
				object obj2 = method2.Invoke(value, new object[1] { j });
				Type type3 = obj2.GetType();
				int num5 = (int)type3.GetField("FirstColIndex").GetValue(obj2);
				int num6 = (int)type3.GetField("LastColIndex").GetValue(obj2);
				DataRow dataRow = dataTable.NewRow();
				for (int k = num5; k <= num6; k++)
				{
					if (k < dataRow.ItemArray.Length)
					{
						MethodInfo method3 = obj2.GetType().GetMethod("GetCell", new Type[1] { typeof(int) });
						object obj3 = method3.Invoke(obj2, new object[1] { k });
						if (withoutTypes)
						{
							dataRow[k] = obj3.GetType().GetProperty("StringValue").GetValue(obj3, null);
						}
						else
						{
							dataRow[k] = obj3;
						}
					}
				}
				dataTable.Rows.Add(dataRow);
			}
			if (firstRowIsHeader && dataTable.Rows.Count > 0)
			{
				DataRow dataRow2 = dataTable.Rows[0];
				foreach (DataColumn column in dataTable.Columns)
				{
					try
					{
						string text = dataRow2[column] as string;
						if (!string.IsNullOrWhiteSpace(text) && dataTable.Columns[text] == null)
						{
							column.ColumnName = text;
						}
					}
					catch
					{
					}
				}
				dataTable.Rows.RemoveAt(0);
			}
			dataSet.Tables.Add(dataTable);
		}
		return dataSet;
	}

	private static DataSet GetDataSetFromXlsx(Stream stream, bool firstRowIsHeader = true)
	{
		IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		excelDataReader.IsFirstRowAsColumnNames = firstRowIsHeader;
		DataSet result = excelDataReader.AsDataSet();
		excelDataReader.Close();
		return result;
	}

	public static DataSet GetDataSetFromXlsWithoutTypes(Stream stream, bool firstRowIsHeader = true)
	{
		return GetDataSetFromXlsAlternate(stream, firstRowIsHeader, withoutTypes: true);
	}

	public static DataSet GetDataSetFromXlsxWithoutTypes(Stream stream, bool firstRowIsHeader = true)
	{
		DataSet dataSetFromXlsx = GetDataSetFromXlsx(stream, firstRowIsHeader);
		return CloneDataSetWithoutTypes(dataSetFromXlsx);
	}

	private static DataSet CloneDataSetWithoutTypes(DataSet inputDataSet)
	{
		DataSet dataSet = new DataSet(inputDataSet.DataSetName);
		foreach (DataTable table in inputDataSet.Tables)
		{
			DataTable dataTable2 = new DataTable(table.TableName);
			foreach (DataColumn column in table.Columns)
			{
				dataTable2.Columns.Add(column.ColumnName, typeof(string));
			}
			foreach (DataRow row in table.Rows)
			{
				DataRow dataRow2 = dataTable2.NewRow();
				dataRow2.ItemArray = (object[])row.ItemArray.Clone();
				dataTable2.Rows.Add(dataRow2);
			}
			dataSet.Tables.Add(dataTable2);
		}
		return dataSet;
	}

	public static void CleanCache()
	{
		lock (lockOjbect)
		{
			cache.Clear();
		}
	}

	private static string CalculateHashCode(Stream stream)
	{
		if (!AllowCache)
		{
			return null;
		}
		try
		{
			long num = Math.Min(5000L, stream.Length);
			byte[] buffer = new byte[num];
			stream.Read(buffer, 0, (int)num);
			return BytesToString(SHA256.Create().ComputeHash(buffer)) + stream.Length;
		}
		finally
		{
			stream.Seek(0L, SeekOrigin.Begin);
		}
	}

	private static string BytesToString(byte[] bytes)
	{
		string text = "";
		foreach (byte b in bytes)
		{
			text += b.ToString("x2");
		}
		return text;
	}
}
