using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors;
using Stimulsoft.Base.Data.Connectors.Google;
using Stimulsoft.Report.Dictionary.Adapters.Google;
using Stimulsoft.Report.Dictionary.DataSources.Google;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary.Databases.Google;

[TypeConverter(typeof(StiBigQueryDatabaseConverter))]
public class StiBigQueryDatabase : StiNoSqlDatabase
{
	public override StiConnectionType ConnectionType => StiConnectionType.Google;

	public string Base64EncodedAuthSecret => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "Base64EncodedAuthSecret", (char[])null);

	public string ProjectId => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "ProjectId", (char[])null);

	public string DatasetId => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "DatasetId", (char[])null);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiBigQueryDatabase;

	public override DialogResult Edit(StiDictionary dictionary, bool isNewDatabase)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return StiDataEditorsHelper.Get().BigQueryDatabaseEdit(this, dictionary, isNewDatabase);
	}

	public override async Task<DialogResult> EditAsync(StiDictionary dictionary, bool isNewDatabase)
	{
		return await StiDataEditorsHelper.Get().BigQueryDatabaseEditAsync(this, dictionary, isNewDatabase);
	}

	public override StiDatabase CreateNew()
	{
		return new StiBigQueryDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiBigQueryConnector.Get(base.ConnectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiBigQueryAdapterService);
	}

	public override StiNoSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiBigQuerySource(nameInSource, name);
	}

	protected override void ApplyDatabaseInformationSource(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll, DataTable dataTable, StiSqlSourceType type = StiSqlSourceType.Table)
	{
		StiNoSqlSource stiNoSqlSource = CreateDataSource(base.Name, StiNameCreation.CreateName(report, dataTable.TableName, addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true));
		stiNoSqlSource.Alias = dataTable.TableName;
		stiNoSqlSource.Query = "select * from `" + dataTable.TableName + "`";
		foreach (DataColumn column in dataTable.Columns)
		{
			stiNoSqlSource.Columns.Add(new StiDataColumn(column.ColumnName, column.DataType));
		}
		report.Dictionary.DataSources.Add(stiNoSqlSource);
	}

	public StiBigQueryDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiBigQueryDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiBigQueryDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiBigQueryDatabase(string name, string alias, string connectionString, bool promptUserNameAndPassword)
		: base(name, alias, connectionString, promptUserNameAndPassword)
	{
	}

	public StiBigQueryDatabase(string name, string alias, string connectionString, bool promptUserNameAndPassword, string key)
		: base(name, alias, connectionString, promptUserNameAndPassword, key)
	{
	}
}
