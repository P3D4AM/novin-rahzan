using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base;

public static class StiGisToDataSetConverter
{
	public static DataSet GetDataSetFromWkt(byte[] content, string separator)
	{
		string text = StiBytesToStringConverter.ConvertBytesToString(content);
		return GetDataSetFromWkt(text, separator);
	}

	public static DataSet GetDataSetFromWkt(string text, string separator)
	{
		if (text == null)
		{
			return null;
		}
		string[] array = text.Split(Environment.NewLine.ToCharArray());
		if (array.Length == 0)
		{
			return new DataSet();
		}
		DataTable dataTable = new DataTable();
		char[] separator2 = separator.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i];
			if (i == 0)
			{
				string[] array2 = text2.Split(separator2);
				string[] array3 = array2;
				foreach (string columnName in array3)
				{
					DataColumn column = new DataColumn(columnName, typeof(string));
					dataTable.Columns.Add(column);
				}
			}
			else if (!string.IsNullOrEmpty(text2))
			{
				string[] array4 = text2.Split(separator2);
				int num = 0;
				DataRow dataRow = dataTable.NewRow();
				string[] array5 = array4;
				foreach (string value in array5)
				{
					dataRow[num] = value;
					num++;
				}
				dataTable.Rows.Add(dataRow);
			}
		}
		DataSet dataSet = new DataSet();
		dataSet.Tables.Add(dataTable);
		return dataSet;
	}

	public static DataSet GetDataSetFromGeoJson(byte[] content)
	{
		string value = StiBytesToStringConverter.ConvertBytesToString(content);
		JObject jObject = JsonConvert.DeserializeObject(value) as JObject;
		return GetDataSetFromGeoJson(jObject);
	}

	public static DataSet GetDataSetFromGeoJson(JObject jObject)
	{
		DataSet dataSet = new DataSet();
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add(new DataColumn("geometry", typeof(string)));
		List<JToken> source = jObject.Children().ToList();
		JToken jToken = source.FirstOrDefault((JToken x) => x is JProperty && ((JProperty)x).Name == "type");
		JToken jToken2 = source.FirstOrDefault((JToken x) => x is JProperty && ((JProperty)x).Name == "features");
		if (jToken == null || jToken2 == null)
		{
			throw new Exception("Data not correct");
		}
		JToken[] array = jToken2.Children().ToArray();
		foreach (JToken item in array[0].Children())
		{
			DataRow dataRow = dataTable.NewRow();
			bool flag = false;
			JToken[] array2 = item.Children().ToArray();
			for (int i = 0; i < array2.Length; i++)
			{
				JProperty jProperty = array2[i] as JProperty;
				if (jProperty.Name == "type")
				{
					if (jProperty.HasValues && jProperty.Value.ToString() != "Feature")
					{
						flag = true;
						break;
					}
				}
				else if (jProperty.Name == "geometry")
				{
					if (jProperty.HasValues)
					{
						string value = jProperty.Value.ToString().Replace(Environment.NewLine, "").Replace(" ", "")
							.Trim();
						dataRow["geometry"] = value;
					}
				}
				else
				{
					if (!(jProperty.Name == "properties"))
					{
						continue;
					}
					for (int j = 0; j < jProperty.ChildrenTokens.Count; j++)
					{
						if (!(jProperty.ChildrenTokens[j] is JObject jObject2))
						{
							continue;
						}
						for (int k = 0; k < jObject2.ChildrenTokens.Count; k++)
						{
							if (jObject2.ChildrenTokens[k] is JProperty { HasValues: not false } jProperty2)
							{
								if (!dataTable.Columns.Contains(jProperty2.Name))
								{
									dataTable.Columns.Add(new DataColumn(jProperty2.Name, typeof(string)));
								}
								dataRow[jProperty2.Name] = jProperty2.Value.ToString();
							}
						}
					}
				}
			}
			if (!flag)
			{
				dataTable.Rows.Add(dataRow);
			}
		}
		dataSet.Tables.Add(dataTable);
		return dataSet;
	}

	private static bool IsCorrectJToken(JToken token, string name)
	{
		if (!(token is JProperty jProperty))
		{
			return false;
		}
		return jProperty.Name == name;
	}
}
