namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal class DatabaseContext
{
	public readonly LexicalScope LexicalScope;

	public readonly DatabaseEx Database;

	public DatabaseContext(LexicalScope lexicalScope, DatabaseEx database)
	{
		LexicalScope = lexicalScope;
		Database = database;
	}
}
