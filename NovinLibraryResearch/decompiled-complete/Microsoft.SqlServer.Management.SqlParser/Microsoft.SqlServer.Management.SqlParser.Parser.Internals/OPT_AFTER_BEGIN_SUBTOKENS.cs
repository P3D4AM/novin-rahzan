using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_AFTER_BEGIN_SUBTOKENS : OptLookup
{
	public OPT_AFTER_BEGIN_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), 173)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[1] { StringToken.Atomic };
	}

	private static int[] GetTokenIds()
	{
		return new int[1] { 688 };
	}
}
