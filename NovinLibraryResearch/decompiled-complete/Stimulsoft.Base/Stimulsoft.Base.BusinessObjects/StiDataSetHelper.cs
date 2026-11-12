using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Stimulsoft.Base.BusinessObjects;

public static class StiDataSetHelper
{
	private static string CheckName(string name)
	{
		StringBuilder stringBuilder = new StringBuilder(name);
		stringBuilder.Replace(" ", "_x0020_");
		stringBuilder.Replace("@", "_x0040_");
		stringBuilder.Replace("~", "_x007e_");
		stringBuilder.Replace("$", "_x0024_");
		stringBuilder.Replace("#", "_x0023_");
		stringBuilder.Replace("%", "_x0025_");
		stringBuilder.Replace("&", "_x0026_");
		stringBuilder.Replace("*", "_x002A_");
		stringBuilder.Replace("^", "_x005E_");
		stringBuilder.Replace("(", "_x0028_");
		stringBuilder.Replace(")", "_x0029_");
		stringBuilder.Replace("!", "_x0021_");
		return stringBuilder.ToString();
	}

	private static bool CheckIsFixedColumn(IEnumerable<StiFixedColumnInfo> fixedColumnsInfo, DataTable table, DataColumn column)
	{
		if (fixedColumnsInfo == null)
		{
			return false;
		}
		foreach (StiFixedColumnInfo item in fixedColumnsInfo)
		{
			if (item.Table == table && item.FixedColumn == column)
			{
				return true;
			}
		}
		return false;
	}

	private static byte[] Save(DataSet dataSet, IEnumerable<StiFixedColumnInfo> fixedColumnsInfo)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Formatting = Formatting.Indented;
		string name = (string.IsNullOrEmpty(dataSet.DataSetName) ? "dataSet1" : dataSet.DataSetName);
		xmlTextWriter.WriteStartElement("StiDataSet");
		xmlTextWriter.WriteAttributeString("name", CheckName(name));
		xmlTextWriter.WriteAttributeString("version", "1.1");
		List<string> list = new List<string>();
		foreach (DataTable table in dataSet.Tables)
		{
			list.Add(table.TableName);
			foreach (DataColumn column in table.Columns)
			{
				list.Add(column.ColumnName);
			}
			foreach (DataRelation childRelation in table.ChildRelations)
			{
				list.Add(childRelation.RelationName);
			}
			foreach (DataRelation parentRelation in table.ParentRelations)
			{
				list.Add(parentRelation.RelationName);
			}
		}
		xmlTextWriter.WriteStartElement("Reduction");
		foreach (string item in list)
		{
			if (!dictionary2.ContainsKey(item))
			{
				string text = $"i{dictionary2.Count}";
				dictionary2.Add(item, text);
				xmlTextWriter.WriteStartElement(CheckName(item));
				xmlTextWriter.WriteString(text);
				xmlTextWriter.WriteEndElement();
			}
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("TablesInfo");
		foreach (DataTable table2 in dataSet.Tables)
		{
			int num = 0;
			string[] array = new string[table2.Columns.Count];
			xmlTextWriter.WriteStartElement(dictionary2[table2.TableName]);
			foreach (DataColumn column2 in table2.Columns)
			{
				xmlTextWriter.WriteStartElement(dictionary2[column2.ColumnName]);
				if (CheckIsFixedColumn(fixedColumnsInfo, table2, column2))
				{
					xmlTextWriter.WriteAttributeString("IsFixed", "true");
				}
				array[num++] = column2.ColumnName;
				xmlTextWriter.WriteString(column2.DataType.ToString());
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			dictionary.Add(table2.TableName, array);
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Contents");
		List<string> list2 = new List<string>();
		foreach (DataTable table3 in dataSet.Tables)
		{
			string[] array2 = dictionary[table3.TableName];
			int num2 = array2.Length;
			foreach (DataRow row in table3.Rows)
			{
				list2.Clear();
				int num3 = 0;
				while (num3 < num2)
				{
					string text2 = array2[num3++];
					object obj = row[text2];
					if (!(obj is DBNull) && obj != null)
					{
						list2.Add(text2);
					}
				}
				if (list2.Count > 0)
				{
					xmlTextWriter.WriteStartElement(dictionary2[table3.TableName]);
					num3 = 0;
					while (num3 < list2.Count)
					{
						string text3 = list2[num3++];
						object value = row[text3];
						xmlTextWriter.WriteStartElement(dictionary2[text3]);
						xmlTextWriter.WriteValue(value);
						xmlTextWriter.WriteEndElement();
					}
					xmlTextWriter.WriteEndElement();
				}
			}
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("RelationsInfo");
		foreach (DataRelation relation in dataSet.Relations)
		{
			xmlTextWriter.WriteStartElement("Relation");
			xmlTextWriter.WriteElementString("RelationName", dictionary2[relation.RelationName]);
			xmlTextWriter.WriteElementString("ParentTable", dictionary2[relation.ParentTable.TableName]);
			xmlTextWriter.WriteElementString("ChildTable", dictionary2[relation.ChildTable.TableName]);
			xmlTextWriter.WriteStartElement("ParentColumns");
			DataColumn[] parentColumns = relation.ParentColumns;
			foreach (DataColumn dataColumn3 in parentColumns)
			{
				xmlTextWriter.WriteStartElement(dictionary2[dataColumn3.ColumnName]);
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("ChildColumns");
			DataColumn[] childColumns = relation.ChildColumns;
			foreach (DataColumn dataColumn4 in childColumns)
			{
				xmlTextWriter.WriteStartElement(dictionary2[dataColumn4.ColumnName]);
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Flush();
		byte[] result = memoryStream.ToArray();
		memoryStream.Close();
		memoryStream.Dispose();
		xmlTextWriter.Close();
		Thread.CurrentThread.CurrentCulture = currentCulture;
		return result;
	}

	public static void SaveDataSet(DataSet dateSet)
	{
		SaveDataSet(dateSet, null);
	}

	public static void SaveDataSet(DataSet dateSet, IEnumerable<StiFixedColumnInfo> fixedColumnsInfo)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		if (dateSet == null && dateSet.Tables.Count <= 0)
		{
			return;
		}
		SaveFileDialog val = new SaveFileDialog();
		try
		{
			((FileDialog)val).Filter = ".data|*.data";
			((FileDialog)val).FileName = "Demo";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				byte[] bytes = Save(dateSet, fixedColumnsInfo);
				File.WriteAllBytes(((FileDialog)val).FileName, bytes);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static byte[] SaveDataSetToByteArray(DataSet dateSet)
	{
		return SaveDataSetToByteArray(dateSet, null);
	}

	public static byte[] SaveDataSetToByteArray(DataSet dateSet, IEnumerable<StiFixedColumnInfo> fixedColumnsInfo)
	{
		byte[] result = null;
		if (dateSet != null && dateSet.Tables.Count > 0)
		{
			result = Save(dateSet, fixedColumnsInfo);
		}
		return result;
	}

	public static string SaveDataSetToString(DataSet dateSet)
	{
		return SaveDataSetToString(dateSet, null);
	}

	public static string SaveDataSetToString(DataSet dateSet, IEnumerable<StiFixedColumnInfo> fixedColumnsInfo)
	{
		byte[] bytes = null;
		if (dateSet != null && dateSet.Tables.Count > 0)
		{
			bytes = Save(dateSet, fixedColumnsInfo);
		}
		return Encoding.UTF8.GetString(bytes);
	}
}
