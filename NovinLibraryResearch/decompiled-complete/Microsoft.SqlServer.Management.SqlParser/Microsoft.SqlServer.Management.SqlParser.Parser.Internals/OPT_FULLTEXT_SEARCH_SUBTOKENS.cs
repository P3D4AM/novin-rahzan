using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_FULLTEXT_SEARCH_SUBTOKENS : OptLookup
{
	public OPT_FULLTEXT_SEARCH_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[6]
		{
			StringToken.Population,
			StringToken.Catalog,
			StringToken.Search,
			StringToken.No,
			StringToken.ChangeTracking,
			StringToken.StopList
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[6] { 630, 628, 633, 632, 629, 631 };
	}
}
