using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_GEODR_OPT_SUBTOKENS : OptLookup
{
	public OPT_GEODR_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[6]
		{
			StringToken.Async,
			StringToken.AllowConnections,
			StringToken.ServiceObjective,
			StringToken.Replace,
			StringToken.SecondaryType,
			StringToken.DatabaseName
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[6] { 903, 900, 902, 901, 904, 905 };
	}
}
