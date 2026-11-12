namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

public enum ParseContextState
{
	Initial,
	Parsing,
	ParseCompleted,
	Binding,
	BindCompleted
}
