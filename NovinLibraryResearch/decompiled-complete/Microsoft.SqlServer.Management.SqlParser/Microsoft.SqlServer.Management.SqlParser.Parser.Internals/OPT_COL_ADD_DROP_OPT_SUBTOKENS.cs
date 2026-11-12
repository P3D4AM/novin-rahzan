using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_COL_ADD_DROP_OPT_SUBTOKENS : OptLookup
{
	public OPT_COL_ADD_DROP_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[4]
		{
			StringToken.Persisted,
			StringToken.Sparse,
			StringToken.Masked,
			StringToken.Hidden
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[4] { 558, 559, 560, 561 };
	}
}
