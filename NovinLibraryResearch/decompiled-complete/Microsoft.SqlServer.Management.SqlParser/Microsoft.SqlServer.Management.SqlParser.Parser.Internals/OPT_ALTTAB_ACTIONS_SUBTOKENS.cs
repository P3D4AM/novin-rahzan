using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_ALTTAB_ACTIONS_SUBTOKENS : OptLookup
{
	public OPT_ALTTAB_ACTIONS_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[4]
		{
			StringToken.Enable,
			StringToken.Rebuild,
			StringToken.Disable,
			StringToken.Switch
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[4] { 592, 593, 592, 594 };
	}
}
