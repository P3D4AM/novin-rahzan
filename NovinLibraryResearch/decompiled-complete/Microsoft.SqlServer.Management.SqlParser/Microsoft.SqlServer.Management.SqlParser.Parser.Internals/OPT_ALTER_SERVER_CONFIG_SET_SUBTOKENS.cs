using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_ALTER_SERVER_CONFIG_SET_SUBTOKENS : OptLookup
{
	public OPT_ALTER_SERVER_CONFIG_SET_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[6]
		{
			StringToken.FailOver,
			StringToken.Diagnostics,
			StringToken.SoftNuma,
			StringToken.Hadr,
			StringToken.Process,
			StringToken.Buffer
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[6] { 667, 668, 671, 669, 666, 670 };
	}
}
