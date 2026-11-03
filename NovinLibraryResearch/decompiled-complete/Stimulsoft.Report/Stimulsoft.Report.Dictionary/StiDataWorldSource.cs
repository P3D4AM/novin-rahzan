using System;
using System.ComponentModel;
using Stimulsoft.Report.Dictionary.Adapters.OnlineService;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiDataWorldSourceConverter))]
public class StiDataWorldSource : StiNoSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiDataWorldSource;

	protected override Type GetDataAdapterType()
	{
		return typeof(StiDataWorldAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiDataWorldSource();
	}

	public StiDataWorldSource()
		: this("", "", "")
	{
	}

	public StiDataWorldSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiDataWorldSource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiDataWorldSource(string nameInSource, string name, string alias, string sqlCommand)
		: base(nameInSource, name, alias, sqlCommand)
	{
	}

	public StiDataWorldSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiDataWorldSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiDataWorldSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiDataWorldSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
