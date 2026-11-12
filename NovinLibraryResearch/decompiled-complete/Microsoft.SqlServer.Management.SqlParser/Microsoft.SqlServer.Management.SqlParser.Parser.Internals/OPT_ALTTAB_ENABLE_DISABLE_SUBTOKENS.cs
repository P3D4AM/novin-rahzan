using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_ALTTAB_ENABLE_DISABLE_SUBTOKENS : OptLookup
{
	public OPT_ALTTAB_ENABLE_DISABLE_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.ChangeTracking,
			StringToken.FileTableNamespace
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 607, 608 };
	}
}
