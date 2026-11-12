namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public enum SqlConstraintType : short
{
	Null,
	NotNull,
	PrimaryKey,
	Unique,
	Identity,
	Default,
	Check,
	ForeignKey,
	RowGuidCol
}
