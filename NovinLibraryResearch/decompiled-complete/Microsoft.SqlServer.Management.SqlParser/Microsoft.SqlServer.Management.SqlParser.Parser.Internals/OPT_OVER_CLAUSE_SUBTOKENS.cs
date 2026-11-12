using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_OVER_CLAUSE_SUBTOKENS : OptLookup
{
	public OPT_OVER_CLAUSE_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.Partition,
			StringToken.Unbounded
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 684, 685 };
	}
}
