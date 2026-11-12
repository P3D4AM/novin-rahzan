using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[Flags]
public enum SqlModuleOptionMask
{
	None = 0,
	Recompile = 2,
	Encryption = 4,
	ExecuteAs = 8,
	SchemaBinding = 0x10,
	ViewMetadata = 0x20,
	ReturnsNullOnNullInput = 0x40,
	CalledOnNullInput = 0x80,
	NativeCompilation = 0x100,
	Invalid = 0x8000,
	NullInputOptions = 0xC0,
	CreateProcedure = 0x11E,
	CreateTrigger = 0xC,
	CreateView = 0x34,
	ExecuteModule = 2
}
