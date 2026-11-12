namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlSetStatement : SqlStatement
{
	internal sealed override string StatementPhrase => "SET";
}
