using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_GRAPH_ID_OR_FILETABLE_SUBTOKENS : OptLookup
{
	public OPT_GRAPH_ID_OR_FILETABLE_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[3]
		{
			StringToken.Filetable,
			StringToken.Edge,
			StringToken.Node
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[3] { 915, 914, 913 };
	}
}
