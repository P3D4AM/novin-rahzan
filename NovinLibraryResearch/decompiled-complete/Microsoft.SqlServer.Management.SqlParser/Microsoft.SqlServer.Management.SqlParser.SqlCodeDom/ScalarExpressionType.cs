namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public enum ScalarExpressionType
{
	Unknown = 0,
	Binary = 1,
	Case = 2,
	Collate = 4,
	ColumnReference = 8,
	Literal = 0x10,
	Function = 0x20,
	GlobalVariableReference = 0x40,
	SubQuery = 0x80,
	Unary = 0x100,
	Udt = 0x200,
	VariableReference = 0x400,
	Null = 0x1000000,
	Error = 0x2000000
}
