using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_DUMP_OPT_SUBTOKENS : OptLookup
{
	public OPT_DUMP_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), 520)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[1] { StringToken.Encryption };
	}

	private static int[] GetTokenIds()
	{
		return new int[1] { 519 };
	}
}
