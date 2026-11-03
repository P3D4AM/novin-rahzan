using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlSourceConverter))]
public class StiSQLiteSource : StiSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiSQLiteSource;

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiSqLiteConnector.Get(connectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiSQLiteAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiSQLiteSource();
	}

	public StiSQLiteSource()
		: this("", "", "")
	{
	}

	public StiSQLiteSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiSQLiteSource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiSQLiteSource(string nameInSource, string name, string alias, string sqlCommand)
		: base(nameInSource, name, alias, sqlCommand)
	{
	}

	public StiSQLiteSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiSQLiteSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiSQLiteSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiSQLiteSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
