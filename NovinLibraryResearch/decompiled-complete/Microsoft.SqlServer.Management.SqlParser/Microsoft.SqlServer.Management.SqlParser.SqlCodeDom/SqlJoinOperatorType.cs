namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public enum SqlJoinOperatorType
{
	CrossJoin,
	CrossApply,
	OuterApply,
	InnerJoin,
	LeftOuterJoin,
	RightOuterJoin,
	FullOuterJoin
}
