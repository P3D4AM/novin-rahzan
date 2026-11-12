namespace Microsoft.Data.SqlClient;

internal enum ActiveDirectoryAuthenticationTimeoutRetryState
{
	NotStarted,
	Retrying,
	HasLoggedIn
}
