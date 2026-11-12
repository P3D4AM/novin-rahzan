using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_CREATE_TAB_OPT_SUBTOKENS : OptLookup
{
	public OPT_CREATE_TAB_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[3]
		{
			StringToken.Federated,
			StringToken.TextImageOn,
			StringToken.FileStreamOn
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[3] { 697, 696, 695 };
	}
}
