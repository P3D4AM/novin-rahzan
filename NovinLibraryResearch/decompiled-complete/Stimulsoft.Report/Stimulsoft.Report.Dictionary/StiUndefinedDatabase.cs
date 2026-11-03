namespace Stimulsoft.Report.Dictionary;

public class StiUndefinedDatabase : StiSqlDatabase
{
	public override string ServiceName => "Undefined SqlDatabase";

	public override StiDatabase CreateNew()
	{
		return new StiUndefinedDatabase();
	}

	public StiUndefinedDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiUndefinedDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiUndefinedDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiUndefinedDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
