using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_LOAD_OPT_SUBTOKENS : OptLookup
{
	public OPT_LOAD_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), 530)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[1] { StringToken.FileStream };
	}

	private static int[] GetTokenIds()
	{
		return new int[1] { 529 };
	}
}
