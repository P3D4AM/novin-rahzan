namespace Stimulsoft.Report.Dictionary;

public class StiUndefinedDataSource : StiSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiUndefinedDataSource;

	public override StiDataSource CreateNew()
	{
		return new StiUndefinedDataSource();
	}

	public StiUndefinedDataSource()
		: this("", "", "")
	{
	}

	public StiUndefinedDataSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiUndefinedDataSource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiUndefinedDataSource(string nameInSource, string name, string alias, string sqlCommand)
		: this(nameInSource, name, alias, sqlCommand, connectOnStart: true)
	{
	}

	public StiUndefinedDataSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: this(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow: false)
	{
	}

	public StiUndefinedDataSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: this(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, 30)
	{
	}

	public StiUndefinedDataSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiUndefinedDataSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
