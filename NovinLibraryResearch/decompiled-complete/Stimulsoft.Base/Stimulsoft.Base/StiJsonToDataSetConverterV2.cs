using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base;

public static class StiJsonToDataSetConverterV2
{
	private class StiJsonMetaData
	{
		public string CollectionName { get; set; }

		public string Address { get; set; }

		public string Cast { get; set; }

		public object Object { get; set; }
	}

	private class StiJsonRelationData
	{
		public DataColumn ChildColumn { get; set; }

		public DataTable ChildTable { get; set; }

		public string ParentTableName { get; set; }
	}

	private class StiJsonRelationIdData
	{
		public object NewValue { get; set; }

		public string RelationId { get; set; }
	}

	private static Hashtable objectCorrection;

	public static DataSet GetDataSet(JToken json)
	{
		return GetDataSet(json, StiRelationDirection.ParentToChild);
	}

	public static DataSet GetDataSet(JToken json, StiRelationDirection relationDirection)
	{
		objectCorrection = new Hashtable();
		json = CorrectJson(json);
		List<StiJsonMetaData> list = new List<StiJsonMetaData>();
		DataSet dataSet = new DataSet();
		FillCollection(list, "", "", "", json, first: true);
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		Hashtable hashtable3 = new Hashtable();
		Hashtable hashtable4 = new Hashtable();
		List<DataColumn> list2 = new List<DataColumn>();
		foreach (StiJsonMetaData item in list)
		{
			if (item == null)
			{
				continue;
			}
			List<StiJsonMetaData> list3 = hashtable2[item.Cast] as List<StiJsonMetaData>;
			if (list3 == null)
			{
				list3 = new List<StiJsonMetaData>();
			}
			hashtable4[list3] = 0;
			hashtable[item.Address] = list3;
			list3.Add(item);
			hashtable2[item.Cast] = list3;
			if (((StiJsonRelationIdData)((JObject)item.Object).Tag).RelationId != "-1" && ((StiJsonRelationIdData)((JObject)item.Object).Tag).RelationId != null && ((StiJsonRelationIdData)((JObject)item.Object).Tag).RelationId.IndexOf("#relation#") == 0)
			{
				string key = ((StiJsonRelationIdData)((JObject)item.Object).Tag).RelationId.Replace("#relation#", "");
				if (hashtable2.ContainsKey(key))
				{
					((StiJsonRelationIdData)((JObject)item.Object).Tag).RelationId = ((List<StiJsonMetaData>)hashtable2[key]).Count.ToString();
				}
				else
				{
					((StiJsonRelationIdData)((JObject)item.Object).Tag).RelationId = "0";
				}
			}
		}
		foreach (object key3 in hashtable2.Keys)
		{
			List<StiJsonMetaData> list4 = hashtable2[key3] as List<StiJsonMetaData>;
			int num = 0;
			foreach (StiJsonMetaData item2 in list4)
			{
				string collectionName = item2.CollectionName;
				DataTable dataTable = dataSet.Tables[collectionName];
				if (dataTable == null)
				{
					dataTable = new DataTable(collectionName);
					dataSet.Tables.Add(dataTable);
				}
				DataRow dataRow = dataTable.NewRow();
				num++;
				foreach (JToken item3 in (IEnumerable<JToken>)(JContainer)item2.Object)
				{
					try
					{
						if (!(item3 is JProperty jProperty))
						{
							continue;
						}
						Type typeFromHandle = typeof(string);
						DataColumn dataColumn = dataTable.Columns[jProperty.Name];
						if (dataColumn == null)
						{
							if (jProperty.Name != "relationId")
							{
								if (jProperty.Value.Type == JTokenType.Integer)
								{
									typeFromHandle = typeof(decimal);
								}
								else if (jProperty.Value.Type == JTokenType.Float)
								{
									typeFromHandle = typeof(decimal);
								}
								else if (jProperty.Value.Type == JTokenType.Boolean)
								{
									typeFromHandle = typeof(bool);
								}
								else if (jProperty.Value.Type == JTokenType.Date)
								{
									typeFromHandle = typeof(DateTime);
								}
							}
							dataColumn = new DataColumn(jProperty.Name, typeFromHandle);
							dataTable.Columns.Add(dataColumn);
						}
						object obj = ((jProperty.Tag is StiJsonRelationIdData { NewValue: not null } stiJsonRelationIdData) ? stiJsonRelationIdData.NewValue : ((JValue)jProperty.Value).Value);
						StiJsonRelationIdData stiJsonRelationIdData2 = ((JContainer)item2.Object).Tag as StiJsonRelationIdData;
						if (jProperty.Name == "relationId" && stiJsonRelationIdData2 != null && stiJsonRelationIdData2.RelationId != null)
						{
							DataColumn dataColumn2 = dataTable.Columns["relationId"];
							if (dataColumn2 == null)
							{
								dataColumn2 = new DataColumn("relationId");
								dataTable.Columns.Add(dataColumn2);
								list2.Add(dataColumn2);
							}
							if (stiJsonRelationIdData2.RelationId == "-1")
							{
								stiJsonRelationIdData2.RelationId = "0";
								List<StiJsonMetaData> list5 = hashtable[item2.Address] as List<StiJsonMetaData>;
								for (int i = (int)hashtable4[list5]; i < list5.Count; i++)
								{
									StiJsonMetaData stiJsonMetaData = list5[i];
									if (stiJsonMetaData.Address == item2.Address)
									{
										stiJsonRelationIdData2.RelationId = i.ToString();
										hashtable4[list5] = i;
										break;
									}
								}
							}
							dataRow[dataColumn2] = stiJsonRelationIdData2.RelationId;
							continue;
						}
						if (typeFromHandle == typeof(string) && obj != null && obj.ToString().IndexOf("#relation#") == 0)
						{
							dataRow[dataColumn] = (num - 1).ToString();
							string key2 = obj.ToString().Replace("#relation#", "");
							if (hashtable.ContainsKey(key2))
							{
								List<StiJsonMetaData> list6 = hashtable[key2] as List<StiJsonMetaData>;
								if (list6.Count > 0)
								{
									StiJsonRelationData value = new StiJsonRelationData
									{
										ChildColumn = dataColumn,
										ChildTable = dataTable,
										ParentTableName = list6[0].CollectionName
									};
									hashtable3[dataTable.TableName + "." + dataColumn.ColumnName] = value;
								}
							}
							continue;
						}
						if (obj == null)
						{
							dataRow[dataColumn] = DBNull.Value;
							continue;
						}
						if (dataColumn.DataType == typeof(object))
						{
							dataRow[dataColumn] = obj;
							continue;
						}
						try
						{
							if (obj.GetType() == typeof(DateTime) && dataColumn.DataType == typeof(string))
							{
								dataRow[dataColumn] = ((DateTime)obj).ToString("o", CultureInfo.InvariantCulture);
							}
							else
							{
								dataRow[dataColumn] = StiConvert.ChangeType(obj, dataColumn.DataType);
							}
						}
						catch
						{
							dataRow[dataColumn] = DBNull.Value;
						}
					}
					catch
					{
					}
				}
				dataTable.Rows.Add(dataRow);
			}
		}
		foreach (StiJsonRelationData value2 in hashtable3.Values)
		{
			DataColumn childColumn = value2.ChildColumn;
			DataTable dataTable2 = dataSet.Tables[value2.ParentTableName];
			DataColumn dataColumn3 = dataTable2.Columns["relationId"];
			if (dataColumn3 == null || childColumn == null)
			{
				continue;
			}
			try
			{
				if (relationDirection == StiRelationDirection.ChildToParent && objectCorrection.ContainsKey(value2.ParentTableName))
				{
					dataSet.Relations.Add(dataTable2.TableName, dataColumn3, childColumn);
				}
				else
				{
					dataSet.Relations.Add(dataTable2.TableName, childColumn, dataColumn3);
				}
			}
			catch
			{
			}
			for (int j = 0; j < list2.Count; j++)
			{
				if (list2[j] == dataColumn3)
				{
					list2.RemoveAt(j);
					break;
				}
			}
		}
		foreach (DataColumn item4 in list2)
		{
			try
			{
				if (item4.Table.Columns.Contains(item4.ColumnName) && item4.Table.Columns.CanRemove(item4))
				{
					item4.Table.Columns.Remove(item4);
				}
			}
			catch
			{
			}
		}
		return dataSet;
	}

	public static DataSet GetDataSet(byte[] content)
	{
		return GetDataSet(content, StiRelationDirection.ParentToChild);
	}

	public static DataSet GetDataSet(byte[] content, StiRelationDirection relationDirection)
	{
		string text = StiBytesToStringConverter.ConvertBytesToString(content);
		return GetDataSet(text, relationDirection);
	}

	public static DataSet GetDataSet(string text)
	{
		return GetDataSet(text, StiRelationDirection.ParentToChild);
	}

	public static DataSet GetDataSet(string text, StiRelationDirection relationDirection)
	{
		if (text == null)
		{
			return null;
		}
		JsonSerializerSettings settings = null;
		if (!StiBaseOptions.TryParseDateTime)
		{
			settings = new JsonSerializerSettings
			{
				DateParseHandling = DateParseHandling.None
			};
		}
		JToken json = JsonConvert.DeserializeObject(text, settings) as JToken;
		return GetDataSet(json, relationDirection);
	}

	public static DataSet GetDataSet(XElement element)
	{
		return GetDataSet(element, StiRelationDirection.ParentToChild);
	}

	public static DataSet GetDataSet(XElement element, StiRelationDirection relationDirection)
	{
		string value = JsonConvert.SerializeXNode((XObject)(object)element, Formatting.Indented);
		JsonSerializerSettings settings = null;
		if (!StiBaseOptions.TryParseDateTime)
		{
			settings = new JsonSerializerSettings
			{
				DateParseHandling = DateParseHandling.None
			};
		}
		JToken json = JsonConvert.DeserializeObject(value, settings) as JToken;
		return GetDataSet(json, relationDirection);
	}

	public static DataSet GetDataSetFromXml(byte[] array)
	{
		return GetDataSetFromXml(array, StiRelationDirection.ParentToChild);
	}

	public static DataSet GetDataSetFromXml(byte[] array, StiRelationDirection relationDirection)
	{
		if (array != null)
		{
			string value;
			using (MemoryStream memoryStream = new MemoryStream(array))
			{
				XElement node = XElement.Load((Stream)memoryStream);
				value = JsonConvert.SerializeXNode((XObject)(object)node, Formatting.Indented);
			}
			JsonSerializerSettings settings = null;
			if (!StiBaseOptions.TryParseDateTime)
			{
				settings = new JsonSerializerSettings
				{
					DateParseHandling = DateParseHandling.None
				};
			}
			JToken json = JsonConvert.DeserializeObject(value, settings) as JToken;
			return GetDataSet(json, relationDirection);
		}
		return null;
	}

	public static DataSet GetDataSetFromFile(string path)
	{
		return GetDataSetFromFile(path, StiRelationDirection.ParentToChild);
	}

	public static DataSet GetDataSetFromFile(string path, StiRelationDirection relationDirection)
	{
		if (File.Exists(path))
		{
			string value = File.ReadAllText(path);
			JsonSerializerSettings settings = null;
			if (!StiBaseOptions.TryParseDateTime)
			{
				settings = new JsonSerializerSettings
				{
					DateParseHandling = DateParseHandling.None
				};
			}
			JToken json = JsonConvert.DeserializeObject(value, settings) as JToken;
			return GetDataSet(json, relationDirection);
		}
		return null;
	}

	private static void CorrectArray(JToken jToken)
	{
		JContainer jContainer = jToken as JContainer;
		if (jToken is JArray jArray)
		{
			for (int i = 0; i < jArray.Count; i++)
			{
				JToken jToken2 = jArray[i];
				if (jToken2 is JValue)
				{
					object value = ((JValue)jToken2).Value;
					jArray[i] = new JObject(new JProperty("value", value));
				}
				else
				{
					CorrectArray(jToken2);
				}
			}
		}
		else
		{
			if (jContainer == null)
			{
				return;
			}
			for (int j = 0; j < jContainer.Count; j++)
			{
				JToken item = jContainer.GetItem(j);
				JProperty jProperty = item as JProperty;
				if (jProperty != null && jProperty.Value.HasValues)
				{
					if (jProperty.Value.Type != JTokenType.Array)
					{
						string key = new Regex("\\[\\d*\\]").Replace(jProperty.Path, "").Replace('.', '_');
						objectCorrection[key] = jProperty.Path;
						JArray jArray2 = new JArray();
						jArray2.Add(jProperty.Value);
						jProperty = new JProperty(jProperty.Name, jArray2);
						jContainer.RemoveItemAt(j);
						jContainer.InsertItem(j, jProperty, skipParentCheck: true);
					}
					CorrectArray(jProperty.Value);
				}
			}
		}
	}

	private static JToken CorrectJson(JToken jToken)
	{
		CorrectArray(jToken);
		JContainer jContainer = jToken as JContainer;
		if (jContainer is JArray)
		{
			JProperty jProperty = new JProperty("root");
			jProperty.Value = jToken;
			return jProperty;
		}
		foreach (JToken item in (IEnumerable<JToken>)jContainer)
		{
			JArray jArray = item as JArray;
			JObject jObject = item as JObject;
			if (item is JProperty jProperty2 && !(jProperty2.Value is JContainer))
			{
				JProperty jProperty3 = new JProperty("root");
				jProperty3.Value = jToken;
				return jProperty3;
			}
		}
		return jToken;
	}

	private static string CorrectJsonString(string json)
	{
		for (int i = 0; i < json.Length; i++)
		{
			if (json[i] == '{')
			{
				return json;
			}
			if (json[i] == '[')
			{
				return "{\"root\": " + json + "}";
			}
		}
		return json;
	}

	private static void FillCollection(List<StiJsonMetaData> collections, string collectionName, string parentName, string address, object json, bool first = false)
	{
		string text = parentName + "_" + collectionName;
		if (string.IsNullOrEmpty(parentName))
		{
			text = collectionName;
		}
		JContainer jContainer = json as JContainer;
		JProperty jProperty = json as JProperty;
		int num = 0;
		foreach (JToken item in (IEnumerable<JToken>)jContainer)
		{
			JArray jArray = item as JArray;
			JObject jObject = item as JObject;
			JProperty jProperty2 = item as JProperty;
			string text2 = ((jProperty2 != null) ? jProperty2.Name : num.ToString());
			if (jProperty != null)
			{
				text2 = jProperty.Name;
			}
			JToken jToken = ((jProperty2 != null) ? jProperty2.Value : item);
			JArray jArray2 = jToken as JArray;
			if (jArray != null || jObject != null || (jProperty2 != null && (jProperty2.Value.Type == JTokenType.Array || jProperty2.Value.Type == JTokenType.Object)))
			{
				bool flag = false;
				if (jContainer is JArray)
				{
					FillCollection(collections, collectionName, parentName, address + ".#array#" + num, item);
				}
				else
				{
					if (!first && jArray2 != null)
					{
						foreach (JToken item2 in jArray2)
						{
							if (item2 is JObject)
							{
								item2.Tag = SetRelationId(item2.Tag, "#relation#" + text);
								((JObject)item2).Add("relationId", "#relation#" + text);
							}
						}
					}
					FillCollection(collections, text2, text, address + "." + text2, jToken);
					if (jArray2 == null)
					{
						jToken.Tag = SetRelationId(jToken.Tag, "-1");
					}
					else
					{
						item.Tag = SetNewValue(item.Tag, "#relation#" + address + "." + text2 + ".#array#0");
						flag = true;
					}
				}
				if (!flag && jArray2 == null)
				{
					item.Tag = SetNewValue(item.Tag, "#relation#" + address + "." + text2);
				}
			}
			num++;
		}
		if (!(json is JArray) && !string.IsNullOrEmpty(address))
		{
			if (!string.IsNullOrEmpty(parentName))
			{
				collectionName = parentName + "_" + collectionName;
			}
			collections.Add(new StiJsonMetaData
			{
				CollectionName = collectionName,
				Address = address,
				Cast = text,
				Object = json
			});
		}
	}

	private static StiJsonRelationIdData SetRelationId(object tag, string relationId)
	{
		StiJsonRelationIdData stiJsonRelationIdData = ((StiJsonRelationIdData)tag) ?? new StiJsonRelationIdData();
		stiJsonRelationIdData.RelationId = relationId;
		return stiJsonRelationIdData;
	}

	private static StiJsonRelationIdData SetNewValue(object tag, object value)
	{
		StiJsonRelationIdData stiJsonRelationIdData = ((StiJsonRelationIdData)tag) ?? new StiJsonRelationIdData();
		stiJsonRelationIdData.NewValue = value;
		return stiJsonRelationIdData;
	}
}
