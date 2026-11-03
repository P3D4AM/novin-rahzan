using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public abstract class StiNoSqlDatabase : StiDatabase
{
	public override StiConnectionType ConnectionType => StiConnectionType.NoSql;

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

	protected virtual string DataAdapterType => GetDataAdapterType().ToString();

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

	public virtual StiNoSqlDataConnector CreateNoSqlConnector(string connectionString = null)
	{
		StiDataConnector obj = CreateConnector(connectionString);
		return (StiNoSqlDataConnector)(object)((obj is StiNoSqlDataConnector) ? obj : null);
	}

	public override void RegData(StiDictionary dictionary, bool loadData)
	{
		StiNoSqlAdapterService dataAdapter = GetDataAdapter();
		if (dataAdapter == null)
		{
			throw new StiException($"A data adapter for the database {((object)this).GetType()} not found");
		}
		dataAdapter.CreateConnectionInDataStore(dictionary, this);
	}

	protected abstract Type GetDataAdapterType();

	public virtual StiNoSqlAdapterService GetDataAdapter()
	{
		Type dataAdapterType = GetDataAdapterType();
		if (dataAdapterType != null)
		{
			return StiActivator.CreateObject(dataAdapterType) as StiNoSqlAdapterService;
		}
		StiDataAdapterService stiDataAdapterService = StiOptions.Services.DataAdapters.Where((StiDataAdapterService s) => ((StiService)s).ServiceEnabled).FirstOrDefault((StiDataAdapterService d) => ((object)d).GetType().ToString() == DataAdapterType);
		if (stiDataAdapterService != null)
		{
			return stiDataAdapterService as StiNoSqlAdapterService;
		}
		throw new Exception($"Data adapter for the {this} database is not found!");
	}

	public override void ApplyDatabaseInformation(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll)
	{
		foreach (DataTable table in information.Tables)
		{
			ApplyDatabaseInformationSource(information, report, informationAll, table);
		}
	}

	protected virtual void ApplyDatabaseInformationSource(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll, DataTable dataTable, StiSqlSourceType type = StiSqlSourceType.Table)
	{
		StiNoSqlSource stiNoSqlSource = CreateDataSource(base.Name, StiNameCreation.CreateName(report, dataTable.TableName, addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true));
		stiNoSqlSource.Alias = dataTable.TableName;
		foreach (DataColumn column in dataTable.Columns)
		{
			stiNoSqlSource.Columns.Add(new StiDataColumn(column.ColumnName, column.DataType));
		}
		report.Dictionary.DataSources.Add(stiNoSqlSource);
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
			StiNoSqlDataConnector val = CreateNoSqlConnector(ConnectionString);
			StiDataSchema val2 = val.RetrieveSchema(base.AllowException);
			if (val2 == null)
			{
				return stiDatabaseInformation;
			}
			stiDatabaseInformation.Tables.AddRange(GetDatabaseInformationTables(val2));
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

	public abstract StiNoSqlSource CreateDataSource(string nameInSource, string name);

	public virtual string GetConnectionStringHelper()
	{
		return null;
	}

	public override DialogResult Edit(StiDictionary dictionary, bool newDatabase)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return StiDataEditorsHelper.Get().NoSqlDatabaseEdit(this, dictionary, newDatabase);
	}

	public override async Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase)
	{
		return await StiDataEditorsHelper.Get().NoSqlDatabaseEditAsync(this, dictionary, newDatabase);
	}

	public virtual string EditConnectionString(string connectionString)
	{
		return StiDatabaseAssembly.EditConnectionString(this, connectionString, GetConnectionStringHelper());
	}

	public virtual string MapUserNameAndPassword(string userName, string password)
	{
		return "User Id = " + userName + "; Password = " + password;
	}

	public StiNoSqlDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiNoSqlDatabase(string name, string connectionString)
		: base(name)
	{
		ConnectionString = connectionString;
	}

	public StiNoSqlDatabase(string name, string alias, string connectionString)
		: base(name, alias)
	{
		ConnectionString = connectionString;
	}

	public StiNoSqlDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias)
	{
		ConnectionString = connectionString;
		PromptUserNameAndPassword = promptUserNameAndpassword;
	}

	public StiNoSqlDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, key)
	{
		ConnectionString = connectionString;
		PromptUserNameAndPassword = promptUserNameAndpassword;
	}
}
