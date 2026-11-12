using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_SECURITY_POLICY_SUBTOKENS : OptLookup
{
	public OPT_SECURITY_POLICY_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[5]
		{
			StringToken.Filter,
			StringToken.Predicate,
			StringToken.After,
			StringToken.Block,
			StringToken.Before
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[5] { 764, 766, 768, 765, 767 };
	}
}
