using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_IND_ACTIONS_SUBTOKENS : OptLookup
{
	public OPT_IND_ACTIONS_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[6]
		{
			StringToken.Pause,
			StringToken.Abort,
			StringToken.Reorganize,
			StringToken.Rebuild,
			StringToken.Resume,
			StringToken.Disable
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[6] { 589, 588, 585, 584, 587, 586 };
	}
}
