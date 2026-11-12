using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_OPT_STORAGE_ID_SUBTOKENS : OptLookup
{
	public OPT_OPT_STORAGE_ID_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.Sparse,
			StringToken.ColumnSet
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 564, 565 };
	}
}
