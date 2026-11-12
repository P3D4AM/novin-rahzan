namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlExecuteStatement : SqlStatement
{
	internal sealed override string StatementPhrase => "EXECUTE";
}
