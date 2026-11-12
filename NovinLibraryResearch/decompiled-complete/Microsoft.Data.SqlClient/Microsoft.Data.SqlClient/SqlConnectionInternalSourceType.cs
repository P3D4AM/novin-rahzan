namespace Microsoft.Data.SqlClient;

internal enum SqlConnectionInternalSourceType
{
	Principle,
	Failover,
	RoutingDestination
}
