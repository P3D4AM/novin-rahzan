using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_SYSTEM_VERSIONING_OPT_SUBTOKENS : OptLookup
{
	public OPT_SYSTEM_VERSIONING_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[3]
		{
			StringToken.HistoryRetentionPeriod,
			StringToken.DataConsistencyCheck,
			StringToken.HistoryTable
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[3] { 740, 739, 738 };
	}
}
