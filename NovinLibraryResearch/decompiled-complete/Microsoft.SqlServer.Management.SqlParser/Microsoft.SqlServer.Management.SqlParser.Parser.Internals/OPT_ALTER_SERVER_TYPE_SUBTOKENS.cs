using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_ALTER_SERVER_TYPE_SUBTOKENS : OptLookup
{
	public OPT_ALTER_SERVER_TYPE_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[3]
		{
			StringToken.Configuration,
			StringToken.Role,
			StringToken.Audit
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[3] { 624, 625, 623 };
	}
}
