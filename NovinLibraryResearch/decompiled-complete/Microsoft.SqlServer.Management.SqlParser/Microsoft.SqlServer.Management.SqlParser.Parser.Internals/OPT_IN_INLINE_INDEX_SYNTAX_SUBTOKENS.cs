using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_IN_INLINE_INDEX_SYNTAX_SUBTOKENS : OptLookup
{
	public OPT_IN_INLINE_INDEX_SYNTAX_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[3]
		{
			StringToken.ColumnStore,
			StringToken.FileStreamOn,
			StringToken.Hash
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[3] { 760, 761, 759 };
	}
}
