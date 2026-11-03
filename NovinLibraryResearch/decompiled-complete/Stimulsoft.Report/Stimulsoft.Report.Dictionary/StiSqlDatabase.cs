using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiSqlDatabase : StiDatabase
{
	private const string EncryptedId = "8pTP5X15uKADcSw7";

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiSqlDatabase;

	protected virtual string DataAdapterType => GetDataAdapterType().ToString();

	public override StiConnectionType ConnectionType => StiConnectionType.Sql;

	[StiOrder(300)]
	[Description("Gets or sets a connection string which contains SQL connection parameters.")]
	public string ConnectionString { get; set; }

	[StiSerializable]
	[Browsable(false)]
	public string ConnectionStringEncrypted
	{
		get
		{
			return StiEncryption.Encrypt(ConnectionString, "8pTP5X15uKADcSw7");
		}
		set
		{
			ConnectionString = StiEncryption.Decrypt(value, "8pTP5X15uKADcSw7");
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(false)]
	[StiOrder(700)]
	[Description("Gets or sets a value which indicates that UserName and Password parameters should be requested from user.")]
	public bool PromptUserNameAndPassword { get; set; }

	[Browsable(false)]
	public virtual bool CanEditConnectionString => StiDatabaseAssembly.CanEditConnectionString(GetConnectionStringHelper());

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.ConnectionString()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ConnectionStringEncrypted", ConnectionStringEncrypted);
		JsonExtensions.AddPropertyBool(val, "PromptUserNameAndPassword", PromptUserNameAndPassword, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "ConnectionStringEncrypted"))
			{
				if (name == "PromptUserNameAndPassword")
				{
					PromptUserNameAndPassword = StiJsonReportObjectHelper.DeserializeBool(item);
				}
			}
			else
			{
				ConnectionStringEncrypted = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public override StiDatabase CreateNew()
	{
		return new StiSqlDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiMsSqlConnector.Get(connectionString);
	}

	public StiSqlDataConnector CreateSqlConnector(string connectionString = null)
	{
		StiDataConnector obj = CreateConnector(connectionString);
		return (StiSqlDataConnector)(object)((obj is StiSqlDataConnector) ? obj : null);
	}

	public virtual StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiSqlSource(nameInSource, name);
	}

	public virtual StiSqlAdapterService GetDataAdapter()
	{
		Type dataAdapterType = GetDataAdapterType();
		if (dataAdapterType != null)
		{
			return StiActivator.CreateObject(dataAdapterType) as StiSqlAdapterService;
		}
		StiDataAdapterService stiDataAdapterService = StiOptions.Services.DataAdapters.Where((StiDataAdapterService s) => ((StiService)s).ServiceEnabled).FirstOrDefault((StiDataAdapterService d) => ((object)d).GetType().ToString() == DataAdapterType);
		if (stiDataAdapterService != null)
		{
			return stiDataAdapterService as StiSqlAdapterService;
		}
		throw new Exception($"Data adapter for the {((object)this).GetType()} database is not found!");
	}

	protected virtual Type GetDataAdapterType()
	{
		return typeof(StiSqlAdapterService);
	}

	public override void RegData(StiDictionary dictionary, bool loadData)
	{
		StiSqlAdapterService dataAdapter = GetDataAdapter();
		if (dataAdapter == null)
		{
			throw new Exception($"Database {((object)this).GetType()} not found");
		}
		dataAdapter.CreateConnectionInDataStore(dictionary, this);
	}

	public virtual string GetConnectionStringHelper()
	{
		return "StiSqlConnectionHelper";
	}

	public override DialogResult Edit(StiDictionary dictionary, bool newDatabase)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return StiDataEditorsHelper.Get().SqlDatabaseEdit(this, dictionary, newDatabase);
	}

	public override async Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase)
	{
		return await StiDataEditorsHelper.Get().SqlDatabaseEditAsync(this, dictionary, newDatabase);
	}

	public virtual string EditConnectionString(string connectionString)
	{
		return StiDatabaseAssembly.EditConnectionString(this, connectionString, GetConnectionStringHelper());
	}

	public virtual string MapUserNameAndPassword(string userName, string password)
	{
		return "User Id = " + userName + "; Password = " + password;
	}

	public override void ApplyDatabaseInformation(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll)
	{
		ApplyDatabaseInformationTables(information, report, informationAll);
		ApplyDatabaseInformationViews(information, report, informationAll);
		ApplyDatabaseInformationProcedures(information, report, informationAll);
	}

	protected virtual void ApplyDatabaseInformationTables(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll)
	{
		foreach (DataTable table in information.Tables)
		{
			ApplyDatabaseInformationSource(information, report, informationAll, table);
		}
	}

	protected virtual void ApplyDatabaseInformationViews(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll)
	{
		foreach (DataTable view in information.Views)
		{
			ApplyDatabaseInformationSource(information, report, informationAll, view);
		}
	}

	protected virtual void ApplyDatabaseInformationProcedures(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll)
	{
		foreach (DataTable storedProcedure in information.StoredProcedures)
		{
			ApplyDatabaseInformationSource(information, report, informationAll, storedProcedure, StiSqlSourceType.StoredProcedure);
		}
	}

	protected virtual void ApplyDatabaseInformationSource(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll, DataTable dataTable, StiSqlSourceType type = StiSqlSourceType.Table)
	{
		StiSqlDataConnector val = CreateSqlConnector(ConnectionString);
		StiSqlSource stiSqlSource = CreateDataSource(base.Name, StiNameCreation.CreateName(report, dataTable.TableName, addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true));
		stiSqlSource.AllowExpressions = !(this is StiOdbcDatabase) || type != StiSqlSourceType.StoredProcedure;
		string sqlCommand;
		if (dataTable.ExtendedProperties["Query"] == null)
		{
			sqlCommand = "select * from " + dataTable.TableName;
		}
		else
		{
			string text2 = (stiSqlSource.SqlCommand = dataTable.ExtendedProperties["Query"] as string);
			sqlCommand = text2;
		}
		stiSqlSource.SqlCommand = sqlCommand;
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataColumn column in dataTable.Columns)
		{
			stringBuilder = ((stringBuilder.Length != 0) ? stringBuilder.AppendFormat(", {0}", column.ColumnName) : stringBuilder.Append(column.ColumnName));
			if (column.Caption == "Parameters")
			{
				stiSqlSource.Parameters.Add(new StiDataParameter
				{
					Name = column.ColumnName,
					Type = val.GetSqlType(column.DataType)
				});
			}
			else
			{
				stiSqlSource.Columns.Add(new StiDataColumn(column.ColumnName, column.DataType));
			}
		}
		stiSqlSource.Type = type;
		DataTable dataTable2 = informationAll?.Tables.FirstOrDefault((DataTable t) => t.TableName == dataTable.TableName);
		if (dataTable2 != null && dataTable.Columns.Count != dataTable2.Columns.Count && stiSqlSource.SqlCommand.Contains("*") && stringBuilder.Length > 0)
		{
			stiSqlSource.SqlCommand = stiSqlSource.SqlCommand.Replace("*", stringBuilder.ToString());
		}
		report.Dictionary.DataSources.Add(stiSqlSource);
	}

	public override void ApplyDatabaseInformation(StiDatabaseInformation information, StiReport report)
	{
		ApplyDatabaseInformation(information, report, null);
	}

	public override StiDatabaseInformation GetDatabaseInformation(StiReport report)
	{
		try
		{
			StiDatabaseInformation stiDatabaseInformation = new StiDatabaseInformation();
			StiSqlDataConnector connector = CreateSqlConnector(ConnectionString);
			StiDataSchema val = StiDataLeader.RetrieveSchema(connector, base.AllowException);
			if (val == null)
			{
				return stiDatabaseInformation;
			}
			stiDatabaseInformation.Tables.AddRange(GetDatabaseInformationTables(val));
			stiDatabaseInformation.Views.AddRange(GetDatabaseInformationViews(val));
			stiDatabaseInformation.StoredProcedures.AddRange(GetDatabaseInformationProcedures(val));
			return stiDatabaseInformation;
		}
		catch (Exception)
		{
			if (base.AllowException)
			{
				throw;
			}
			return null;
		}
	}

	protected virtual List<DataTable> GetDatabaseInformationTables(StiDataSchema dataSchema)
	{
		List<DataTable> list = new List<DataTable>();
		foreach (StiDataTableSchema table2 in dataSchema.Tables)
		{
			try
			{
				DataTable table = new DataTable(((StiObjectSchema)table2).Name);
				table2.Columns.ForEach(delegate(StiDataColumnSchema schemaColumn)
				{
					if (table.Columns[((StiObjectSchema)schemaColumn).Name] == null)
					{
						table.Columns.Add(((StiObjectSchema)schemaColumn).Name, schemaColumn.Type);
					}
				});
				if (!string.IsNullOrWhiteSpace(table2.Query))
				{
					table.ExtendedProperties["Query"] = table2.Query;
				}
				list.Add(table);
			}
			catch
			{
			}
		}
		return list;
	}

	protected virtual List<DataTable> GetDatabaseInformationViews(StiDataSchema dataSchema)
	{
		List<DataTable> list = new List<DataTable>();
		foreach (StiDataTableSchema view2 in dataSchema.Views)
		{
			try
			{
				DataTable view = new DataTable(((StiObjectSchema)view2).Name);
				view2.Columns.ForEach(delegate(StiDataColumnSchema schemaColumn)
				{
					if (view.Columns[((StiObjectSchema)schemaColumn).Name] == null)
					{
						view.Columns.Add(((StiObjectSchema)schemaColumn).Name, schemaColumn.Type);
					}
				});
				if (!string.IsNullOrWhiteSpace(view2.Query))
				{
					view.ExtendedProperties["Query"] = view2.Query;
				}
				list.Add(view);
			}
			catch
			{
			}
		}
		return list;
	}

	protected virtual List<DataTable> GetDatabaseInformationProcedures(StiDataSchema dataSchema)
	{
		List<DataTable> list = new List<DataTable>();
		foreach (StiDataTableSchema storedProcedure in dataSchema.StoredProcedures)
		{
			try
			{
				DataTable proc = new DataTable(((StiObjectSchema)storedProcedure).Name);
				storedProcedure.Columns.ForEach(delegate(StiDataColumnSchema schemaColumn)
				{
					if (proc.Columns[((StiObjectSchema)schemaColumn).Name] == null)
					{
						proc.Columns.Add(new DataColumn(((StiObjectSchema)schemaColumn).Name, schemaColumn.Type)
						{
							Caption = "Columns"
						});
					}
				});
				storedProcedure.Parameters.ForEach(delegate(StiDataParameterSchema schemaParameter)
				{
					if (proc.Columns[((StiObjectSchema)schemaParameter).Name] == null)
					{
						proc.Columns.Add(new DataColumn(((StiObjectSchema)schemaParameter).Name, schemaParameter.Type)
						{
							Caption = "Parameters"
						});
					}
				});
				if (!string.IsNullOrWhiteSpace(storedProcedure.Query))
				{
					proc.ExtendedProperties["Query"] = storedProcedure.Query;
				}
				list.Add(proc);
			}
			catch
			{
			}
		}
		return list;
	}

	public StiSqlDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiSqlDatabase(string name, string connectionString)
		: base(name)
	{
		ConnectionString = connectionString;
	}

	public StiSqlDatabase(string name, string alias, string connectionString)
		: base(name, alias)
	{
		ConnectionString = connectionString;
	}

	public StiSqlDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias)
	{
		ConnectionString = connectionString;
		PromptUserNameAndPassword = promptUserNameAndpassword;
	}

	public StiSqlDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, key)
	{
		ConnectionString = connectionString;
		PromptUserNameAndPassword = promptUserNameAndpassword;
	}
}
