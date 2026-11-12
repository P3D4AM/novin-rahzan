namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public enum SqlModuleOptionType
{
	None,
	Recompile,
	Encryption,
	ExecuteAs,
	SchemaBinding,
	ViewMetadata,
	ReturnsNullOnNullInput,
	CalledOnNullInput,
	NativeCompilation
}
