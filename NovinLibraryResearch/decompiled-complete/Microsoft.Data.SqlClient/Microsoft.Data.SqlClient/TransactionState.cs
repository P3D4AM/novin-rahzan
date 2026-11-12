namespace Microsoft.Data.SqlClient;

internal enum TransactionState
{
	Pending,
	Active,
	Aborted,
	Committed,
	Unknown
}
