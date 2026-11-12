using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_DB_SCOPED_CONFIG_OPT_SUBTOKENS : OptLookup
{
	public OPT_DB_SCOPED_CONFIG_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[4]
		{
			StringToken.MaxDop,
			StringToken.QueryOptimizerHotfixes,
			StringToken.LegacyCardinalityEstimation,
			StringToken.ParameterSniffing
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[4] { 908, 910, 910, 910 };
	}
}
