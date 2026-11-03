using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors;
using Stimulsoft.Base.Data.Connectors.Google;
using Stimulsoft.Report.Dictionary.Adapters.Google;
using Stimulsoft.Report.Dictionary.DataSources.Google;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary.Databases.Google;

[TypeConverter(typeof(StiFirebaseDatabaseConverter))]
public class StiFirebaseDatabase : StiNoSqlDatabase
{
	public override StiConnectionType ConnectionType => StiConnectionType.Google;

	public string AuthSecret => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "AuthSecret", (char[])null);

	public string BasePath => StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "BasePath", (char[])null);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFirebaseDatabase;

	public override DialogResult Edit(StiDictionary dictionary, bool newDatabase)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return StiDataEditorsHelper.Get().FirebaseDatabaseEdit(this, dictionary, newDatabase);
	}

	public override async Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase)
	{
		return await StiDataEditorsHelper.Get().FirebaseDatabaseEditAsync(this, dictionary, newDatabase);
	}

	public override StiDatabase CreateNew()
	{
		return new StiFirebaseDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiFirebaseConnector.Get(base.ConnectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiFirebaseAdapterService);
	}

	public override StiNoSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiFirebaseSource(nameInSource, name);
	}

	public StiFirebaseDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiFirebaseDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiFirebaseDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiFirebaseDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiFirebaseDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
