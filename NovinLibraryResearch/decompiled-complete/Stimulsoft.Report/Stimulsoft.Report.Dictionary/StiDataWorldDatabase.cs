using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors;
using Stimulsoft.Base.Data.Connectors.OnlineServices;
using Stimulsoft.Report.Dictionary.Adapters.OnlineService;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiDataWorldDatabaseConverter))]
public class StiDataWorldDatabase : StiNoSqlDatabase
{
	public override StiConnectionType ConnectionType => StiConnectionType.OnlineServices;

	public string Owner => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "Owner", (char[])null);

	public string Token => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "Token", (char[])null);

	public string Database => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "Database", (char[])null);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiDataWorldDatabase;

	public override DialogResult Edit(StiDictionary dictionary, bool newDatabase)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return StiDataEditorsHelper.Get().DataWorldDatabaseEdit(this, dictionary, newDatabase);
	}

	public override async Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase)
	{
		return await StiDataEditorsHelper.Get().DataWorldDatabaseEditAsync(this, dictionary, newDatabase);
	}

	public override StiDatabase CreateNew()
	{
		return new StiDataWorldDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiDataWorldConnector.Get(connectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiDataWorldAdapterService);
	}

	public override StiNoSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiDataWorldSource(nameInSource, name);
	}

	public StiDataWorldDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiDataWorldDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiDataWorldDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiDataWorldDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiDataWorldDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
