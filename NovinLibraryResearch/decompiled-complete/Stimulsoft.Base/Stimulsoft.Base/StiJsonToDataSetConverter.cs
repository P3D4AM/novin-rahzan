using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base;

public static class StiJsonToDataSetConverter
{
	private static DataTable mainDataTable;

	private static Hashtable hashDublicateTableName;

	public static DataSet GetDataSet(JToken token)
	{
		DataSet dataSet = new DataSet();
		ParseJToken(dataSet, null, token);
		CheckColumnType(dataSet);
		RemoveEmptyTable(dataSet);
		hashDublicateTableName = null;
		return dataSet;
	}

	public static DataSet GetDataSet(List<JToken> tokens, bool useOneTable = false)
	{
		DataSet dataSet = new DataSet();
		if (useOneTable)
		{
			mainDataTable = new DataTable();
			dataSet.Tables.Add(mainDataTable);
		}
		JArray jArray = new JArray();
		foreach (JToken token in tokens)
		{
			jArray.Add(token);
		}
		ParseJToken(dataSet, "", jArray);
		CheckColumnType(dataSet);
		RemoveEmptyTable(dataSet);
		hashDublicateTableName = null;
		return dataSet;
	}

	public static DataSet GetDataSet(byte[] content)
	{
		string text = StiBytesToStringConverter.ConvertBytesToString(content);
		return GetDataSet(text);
	}

	public static DataSet GetDataSet(string text)
	{
		if (text == null)
		{
			return null;
		}
		JToken token = JsonConvert.DeserializeObject(text) as JToken;
		return GetDataSet(token);
	}

	public static DataSet GetDataSet(XElement element)
	{
		string value = JsonConvert.SerializeXNode((XObject)(object)element, Formatting.Indented);
		JToken token = JsonConvert.DeserializeObject(value) as JToken;
		return GetDataSet(token);
	}

	public static DataSet GetDataSetFromFile(string path)
	{
		if (File.Exists(path))
		{
			string value = File.ReadAllText(path);
			JToken token = JsonConvert.DeserializeObject(value) as JToken;
			return GetDataSet(token);
		}
		if (!string.IsNullOrEmpty(path) && (path.ToLower().StartsWith("http:") || path.ToLower().StartsWith("https:")))
		{
			byte[] bytes = StiBytesFromURL.Load(path);
			string @string = Encoding.UTF8.GetString(bytes);
			JToken token2 = JsonConvert.DeserializeObject(@string) as JToken;
			return GetDataSet(token2);
		}
		return null;
	}

	public static DataSet GetDataSetFromXmlFile(string path)
	{
		if (File.Exists(path))
		{
			XElement node = XElement.Load(path);
			string value = JsonConvert.SerializeXNode((XObject)(object)node, Formatting.Indented);
			JToken token = JsonConvert.DeserializeObject(value) as JToken;
			return GetDataSet(token);
		}
		return null;
	}

	public static DataSet GetDataSetFromXml(byte[] array)
	{
		if (array != null)
		{
			string value;
			using (MemoryStream memoryStream = new MemoryStream(array))
			{
				XElement node = XElement.Load((Stream)memoryStream);
				value = JsonConvert.SerializeXNode((XObject)(object)node, Formatting.Indented);
			}
			JToken token = JsonConvert.DeserializeObject(value) as JToken;
			return GetDataSet(token);
		}
		return null;
	}

	internal static void CheckColumnType(DataSet dataSet)
	{
		if (StiBaseOptions.JsonParseTypeAsString)
		{
			return;
		}
		for (int i = 0; i < dataSet.Tables.Count; i++)
		{
			for (int j = 0; j < dataSet.Tables[i].Columns.Count; j++)
			{
				Type type = null;
				for (int k = 0; k < dataSet.Tables[i].Rows.Count; k++)
				{
					object obj = dataSet.Tables[i].Rows[k][j];
					Type type2 = ParseString(obj.ToString());
					if (type == null)
					{
						type = type2;
					}
					else if (!type.Equals(type2))
					{
						type = null;
						break;
					}
				}
				if (type != null && !type.Equals(typeof(string)))
				{
					ChangeColumnDataType(dataSet.Tables[i], dataSet.Tables[i].Columns[j], type);
				}
			}
		}
	}

	private static void RemoveEmptyTable(DataSet dataSet)
	{
		DataTable dataTable = null;
		if (dataSet.Tables.Count > 0)
		{
			dataTable = dataSet.Tables[0];
		}
		if (dataTable != null && dataTable.TableName == "Table1" && (dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0))
		{
			dataSet.Tables.Remove(dataTable);
		}
	}

	private static Type ParseString(string str)
	{
		if (bool.TryParse(str, out var _))
		{
			return typeof(bool);
		}
		if (double.TryParse(str, out var _))
		{
			return typeof(double);
		}
		if (DateTime.TryParse(str, out var _))
		{
			return typeof(DateTime);
		}
		if (decimal.TryParse(str, out var _))
		{
			return typeof(decimal);
		}
		if (Guid.TryParse(str, out var _))
		{
			return typeof(Guid);
		}
		if (TimeSpan.TryParse(str, out var _))
		{
			return typeof(TimeSpan);
		}
		if (char.TryParse(str, out var _))
		{
			return typeof(char);
		}
		return typeof(string);
	}

	private static void ChangeColumnDataType(DataTable table, DataColumn column, Type newtype)
	{
		if (!table.Columns.CanRemove(column))
		{
			return;
		}
		DataColumn dataColumn = new DataColumn("temp", newtype);
		table.Columns.Add(dataColumn);
		try
		{
			foreach (DataRow row in table.Rows)
			{
				row["temp"] = StiConvert.ChangeType(row[column], newtype);
			}
			int ordinal = table.Columns.IndexOf(column);
			table.Columns.Remove(column);
			dataColumn.ColumnName = column.ColumnName;
			table.Columns[dataColumn.ColumnName].SetOrdinal(ordinal);
		}
		catch (Exception)
		{
			if (table.Columns.Contains("temp") && table.Columns.Contains(dataColumn.ColumnName))
			{
				table.Columns.Remove(dataColumn);
			}
		}
	}

	private static void ParseJToken(DataSet dataSet, string tokenName, JToken token, DataTable tableProperties = null, DataRow row = null)
	{
		if (token is JContainer)
		{
			ParseJContainer(dataSet, tokenName, token as JContainer, tableProperties, row);
		}
		else if (token is JValue)
		{
			ParseJValue(dataSet, token as JValue);
		}
	}

	private static void ParseJValue(DataSet dataSet, JValue jValue)
	{
		if (mainDataTable == null)
		{
			DataColumn column = new DataColumn("Value", typeof(string));
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add(column);
			DataRow dataRow = dataTable.NewRow();
			dataRow["Value"] = jValue.Value;
			dataTable.Rows.Add(dataRow);
			dataTable.PrimaryKey = new DataColumn[1] { dataTable.Columns[0] };
			dataSet.Tables.Add(dataTable);
		}
	}

	private static void ParseJContainer(DataSet dataSet, string tokenName, JContainer jContainer, DataTable tableProperties = null, DataRow rowProperties = null)
	{
		if (jContainer is JArray)
		{
			ParseJArray(dataSet, tokenName, jContainer as JArray);
		}
		else if (!(jContainer is JConstructor))
		{
			if (jContainer is JObject)
			{
				ParseJObject(dataSet, tokenName, jContainer as JObject);
			}
			else if (jContainer is JProperty)
			{
				ParseJProperty(dataSet, jContainer as JProperty, tableProperties, rowProperties);
			}
		}
	}

	private static void ParseJArray(DataSet dataSet, string tokenName, JArray jArray)
	{
		if (jArray.Count == 0)
		{
			return;
		}
		DataTable table = new DataTable();
		Hashtable hashChildTables = new Hashtable();
		if (TryParseArrayToTable(jArray, out table, out hashChildTables))
		{
			if (mainDataTable != null)
			{
				ParseArrayToTable(jArray, mainDataTable);
				return;
			}
			table.TableName = GetTableName(dataSet, tokenName);
			dataSet.Tables.Add(table);
			if (hashChildTables.Count <= 0)
			{
				return;
			}
			{
				foreach (string key in hashChildTables.Keys)
				{
					JArray array = hashChildTables[key] as JArray;
					DataTable dataTable = new DataTable();
					dataTable.TableName = GetTableName(dataSet, key);
					ParseArrayToTable(array, dataTable);
					string name = $"relation{table.TableName}_{dataTable.TableName}";
					dataSet.Tables.Add(dataTable);
					DataColumn dataColumn = dataTable.Columns["relationId"];
					if (dataColumn != null)
					{
						dataSet.Relations.Add(name, table.Columns[key], dataColumn);
					}
				}
				return;
			}
		}
		if (!CheckArrayForSimpleCreateTable(jArray))
		{
			return;
		}
		if (mainDataTable == null)
		{
			dataSet.Tables.Add(table);
		}
		foreach (JToken item in jArray)
		{
			JArray jArray2 = item as JArray;
			while (table.Columns.Count < jArray2.Count)
			{
				table.Columns.Add(new DataColumn());
			}
			DataRow dataRow = table.NewRow();
			for (int i = 0; i < jArray2.Count; i++)
			{
				dataRow[i] = jArray2[i];
			}
			table.Rows.Add(dataRow);
		}
	}

	private static bool CheckArrayForSimpleCreateTable(JArray jArray)
	{
		foreach (JToken item in jArray)
		{
			if (!(item is JArray))
			{
				return false;
			}
		}
		return true;
	}

	private static bool TryParseArrayToTable(JArray array, out DataTable table, out Hashtable hashChildTables)
	{
		table = new DataTable();
		hashChildTables = new Hashtable();
		if (array.Count > 0 && array[0] is JValue)
		{
			table.Columns.Add(new DataColumn("Value"));
			for (int i = 0; i < array.Count; i++)
			{
				if (array[i] is JValue jValue)
				{
					DataRow dataRow = table.NewRow();
					dataRow[0] = jValue.Value;
					table.Rows.Add(dataRow);
					continue;
				}
				return false;
			}
		}
		else
		{
			for (int j = 0; j < array.Count; j++)
			{
				if (array[j] is JObject jObject)
				{
					DataRow dataRow2 = table.NewRow();
					for (int k = 0; k < jObject.Children().Count(); k++)
					{
						if (jObject.GetItem(k) is JProperty jProperty)
						{
							if (!table.Columns.Contains(jProperty.Name))
							{
								table.Columns.Add(new DataColumn(jProperty.Name));
							}
							int columnIndex = table.Columns.IndexOf(jProperty.Name);
							if (jProperty.Value is JArray jArray && ValidJArrayStruct(jArray))
							{
								dataRow2[columnIndex] = j;
								FillRelationIdInJArray("relationId", j, jArray);
								if (hashChildTables.ContainsKey(jProperty.Name))
								{
									JArray jArray2 = hashChildTables[jProperty.Name] as JArray;
									hashChildTables[jProperty.Name] = UnitJArray(jArray2, jArray);
								}
								else
								{
									hashChildTables.Add(jProperty.Name, jArray);
								}
							}
							else
							{
								dataRow2[columnIndex] = jProperty.Value;
							}
							continue;
						}
						return false;
					}
					table.Rows.Add(dataRow2);
					continue;
				}
				return false;
			}
		}
		return true;
	}

	private static JArray UnitJArray(JArray jArray1, JArray jArray2)
	{
		for (int i = 0; i < jArray2.Count; i++)
		{
			if (jArray2[i] is JObject item)
			{
				jArray1.Add(item);
			}
		}
		return jArray1;
	}

	private static void FillRelationIdInJArray(string relationName, int relationId, JArray jArray)
	{
		for (int i = 0; i < jArray.Count; i++)
		{
			if (jArray[i] is JObject jObject)
			{
				jObject.Add(new JProperty(relationName, relationId));
			}
		}
	}

	private static bool ValidJArrayStruct(JArray jArray)
	{
		for (int i = 0; i < jArray.Count; i++)
		{
			if (!(jArray[i] is JObject jObject))
			{
				continue;
			}
			for (int j = 0; j < jObject.Children().Count(); j++)
			{
				if (!(jObject.GetItem(j) is JProperty))
				{
					return false;
				}
			}
		}
		return true;
	}

	private static void ParseArrayToTable(JArray array, DataTable table)
	{
		if (array.Count > 0 && array[0] is JValue)
		{
			table.Columns.Add(new DataColumn("Value"));
			for (int i = 0; i < array.Count; i++)
			{
				if (array[i] is JValue jValue)
				{
					DataRow dataRow = table.NewRow();
					dataRow[0] = jValue.Value;
					table.Rows.Add(dataRow);
				}
			}
			return;
		}
		foreach (JToken item in array)
		{
			if (!(item is JObject jObject))
			{
				continue;
			}
			DataRow dataRow2 = table.NewRow();
			foreach (JToken item2 in jObject.Children())
			{
				if (item2 is JProperty jProperty)
				{
					if (!table.Columns.Contains(jProperty.Name))
					{
						table.Columns.Add(new DataColumn(jProperty.Name));
					}
					int columnIndex = table.Columns.IndexOf(jProperty.Name);
					DateTime? dateTime = ParseJTokenToDateTime(jProperty.Value);
					dataRow2[columnIndex] = (dateTime.HasValue ? ((JToken)dateTime.GetValueOrDefault()) : jProperty.Value);
				}
			}
			table.Rows.Add(dataRow2);
		}
	}

	private static void ParseJProperty(DataSet dataSet, JProperty jProperty, DataTable tableProperties, DataRow rowProperties = null)
	{
		if (jProperty.Value is JValue)
		{
			if (tableProperties == null)
			{
				tableProperties = ((mainDataTable != null) ? mainDataTable : new DataTable
				{
					TableName = jProperty.Name
				});
				if (mainDataTable == null)
				{
					dataSet.Tables.Add(tableProperties);
				}
				if (StiBaseOptions.JsonParseCreatingTableProperties)
				{
					if (!tableProperties.Columns.Contains("Name"))
					{
						tableProperties.Columns.Add("Name");
					}
					if (!tableProperties.Columns.Contains("Value"))
					{
						tableProperties.Columns.Add("Value");
					}
				}
			}
			if (StiBaseOptions.JsonParseCreatingTableProperties)
			{
				DataRow dataRow = tableProperties.NewRow();
				dataRow[0] = jProperty.Name;
				dataRow[1] = jProperty.Value;
				tableProperties.Rows.Add(dataRow);
			}
			else
			{
				tableProperties.Columns.Add(new DataColumn(jProperty.Name));
				int columnIndex = tableProperties.Columns.IndexOf(jProperty.Name);
				DateTime? dateTime = ParseJTokenToDateTime(jProperty.Value);
				rowProperties[columnIndex] = (dateTime.HasValue ? ((JToken)dateTime.GetValueOrDefault()) : jProperty.Value);
			}
		}
		else
		{
			ParseJToken(dataSet, jProperty.Name, jProperty.Value, tableProperties);
		}
	}

	private static void ParseJObject(DataSet dataSet, string tokenName, JObject jObject)
	{
		DataTable dataTable = ((mainDataTable != null) ? mainDataTable : new DataTable
		{
			TableName = GetTableName(dataSet, tokenName)
		});
		DataRow row = null;
		if (StiBaseOptions.JsonParseCreatingTableProperties)
		{
			if (!dataTable.Columns.Contains("Name"))
			{
				dataTable.Columns.Add("Name");
			}
			if (!dataTable.Columns.Contains("Value"))
			{
				dataTable.Columns.Add("Value");
			}
		}
		else
		{
			row = dataTable.NewRow();
		}
		if (mainDataTable == null)
		{
			dataSet.Tables.Add(dataTable);
		}
		for (int i = 0; i < jObject.Count; i++)
		{
			ParseJToken(dataSet, null, jObject.Children().ToArray()[i], dataTable, row);
		}
		if (!StiBaseOptions.JsonParseCreatingTableProperties)
		{
			dataTable.Rows.Add(row);
		}
	}

	private static DateTime? ParseJTokenToDateTime(JToken token)
	{
		string text = token.ToString();
		if (text.StartsWith("{\r\n  \"$date\": ") && text.EndsWith("\r\n}"))
		{
			string text2 = text.Replace("{\r\n  \"$date\": ", "");
			text2 = text2.Replace("\r\n}", "");
			if (double.TryParse(text2, out var result))
			{
				return DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc).AddMilliseconds(result);
			}
		}
		return null;
	}

	private static string GetTableName(DataSet dataSet, string tableName)
	{
		if (dataSet.Tables.Contains(tableName))
		{
			if (hashDublicateTableName == null)
			{
				hashDublicateTableName = new Hashtable();
			}
			if (hashDublicateTableName.ContainsKey(tableName))
			{
				int num = (int)hashDublicateTableName[tableName] + 1;
				hashDublicateTableName[tableName] = num;
			}
			else
			{
				hashDublicateTableName.Add(tableName, 1);
			}
			return $"{tableName}{hashDublicateTableName[tableName].ToString()}";
		}
		return tableName;
	}
}
