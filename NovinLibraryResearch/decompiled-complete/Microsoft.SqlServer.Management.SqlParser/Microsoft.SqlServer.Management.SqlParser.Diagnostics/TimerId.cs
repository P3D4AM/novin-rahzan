namespace Microsoft.SqlServer.Management.SqlParser.Diagnostics;

public enum TimerId : byte
{
	None,
	Root,
	Parse,
	Bind,
	Lexer,
	RangeMin_Refresh,
	Refresh_Server,
	Refresh_Database,
	Refresh_Schema,
	Refresh_Table,
	RangeMax_Refresh,
	Serialize_CodeSerialize,
	Serialize_Driver,
	Serialize_MetadataGen,
	Serialize_TypeCompaction,
	Serialize_CodeDomGen
}
