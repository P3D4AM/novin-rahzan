using System;
using System.ComponentModel;

namespace Stimulsoft.Report.Dictionary;

[Obsolete("Please use StiOracleDatabase class instead StiOracleODPDatabase!")]
[EditorBrowsable(EditorBrowsableState.Never)]
public class StiOracleODPDatabase : StiOracleDatabase
{
	public override StiDatabase CreateNew()
	{
		return new StiOracleODPDatabase();
	}

	public StiOracleODPDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiOracleODPDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiOracleODPDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiOracleODPDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiOracleODPDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
