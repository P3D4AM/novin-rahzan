namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public enum SqlDropObjectType : short
{
	Table,
	Proc,
	Procedure,
	Function,
	View,
	Rule,
	Default,
	Trigger,
	Aggregate,
	Synonym,
	Sequence,
	SecurityPolicy
}
