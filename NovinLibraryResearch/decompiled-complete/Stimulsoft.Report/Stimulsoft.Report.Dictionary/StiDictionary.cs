using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.SaveLoad;

namespace Stimulsoft.Report.Dictionary;

[Serializable]
public class StiDictionary : ICloneable, IStiStateSaveRestore, IStiJsonReportObject, IStiAppDictionary
{
	[Browsable(false)]
	public string[] ReconnectListForRequestFromUserVariables { get; set; }

	[Browsable(false)]
	public static Hashtable CacheUserNamesAndPasswords { get; set; }

	[Browsable(false)]
	public bool UseInternalData { get; set; }

	[Browsable(false)]
	public StiRestrictions Restrictions { get; set; } = new StiRestrictions();


	public static StiAutoSynchronizeMode AutoSynchronize
	{
		get
		{
			return StiOptions.Dictionary.AutoSynchronize;
		}
		set
		{
			StiOptions.Dictionary.AutoSynchronize = value;
		}
	}

	[Browsable(false)]
	public DataSet CacheDataSet { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public StiReport Report { get; set; }

	[Browsable(false)]
	public StiDataCollection DataStore { get; set; } = new StiDataCollection();


	[TypeConverter(typeof(ExpandableObjectConverter))]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiVariablesCollection Variables { get; set; } = new StiVariablesCollection();


	[TypeConverter(typeof(ExpandableObjectConverter))]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiResourcesCollection Resources { get; set; } = new StiResourcesCollection();


	[TypeConverter(typeof(ExpandableObjectConverter))]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiDataSourcesCollection DataSources { get; set; }

	[TypeConverter(typeof(ExpandableObjectConverter))]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiDatabaseCollection Databases { get; set; }

	[TypeConverter(typeof(ExpandableObjectConverter))]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiBusinessObjectsCollection BusinessObjects { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public virtual StiDataRelationsCollection Relations { get; set; }

	[Browsable(false)]
	public bool IsRequestFromUserVariablesPresent
	{
		get
		{
			if (Variables == null)
			{
				return false;
			}
			foreach (StiVariable variable in Variables)
			{
				if (variable.RequestFromUser)
				{
					return true;
				}
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyJObject(val, "Resources", Resources.SaveToJsonObjectEx(mode));
		JsonExtensions.AddPropertyJObject(val, "Variables", Variables.SaveToJsonObjectEx(mode));
		JsonExtensions.AddPropertyJObject(val, "DataSources", DataSources.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Databases", Databases.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "BusinessObjects", BusinessObjects.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Relations", Relations.SaveToJsonObject(mode));
		if (((JContainer)val).Count == 0)
		{
			return null;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Resources":
				Resources.LoadFromJsonObjectEx((JObject)item.Value, Report);
				break;
			case "Variables":
				Variables.LoadFromJsonObjectEx((JObject)item.Value, Report);
				break;
			case "DataSources":
				DataSources.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Databases":
				Databases.LoadFromJsonObject((JObject)item.Value);
				break;
			case "BusinessObjects":
				BusinessObjects.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Relations":
				Relations.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public virtual void SaveState(string stateName)
	{
		foreach (StiDataSource dataSource in DataSources)
		{
			dataSource.SaveState(stateName);
		}
	}

	public virtual void RestoreState(string stateName)
	{
		foreach (StiDataSource dataSource in DataSources)
		{
			dataSource.RestoreState(stateName);
		}
	}

	public virtual void ClearAllStates()
	{
		foreach (StiDataSource dataSource in DataSources)
		{
			dataSource.ClearAllStates();
		}
	}

	public object Clone()
	{
		StiDictionary stiDictionary = new StiDictionary();
		using MemoryStream memoryStream = new MemoryStream();
		Save(memoryStream);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		stiDictionary.Load(memoryStream);
		stiDictionary.DataStore = new StiDataCollection();
		return stiDictionary;
	}

	IEnumerable<IStiAppDataSource> IStiAppDictionary.FetchDataSources()
	{
		return DataSources.Cast<IStiAppDataSource>();
	}

	IEnumerable<IStiAppDataRelation> IStiAppDictionary.FetchDataRelations()
	{
		return Relations.Cast<IStiAppDataRelation>();
	}

	IEnumerable<IStiAppVariable> IStiAppDictionary.FetchVariables()
	{
		return Variables.Cast<IStiAppVariable>();
	}

	IStiAppDataSource IStiAppDictionary.GetDataSourceByName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return null;
		}
		return (IStiAppDataSource)(object)DataSources[name];
	}

	IStiAppDataColumn IStiAppDictionary.GetColumnByName(string name)
	{
		return (IStiAppDataColumn)(object)StiDataColumn.GetDataColumnFromColumnName(this, name, allowRelationName: true);
	}

	IStiAppVariable IStiAppDictionary.GetVariableByName(string name)
	{
		name = StiVariableExpressionHelper.GetSimpleName(name);
		if (string.IsNullOrWhiteSpace(name))
		{
			return null;
		}
		StiVariable stiVariable = (from StiVariable v in Variables
			where !string.IsNullOrWhiteSpace(v.Name)
			select v).FirstOrDefault((StiVariable v) => v.Name.Trim().ToLowerInvariant() == name);
		if (stiVariable != null)
		{
			return (IStiAppVariable)(object)stiVariable;
		}
		if (name.EndsWith(".from"))
		{
			name = name.Substring(0, name.Length - ".from".Length);
			return (IStiAppVariable)(object)(from StiVariable v in Variables
				where !string.IsNullOrWhiteSpace(v.Name)
				select v).FirstOrDefault((StiVariable v) => v.Name.Trim().ToLowerInvariant() == name);
		}
		if (name.EndsWith(".to"))
		{
			name = name.Substring(0, name.Length - ".to".Length);
			return (IStiAppVariable)(object)(from StiVariable v in Variables
				where !string.IsNullOrWhiteSpace(v.Name)
				select v).FirstOrDefault((StiVariable v) => v.Name.Trim().ToLowerInvariant() == name);
		}
		return null;
	}

	object IStiAppDictionary.GetVariableValueByName(string name)
	{
		name = StiVariableExpressionHelper.GetSimpleName(name);
		if (string.IsNullOrWhiteSpace(name))
		{
			return null;
		}
		if (((IStiAppDictionary)this).GetVariableByName(name) is StiVariable stiVariable && Range.IsRangeType(stiVariable.Type))
		{
			if (name.EndsWith(".from"))
			{
				name = name.Substring(0, name.Length - ".from".Length);
				if (!(GetVariableValueInternal(name) is Range range))
				{
					return null;
				}
				return range.FromObject;
			}
			if (name.EndsWith(".to"))
			{
				name = name.Substring(0, name.Length - ".to".Length);
				if (!(GetVariableValueInternal(name) is Range range2))
				{
					return null;
				}
				return range2.ToObject;
			}
		}
		return GetVariableValueInternal(name);
	}

	private object GetVariableValueInternal(string name)
	{
		StiReport report = Report;
		if (((report != null && report.CalculationMode == StiCalculationMode.Interpretation) || StiOptions.Engine.ForceInterpretationMode) && ((IStiAppDictionary)this).GetVariableByName(name) is StiVariable { ReadOnly: not false, InitBy: StiVariableInitBy.Expression } stiVariable)
		{
			return StiReportParser.Parse("{" + stiVariable.Value + "}", (IStiReport)(object)Report, allowCache: false);
		}
		if (Report?.CompiledReport != null && Report?.CompiledReport[name] != null)
		{
			return Report?.CompiledReport[name];
		}
		if (Report != null && Report[name] != null)
		{
			return Report[name];
		}
		IStiAppVariable variableByName = ((IStiAppDictionary)this).GetVariableByName(name);
		if (variableByName == null)
		{
			return null;
		}
		return variableByName.GetValue();
	}

	public bool IsSystemVariable(string name)
	{
		return new string[8] { "reportname", "reportalias", "reportauthor", "reportdescription", "reportcreated", "reportchanged", "time", "today" }.Contains(name.ToLowerInvariant());
	}

	public bool IsReadOnlyVariable(string name)
	{
		if (((IStiAppDictionary)this).GetVariableByName(name) is StiVariable stiVariable)
		{
			return stiVariable.ReadOnly;
		}
		return false;
	}

	public object GetSystemVariableValue(string name)
	{
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		return name.ToLowerInvariant() switch
		{
			"reportname" => Report?.ReportName, 
			"reportalias" => Report?.ReportAlias, 
			"reportauthor" => Report?.ReportAuthor, 
			"reportdescription" => Report?.ReportDescription, 
			"reportcreated" => Report?.ReportCreated, 
			"reportchanged" => Report?.ReportChanged, 
			"time" => DateTime.Now, 
			"today" => DateTime.Today, 
			_ => throw new StiSystemVariableNotRecognizedException(name), 
		};
	}

	IStiApp IStiAppDictionary.GetApp()
	{
		return (IStiApp)(object)Report;
	}

	IEnumerable<IStiAppConnection> IStiAppDictionary.OpenConnections(IEnumerable<IStiAppConnection> connections)
	{
		if (connections == null)
		{
			connections = (IEnumerable<IStiAppConnection>)Databases.ToList();
		}
		connections = connections.Where((IStiAppConnection c) => !StiDataConnections.IsConnectionActive(c)).ToList();
		foreach (StiDatabase connection in connections)
		{
			List<StiData> second = DataStore.ToList();
			connection?.InvokeConnecting();
			StiDataLeader.RegData(connection, this, loadData: true);
			connection?.InvokeConnected();
			List<object> list = DataStore.ToList().Except(second).Cast<object>()
				.ToList();
			StiDataConnections.RegisterConnection((IStiAppConnection)(object)connection, list);
		}
		return connections;
	}

	void IStiAppDictionary.CloseConnections(IEnumerable<IStiAppConnection> connections)
	{
		if (connections == null)
		{
			connections = (IEnumerable<IStiAppConnection>)Databases.ToList();
		}
		foreach (StiDatabase connection in connections)
		{
			connection?.InvokeDisconnecting();
			List<StiData> list = StiDataConnections.UnRegisterConnection((IStiAppConnection)(object)connection)?.Cast<StiData>()?.ToList();
			if (list == null)
			{
				continue;
			}
			DisconnectingConnectionInDataStore(list);
			list.ForEach(delegate(StiData d)
			{
				if (DataStore.Contains(d))
				{
					DataStore.Remove(d);
				}
			});
		}
	}

	public void SortItems()
	{
		SortItems(StiSortOrder.Asc);
	}

	public void SortItems(StiSortOrder order)
	{
		DataSources.Sort(order);
		Variables.Sort(order);
		BusinessObjects.Sort(order);
	}

	public void Load(StiDictionarySLService service, Stream stream)
	{
		service.Load(this, stream);
	}

	public void Load(Stream stream)
	{
		StiXmlDictionarySLService service = new StiXmlDictionarySLService();
		Load(service, stream);
	}

	public void Load(string path)
	{
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		Load(fileStream);
		fileStream.Close();
		fileStream.Dispose();
	}

	public void Load(StiDictionarySLService service, string path)
	{
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		service.Load(this, fileStream);
		fileStream.Close();
		fileStream.Dispose();
	}

	public void Merge(StiDictionarySLService service, Stream stream)
	{
		service.Merge(this, stream);
	}

	public void Merge(Stream stream)
	{
		StiXmlDictionarySLService service = new StiXmlDictionarySLService();
		Merge(service, stream);
	}

	public void Merge(string path)
	{
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		Merge(fileStream);
		fileStream.Close();
		fileStream.Dispose();
	}

	public void Merge(StiDictionarySLService service, string path)
	{
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		service.Merge(this, fileStream);
		fileStream.Close();
		fileStream.Dispose();
	}

	public void Merge(StiDictionary dictionary)
	{
		foreach (StiDataSource dataSource in dictionary.DataSources)
		{
			StiDataSource stiDataSource2 = DataSources[dataSource.Name];
			if (stiDataSource2 == null)
			{
				stiDataSource2 = (StiDataSource)dataSource.Clone();
				stiDataSource2.Name = StiNameCreation.CreateName(Report, stiDataSource2.Name, addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
				DataSources.Add(stiDataSource2);
				continue;
			}
			foreach (StiDataColumn column in dataSource.Columns)
			{
				StiDataColumn stiDataColumn2 = stiDataSource2.Columns[column.Name];
				if (stiDataColumn2 == null)
				{
					stiDataSource2.Columns.Add((StiDataColumn)column.Clone());
				}
			}
		}
		foreach (StiDataRelation relation in dictionary.Relations)
		{
			StiDataRelation stiDataRelation2 = Relations[relation.NameInSource];
			if (stiDataRelation2 == null || !(stiDataRelation2.Name == relation.Name))
			{
				stiDataRelation2 = (StiDataRelation)relation.Clone();
				stiDataRelation2.ParentSource = DataSources[stiDataRelation2.ParentSource.Name];
				stiDataRelation2.ChildSource = DataSources[stiDataRelation2.ChildSource.Name];
				stiDataRelation2.Name = GetRelationName(stiDataRelation2);
				Relations.Add(stiDataRelation2);
			}
		}
		Variables.AddRange(dictionary.Variables);
		Connect(loadData: false);
	}

	public void Save(StiDictionarySLService service, Stream stream)
	{
		service.Save(this, stream);
	}

	public void Save(Stream stream)
	{
		StiXmlDictionarySLService service = new StiXmlDictionarySLService();
		Save(service, stream);
	}

	public void Save(string path)
	{
		StiFileUtils.ProcessReadOnly(path);
		FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		Save(fileStream);
		fileStream.Close();
		fileStream.Dispose();
	}

	public void Save(StiDictionarySLService service, string path)
	{
		StiFileUtils.ProcessReadOnly(path);
		FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		service.Save(this, fileStream);
		fileStream.Close();
		fileStream.Dispose();
	}

	public void ImportXMLSchema(DataSet dataSet)
	{
		try
		{
			DataSources.Clear();
			Relations.Clear();
			StiDataTableSetNameService stiDataTableSetNameService = StiOptions.Services.DataTableSetName.FirstOrDefault();
			Hashtable hashtable = new Hashtable();
			foreach (DataTable table in dataSet.Tables)
			{
				StiDataTableSource stiDataTableSource = new StiDataTableSource();
				stiDataTableSetNameService?.SetName(stiDataTableSource, Report, dataSet, table.TableName);
				stiDataTableSource.Name = StiNameCreation.CreateName(Report, stiDataTableSource.Name, addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
				hashtable[table.TableName] = stiDataTableSource.Name;
				DataSources.Add(stiDataTableSource);
				foreach (DataColumn column in table.Columns)
				{
					stiDataTableSource.Columns.Add(new StiDataColumn(column.ColumnName, column.Caption, column.DataType));
				}
			}
			foreach (DataRelation relation in dataSet.Relations)
			{
				List<string> list = new List<string>();
				DataColumn[] parentColumns = relation.ParentColumns;
				foreach (DataColumn dataColumn2 in parentColumns)
				{
					list.Add(dataColumn2.ColumnName);
				}
				List<string> list2 = new List<string>();
				DataColumn[] childColumns = relation.ChildColumns;
				foreach (DataColumn dataColumn3 in childColumns)
				{
					list2.Add(dataColumn3.ColumnName);
				}
				StiDataRelation stiDataRelation = new StiDataRelation(relation.RelationName, DataSources[hashtable[relation.ParentTable.TableName] as string], DataSources[hashtable[relation.ChildTable.TableName] as string], list.ToArray(), list2.ToArray());
				StiDataRelationSetName.SetName(stiDataRelation, Report, dataSet, relation.RelationName);
				stiDataRelation.Name = GetRelationName(stiDataRelation);
				Relations.Add(stiDataRelation);
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "ImportXMLSchema...ERROR");
			StiLogService.Write(GetType(), e);
		}
		Connect(loadData: false);
	}

	public void MergeXMLSchema(DataSet dataSet)
	{
		StiDictionary stiDictionary = new StiDictionary(Report);
		stiDictionary.ImportXMLSchema(dataSet);
		Merge(stiDictionary);
	}

	public DataSet ExportXMLSchema(string dataSetName)
	{
		DataSet dataSet = new DataSet(dataSetName)
		{
			EnforceConstraints = false,
			Locale = CultureInfo.InvariantCulture
		};
		try
		{
			foreach (StiDataSource dataSource in DataSources)
			{
				DataTable dataTable = new DataTable(dataSource.Name);
				dataSet.Tables.Add(dataTable);
				foreach (StiDataColumn column in dataSource.Columns)
				{
					if (column.Type.IsGenericType && column.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
					{
						dataTable.Columns.Add(new DataColumn(column.Name, Nullable.GetUnderlyingType(column.Type))
						{
							AllowDBNull = true,
							Caption = column.Alias
						});
					}
					else
					{
						dataTable.Columns.Add(new DataColumn(column.Name, column.Type)
						{
							Caption = column.Alias
						});
					}
				}
			}
			foreach (StiDataRelation relation in Relations)
			{
				List<DataColumn> list = new List<DataColumn>();
				string[] parentColumns = relation.ParentColumns;
				foreach (string name in parentColumns)
				{
					list.Add(dataSet.Tables[relation.ParentSource.Name].Columns[name]);
				}
				List<DataColumn> list2 = new List<DataColumn>();
				string[] childColumns = relation.ChildColumns;
				foreach (string name2 in childColumns)
				{
					list2.Add(dataSet.Tables[relation.ChildSource.Name].Columns[name2]);
				}
				dataSet.Relations.Add(new DataRelation(relation.NameInSource, list.ToArray(), list2.ToArray(), createConstraints: false));
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "ExportXMLSchema...ERROR");
			StiLogService.Write(GetType(), e);
		}
		return dataSet;
	}

	public void RemoveUnusedData()
	{
		StiDataRetrieval stiDataRetrieval = new StiDataRetrieval();
		stiDataRetrieval.Retrieval(Report);
		int num = 0;
		while (num < Relations.Count)
		{
			if (stiDataRetrieval.UsedRelations[Relations[num]] == null)
			{
				Relations.Remove(Relations[num]);
			}
			else
			{
				num++;
			}
		}
		foreach (StiDataSource dataSource in DataSources)
		{
			num = 0;
			while (num < dataSource.Columns.Count)
			{
				if (stiDataRetrieval.UsedColumns[dataSource.Columns[num]] == null)
				{
					dataSource.Columns.Remove(dataSource.Columns[num]);
				}
				else
				{
					num++;
				}
			}
		}
		num = 0;
		while (num < DataSources.Count)
		{
			if (stiDataRetrieval.UsedDataSources[DataSources[num]] == null)
			{
				DataSources.Remove(DataSources[num]);
			}
			else
			{
				num++;
			}
		}
		stiDataRetrieval.Dispose();
	}

	public void RemoveUnusedDataSourcesV2()
	{
		Hashtable usedDataSourcesNames = StiDataSourceHelper.GetUsedDataSourcesNames(Report);
		int num = 0;
		while (num < DataSources.Count)
		{
			if (!usedDataSourcesNames.ContainsKey(DataSources[num].Name))
			{
				DataSources.Remove(DataSources[num]);
			}
			else
			{
				num++;
			}
		}
	}

	public void RetrievalData(out Hashtable usedRelations, out Hashtable usedDataSources, out Hashtable usedColumns)
	{
		StiDataRetrieval stiDataRetrieval = new StiDataRetrieval();
		stiDataRetrieval.Retrieval(Report);
		usedRelations = stiDataRetrieval.UsedRelations;
		usedDataSources = stiDataRetrieval.UsedDataSources;
		usedColumns = stiDataRetrieval.UsedColumns;
	}

	public StiDataRelationsCollection GetUnusedRelationsFromDataStore()
	{
		Hashtable categories = new Hashtable();
		return GetUnusedRelationsFromDataStore(ref categories);
	}

	public StiDataRelationsCollection GetUnusedRelationsFromDataStore(string tableName)
	{
		Hashtable categories = new Hashtable();
		return GetUnusedRelationsFromDataStore(tableName, ref categories);
	}

	public StiDataRelationsCollection GetUnusedRelationsFromDataStore(ref Hashtable categories)
	{
		StiDataRelationsCollection stiDataRelationsCollection = new StiDataRelationsCollection(this);
		List<StiDataTableSource> list = (from tableSource in DataSources.Cast<StiDataSource>().OfType<StiDataTableSource>()
			where tableSource.DataTable != null
			select tableSource).ToList();
		foreach (StiDataTableSource item in list)
		{
			DataRelationCollection parentRelations = item.DataTable.ParentRelations;
			foreach (DataRelation item2 in parentRelations)
			{
				foreach (StiDataTableSource item3 in list)
				{
					if (item3.DataTable != item2.ParentTable)
					{
						continue;
					}
					int num = 0;
					foreach (StiDataRelation relation in Relations)
					{
						if (StiStrFix.Del_(relation.NameInSource) == StiStrFix.Del_(item2.RelationName))
						{
							break;
						}
						num++;
					}
					if (num == Relations.Count)
					{
						StiDataRelation stiDataRelation2 = new StiDataRelation(item2.RelationName, item3, item, item2.ParentColumns.Select((DataColumn column) => column.ColumnName).ToArray(), item2.ChildColumns.Select((DataColumn column) => column.ColumnName).ToArray());
						StiDataRelationSetName.SetName(stiDataRelation2, Report, item2.DataSet, item2.RelationName);
						stiDataRelationsCollection.Add(stiDataRelation2);
						string categoryName = item.GetCategoryName();
						categories[stiDataRelation2] = categoryName;
						break;
					}
				}
			}
		}
		return stiDataRelationsCollection;
	}

	public StiDataRelationsCollection GetUnusedRelationsFromDataStore(string tableName, ref Hashtable categories)
	{
		StiDataRelationsCollection stiDataRelationsCollection = new StiDataRelationsCollection(this);
		List<StiDataTableSource> list = (from tableSource in DataSources.Cast<StiDataSource>().OfType<StiDataTableSource>()
			where tableSource.DataTable != null && tableSource.NameInSource != null && StringExt.StartsWithInvariant(tableSource.NameInSource, tableName + ".")
			select tableSource).ToList();
		foreach (StiDataTableSource item in list)
		{
			DataRelationCollection parentRelations = item.DataTable.ParentRelations;
			foreach (DataRelation item2 in parentRelations)
			{
				foreach (StiDataTableSource item3 in list)
				{
					if (item3.DataTable != item2.ParentTable)
					{
						continue;
					}
					int num = 0;
					foreach (StiDataRelation relation in Relations)
					{
						if (StiStrFix.Del_(relation.NameInSource) == StiStrFix.Del_(item2.RelationName))
						{
							break;
						}
						num++;
					}
					if (num == Relations.Count)
					{
						StiDataRelation stiDataRelation2 = new StiDataRelation(item2.RelationName, item3, item, item2.ParentColumns.Select((DataColumn column) => column.ColumnName).ToArray(), item2.ChildColumns.Select((DataColumn column) => column.ColumnName).ToArray());
						StiDataRelationSetName.SetName(stiDataRelation2, Report, item2.DataSet, item2.RelationName);
						stiDataRelationsCollection.Add(stiDataRelation2);
						string categoryName = item.GetCategoryName();
						categories[stiDataRelation2] = categoryName;
						break;
					}
				}
			}
		}
		return stiDataRelationsCollection;
	}

	public static async Task DoAutoSynchronizeAsync(StiReport report)
	{
		await Task.Run(delegate
		{
			DoAutoSynchronize(report);
		});
	}

	public static void DoAutoSynchronize(StiReport report)
	{
		if (report != null && (AutoSynchronize == StiAutoSynchronizeMode.Always || (AutoSynchronize == StiAutoSynchronizeMode.IfDictionaryEmpty && report.Dictionary.DataSources.Count == 0)))
		{
			report.Dictionary.Synchronize();
		}
	}

	private void Synchronize(StiBusinessObjectsCollection businessObjects)
	{
		foreach (StiBusinessObject businessObject in businessObjects)
		{
			object businessObjectData = businessObject.GetBusinessObjectData();
			if (businessObjectData != null)
			{
				SynchronizeColumns(businessObjectData, businessObject);
			}
			Synchronize(businessObject.BusinessObjects);
		}
	}

	public async Task SynchronizeAsync()
	{
		await Task.Run(delegate
		{
			Synchronize();
		});
	}

	public void Synchronize()
	{
		lock (this)
		{
			foreach (StiDataSource dataSource in DataSources)
			{
				dataSource.Columns.CachedDataColumns.Clear();
			}
			DataSources.CachedDataSources.Clear();
			foreach (StiData data in from d in DataStore.ToList()
				where !(d.ViewData is IDbConnection)
				select d)
			{
				StiDataStoreSource stiDataStoreSource = (from d in DataSources.ToList()
					where d is StiDataStoreSource && !(d is StiSqlSource) && !(d is StiVirtualSource) && !(d is StiDataTransformation)
					select d).Cast<StiDataStoreSource>().FirstOrDefault((StiDataStoreSource d) => d.NameInSource.ToLowerInvariant() == data.Name.ToLowerInvariant());
				if (stiDataStoreSource == null)
				{
					StiDataAdapterService dataAdapter = StiDataAdapterService.GetDataAdapter(data);
					if (dataAdapter != null)
					{
						StiDataSource stiDataSource2 = dataAdapter.Create(this, addToDictionary: false);
						StiDataColumnsCollection columnsFromData = StiDataLeader.GetColumnsFromData(dataAdapter, data, stiDataSource2);
						stiDataSource2.Columns.AddRange(columnsFromData);
						dataAdapter.SetDataSourceNames(data, stiDataSource2);
						stiDataSource2.Name = StiNameCreation.CreateName(Report, stiDataSource2.Name, addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
						DataSources.Add(stiDataSource2);
						SynchronizeColumns(data, stiDataSource2);
					}
				}
				else
				{
					SynchronizeColumns(data, stiDataStoreSource);
				}
			}
			foreach (StiData item in DataStore)
			{
				bool flag = false;
				foreach (StiDataSource dataSource2 in DataSources)
				{
					if (dataSource2 is StiDataStoreSource stiDataStoreSource2 && stiDataStoreSource2.NameInSource.ToLower(CultureInfo.InvariantCulture) == item.Name.ToLower(CultureInfo.InvariantCulture))
					{
						flag = true;
						break;
					}
				}
				if (!flag && !(item.Data is IDbConnection))
				{
					StiBusinessObjectAdapterService stiBusinessObjectAdapterService = new StiBusinessObjectAdapterService();
					StiDataSource stiDataSource4 = stiBusinessObjectAdapterService.Create(this, addToDictionary: false);
					StiDataColumnsCollection columnsFromData2 = StiDataLeader.GetColumnsFromData(stiBusinessObjectAdapterService, item, stiDataSource4);
					stiDataSource4.Columns.AddRange(columnsFromData2);
					stiBusinessObjectAdapterService.SetDataSourceNames(item, stiDataSource4);
					stiDataSource4.Name = StiNameCreation.CreateName(Report, stiDataSource4.Name, addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
					DataSources.Add(stiDataSource4);
				}
			}
			SynchronizeBusinessObjects();
			Connect(loadData: false);
			StiDataRelationsCollection unusedRelationsFromDataStore = GetUnusedRelationsFromDataStore();
			foreach (StiDataRelation rel in unusedRelationsFromDataStore)
			{
				if (!Relations.Cast<StiDataRelation>().Any((StiDataRelation relation) => StiStrFix.Del_(rel.NameInSource) == StiStrFix.Del_(relation.NameInSource)))
				{
					rel.Name = GetRelationName(rel);
					Relations.Add(rel);
				}
			}
			Connect(loadData: false);
			StiOptions.Engine.GlobalEvents.InvokeReportSynchronized(this, new EventArgs());
		}
	}

	public void SynchronizeRelations()
	{
		Connect(loadData: false);
		StiDataRelationsCollection unusedRelationsFromDataStore = GetUnusedRelationsFromDataStore();
		foreach (StiDataRelation rel in unusedRelationsFromDataStore)
		{
			if (!Relations.Cast<StiDataRelation>().Any((StiDataRelation relation) => StiStrFix.Del_(rel.NameInSource) == StiStrFix.Del_(relation.NameInSource)))
			{
				rel.Name = GetRelationName(rel);
				Relations.Add(rel);
			}
		}
		Connect(loadData: false);
	}

	public async Task SynchronizeRelationsAsync(string tableName)
	{
		await Task.Run(delegate
		{
			SynchronizeRelations(tableName);
		});
	}

	public void SynchronizeRelations(string tableName)
	{
		Connect(loadData: false);
		StiDataRelationsCollection unusedRelationsFromDataStore = GetUnusedRelationsFromDataStore(tableName);
		foreach (StiDataRelation rel in unusedRelationsFromDataStore)
		{
			if (!Relations.Cast<StiDataRelation>().Any((StiDataRelation relation) => StiStrFix.Del_(rel.NameInSource) == StiStrFix.Del_(relation.NameInSource)))
			{
				rel.Name = GetRelationName(rel);
				Relations.Add(rel);
			}
		}
		Connect(loadData: false);
	}

	public void SynchronizeBusinessObjects()
	{
		foreach (StiBusinessObjectData item in Report.BusinessObjectsStore)
		{
			bool flag = false;
			foreach (StiBusinessObject businessObject in BusinessObjects)
			{
				if (businessObject.Name == item.Name)
				{
					flag = true;
					businessObject.Alias = item.Alias;
					businessObject.Category = item.Category;
					SynchronizeColumns(item, businessObject);
					Synchronize(businessObject.BusinessObjects);
					break;
				}
			}
			if (!flag)
			{
				StiBusinessObject source = new StiBusinessObject(item.Category, item.Name, item.Alias, null);
				BusinessObjects.Add(source);
				SynchronizeColumns(item, source);
			}
		}
	}

	public void SynchronizeBusinessObjects(int maxLevel)
	{
		SynchronizeBusinessObjects();
		SynchronizeBusinessObjects(BusinessObjects, 0, maxLevel);
	}

	private void SynchronizeBusinessObjects(StiBusinessObjectsCollection objects, int curLevel, int maxLevel)
	{
		foreach (StiBusinessObject @object in objects)
		{
			object businessObjectData = @object.GetBusinessObjectData();
			if (businessObjectData != null)
			{
				SynchronizeColumns(businessObjectData, @object);
			}
			StiDataColumnsCollection stiDataColumnsCollection = ((businessObjectData != null) ? StiBusinessObjectHelper.GetColumnsFromData(businessObjectData, includeChildDataSources: true) : null);
			if (stiDataColumnsCollection != null)
			{
				if (StiOptions.Dictionary.BusinessObjects.ColumnsSynchronizationMode == StiColumnsSynchronizationMode.RemoveAbsentColumns)
				{
					Hashtable hashtable = new Hashtable();
					foreach (StiDataColumn item in stiDataColumnsCollection)
					{
						if (!StiBusinessObjectHelper.IsDataColumn(item.Type))
						{
							hashtable[item.Name] = null;
						}
					}
					int num = 0;
					while (num < @object.BusinessObjects.Count)
					{
						StiBusinessObject stiBusinessObject2 = @object.BusinessObjects[num];
						if (!hashtable.ContainsKey(stiBusinessObject2.Name))
						{
							@object.BusinessObjects.RemoveAt(num);
						}
						else
						{
							num++;
						}
					}
					hashtable.Clear();
				}
				foreach (StiDataColumn item2 in stiDataColumnsCollection)
				{
					if (StiBusinessObjectHelper.IsDataColumn(item2.Type))
					{
						continue;
					}
					bool flag = false;
					foreach (StiBusinessObject businessObject in @object.BusinessObjects)
					{
						if (businessObject.Name == item2.Name)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						@object.BusinessObjects.Add(new StiBusinessObject
						{
							Name = item2.Name,
							Alias = item2.Alias
						});
					}
				}
			}
			if (curLevel < maxLevel)
			{
				SynchronizeBusinessObjects(@object.BusinessObjects, curLevel + 1, maxLevel);
			}
		}
	}

	public void SynchronizeColumns(StiData data, StiDataSource dataSource)
	{
		StiDataAdapterService stiDataAdapterService = ((data == null && dataSource is StiDBaseSource) ? new StiDBaseAdapterService() : ((data != null || !(dataSource is StiCsvSource)) ? StiDataAdapterService.GetDataAdapter(data) : new StiCsvAdapterService()));
		if (stiDataAdapterService == null)
		{
			return;
		}
		StiDataColumnsCollection columnsFromData = StiDataLeader.GetColumnsFromData(stiDataAdapterService, data, dataSource);
		foreach (StiDataColumn column in dataSource.Columns)
		{
			bool flag = false;
			foreach (StiDataColumn item in columnsFromData)
			{
				if (column.NameInSource == item.NameInSource)
				{
					item.Name = column.Name;
					if (StiOptions.Designer.AutoCorrectDataColumnName)
					{
						item.Name = StiNameValidator.CorrectName(item.Name, Report);
					}
					item.Alias = column.Alias;
					item.NameInSource = column.NameInSource;
					item.Key = column.Key;
					if (StiOptions.Dictionary.ColumnTypeSynchronizationMode == StiColumnTypeSynchronizationMode.KeepAsIs)
					{
						item.Type = column.Type;
					}
					else if ((column.Type == typeof(DateTime) || column.Type == typeof(DateTime?)) && StiOptions.Dictionary.UseNullableDateTime)
					{
						item.Type = typeof(DateTime?);
					}
					else if ((column.Type == typeof(TimeSpan) || column.Type == typeof(TimeSpan?)) && StiOptions.Dictionary.UseNullableTimeSpan)
					{
						item.Type = typeof(TimeSpan?);
					}
					flag = true;
				}
			}
			if ((!flag && StiOptions.Dictionary.ColumnsSynchronizationMode == StiColumnsSynchronizationMode.KeepAbsentColumns) || column is StiCalcDataColumn)
			{
				columnsFromData.Add(column);
			}
		}
		Hashtable hashtable = new Hashtable();
		for (int num = columnsFromData.Count - 1; num >= 0; num--)
		{
			StiDataColumn stiDataColumn3 = columnsFromData[num];
			int num2 = 1;
			string text = null;
			while (true)
			{
				text = ((num2 == 1) ? stiDataColumn3.Name : $"{stiDataColumn3.Name}{num2}");
				if (hashtable[text] == null)
				{
					break;
				}
				num2++;
			}
			hashtable[text] = text;
			stiDataColumn3.Name = text;
		}
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		foreach (StiDataColumn column2 in dataSource.Columns)
		{
			foreach (StiDataColumn item2 in columnsFromData)
			{
				if (column2.NameInSource == item2.NameInSource)
				{
					stiDataColumnsCollection.Add(item2);
					columnsFromData.Remove(item2);
					break;
				}
			}
		}
		stiDataColumnsCollection.AddRange(columnsFromData);
		dataSource.Columns.Clear();
		dataSource.Columns.AddRange(stiDataColumnsCollection);
	}

	public void SynchronizeColumns(StiBusinessObjectData data, StiBusinessObject source)
	{
		SynchronizeColumns(data.BusinessObjectValue, source);
	}

	public void SynchronizeColumns(object data, StiBusinessObject source)
	{
		StiDataColumnsCollection columnsFromData = StiBusinessObjectHelper.GetColumnsFromData(data);
		foreach (StiDataColumn column in source.Columns)
		{
			bool flag = false;
			foreach (StiDataColumn item in columnsFromData)
			{
				if (column.NameInSource == item.NameInSource)
				{
					item.Name = column.Name;
					if (StiOptions.Designer.AutoCorrectDataColumnName)
					{
						item.Name = StiNameValidator.CorrectName(item.Name, Report);
					}
					item.Alias = column.Alias;
					item.NameInSource = column.NameInSource;
					item.Key = column.Key;
					if (StiOptions.Dictionary.ColumnTypeSynchronizationMode == StiColumnTypeSynchronizationMode.KeepAsIs)
					{
						item.Type = column.Type;
					}
					else if ((column.Type == typeof(DateTime) || column.Type == typeof(DateTime?)) && StiOptions.Dictionary.UseNullableDateTime)
					{
						item.Type = typeof(DateTime?);
					}
					else if (column.Type == typeof(TimeSpan) || (column.Type == typeof(TimeSpan?) && StiOptions.Dictionary.UseNullableTimeSpan))
					{
						item.Type = typeof(TimeSpan?);
					}
					flag = true;
					break;
				}
			}
			if (!flag && StiOptions.Dictionary.BusinessObjects.ColumnsSynchronizationMode == StiColumnsSynchronizationMode.KeepAbsentColumns)
			{
				columnsFromData.Add(column);
			}
		}
		Hashtable hashtable = new Hashtable();
		for (int num = columnsFromData.Count - 1; num >= 0; num--)
		{
			StiDataColumn stiDataColumn3 = columnsFromData[num];
			int num2 = 1;
			string text;
			while (true)
			{
				text = ((num2 == 1) ? stiDataColumn3.Name : $"{stiDataColumn3.Name}{num2}");
				if (hashtable[text] == null)
				{
					break;
				}
				num2++;
			}
			hashtable[text] = text;
			stiDataColumn3.Name = text;
		}
		source.Columns.Clear();
		source.Columns.AddRange(columnsFromData);
	}

	public void Clear()
	{
		BusinessObjects.Clear();
		Variables.Clear();
		Databases.Clear();
		DataSources.Clear();
		Relations.Clear();
		DataStore.ClearReportDatabase();
		Resources.Clear();
		Restrictions.Clear();
	}

	private void DisposeCacheDataSet()
	{
		if (CacheDataSet == null)
		{
			return;
		}
		foreach (DataTable table in CacheDataSet.Tables)
		{
			for (int num = table.ChildRelations.Count - 1; num >= 0; num--)
			{
				if (table.ChildRelations[num].ChildTable.Constraints.Contains(table.ChildRelations[num].RelationName))
				{
					table.ChildRelations[num].ChildTable.Constraints.Remove(table.ChildRelations[num].RelationName);
				}
				table.ChildRelations.RemoveAt(num);
			}
			table.ChildRelations.Clear();
			table.ParentRelations.Clear();
			table.Constraints.Clear();
		}
		CacheDataSet.Relations.Clear();
		CacheDataSet.Tables.Clear();
		CacheDataSet.Dispose();
		CacheDataSet = null;
	}

	public void RenameDatabase(StiDatabase database, string newName)
	{
		if (database.Name == newName)
		{
			return;
		}
		string text = database.Name + ".";
		string newValue = newName + ".";
		for (int i = 0; i < DataSources.Count; i++)
		{
			if (DataSources[i] is StiDataTableSource stiDataTableSource)
			{
				if (stiDataTableSource.NameInSource.StartsWith(text))
				{
					stiDataTableSource.NameInSource = stiDataTableSource.NameInSource.Replace(text, newValue);
				}
				else if (stiDataTableSource.NameInSource == database.Name)
				{
					stiDataTableSource.NameInSource = newName;
				}
			}
		}
		database.Name = newName;
	}

	public DataSet GetDataSet(string dataSetName)
	{
		DataSet dataSet = new DataSet(dataSetName);
		Connect();
		foreach (StiDataSource dataSource in DataSources)
		{
			if (dataSet.Tables[dataSource.Name] == null)
			{
				DataTable dataTable = dataSource.GetDataTable();
				dataSet.Tables.Add(dataTable);
			}
		}
		return dataSet;
	}

	public IDbConnection GetConnection(string name)
	{
		foreach (StiData item in DataStore)
		{
			if (item.Data is IDbConnection result && item.Name == name)
			{
				return result;
			}
		}
		return null;
	}

	public void ConnectToDatabases(bool loadData = true, bool throwException = true)
	{
		ConnectToDatabases(Databases.ToList(), loadData, throwException);
	}

	internal void ConnectToDatabases(List<StiDatabase> databases, bool loadData = true, bool throwException = true)
	{
		DataStore.ClearReportDatabase();
		foreach (StiDatabase database in databases)
		{
			try
			{
				database.InvokeConnecting();
				StiDataLeader.RegData(database, this, loadData);
				database.InvokeConnected();
			}
			catch (Exception ex)
			{
				if (throwException)
				{
					throw ex;
				}
			}
		}
	}

	internal void TryConnect(bool loadData = true, List<StiDataSource> dataSources = null)
	{
		try
		{
			Connect(loadData, dataSources);
		}
		catch
		{
		}
	}

	internal void Connect(List<StiDatabase> databases)
	{
		ConnectToDatabases(databases, loadData: true, throwException: false);
	}

	public async Task ConnectAsync(bool loadData = true, List<StiDataSource> dataSources = null)
	{
		await Task.Run(delegate
		{
			Connect(loadData, dataSources);
		});
	}

	public void Connect(bool loadData = true, List<StiDataSource> dataSources = null)
	{
		try
		{
			DisposeCacheDataSet();
			CacheDataSet = new DataSet("CacheDataSet");
			ConnectToDatabases(loadData);
			if (dataSources != null)
			{
				dataSources.ForEach(delegate(StiDataSource d)
				{
					StiDataLeader.Connect(d, loadData);
				});
			}
			else if (Report.RetrieveOnlyUsedData && loadData)
			{
				Hashtable dataSourceNames = StiDataSourceHelper.GetUsedDataSourcesNames(Report);
				(from d in DataSources.ToList()
					where dataSourceNames.ContainsKey(d.Name)
					select d).ToList().ForEach(delegate(StiDataSource d)
				{
					StiDataLeader.Connect(d);
				});
			}
			else
			{
				DataSources.Connect(loadData);
			}
			BusinessObjects.Connect();
			if (loadData)
			{
				RegRelations();
			}
		}
		catch
		{
			if (loadData)
			{
				throw;
			}
		}
	}

	public void ConnectVirtualDataSources()
	{
		Hashtable hashtable = new Hashtable();
		bool flag = true;
		while (flag)
		{
			flag = false;
			foreach (StiVirtualSource item in DataSources.FetchAllVirtualDataSources())
			{
				StiDataSource stiDataSource = Report.Dictionary.DataSources[item.NameInSource];
				if (stiDataSource == null || stiDataSource.Name == item.Name)
				{
					hashtable[item] = item;
					continue;
				}
				if (stiDataSource is StiVirtualSource && hashtable[stiDataSource] == null)
				{
					flag = true;
					continue;
				}
				item.ConnectToData();
				hashtable[item] = item;
			}
		}
	}

	public void ConnectDataTransformations()
	{
		DataSources.FetchAllDataTransformations().ForEach(delegate(StiDataTransformation d)
		{
			d.ConnectToData();
		});
	}

	public void ConnectCrossTabDataSources()
	{
		DataSources.FetchAllCrossTabDataSources().ForEach(ConnectCrossTabDataSources);
	}

	public void ConnectCrossTabDataSources(StiCrossTabDataSource dataSource)
	{
		if (Report.GetComponents().ToList().FirstOrDefault((StiComponent c) => c is StiCrossTab && c.Name == dataSource.NameInSource) is StiCrossTab stiCrossTab)
		{
			DataTable crossDataTable = StiCrossTabHelper.CreateCrossForCrossTabDataSource(stiCrossTab);
			dataSource.ConnectToData(crossDataTable, stiCrossTab);
		}
	}

	internal void TryDisconnect()
	{
		try
		{
			Disconnect();
		}
		catch
		{
		}
	}

	public void Disconnect()
	{
		DisconnectingDatabases(Databases.ToList());
		DisconnectingConnectionInDataStore();
		DisposeCacheDataSet();
		BusinessObjects.Disconnect();
		DataSources.Disconnect();
		DataStore.ClearReportDatabase();
		DisconnectedDatabases(Databases.ToList());
		StiDataConnections.UnRegisterConnections(Databases.Cast<IStiAppConnection>().ToList());
	}

	internal void DisconnectingDatabases(List<StiDatabase> databases)
	{
		databases.ForEach(delegate(StiDatabase d)
		{
			d.InvokeDisconnecting();
		});
	}

	internal void DisconnectedDatabases(List<StiDatabase> databases)
	{
		databases.ForEach(delegate(StiDatabase d)
		{
			d.InvokeDisconnected();
		});
	}

	internal void DisconnectingConnectionInDataStore(List<StiData> datas = null)
	{
		if (datas == null)
		{
			datas = (from d in DataStore.ToList()
				where d.Data is IDbConnection
				select d).ToList();
		}
		foreach (StiData data in datas)
		{
			IDbConnection dbConnection = data?.Data as IDbConnection;
			if (data.OriginalConnectionState is ConnectionState)
			{
				if ((ConnectionState)data.OriginalConnectionState != ConnectionState.Open)
				{
					dbConnection?.Close();
				}
			}
			else if (dbConnection != null && dbConnection.State != 0)
			{
				dbConnection?.Close();
			}
		}
	}

	private bool CheckRelation(DataSet dataSet, string relationName, DataColumn[] parentColumns, DataColumn[] childColumns)
	{
		DataRelation dataRelation = dataSet.Relations[relationName] ?? dataSet.Relations[relationName.Trim()];
		if (dataRelation != null)
		{
			if (EqualsRelationColumns(parentColumns, dataRelation.ParentColumns) && EqualsRelationColumns(childColumns, dataRelation.ChildColumns))
			{
				return true;
			}
			if (dataRelation.ChildTable.Constraints.Contains(dataRelation.RelationName))
			{
				dataRelation.ChildTable.Constraints.Remove(dataRelation.RelationName);
			}
			dataSet.Relations.Remove(dataRelation);
		}
		int num = 0;
		while (num < dataSet.Relations.Count)
		{
			dataRelation = dataSet.Relations[num];
			if (EqualsRelationColumns(dataRelation.ParentColumns, parentColumns) && EqualsRelationColumns(dataRelation.ChildColumns, childColumns))
			{
				if (dataRelation.ChildTable.Constraints.Contains(dataRelation.RelationName))
				{
					dataRelation.ChildTable.Constraints.Remove(dataRelation.RelationName);
				}
				dataSet.Relations.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
		return false;
	}

	private bool EqualsRelationColumns(DataColumn[] columns, DataColumn[] cols)
	{
		return columns.All((DataColumn column) => cols.Any((DataColumn col) => col.ColumnName == column.ColumnName && col.Table == column.Table));
	}

	private string GetRelationName(StiDataRelation relation)
	{
		string relationName;
		string arg = (relationName = relation.Name);
		if (relation.ChildSource.Columns.Cast<StiDataColumn>().Any((StiDataColumn column) => column.Name == relationName))
		{
			arg = (relationName = "Parent" + relationName);
		}
		int num = 2;
		IEnumerable<StiDataRelation> source = Relations.Cast<StiDataRelation>();
		while (!source.Where((StiDataRelation r) => r.ChildSource == relation.ChildSource && r != relation).All((StiDataRelation r) => r.Name != relationName))
		{
			relationName = $"{arg}_{num++}";
		}
		IEnumerable<StiDataSource> source2 = DataSources.Cast<StiDataSource>();
		while (!source2.All((StiDataSource dataSource) => "Parent" + dataSource.Name != relationName))
		{
			relationName = $"{arg}_{num++}";
		}
		return relationName;
	}

	public void RegRelations(bool virtualSources = false)
	{
		Relations.ToList().ForEach(delegate(StiDataRelation r)
		{
			RegRelation(r, virtualSources);
		});
	}

	public void RegRelation(StiDataRelation relation, bool virtualSources = false)
	{
		try
		{
			StiDataSource parentSource = relation.ParentSource;
			StiDataSource childSource = relation.ChildSource;
			bool flag = parentSource is StiVirtualSource || childSource is StiVirtualSource;
			if ((!virtualSources && flag) || (virtualSources && !flag))
			{
				return;
			}
			if (!Report.IsDesigning && parentSource == null)
			{
				throw new Exception("Please, check the relation '" + relation.Name + "'. The report engine can't find the parent Data Source!");
			}
			if (childSource == null)
			{
				throw new Exception("Please, check the relation '" + relation.Name + "'. The report engine can't find the child Data Source!");
			}
			if (parentSource.DataTable == null || childSource.DataTable == null)
			{
				return;
			}
			if (!Report.IsDesigning && parentSource.DataTable.DataSet == null)
			{
				throw new Exception("Parent Data Source '" + parentSource.Name + "' is not located in a DataSet and can't be used in the relation '" + relation.Name + "'! You can use the CacheAllData property of a report to cache this Data Source to one DataSet.");
			}
			if (childSource.DataTable.DataSet == null)
			{
				throw new Exception("The child Data Source '" + childSource.Name + "' is not located in a DataSet and can't be used in the relation '" + relation.Name + "'!You can use the CacheAllData property of a report to cache this Data Source to one DataSet.");
			}
			if (!Report.IsDesigning && parentSource.DataTable.DataSet != childSource.DataTable.DataSet)
			{
				throw new Exception("The parent Data Source '" + parentSource.Name + "' and the Child Data Source '" + childSource.Name + "' is not located in one DataSet and can't be used in the relation '" + relation.Name + "'! You can use the CacheAllData property of a report to cache this Data Source to one DataSet.");
			}
			if (!Report.IsDesigning && relation.ParentColumns.Length == 0)
			{
				throw new Exception("Parent columns is specified in the relation '{relation.Name}'! This relation can't be created!");
			}
			if (relation.ChildColumns.Length == 0)
			{
				throw new Exception("Child columns is specified in the relation '{relation.Name}'! This relation can't be created!");
			}
			if (!Report.IsDesigning && relation.ParentColumns.Length != relation.ChildColumns.Length)
			{
				throw new Exception("The count of parent columns is not equal to the count of child columns in the relation '{relation.Name}'! This relation can't be created!");
			}
			DataColumn[] array = new DataColumn[relation.ParentColumns.Length];
			int num = 0;
			string[] parentColumns = relation.ParentColumns;
			foreach (string text in parentColumns)
			{
				if (parentSource.DataTable.Columns[text] == null)
				{
					throw new Exception("The column '" + text + "' does not exist in the Data Source '" + parentSource.Name + "' and the relation '" + relation.Name + "' can't be created!");
				}
				array[num] = parentSource.DataTable.Columns[text];
				num++;
			}
			DataColumn[] array2 = new DataColumn[relation.ChildColumns.Length];
			num = 0;
			string[] childColumns = relation.ChildColumns;
			foreach (string text2 in childColumns)
			{
				if (childSource.DataTable.Columns[text2] == null)
				{
					throw new Exception("The column '" + text2 + "' does not exist in the Data Source '" + childSource.Name + "' and the relation '" + relation.Name + "' can't be created!");
				}
				array2[num] = childSource.DataTable.Columns[text2];
				num++;
			}
			DataSet dataSet = parentSource.DataTable.DataSet;
			if (array.Length != 0 && array2.Length != 0 && !CheckRelation(dataSet, relation.NameInSource, array, array2))
			{
				dataSet.EnforceConstraints = false;
				dataSet.Relations.Add(relation.NameInSource, array, array2);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(GetType(), "Creating relation '" + relation.NameInSource + "'...ERROR");
			StiLogService.Write(GetType(), ex);
			if (!StiOptions.Dictionary.HideRelationExceptions && !Report.IsDesigning)
			{
				throw ex;
			}
		}
	}

	public StiDictionary()
		: this(null)
	{
	}

	public StiDictionary(StiReport report)
	{
		Report = report;
		DataSources = new StiDataSourcesCollection(this);
		Relations = new StiDataRelationsCollection(this);
		Databases = new StiDatabaseCollection();
		BusinessObjects = new StiBusinessObjectsCollection(this, null);
	}
}
