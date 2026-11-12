using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_COL_DEF_OPT_SUBTOKENS : OptLookup
{
	public OPT_COL_DEF_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[7]
		{
			StringToken.Masked,
			StringToken.Encrypted,
			StringToken.Generated,
			StringToken.Sparse,
			StringToken.Hidden,
			StringToken.ColumnSet,
			StringToken.FileStream
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[7] { 554, 552, 551, 549, 555, 550, 553 };
	}
}
