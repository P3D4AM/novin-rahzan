using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_AFTER_FTS_WITH_SUBTOKENS : OptLookup
{
	public OPT_AFTER_FTS_WITH_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[3]
		{
			StringToken.PropertyDescription,
			StringToken.PropertySetGuid,
			StringToken.PropertyIntId
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[3] { 642, 640, 641 };
	}
}
