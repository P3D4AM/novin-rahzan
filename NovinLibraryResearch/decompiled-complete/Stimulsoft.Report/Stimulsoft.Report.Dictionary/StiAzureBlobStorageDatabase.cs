using System;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiAzureBlobStorageDatabaseConverter))]
public class StiAzureBlobStorageDatabase : StiNoSqlDatabase
{
	public override StiConnectionType ConnectionType => StiConnectionType.Azure;

	public string AccountKey => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "AccountKey", (char[])null);

	public string AccountName => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "AccountName", (char[])null);

	public string ContainerName => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "ContainerName", (char[])null);

	public string BlobName => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "BlobName", (char[])null);

	public string BlobContentType => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "BlobContentType", (char[])null);

	public int CodePage => int.Parse(StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "CodePage", (char[])null) ?? "0");

	public string Delimiter => Regex.Match(base.ConnectionString, "Delimiter=\"(.*?)\";").Groups[1].Value;

	public bool FirstRowIsHeader => bool.Parse(StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "FirstRowIsHeader", (char[])null) ?? bool.TrueString);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiAzureBlobStorageDatabase;

	public override DialogResult Edit(StiDictionary dictionary, bool isNewDatabase)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return StiDataEditorsHelper.Get().AzureBlobStorageDatabaseEdit(this, dictionary, isNewDatabase);
	}

	public override async Task<DialogResult> EditAsync(StiDictionary dictionary, bool isNewDatabase)
	{
		return await StiDataEditorsHelper.Get().AzureBlobStorageDatabaseEditAsync(this, dictionary, isNewDatabase);
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiAzureBlobStorageConnector.Get(connectionString);
	}

	public override StiNoSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiAzureBlobStorageSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiAzureBlobStorageAdapterService);
	}

	public override StiDatabase CreateNew()
	{
		return new StiAzureBlobStorageDatabase();
	}

	protected override void ApplyDatabaseInformationSource(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll, DataTable dataTable, StiSqlSourceType type = StiSqlSourceType.Table)
	{
		StiNoSqlSource stiNoSqlSource = CreateDataSource(base.Name, StiNameCreation.CreateName(report, dataTable.TableName, addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true));
		stiNoSqlSource.Alias = dataTable.TableName;
		stiNoSqlSource.Query = dataTable.TableName;
		foreach (DataColumn column in dataTable.Columns)
		{
			stiNoSqlSource.Columns.Add(new StiDataColumn(column.ColumnName, column.DataType));
		}
		report.Dictionary.DataSources.Add(stiNoSqlSource);
	}

	public StiAzureBlobStorageDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiAzureBlobStorageDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiAzureBlobStorageDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiAzureBlobStorageDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiAzureBlobStorageDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
