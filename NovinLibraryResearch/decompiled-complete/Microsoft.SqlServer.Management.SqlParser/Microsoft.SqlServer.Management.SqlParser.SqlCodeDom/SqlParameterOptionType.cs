using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[Flags]
public enum SqlParameterOptionType
{
	None = 0,
	Output = 1,
	ReadOnly = 2,
	Invalid = 0x8000,
	FunctionValidOptions = 2,
	ProcedureValidOptions = 3
}
