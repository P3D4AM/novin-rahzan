using System;
using System.ComponentModel;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiQuickBooksSourceConverter))]
public class StiQuickBooksSource : StiSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiQuickBooksSource;

	protected override Type GetDataAdapterType()
	{
		return typeof(StiQuickBooksAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiQuickBooksSource();
	}

	public StiQuickBooksSource()
		: this("", "", "")
	{
	}

	public StiQuickBooksSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiQuickBooksSource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiQuickBooksSource(string nameInSource, string name, string alias, string sqlCommand)
		: base(nameInSource, name, alias, sqlCommand)
	{
	}

	public StiQuickBooksSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiQuickBooksSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiQuickBooksSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiQuickBooksSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
