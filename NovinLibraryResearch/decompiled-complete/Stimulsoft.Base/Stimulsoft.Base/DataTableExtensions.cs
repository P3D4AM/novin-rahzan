using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base;

public static class DataTableExtensions
{
	public static byte[] WriteToArray(this DataTable table)
	{
		if (table == null)
		{
			return null;
		}
		using MemoryStream memoryStream = new MemoryStream();
		table.WriteXml(memoryStream, XmlWriteMode.WriteSchema);
		return memoryStream.ToArray();
	}

	public static string WriteToString(this DataTable table)
	{
		if (table == null)
		{
			return null;
		}
		using StringWriter stringWriter = new StringWriter();
		table.WriteXml(stringWriter, XmlWriteMode.WriteSchema);
		return stringWriter.ToString();
	}

	public static string WriteToJson(this DataTable table)
	{
		if (table == null)
		{
			return null;
		}
		return JsonConvert.SerializeObject(table, Formatting.Indented, StiJsonHelper.DefaultSerializerSettings);
	}

	public static string WriteToString(this DataTable table, StiDataFormatType type)
	{
		switch (type)
		{
		case StiDataFormatType.Xml:
			return table.WriteToString();
		case StiDataFormatType.Json:
		{
			if (table.Rows.Count != 0)
			{
				break;
			}
			List<StiDataColumnSchema> list = new List<StiDataColumnSchema>();
			foreach (DataColumn column in table.Columns)
			{
				list.Add(new StiDataColumnSchema(column.ColumnName, column.DataType));
			}
			return JsonConvert.SerializeObject(list);
		}
		}
		return table.WriteToJson();
	}

	public static void Read(this DataTable table, byte[] content)
	{
		if (content == null || content.Length == 0)
		{
			return;
		}
		using MemoryStream stream = new MemoryStream(content);
		table.ReadXml(stream);
	}

	public static void Read(this DataTable table, string content)
	{
		if (content == null)
		{
			return;
		}
		using StringReader reader = new StringReader(content);
		table.ReadXml(reader);
	}

	public static DataTable ReadFromJson(string json)
	{
		if (string.IsNullOrWhiteSpace(json))
		{
			return null;
		}
		return JsonConvert.DeserializeObject<DataTable>(json);
	}

	public static DataTable ReadFromString(string content)
	{
		if (string.IsNullOrWhiteSpace(content))
		{
			return null;
		}
		DataTable dataTable = new DataTable();
		dataTable.Read(content);
		return dataTable;
	}

	public static DataTable ReadFrom(StiDataFormatType type, string str)
	{
		if (type != StiDataFormatType.Json)
		{
			return ReadFromString(str);
		}
		return ReadFromJson(str);
	}

	public static DataTable AddColumn(this DataTable table, string name)
	{
		return table.AddColumn(name, typeof(string));
	}

	public static DataTable AddColumn(this DataTable table, string name, Type type)
	{
		table.Columns.Add(name, type);
		return table;
	}

	public static DataTable AddColumns(this DataTable table, params string[] names)
	{
		names.ToList().ForEach(delegate(string n)
		{
			table.Columns.Add(n);
		});
		return table;
	}
}
