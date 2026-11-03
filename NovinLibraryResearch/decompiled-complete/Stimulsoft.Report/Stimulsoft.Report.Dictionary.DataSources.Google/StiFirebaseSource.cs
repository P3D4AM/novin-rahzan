using System;
using System.ComponentModel;
using Stimulsoft.Report.Dictionary.Adapters.Google;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary.DataSources.Google;

[TypeConverter(typeof(StiFirebaseSourceConverter))]
public class StiFirebaseSource : StiNoSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiFirebaseDatabase;

	protected override Type GetDataAdapterType()
	{
		return typeof(StiFirebaseAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiFirebaseSource();
	}

	public StiFirebaseSource()
		: base("", "", "", null)
	{
	}

	public StiFirebaseSource(string nameInSource, string name)
		: base(nameInSource, name)
	{
	}

	public StiFirebaseSource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiFirebaseSource(string nameInSource, string name, string alias, string sqlCommand)
		: base(nameInSource, name, alias, sqlCommand)
	{
	}

	public StiFirebaseSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiFirebaseSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiFirebaseSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiFirebaseSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
