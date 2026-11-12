namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlIndexOption : SqlCodeObject
{
	private readonly SqlIndexOptionType type;

	internal abstract string Phrase { get; }

	public SqlIndexOptionType Type => type;

	internal SqlIndexOption(SqlIndexOptionType type)
	{
		this.type = type;
	}
}
