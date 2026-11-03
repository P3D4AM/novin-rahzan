using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Extensions;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[JsonObject]
public abstract class StiDataSource : ICloneable, IStiAppDataSource, IStiAppCell, IStiStateSaveRestore, IStiEnumerator, IEnumerator, IEnumerable, IStiName, IStiAlias, IStiInherited, IStiPropertyGridObject, IStiJsonReportObject
{
	public enum Order
	{
		NameInSource = 100,
		Name = 200,
		Alias = 300,
		AllowExpressions = 400,
		Columns = 500,
		CommandTimeout = 600,
		ConnectOnStart = 700,
		Parameters = 800,
		Query = 850,
		ReconnectOnEachRow = 900,
		SqlCommand = 1000,
		Type = 1100
	}

	private string name;

	protected int positionValue;

	protected bool isBofValue;

	protected bool isEofValue;

	private StiStatesManager states;

	private string relationNameStored;

	private bool isFilterProcessing;

	private bool isConnectProcessing;

	private object resFilterMethod;

	private string[] resSortColumns;

	private DataTable dataTable;

	private StiDataColumnsCollection columns;

	[Browsable(false)]
	public virtual StiComponentId ComponentId => StiComponentId.StiDataSource;

	[Browsable(false)]
	public string PropName => name;

	[Browsable(false)]
	[DefaultValue(false)]
	[StiSerializable]
	public bool Inherited { get; set; }

	object IEnumerator.Current => this;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets of the Data Source name.")]
	[ParenthesizePropertyName(true)]
	[StiOrder(200)]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			if (Dictionary != null && Dictionary.Report != null && Dictionary.Report.IsDesigning && StiOptions.Designer.AutoCorrectDataSourceName)
			{
				value = StiNameValidator.CorrectName(value, Dictionary.Report);
			}
			name = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets of the Data Source alias.")]
	[ParenthesizePropertyName(true)]
	[StiOrder(300)]
	public string Alias { get; set; }

	[Browsable(false)]
	public virtual int Position
	{
		get
		{
			return positionValue;
		}
		set
		{
			positionValue = value;
		}
	}

	[Browsable(false)]
	public int RealCount
	{
		get
		{
			if (DetailRows != null)
			{
				return DetailRows.Length;
			}
			if (DataTable != null)
			{
				return DataTable.Rows.Count;
			}
			return 0;
		}
	}

	[Browsable(false)]
	public int Count
	{
		get
		{
			if (RealCount == 0 && StiOptions.Engine.EmulateData)
			{
				if (DataTable == null)
				{
					return 1;
				}
				if (DataTable != null && DataTable.Rows.Count == 0)
				{
					return 1;
				}
			}
			return RealCount;
		}
	}

	[Browsable(false)]
	public virtual bool IsBof
	{
		get
		{
			if (IsEmpty)
			{
				return true;
			}
			return isBofValue;
		}
		set
		{
			isBofValue = value;
		}
	}

	[Browsable(false)]
	public virtual bool IsEof
	{
		get
		{
			if (IsEmpty)
			{
				return true;
			}
			return isEofValue;
		}
		set
		{
			isEofValue = value;
		}
	}

	[Browsable(false)]
	public virtual bool IsEmpty => Count == 0;

	protected StiStatesManager States => states ?? (states = new StiStatesManager());

	[Browsable(false)]
	public bool IsConnected => dataTable != null;

	protected virtual string DataAdapterType => GetDataAdapterType().ToString();

	public virtual object this[string columnName] => GetData(columnName);

	[Browsable(false)]
	public StiRowsCollection Rows { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	[Browsable(false)]
	public string Key { get; set; }

	internal bool IsInited { get; set; }

	internal bool InitForSubreport { get; set; }

	protected Hashtable ColumnIndexes { get; set; }

	protected internal DataRow[] DetailRows { get; set; }

	protected internal Hashtable RowToLevel { get; set; }

	[Browsable(false)]
	[TypeConverter(typeof(StiDataParametersCollectionConverter))]
	[Editor("Stimulsoft.Report.Dictionary.Design.StiDataParametersEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual StiDataParametersCollection Parameters { get; set; }

	internal string NameOfDataBandWhichInitDataSource { get; private set; }

	[Browsable(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[Description("Gets or sets value which indicates that datasource not connect to the data automatically.")]
	public virtual bool ConnectOnStart { get; set; } = true;


	[Browsable(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets value which indicates in which order that datasource will be connect to the data.")]
	public virtual int ConnectionOrder { get; set; } = 1;


	[Browsable(false)]
	public DataTable DataTable
	{
		get
		{
			return dataTable;
		}
		set
		{
			dataTable = value;
			DetailRows = null;
			if (value != null)
			{
				CheckColumnsIndexs();
				if (((this is StiDataTableSource && Dictionary.Report.CacheAllData) || dataTable?.DataSet == null) && Dictionary.CacheDataSet != null && DataTable?.TableName != null)
				{
					int num = 0;
					string text = DataTable.TableName;
					while (Dictionary.CacheDataSet.Tables[text] != null)
					{
						num++;
						text = $"{DataTable.TableName}{num}";
					}
					if (DataTable.TableName != text)
					{
						DataTable.TableName = text;
					}
					Dictionary.CacheDataSet.Tables.Add(DataTable);
				}
			}
			First();
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[StiSerializable]
	public bool IsCloud { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public StiDictionary Dictionary { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the column collection.")]
	[StiOrder(500)]
	[TypeConverter(typeof(StiDataColumnsCollectionConverter))]
	[Editor("Stimulsoft.Report.Dictionary.Design.StiDataColumnsEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual StiDataColumnsCollection Columns
	{
		get
		{
			return columns;
		}
		set
		{
			if (columns == null)
			{
				return;
			}
			columns = value;
			if (value != null)
			{
				value.DataSource = this;
				value.ToList().ForEach(delegate(StiDataColumn c)
				{
					c.DataSource = this;
				});
			}
		}
	}

	public event EventHandler Connecting;

	public event EventHandler Disconnecting;

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyBool(val, "Inherited", Inherited, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		JsonExtensions.AddPropertyBool(val, "IsCloud", IsCloud, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Alias", Alias);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Key", Key);
		JsonExtensions.AddPropertyJObject(val, "Columns", Columns.SaveToJsonObject(mode));
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Inherited":
				Inherited = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Name":
				name = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "IsCloud":
				IsCloud = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Alias":
				Alias = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Key":
				Key = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Columns":
				columns.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public virtual StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.ConnectOnStart()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public string GetNameInSource()
	{
		StiDataStoreSource stiDataStoreSource = this as StiDataStoreSource;
		if (!string.IsNullOrEmpty(stiDataStoreSource?.NameInSource))
		{
			return stiDataStoreSource?.NameInSource;
		}
		return "";
	}

	public string GetName()
	{
		return Name;
	}

	public virtual DataTable GetDataTable(bool allowConnectToData)
	{
		object obj = ((Dictionary != null) ? ((object)Dictionary) : ((object)this));
		lock (obj)
		{
			if (!allowConnectToData)
			{
				return DataTable;
			}
			try
			{
				StiSqlSource stiSqlSource = this as StiSqlSource;
				StiVirtualSource stiVirtualSource = this as StiVirtualSource;
				if (stiSqlSource != null)
				{
					ConnectSqlSource(stiSqlSource);
				}
				else if (stiVirtualSource != null)
				{
					stiVirtualSource.ConnectToData(allowConnect: true);
				}
				else
				{
					StiDataLeader.Connect(this);
				}
				return DataTable;
			}
			finally
			{
				DataTable = null;
			}
		}
	}

	private void ConnectSqlSource(StiSqlSource sqlSource)
	{
		string sqlCommand = sqlSource.SqlCommand;
		try
		{
			StiDataSourceParserHelper.ConnectSqlSource(sqlSource);
			StiDataLeader.Connect(this);
		}
		finally
		{
			sqlSource.SqlCommand = sqlCommand;
		}
	}

	public IStiAppDictionary GetDictionary()
	{
		return (IStiAppDictionary)(object)Dictionary;
	}

	public IEnumerable<IStiAppDataColumn> FetchColumns()
	{
		return Columns.Cast<IStiAppDataColumn>();
	}

	public IStiAppConnection GetConnection()
	{
		return (IStiAppConnection)(object)StiDataSourceHelper.GetDatabaseFromDataSource(this);
	}

	public IEnumerable<IStiAppDataRelation> FetchParentRelations(bool activePreferred)
	{
		return (IEnumerable<IStiAppDataRelation>)this.ParentRelationList(activePreferred);
	}

	public IEnumerable<IStiAppDataRelation> FetchChildRelations(bool activePreferred)
	{
		return (IEnumerable<IStiAppDataRelation>)this.ChildRelationList(activePreferred);
	}

	public IEnumerable<object[]> FetchColumnValues(string[] names)
	{
		List<object[]> list = new List<object[]>();
		if (names == null || names.Length == 0)
		{
			return list;
		}
		IEnumerable<StiDataColumn> enumerable = names.Select((string name) => Columns[name]);
		if (enumerable.All((StiDataColumn c) => c == null))
		{
			return list;
		}
		bool flag = DataTable == null || DataTable.Rows.Count == 0;
		if (flag)
		{
			StiDataLeader.Connect(this);
		}
		if (DataTable == null || DataTable.Rows.Count == 0)
		{
			return list;
		}
		First();
		foreach (DataRow row in DataTable.Rows)
		{
			object[] array = new object[enumerable.Count()];
			int num = 0;
			foreach (StiDataColumn item in enumerable)
			{
				if (item is StiCalcDataColumn)
				{
					string expression = "{" + (item as StiCalcDataColumn).Value + "}";
					string text = StiReportParser.Parse(expression, (IStiReportComponent)(object)Dictionary.Report.Pages[0], allowCache: false, null, allowDataLoading: false);
					try
					{
						array[num] = StiConvert.ChangeType((object)text, item.Type);
					}
					catch
					{
					}
				}
				else
				{
					int columnIndex = dataTable.Columns.IndexOf(item.NameInSource);
					object obj2 = row[columnIndex];
					array[num] = obj2;
				}
				num++;
			}
			list.Add(array);
			Next();
		}
		First();
		if (flag)
		{
			StiDataLeader.Disconnect(this);
		}
		return list;
	}

	string IStiAppCell.GetKey()
	{
		Key = StiKeyHelper.GetOrGeneratedKey(Key);
		return Key;
	}

	void IStiAppCell.SetKey(string key)
	{
		Key = key;
	}

	bool IEnumerator.MoveNext()
	{
		Next();
		return !IsEof;
	}

	void IEnumerator.Reset()
	{
		First();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this;
	}

	public virtual void First()
	{
		positionValue = 0;
		isEofValue = false;
		isBofValue = true;
	}

	public virtual void Prior()
	{
		isBofValue = false;
		isEofValue = false;
		if (positionValue <= 0)
		{
			isBofValue = true;
		}
		else
		{
			positionValue--;
		}
	}

	public virtual void Next()
	{
		isBofValue = false;
		isEofValue = false;
		if (positionValue >= Count - 1)
		{
			isEofValue = true;
		}
		else
		{
			positionValue++;
		}
	}

	public virtual void Last()
	{
		positionValue = Count - 1;
		isEofValue = true;
		isBofValue = false;
	}

	public virtual object Clone()
	{
		StiDataSource stiDataSource = (StiDataSource)MemberwiseClone();
		stiDataSource.columns = new StiDataColumnsCollection(stiDataSource);
		foreach (StiDataColumn column in columns)
		{
			StiDataColumn stiDataColumn2 = (StiDataColumn)column.Clone();
			stiDataColumn2.DataSource = stiDataSource;
			stiDataSource.Columns.Add(stiDataColumn2);
		}
		return stiDataSource;
	}

	public virtual void SaveState(string stateName)
	{
		States.PushInt(stateName, this, "positionValue", positionValue);
		States.PushBool(stateName, this, "isEofValue", isEofValue);
		States.PushBool(stateName, this, "isBofValue", isBofValue);
		if (DetailRows != null && this is StiSqlSource && (this as StiSqlSource).ReconnectOnEachRow)
		{
			DataTable dataTable = this.dataTable.Clone();
			for (int i = 0; i < DetailRows.Length; i++)
			{
				dataTable.ImportRow(DetailRows[i]);
				DetailRows[i] = dataTable.Rows[i];
			}
		}
		if (this is StiSqlSource)
		{
			States.PushBool(stateName, this, "reconnectOnEachRow", (this as StiSqlSource).ReconnectOnEachRow);
		}
		States.Push(stateName, this, "detailRows", DetailRows);
		States.Push(stateName, this, "resFilterMethod", resFilterMethod);
		States.PushBool(stateName, this, "IsInited", IsInited);
	}

	public virtual void RestoreState(string stateName)
	{
		if (States.IsExist(stateName, this))
		{
			isBofValue = States.PopBool(stateName, this, "isBofValue");
			isEofValue = States.PopBool(stateName, this, "isEofValue");
			positionValue = States.PopInt(stateName, this, "positionValue");
			DetailRows = (DataRow[])States.Pop(stateName, this, "detailRows");
			resFilterMethod = States.Pop(stateName, this, "resFilterMethod");
			IsInited = States.PopBool(stateName, this, "IsInited");
			if (this is StiSqlSource)
			{
				(this as StiSqlSource).ReconnectOnEachRow = States.PopBool(stateName, this, "reconnectOnEachRow");
			}
			States.ClearState(stateName);
		}
	}

	public virtual void ClearAllStates()
	{
		states = null;
	}

	private bool IsEqualSort(string[] sortColumns1, string[] sortColumns2)
	{
		if (sortColumns1 == null && sortColumns2 == null)
		{
			return true;
		}
		if (sortColumns1 != null && sortColumns2 == null)
		{
			return false;
		}
		if (sortColumns1 == null && sortColumns2 != null)
		{
			return false;
		}
		if (sortColumns1.Length != sortColumns2.Length)
		{
			return false;
		}
		int num = 0;
		foreach (string text in sortColumns1)
		{
			if (text != sortColumns2[num])
			{
				return false;
			}
			num++;
		}
		return true;
	}

	public void SetData(StiDataBand dataBand, string relationName, object filterMethod, string[] sortColumns, bool reinit, StiComponent component)
	{
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Invalid comparison between Unknown and I4
		bool flag = false;
		if (this is StiSqlSource)
		{
			flag = ((StiSqlSource)this).ReconnectOnEachRow;
		}
		if (resFilterMethod != filterMethod)
		{
			IsInited = false;
		}
		if (!IsEqualSort(sortColumns, resSortColumns))
		{
			IsInited = false;
		}
		if (dataBand != null)
		{
			if (dataBand.Name != NameOfDataBandWhichInitDataSource)
			{
				IsInited = false;
			}
			NameOfDataBandWhichInitDataSource = dataBand.Name;
		}
		if (!flag && IsInited && !reinit && !InitForSubreport && relationName == relationNameStored)
		{
			return;
		}
		if (dataBand != null)
		{
			StiReport report = dataBand.Report;
			if (report == null && component != null)
			{
				report = component.Report;
			}
			if (report != null && report.CacheTotals && report.CachedTotals != null && !report.CachedTotalsLocked)
			{
				report.CachedTotals[dataBand] = null;
			}
		}
		InitForSubreport = false;
		resFilterMethod = filterMethod;
		resSortColumns = sortColumns;
		StiComponentsCollection groupHeaders = null;
		if (dataBand != null)
		{
			if ((int)dataBand.Report.EngineVersion == 1)
			{
				groupHeaders = dataBand.DataBandInfoV1.GroupHeaderComponents;
			}
			else
			{
				groupHeaders = dataBand.DataBandInfoV2.GroupHeaders;
				dataBand.DataBandInfoV2.GroupHeaderCachedResults = null;
				dataBand.DataBandInfoV2.GroupFooterCachedResults = null;
			}
		}
		DetailRows = null;
		SetDetails(relationName);
		if (StiOptions.Engine.FilterDataInDataSourceBeforeSorting)
		{
			SetFilter(filterMethod);
		}
		object[,,] conditions = GetConditions(dataBand);
		SetSort(conditions, sortColumns, component, dataBand, groupHeaders);
		if (!StiOptions.Engine.FilterDataInDataSourceBeforeSorting)
		{
			SetFilter(filterMethod);
		}
		IsInited = true;
	}

	internal object[,,] GetConditions(StiDataBand dataBand)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Invalid comparison between Unknown and I4
		object[,,] array = null;
		if (dataBand == null)
		{
			return null;
		}
		StiComponentsCollection stiComponentsCollection = (((int)dataBand.Report.EngineVersion != 1) ? dataBand.DataBandInfoV2.GroupHeaders : dataBand.DataBandInfoV1.GroupHeaderComponents);
		if (stiComponentsCollection != null && stiComponentsCollection.Count > 0)
		{
			bool isEof = dataBand.IsEof;
			bool isBof = dataBand.IsBof;
			int num = 0;
			foreach (StiGroupHeaderBand item in stiComponentsCollection)
			{
				if (item.SortDirection != StiGroupSortDirection.None)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			array = new object[dataBand.Count, num, 2];
			for (int i = 0; i < dataBand.Count; i++)
			{
				dataBand.Position = i;
				int num2 = 0;
				foreach (StiGroupHeaderBand item2 in stiComponentsCollection)
				{
					if (item2.SortDirection != StiGroupSortDirection.None)
					{
						if ((int)dataBand.Report.EngineVersion == 1)
						{
							array[i, num2, 0] = StiGroupHeaderBandV1Builder.GetCurrentConditionValue(item2);
						}
						else
						{
							array[i, num2, 0] = StiGroupHeaderBandV2Builder.GetCurrentConditionValue(item2);
						}
						array[i, num2, 1] = item2.SortDirection;
						num2++;
					}
				}
			}
			dataBand.IsEof = isEof;
			dataBand.IsBof = isBof;
		}
		return array;
	}

	public void SetDetails(string relationName)
	{
		if (this is StiSqlSource { ReconnectOnEachRow: not false } stiSqlSource)
		{
			stiSqlSource.UpdateParameters();
		}
		if (string.IsNullOrEmpty(relationName))
		{
			return;
		}
		StiDataRelation stiDataRelation = Dictionary.Relations[relationName];
		if (stiDataRelation == null)
		{
			return;
		}
		StiDataSource parentSource = stiDataRelation.ParentSource;
		if (parentSource == null)
		{
			return;
		}
		DataTable dataTable = parentSource.DataTable;
		if (dataTable == null)
		{
			return;
		}
		if (parentSource.DetailRows != null)
		{
			if (parentSource.DetailRows.Length != 0)
			{
				if (parentSource.Position < parentSource.DetailRows.Length)
				{
					DetailRows = parentSource.DetailRows[parentSource.Position].GetChildRows(relationName);
				}
				else
				{
					DetailRows = new DataRow[0];
				}
			}
			else
			{
				DetailRows = parentSource.DetailRows;
			}
		}
		else if (parentSource.Position < dataTable.Rows.Count)
		{
			DetailRows = dataTable.Rows[parentSource.Position].GetChildRows(relationName);
		}
	}

	public void SetFilter(object filterMethod)
	{
		if (isFilterProcessing || filterMethod == null || RealCount <= 0)
		{
			return;
		}
		isFilterProcessing = true;
		try
		{
			int position = Position;
			int line = Dictionary.Report.Line;
			List<DataRow> list = new List<DataRow>();
			int count = Count;
			if (filterMethod is StiFilterEventHandler)
			{
				StiFilterEventHandler stiFilterEventHandler = (StiFilterEventHandler)filterMethod;
				for (int i = 0; i < count; i++)
				{
					Position = i;
					Dictionary.Report.Line = i + 1;
					try
					{
						StiFilterEventArgs stiFilterEventArgs = new StiFilterEventArgs();
						stiFilterEventHandler(this, stiFilterEventArgs);
						if (stiFilterEventArgs.Value)
						{
							list.Add((DetailRows == null) ? DataTable.Rows[i] : DetailRows[i]);
						}
					}
					catch
					{
					}
				}
			}
			else if (filterMethod is StiParser.StiFilterParserData)
			{
				StiParser.StiFilterParserData stiFilterParserData = (StiParser.StiFilterParserData)filterMethod;
				StiParserParameters stiParserParameters = new StiParserParameters
				{
					ReturnAsmList = true
				};
				object objectAsmList = StiParser.ParseTextValue(stiFilterParserData.Expression, stiFilterParserData.Component, stiFilterParserData.Component, stiParserParameters);
				for (int j = 0; j < count; j++)
				{
					Position = j;
					Dictionary.Report.Line = j + 1;
					try
					{
						object obj2 = stiParserParameters.Parser.ExecuteAsm(objectAsmList);
						if (obj2 is bool && (bool)obj2)
						{
							list.Add((DetailRows == null) ? DataTable.Rows[j] : DetailRows[j]);
						}
					}
					catch
					{
					}
				}
			}
			DetailRows = list.ToArray();
			Position = position;
			Dictionary.Report.Line = line;
		}
		finally
		{
			isFilterProcessing = false;
		}
	}

	public void SetSort(object[,,] conditions, string[] sortColumns, StiComponent component, StiDataBand databand, StiComponentsCollection groupHeaders)
	{
		try
		{
			if (component is StiHierarchicalBand)
			{
				if (RealCount != 0)
				{
					if (DetailRows == null && DataTable != null)
					{
						DetailRows = new DataRow[DataTable.Rows.Count];
						DataTable.Rows.CopyTo(DetailRows, 0);
					}
					Hashtable hashtable = new Hashtable();
					int num = 0;
					DataRow[] detailRows = DetailRows;
					foreach (DataRow key in detailRows)
					{
						hashtable[key] = num++;
					}
					StiHierarchicalDataSort stiHierarchicalDataSort = new StiHierarchicalDataSort(this, component as StiHierarchicalBand, sortColumns);
					stiHierarchicalDataSort.Process(hashtable);
				}
			}
			else
			{
				if ((conditions == null && sortColumns == null) || RealCount <= 0)
				{
					return;
				}
				List<DataRow> list = new List<DataRow>();
				if (DetailRows == null && DataTable != null)
				{
					foreach (DataRow row in dataTable.Rows)
					{
						list.Add(row);
					}
				}
				if (DetailRows != null)
				{
					DataRow[] detailRows2 = DetailRows;
					foreach (DataRow item2 in detailRows2)
					{
						list.Add(item2);
					}
				}
				Hashtable hashtable2 = new Hashtable();
				int num2 = 0;
				foreach (DataRow item3 in list)
				{
					hashtable2[item3] = num2++;
				}
				StiDataSort stiDataSort = new StiDataSort(hashtable2, conditions, sortColumns, this);
				list.Sort(stiDataSort);
				int num3 = 0;
				DetailRows = new DataRow[list.Count];
				foreach (DataRow item4 in list)
				{
					DetailRows[num3++] = item4;
				}
				bool flag = false;
				if (groupHeaders != null)
				{
					foreach (StiGroupHeaderBand groupHeader in groupHeaders)
					{
						if (groupHeader.SummarySortDirection != StiGroupSortDirection.None)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					Hashtable hashtable3 = new Hashtable();
					Hashtable hashtable4 = new Hashtable();
					int[] array = new int[groupHeaders.Count];
					Hashtable hashtable5 = new Hashtable();
					int num4 = 0;
					foreach (DataRow item5 in list)
					{
						databand.Position = num4;
						hashtable5[item5] = num4;
						StiDataBandV2Builder.PrepareGroupResults(databand);
						int num5 = 0;
						foreach (StiGroupHeaderBand groupHeader2 in groupHeaders)
						{
							if (databand.DataBandInfoV2.GroupHeaderResults[num5] && groupHeader2.Report != null)
							{
								StiReport report = groupHeader2.Report;
								if (report.CacheTotals && report.CachedTotals != null)
								{
									report.CachedTotals[groupHeader2] = null;
								}
							}
							string text = null;
							object data = groupHeader2;
							if (groupHeader2.SummaryType != StiGroupSummaryType.Count)
							{
								if (groupHeader2.Report.CalculationMode == StiCalculationMode.Compilation)
								{
									text = groupHeader2.Name + "__GetSummaryExpression";
									if (Totals.GetMethod(groupHeader2.Report, text) == null)
									{
										continue;
									}
								}
								else
								{
									if (string.IsNullOrEmpty(groupHeader2.SummaryExpression.Value))
									{
										continue;
									}
									StiParserParameters stiParserParameters = new StiParserParameters
									{
										ExecuteIfStoreToPrint = false,
										ReturnAsmList = true
									};
									List<StiParser.StiAsmCommand> asmList = (List<StiParser.StiAsmCommand>)StiParser.ParseTextValue(groupHeader2.SummaryExpression.Value, groupHeader2, stiParserParameters);
									data = new StiParser.StiParserData(groupHeader2, asmList, stiParserParameters.Parser);
								}
							}
							object value = null;
							switch (groupHeader2.SummaryType)
							{
							case StiGroupSummaryType.Avg:
								try
								{
									value = Convert.ChangeType(Totals.Avg(data, groupHeader2.Report, text), typeof(decimal));
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.AvgDate:
								try
								{
									value = Convert.ChangeType(Totals.AvgDate(data, groupHeader2.Report, text), typeof(DateTime));
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.AvgTime:
								try
								{
									value = Convert.ChangeType(Totals.AvgTime(data, groupHeader2.Report, text), typeof(TimeSpan));
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.Count:
								try
								{
									value = Totals.Count(groupHeader2);
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.CountDistinct:
								try
								{
									value = Totals.CountDistinct(groupHeader2, groupHeader2.Report, text);
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.Max:
								try
								{
									value = Convert.ChangeType(Totals.Max(data, groupHeader2.Report, text), typeof(decimal));
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.MaxDate:
								try
								{
									value = Convert.ChangeType(Totals.MaxDate(data, groupHeader2.Report, text), typeof(DateTime));
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.MaxTime:
								try
								{
									value = Convert.ChangeType(Totals.MaxTime(data, groupHeader2.Report, text), typeof(TimeSpan));
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.Median:
								try
								{
									value = Convert.ChangeType(Totals.Median(data, groupHeader2.Report, text), typeof(decimal));
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.Min:
								try
								{
									value = Convert.ChangeType(Totals.Min(data, groupHeader2.Report, text), typeof(decimal));
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.MinDate:
								try
								{
									value = Convert.ChangeType(Totals.MinDate(data, groupHeader2.Report, text), typeof(DateTime));
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.MinTime:
								try
								{
									value = Convert.ChangeType(Totals.MinTime(data, groupHeader2.Report, text), typeof(TimeSpan));
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.Mode:
								try
								{
									value = Convert.ChangeType(Totals.Mode(data, groupHeader2.Report, text), typeof(decimal));
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.Sum:
								try
								{
									value = Convert.ChangeType(Totals.Sum(data, groupHeader2.Report, text), typeof(decimal));
								}
								catch
								{
								}
								break;
							case StiGroupSummaryType.SumTime:
								try
								{
									value = Convert.ChangeType(Totals.SumTime(data, groupHeader2.Report, text), typeof(TimeSpan));
								}
								catch
								{
								}
								break;
							}
							if (databand.DataBandInfoV2.GroupHeaderResults[num5])
							{
								array[num5]++;
							}
							Hashtable hashtable6 = hashtable3[groupHeader2] as Hashtable;
							if (hashtable6 == null)
							{
								hashtable6 = (Hashtable)(hashtable3[groupHeader2] = new Hashtable());
							}
							hashtable6[item5] = value;
							hashtable6 = hashtable4[groupHeader2] as Hashtable;
							if (hashtable6 == null)
							{
								hashtable6 = (Hashtable)(hashtable4[groupHeader2] = new Hashtable());
							}
							hashtable6[item5] = array[num5];
							num5++;
						}
						num4++;
					}
					if (hashtable3 != null && hashtable3.Count > 0)
					{
						databand.DataBandInfoV2.GroupHeaderCachedResults = null;
						databand.DataBandInfoV2.GroupFooterCachedResults = null;
						StiGroupSummaryDataSort stiGroupSummaryDataSort = new StiGroupSummaryDataSort(hashtable3, hashtable4, groupHeaders, hashtable5);
						list.Sort(stiGroupSummaryDataSort);
						num3 = 0;
						DetailRows = new DataRow[list.Count];
						foreach (DataRow item6 in list)
						{
							DetailRows[num3++] = item6;
						}
						stiGroupSummaryDataSort.Clear();
					}
				}
				stiDataSort.Clear();
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "SetSort...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}

	public void ResetDetailsRows()
	{
		DetailRows = null;
	}

	public void ResetData()
	{
		IsInited = false;
		relationNameStored = null;
		resFilterMethod = null;
		resSortColumns = null;
	}

	internal DataRow GetDataRow(int index)
	{
		if (DetailRows == null)
		{
			if (DataTable == null || index >= DataTable.Rows.Count)
			{
				return null;
			}
			return DataTable.Rows[index];
		}
		if (index >= DetailRows.Length)
		{
			return null;
		}
		return DetailRows[index];
	}

	public StiDataRow GetParentData(string relation)
	{
		DataRow dataRow = GetDataRow(Position);
		if (dataRow == null)
		{
			return new StiDataRow(Dictionary.Relations[relation]?.ParentSource, dataRow);
		}
		StiDataRelation stiDataRelation = Dictionary.Relations[relation];
		if (stiDataRelation != null)
		{
			relation = stiDataRelation.NameInSource;
		}
		DataRow[] parentRows = dataRow.GetParentRows(relation);
		return new StiDataRow(dataRow: (parentRows == null || parentRows.Length == 0) ? null : parentRows[0], dataSource: Dictionary.Relations[relation]?.ParentSource);
	}

	public virtual StiDataRelationsCollection GetParentRelations()
	{
		StiDataRelationsCollection stiDataRelationsCollection = new StiDataRelationsCollection(Dictionary);
		if (Dictionary == null)
		{
			return stiDataRelationsCollection;
		}
		foreach (StiDataRelation relation in Dictionary.Relations)
		{
			if (relation.ChildSource == this)
			{
				stiDataRelationsCollection.Add(relation);
			}
		}
		return stiDataRelationsCollection;
	}

	public virtual StiDataRelationsCollection GetChildRelations()
	{
		StiDataRelationsCollection stiDataRelationsCollection = new StiDataRelationsCollection(Dictionary);
		foreach (StiDataRelation relation in Dictionary.Relations)
		{
			if (relation.ParentSource == this)
			{
				stiDataRelationsCollection.Add(relation);
			}
		}
		return stiDataRelationsCollection;
	}

	public virtual StiDataSource GetParentDataSource(string relationName, bool allowRelationName = false)
	{
		foreach (StiDataRelation relation in Dictionary.Relations)
		{
			if (relation.ChildSource == this && relation.NameInSource == relationName)
			{
				return relation.ParentSource;
			}
			if (relation.ChildSource == this && allowRelationName && relation.Name == relationName)
			{
				return relation.ParentSource;
			}
		}
		foreach (StiDataRelation relation2 in Dictionary.Relations)
		{
			if (relation2.ChildSource == this && StiNameValidator.CorrectName(relation2.NameInSource, Dictionary?.Report) == relationName)
			{
				return relation2.ParentSource;
			}
		}
		return null;
	}

	public virtual StiDataSource GetChildDataSource(string relationName)
	{
		foreach (StiDataRelation relation in Dictionary.Relations)
		{
			if (relation.ParentSource == this && relation.NameInSource == relationName)
			{
				return relation.ChildSource;
			}
		}
		return null;
	}

	protected void InvokeConnecting()
	{
		try
		{
			this.Connecting?.Invoke(this, EventArgs.Empty);
			StiDictionary dictionary = Dictionary;
			if (dictionary == null || dictionary.Report?.CalculationMode != StiCalculationMode.Interpretation)
			{
				return;
			}
			StiText stiText = new StiText
			{
				Name = "**DataSourceParameter**",
				Page = Dictionary.Report.Pages[0]
			};
			foreach (StiDataParameter parameter in Parameters)
			{
				parameter.ParameterValue = StiParser.ParseTextValue("{" + parameter.Value + "}", stiText);
			}
			if (this is StiSqlSource stiSqlSource)
			{
				object obj = null;
				if (Dictionary.Report.Variables != null && Dictionary.Report.Variables["**StoredDataSourceSqlCommandForInterpretationMode**" + stiSqlSource.Name] != null)
				{
					obj = Dictionary.Report.Variables["**StoredDataSourceSqlCommandForInterpretationMode**" + stiSqlSource.Name];
				}
				if (obj != null && obj is string)
				{
					stiText.Name = "**DataSourceSqlCommand**";
					stiSqlSource.SqlCommand = Convert.ToString(StiParser.ParseTextValue(obj as string, stiText));
				}
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Datasource '" + Name + "' connecting...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}

	protected void InvokeDisconnecting()
	{
		try
		{
			this.Disconnecting?.Invoke(this, EventArgs.Empty);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Datasource '" + Name + "' disconnecting...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}

	public void Connect()
	{
		StiDataLeader.Connect(this);
	}

	public void Connect(bool loadData, bool invokeEvents = true)
	{
		Connect(null, loadData, invokeEvents);
	}

	public void Connect(StiDataCollection datas)
	{
		Connect(datas, loadData: true);
	}

	public virtual void Connect(StiDataCollection datas, bool loadData, bool invokeEvents = true)
	{
		if (isConnectProcessing)
		{
			return;
		}
		isConnectProcessing = true;
		if (this is StiSqlSource && string.IsNullOrWhiteSpace((this as StiSqlSource).SqlCommand))
		{
			invokeEvents = true;
		}
		if (invokeEvents && Dictionary != null && !Dictionary.UseInternalData)
		{
			InvokeConnecting();
		}
		StiDataLeader.ConnectDataSourceToData(GetDataAdapter(), Dictionary, this, loadData);
		if ((DataTable == null || RealCount == 0) && StiOptions.Engine.EmulateData && Dictionary.Report.IsRendering)
		{
			DataTable dataTable = DataTable;
			if (dataTable == null)
			{
				dataTable = new DataTable();
				foreach (StiDataColumn column in Columns)
				{
					dataTable.Columns.Add(new DataColumn(column.NameInSource, column.Type));
				}
			}
			try
			{
				DataRow dataRow = dataTable.NewRow();
				foreach (StiDataColumn column2 in Columns)
				{
					Type type = column2.Type;
					object value = DBNull.Value;
					if (type == typeof(string))
					{
						value = ((DataTable == null || DataTable.Columns[column2.NameInSource] == null || !(DataTable.Columns[column2.NameInSource].GetType() == typeof(string))) ? "" : "Test");
					}
					else if (type == typeof(bool))
					{
						value = true;
					}
					else if (type == typeof(DateTime))
					{
						value = DateTime.Now;
					}
					else if (type == typeof(char))
					{
						value = ' ';
					}
					else if (type.IsPrimitive)
					{
						value = Convert.ChangeType(0, type);
					}
					dataRow[column2.NameInSource] = value;
				}
				dataTable.Rows.Add(dataRow);
			}
			catch
			{
			}
			DataTable = null;
			DataTable = dataTable;
		}
		isConnectProcessing = false;
	}

	public virtual void Disconnect()
	{
		InvokeDisconnecting();
		if (DataTable != null)
		{
			if (Dictionary != null && Dictionary.CacheDataSet != null && Dictionary.CacheDataSet.Tables.IndexOf(DataTable) != -1)
			{
				int num = 0;
				while (num < Dictionary.CacheDataSet.Relations.Count)
				{
					DataRelation dataRelation = Dictionary.CacheDataSet.Relations[num];
					if (dataRelation.ParentTable == DataTable)
					{
						dataRelation.ChildTable.Constraints.Remove(dataRelation.RelationName);
					}
					if (dataRelation.ChildTable == DataTable && dataRelation.ParentTable.Constraints.Contains(dataRelation.RelationName))
					{
						dataRelation.ParentTable.Constraints.Remove(dataRelation.RelationName);
					}
					if (dataRelation.ParentTable == DataTable || dataRelation.ChildTable == DataTable)
					{
						Dictionary.CacheDataSet.Relations.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
				DataTable.ChildRelations.Clear();
				DataTable.ParentRelations.Clear();
				DataTable.Constraints.Clear();
				Dictionary.CacheDataSet.Tables.Remove(DataTable);
			}
			DataTable = null;
		}
		RowToLevel = null;
		DetailRows = null;
		isConnectProcessing = false;
	}

	protected abstract Type GetDataAdapterType();

	public void FillColumns()
	{
		try
		{
			if (this is StiDBaseSource || this is StiCsvSource)
			{
				Columns.Clear();
				Dictionary.SynchronizeColumns(null, this);
				return;
			}
			StiDataAdapterService dataAdapter = StiDataAdapterService.GetDataAdapter(this);
			if (dataAdapter == null)
			{
				return;
			}
			StiDataStoreSource storeSource = this as StiDataStoreSource;
			if (storeSource == null || storeSource.NameInSource == null || storeSource.NameInSource.Trim().Length == 0)
			{
				return;
			}
			StiData stiData = Dictionary.DataStore.ToList().FirstOrDefault((StiData data) => data.Name.ToLowerInvariant() == storeSource.NameInSource.ToLowerInvariant());
			if (stiData != null)
			{
				StiSqlSource stiSqlSource = this as StiSqlSource;
				Columns.Clear();
				if (stiSqlSource != null)
				{
					StiDataColumnsCollection columnsFromData = StiDataLeader.GetColumnsFromData(dataAdapter, stiData, this);
					columns.AddRange(columnsFromData);
				}
				else
				{
					Dictionary.SynchronizeColumns(stiData, this);
				}
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}

	public virtual StiDataAdapterService GetDataAdapter()
	{
		IEnumerable<StiDataAdapterService> source = StiOptions.Services.DataAdapters.Where((StiDataAdapterService s) => ((StiService)s).ServiceEnabled);
		Type dataAdapterType = GetDataAdapterType();
		if (dataAdapterType != null)
		{
			return StiActivator.CreateObject(dataAdapterType) as StiDataAdapterService;
		}
		StiDataAdapterService stiDataAdapterService = ((Dictionary != null && Dictionary.UseInternalData) ? source.FirstOrDefault((StiDataAdapterService d) => d is StiDataTableAdapterService) : null);
		if (stiDataAdapterService != null)
		{
			return stiDataAdapterService;
		}
		return source.FirstOrDefault((StiDataAdapterService d) => ((object)d).GetType().ToString() == DataAdapterType);
	}

	public DataTable GetDataTable(DataTable table)
	{
		DataTable dataTable = table.Clone();
		int[] array = new int[dataTable.Columns.Count];
		for (int i = 0; i < dataTable.Columns.Count; i++)
		{
			DataColumn dataColumn = dataTable.Columns[i];
			array[i] = table.Columns.IndexOf(dataColumn.ColumnName);
		}
		for (int j = 0; j < table.Rows.Count; j++)
		{
			DataRow dataRow = dataTable.NewRow();
			for (int k = 0; k < dataTable.Columns.Count; k++)
			{
				try
				{
					dataRow[k] = table.Rows[j][array[k]];
				}
				catch
				{
				}
			}
			dataTable.Rows.Add(dataRow);
		}
		return dataTable;
	}

	public DataTable GetDataTable()
	{
		try
		{
			if (DataTable != null)
			{
				return GetDataTable(DataTable);
			}
			return new DataTable(Name);
		}
		catch
		{
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return new DataTable(Name);
	}

	public object GetData(string columnName)
	{
		try
		{
			return GetData(columnName, Position);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return null;
	}

	public object GetData(string columnName, int index)
	{
		if (IsConnected)
		{
			int columnIndex = GetColumnIndex(columnName);
			if (columnIndex < 0)
			{
				return null;
			}
			return GetDataRow(index)?[columnIndex];
		}
		DataTable dataTable = StiDataPicker.GetFromCache((IStiAppDataSource)(object)this);
		if (dataTable == null && Dictionary?.Report != null && Dictionary.Report.Pages.ToList().Any((StiPage p) => p.IsPage))
		{
			StiDataLeader.Connect(this, loadData: true, invokeEvents: false);
			dataTable = GetDataTable()?.Copy();
			dataTable = StiDataPicker.ProcessCalculatedColumns(dataTable, (IStiAppDataSource)(object)this);
			StiDataPicker.AddToCache((IStiAppDataSource)(object)this, dataTable);
			StiDataLeader.Disconnect(this);
		}
		if (dataTable != null)
		{
			if (!dataTable.Columns.Contains(columnName) || index < 0 || index >= dataTable.Rows.Count)
			{
				return DBNull.Value;
			}
			return dataTable.Rows[index][columnName];
		}
		return DBNull.Value;
	}

	public int GetColumnIndex(string columnName)
	{
		if (ColumnIndexes != null)
		{
			object obj = ColumnIndexes[columnName];
			if ((obj == null || (obj is int && (int)obj == -1)) && obj == null)
			{
				obj = ColumnIndexes[columnName.ToLower(CultureInfo.InvariantCulture)];
			}
			if (obj == null)
			{
				return -1;
			}
			if ((int)obj == -1)
			{
				string message = "Column '" + columnName + "' from data source '" + Name + "' not found in table '" + DataTable.TableName + "'";
				if (StiOptions.Engine.AllowThrowExceptionWhenColumnDoesNotExists)
				{
					throw new Exception(message);
				}
				StiLogService.Write(GetType(), message);
			}
			return (int)obj;
		}
		int num = 0;
		foreach (StiDataColumn column in Columns)
		{
			if (column.NameInSource == columnName)
			{
				return num;
			}
			num++;
		}
		num = 0;
		foreach (StiDataColumn column2 in Columns)
		{
			if (column2.Name == columnName || column2.Alias == columnName)
			{
				return num;
			}
			num++;
		}
		return -1;
	}

	public void SynchronizeColumns()
	{
		if (!(this is StiDataStoreSource stiDataStoreSource))
		{
			return;
		}
		StiData stiData = null;
		foreach (StiData item in Dictionary.DataStore)
		{
			if (item.Name.ToLowerInvariant() == stiDataStoreSource.NameInSource.ToLowerInvariant())
			{
				stiData = item;
				break;
			}
			if (StiOptions.Dictionary.NotIdenticalNameAndAliasAtRegistrationOfNewData && item.Alias.ToLowerInvariant() == stiDataStoreSource.NameInSource.ToLowerInvariant())
			{
				item.Name = item.Alias;
				stiData = item;
				break;
			}
		}
		if (stiData != null)
		{
			Dictionary.SynchronizeColumns(stiData, this);
		}
	}

	public void CheckColumnsIndexs()
	{
		if ((ColumnIndexes != null && ColumnIndexes.Keys.Count != 0) || dataTable == null || DataTable.Columns.Count <= 0)
		{
			return;
		}
		ColumnIndexes = new Hashtable();
		Hashtable hashtable = new Hashtable();
		for (int i = 0; i < DataTable.Columns.Count; i++)
		{
			hashtable[DataTable.Columns[i].ColumnName.ToLowerInvariant()] = i;
		}
		foreach (StiDataColumn column in Columns)
		{
			if (!(column is StiCalcDataColumn))
			{
				int num = -1;
				object obj = hashtable[column.NameInSource.ToLowerInvariant()];
				if (obj != null)
				{
					num = (int)obj;
				}
				ColumnIndexes[column.NameInSource.ToLowerInvariant()] = num;
				ColumnIndexes[column.NameInSource] = num;
				ColumnIndexes[column.Name] = num;
				ColumnIndexes[column.Alias] = num;
				ColumnIndexes[StiNameValidator.CorrectName(column.Name, Dictionary?.Report)] = num;
			}
		}
	}

	public override string ToString()
	{
		return ToString(onlyAlias: false);
	}

	public string ToString(bool onlyAlias)
	{
		if (onlyAlias && !string.IsNullOrWhiteSpace(Alias))
		{
			return Alias;
		}
		if (Name == Alias || string.IsNullOrWhiteSpace(Alias))
		{
			return Name;
		}
		return Name + " [" + Alias + "]";
	}

	public int GetLevel()
	{
		if (RowToLevel == null)
		{
			return 0;
		}
		DataRow dataRow = null;
		if (DetailRows != null)
		{
			if (DetailRows.Length <= Position)
			{
				return 0;
			}
			dataRow = DetailRows[Position];
		}
		else
		{
			if (DataTable == null || DataTable.Rows.Count <= Position)
			{
				return 0;
			}
			dataRow = DataTable.Rows[Position];
		}
		object obj = RowToLevel[dataRow];
		if (obj is int)
		{
			return (int)obj;
		}
		return 0;
	}

	public virtual string GetCategoryName()
	{
		StiDataAdapterService dataAdapter = StiDataAdapterService.GetDataAdapter(this);
		if (dataAdapter != null)
		{
			return ((StiService)dataAdapter).ServiceName;
		}
		return string.Empty;
	}

	public virtual StiDataSource CreateNew()
	{
		throw new NotImplementedException();
	}

	private bool ShouldSerializeParameters()
	{
		if (Parameters != null)
		{
			return Parameters.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeColumns()
	{
		if (Columns != null)
		{
			return Columns.Count > 0;
		}
		return true;
	}

	public StiDataSource(string name, string alias)
	{
		Name = name;
		Alias = alias;
		if (string.IsNullOrEmpty(Alias))
		{
			Alias = name;
		}
		Rows = new StiRowsCollection(this);
		columns = new StiDataColumnsCollection(this);
		Parameters = new StiDataParametersCollection(this);
		Key = StiKeyHelper.GenerateKey();
	}

	public StiDataSource(string name, string alias, string key)
		: this(name, alias)
	{
		Key = key;
	}
}
