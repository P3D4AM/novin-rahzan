using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlSourceConverter))]
public class StiSqlSource : StiDataTableSource
{
	public override StiComponentId ComponentId => StiComponentId.StiSqlSource;

	[StiSerializable]
	[Browsable(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[Description("Gets or sets value which indicates that datasource not connect to the data automatically.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(700)]
	public override bool ConnectOnStart
	{
		get
		{
			return base.ConnectOnStart;
		}
		set
		{
			base.ConnectOnStart = value;
		}
	}

	[StiSerializable]
	[Browsable(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[Description("Gets or sets value which indicates that sql query of the datasource can contain script expressions or no.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(400)]
	public bool AllowExpressions { get; set; } = true;


	[StiSerializable]
	[Browsable(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(StiSqlSourceType.Table)]
	[Description("Gets or sets value which indicates type of sql datasource.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiOrder(1100)]
	public StiSqlSourceType Type { get; set; }

	[Browsable(false)]
	public IDbDataAdapter DataAdapter { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(0)]
	[Description("Gets or sets a number of seconds to wait while attempting to execute a command, before canceling the attempt and generate an error. Default is 30.")]
	[StiOrder(600)]
	public int CommandTimeout { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[Description("Gets or sets value which indicates that datasource reconnect on each master row in master-detail reports.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(900)]
	public bool ReconnectOnEachRow { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a SQL statement to execute at the Data Source.")]
	[StiOrder(1000)]
	public virtual string SqlCommand { get; set; } = string.Empty;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the parameter collection of the SQL query.")]
	[Browsable(true)]
	[StiOrder(800)]
	public override StiDataParametersCollection Parameters
	{
		get
		{
			return base.Parameters;
		}
		set
		{
			base.Parameters = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "ConnectOnStart", ConnectOnStart, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowExpressions", AllowExpressions, true, false);
		JsonExtensions.AddPropertyEnum(val, "Type", (Enum)Type, (Enum)StiSqlSourceType.Table);
		JsonExtensions.AddPropertyInt(val, "CommandTimeout", CommandTimeout, 30);
		JsonExtensions.AddPropertyBool(val, "ReconnectOnEachRow", ReconnectOnEachRow, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "SqlCommand", SqlCommand);
		JsonExtensions.AddPropertyJObject(val, "Parameters", Parameters.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ConnectOnStart":
				ConnectOnStart = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowExpressions":
				AllowExpressions = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Type":
				Type = StiJsonReportObjectHelper.DeserializeEnum<StiSqlSourceType>(item);
				break;
			case "CommandTimeout":
				CommandTimeout = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "ReconnectOnEachRow":
				ReconnectOnEachRow = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "SqlCommand":
				SqlCommand = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Parameters":
				Parameters.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[9]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.NameInSource(),
			propertiesHelper.ConnectOnStart(),
			propertiesHelper.AllowExpressions(),
			propertiesHelper.DataSourceType(),
			propertiesHelper.CommandTimeout(),
			propertiesHelper.ReconnectOnEachRow(),
			propertiesHelper.SqlCommand()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	private bool ShouldSerializeParameters()
	{
		if (Parameters != null)
		{
			return Parameters.Count > 0;
		}
		return true;
	}

	public virtual StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiMsSqlConnector.Get(connectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiSqlAdapterService);
	}

	public virtual Type GetParameterTypesEnum()
	{
		return CreateConnector().SqlType;
	}

	public Type ConvertDbTypeToType(int sqlType)
	{
		return ConvertDbTypeToTypeInternal(sqlType);
	}

	protected virtual Type ConvertDbTypeToTypeInternal(int sqlType)
	{
		return CreateConnector().GetNetType(sqlType);
	}

	public virtual StiDataParameter AddParameter()
	{
		StiDataParameter stiDataParameter = new StiDataParameter(Loc.Get("PropertyMain", "Parameter"), string.Empty, CreateConnector().DefaultSqlType, 0);
		Parameters.Add(stiDataParameter);
		return stiDataParameter;
	}

	public virtual void UpdateParameters()
	{
		if (base.DataTable != null && DataAdapter != null && Parameters.Count != 0)
		{
			InvokeConnecting();
			RetrieveData();
		}
	}

	public void RetrieveData(bool schemaOnly = false)
	{
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Invalid comparison between Unknown and I4
		StiSqlDataConnector val = CreateConnector();
		DbDataAdapter dbDataAdapter = DataAdapter as DbDataAdapter;
		if (base.DataTable == null)
		{
			base.DataTable = new DataTable(base.Name);
		}
		else
		{
			base.DataTable.Rows.Clear();
			if (base.Dictionary.CacheDataSet != null)
			{
				if (base.DataTable.DataSet == null)
				{
					base.DataTable = base.DataTable;
				}
				else if (base.DataTable.DataSet != base.Dictionary.CacheDataSet)
				{
					base.DataTable.DataSet.Tables.Remove(base.DataTable);
					base.DataTable = base.DataTable;
				}
			}
		}
		if (Parameters.Count > 0)
		{
			if (StiOptions.Engine.AllowPrepareSqlQueries)
			{
				dbDataAdapter.SelectCommand.Prepare();
			}
			InvokeConnecting();
			foreach (StiDataParameter parameter in Parameters)
			{
				object obj = parameter.GetParameterValue();
				if (obj is DateTime { Ticks: 0L })
				{
					obj = DBNull.Value;
				}
				dbDataAdapter.SelectCommand.Parameters[parameter.Name].Value = obj ?? DBNull.Value;
				if ((int)((StiDataConnector)val).ConnectionIdent == 10 && parameter.Type == 121)
				{
					dbDataAdapter.SelectCommand.Parameters[parameter.Name].Direction = ParameterDirection.Output;
				}
			}
		}
		val.SetTimeout(dbDataAdapter.SelectCommand, CommandTimeout);
		StiDataAdapterHelper.Fill(base.Dictionary, dbDataAdapter, base.DataTable, schemaOnly);
		CheckColumnsIndexs();
	}

	public override void Disconnect()
	{
		base.Disconnect();
		try
		{
			if (DataAdapter is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
		catch
		{
		}
		DataAdapter = null;
	}

	public string GetSqlFilterQuery()
	{
		if (base.Dictionary?.Report == null)
		{
			return null;
		}
		StiComponentsCollection components = base.Dictionary.Report.GetComponents();
		foreach (StiComponent item in components)
		{
			if (item is StiDataBand { FilterEngine: StiFilterEngine.SQLQuery } stiDataBand && stiDataBand.Filters.Count > 0 && stiDataBand.DataSourceName == base.Name && stiDataBand.DataSource == this)
			{
				return StiDataBandSQLFilterHelper.GetFilter(stiDataBand, useDataSourceNames: false);
			}
		}
		return null;
	}

	public string GetFinalSqlCommand()
	{
		string text = "Get" + base.Name + "_SqlCommand";
		Type type = base.Dictionary.Report.GetType();
		MethodInfo method = type.GetMethod(text, new Type[2]
		{
			typeof(object),
			typeof(EventArgs)
		});
		if (method == null)
		{
			return null;
		}
		method.Invoke(base.Dictionary.Report, new object[2]
		{
			this,
			EventArgs.Empty
		});
		string sqlFilterQuery = GetSqlFilterQuery();
		if (sqlFilterQuery == null)
		{
			return SqlCommand;
		}
		if (SqlCommand != null && SqlCommand.ToLowerInvariant().Contains("where"))
		{
			return SqlCommand + " AND " + sqlFilterQuery;
		}
		return SqlCommand + " WHERE " + sqlFilterQuery;
	}

	public override StiDataSource CreateNew()
	{
		return new StiSqlSource();
	}

	public StiSqlSource()
		: this("", "", "")
	{
	}

	public StiSqlSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiSqlSource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiSqlSource(string nameInSource, string name, string alias, string sqlCommand)
		: this(nameInSource, name, alias, sqlCommand, connectOnStart: true)
	{
	}

	public StiSqlSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: this(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow: false)
	{
	}

	public StiSqlSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: this(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, 30)
	{
	}

	public StiSqlSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias)
	{
		SqlCommand = sqlCommand;
		ConnectOnStart = connectOnStart;
		ReconnectOnEachRow = reconnectOnEachRow;
		CommandTimeout = commandTimeout;
		ConnectionOrder = 2;
	}

	public StiSqlSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, key)
	{
		SqlCommand = sqlCommand;
		ConnectOnStart = connectOnStart;
		ReconnectOnEachRow = reconnectOnEachRow;
		CommandTimeout = commandTimeout;
		ConnectionOrder = 2;
	}
}
