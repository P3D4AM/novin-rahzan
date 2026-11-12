using System;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

[Flags]
public enum SqlEngineEdition
{
	None = 0,
	PersonalOrDesktopEngine = 1,
	Standard = 2,
	Enterprise = 4,
	Express = 8,
	AzureDatabase = 0x10,
	DataWarehouse = 0x20,
	StretchDatabase = 0x40,
	ManagedInstance = 0x80,
	Azure = 0xF0,
	OnPremises = 0xF
}
