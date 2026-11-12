using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[Flags]
public enum SelectExpressionType
{
	Unknown = 0,
	Scalar = 1,
	Star = 2,
	VariableAssignment = 4,
	Error = 0x8000000
}
