using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiQuickBooksDatabaseConverter))]
public class StiQuickBooksDatabase : StiSqlDatabase
{
	public override StiConnectionType ConnectionType => StiConnectionType.OnlineServices;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiQuickBooksDatabase;

	public override DialogResult Edit(StiDictionary dictionary, bool newDatabase)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return StiDataEditorsHelper.Get().QuickBooksDatabaseEdit(this, dictionary, newDatabase);
	}

	public override async Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase)
	{
		return await StiDataEditorsHelper.Get().QuickBooksDatabaseEditAsync(this, dictionary, newDatabase);
	}

	public override StiDatabase CreateNew()
	{
		return new StiQuickBooksDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiQuickBooksConnector.Get(connectionString ?? base.ConnectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiQuickBooksAdapterService);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiQuickBooksSource(nameInSource, name);
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

	public StiQuickBooksDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiQuickBooksDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiQuickBooksDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiQuickBooksDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiQuickBooksDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
