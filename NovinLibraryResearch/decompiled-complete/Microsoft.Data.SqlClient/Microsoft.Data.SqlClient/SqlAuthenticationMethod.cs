namespace Microsoft.Data.SqlClient;

public enum SqlAuthenticationMethod
{
	NotSpecified,
	SqlPassword,
	ActiveDirectoryPassword,
	ActiveDirectoryIntegrated,
	ActiveDirectoryInteractive,
	ActiveDirectoryServicePrincipal,
	ActiveDirectoryDeviceCodeFlow,
	ActiveDirectoryManagedIdentity,
	ActiveDirectoryMSI,
	ActiveDirectoryDefault
}
