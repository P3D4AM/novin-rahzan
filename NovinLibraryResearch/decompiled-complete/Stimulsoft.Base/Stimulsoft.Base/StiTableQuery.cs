namespace Stimulsoft.Base;

public class StiTableQuery
{
	private StiSqlDataConnector connector;

	private string CorrectName(string name)
	{
		string text = ((connector != null) ? connector.GetDatabaseSpecificName(name) : null);
		string text2 = ((name.Trim().IndexOfAny(new char[3] { ' ', '.', '-' }) != -1) ? text : name);
		if (!string.IsNullOrWhiteSpace(text2))
		{
			return text2;
		}
		return name;
	}

	public string GetName(string schema, string table)
	{
		if (!string.IsNullOrWhiteSpace(schema))
		{
			return CorrectName(schema) + "." + CorrectName(table);
		}
		return CorrectName(table);
	}

	public string GetSelectQuery(string table)
	{
		return GetSelectQuery(null, table);
	}

	public string GetSelectQuery(string schema, string table)
	{
		return "select * from " + GetName(schema, table);
	}

	public string GetExecuteQuery(string table)
	{
		return GetExecuteQuery(null, table);
	}

	public string GetExecuteQuery(string schema, string table)
	{
		return "execute " + GetName(schema, table);
	}

	public string GetCallQuery(string table)
	{
		return GetCallQuery(null, table);
	}

	public string GetCallQuery(string schema, string table)
	{
		return "call " + GetName(schema, table);
	}

	public string GetProcQuery(string table)
	{
		return GetProcQuery(null, table);
	}

	public string GetProcQuery(string schema, string table)
	{
		return GetName(schema, table);
	}

	public static StiTableQuery Get(StiSqlDataConnector connector)
	{
		return new StiTableQuery(connector);
	}

	public StiTableQuery(StiSqlDataConnector connector)
	{
		this.connector = connector;
	}
}
