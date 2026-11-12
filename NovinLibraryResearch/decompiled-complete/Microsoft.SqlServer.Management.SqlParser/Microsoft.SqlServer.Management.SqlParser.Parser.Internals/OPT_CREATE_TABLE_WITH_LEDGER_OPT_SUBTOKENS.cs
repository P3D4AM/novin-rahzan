using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_CREATE_TABLE_WITH_LEDGER_OPT_SUBTOKENS : OptLookup
{
	public OPT_CREATE_TABLE_WITH_LEDGER_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[6]
		{
			StringToken.TransactionIdColumnName,
			StringToken.LedgerView,
			StringToken.AppendOnly,
			StringToken.OperationTypeDescColumnName,
			StringToken.OperationTypeColumnName,
			StringToken.SequenceNumberColumnName
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[6] { 745, 743, 744, 748, 747, 746 };
	}
}
