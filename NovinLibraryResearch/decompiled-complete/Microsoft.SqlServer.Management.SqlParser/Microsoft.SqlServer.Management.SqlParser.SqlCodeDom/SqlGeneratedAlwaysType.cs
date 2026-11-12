namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public enum SqlGeneratedAlwaysType
{
	None,
	AsRowStart,
	AsRowEnd,
	AsTransactionIdStart,
	AsTransactionIdEnd,
	AsSequenceNumberStart,
	AsSequenceNumberEnd
}
