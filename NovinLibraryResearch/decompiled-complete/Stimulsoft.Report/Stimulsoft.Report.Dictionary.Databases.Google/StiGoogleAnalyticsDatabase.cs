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

[TypeConverter(typeof(StiGoogleAnalyticsDatabaseConverter))]
public class StiGoogleAnalyticsDatabase : StiNoSqlDatabase
{
	public override StiConnectionType ConnectionType => StiConnectionType.Google;

	public string Base64EncodedAuthSecret => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "Base64EncodedAuthSecret", (char[])null);

	public string AccountId => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "AccountId", (char[])null);

	public string PropertyId => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "PropertyId", (char[])null);

	public string ViewId => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "ViewId", (char[])null);

	public string Metrics => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "Metrics", new char[1] { ';' });

	public string Dimensions => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "Dimensions", new char[1] { ';' });

	public string StartDate => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "StartDate", (char[])null);

	public string EndDate => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "EndDate", (char[])null);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiGoogleAnalyticsDatabase;

	public override DialogResult Edit(StiDictionary dictionary, bool isNewDatabase)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return StiDataEditorsHelper.Get().GoogleAnalyticsDatabaseEdit(this, dictionary, isNewDatabase);
	}

	public override async Task<DialogResult> EditAsync(StiDictionary dictionary, bool isNewDatabase)
	{
		return await StiDataEditorsHelper.Get().GoogleAnalyticsDatabaseEditAsync(this, dictionary, isNewDatabase);
	}

	public override StiDatabase CreateNew()
	{
		return new StiGoogleAnalyticsDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiGoogleAnalyticsConnector.Get(base.ConnectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiGoogleAnalyticsAdapterService);
	}

	public override StiNoSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiGoogleAnalyticsSource(nameInSource, name);
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

	public StiGoogleAnalyticsDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiGoogleAnalyticsDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiGoogleAnalyticsDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiGoogleAnalyticsDatabase(string name, string alias, string connectionString, bool promptUserNameAndPassword)
		: base(name, alias, connectionString, promptUserNameAndPassword)
	{
	}

	public StiGoogleAnalyticsDatabase(string name, string alias, string connectionString, bool promptUserNameAndPassword, string key)
		: base(name, alias, connectionString, promptUserNameAndPassword, key)
	{
	}
}
