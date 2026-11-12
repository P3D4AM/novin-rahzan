using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_TAB_LOCK_OPT_SUBTOKENS : OptLookup
{
	public OPT_TAB_LOCK_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.MaxDuration,
			StringToken.AbortAfterWait
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 541, 542 };
	}
}
