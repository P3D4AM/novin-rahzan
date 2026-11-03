using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiMongoDbSourceConverter))]
public class StiMongoDbSource : StiNoSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiMongoDbSource;

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		throw new NotImplementedException();
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiMongoDbAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiMongoDbSource();
	}

	public StiMongoDbSource()
		: this("", "", "")
	{
	}

	public StiMongoDbSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiMongoDbSource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiMongoDbSource(string nameInSource, string name, string alias, string sqlCommand)
		: base(nameInSource, name, alias, sqlCommand)
	{
	}

	public StiMongoDbSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiMongoDbSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiMongoDbSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiMongoDbSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
