using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_AUTO_TUNING_DATABASE_TUNING_MODE_OPT_SUBTOKENS : OptLookup
{
	public OPT_AUTO_TUNING_DATABASE_TUNING_MODE_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[3]
		{
			StringToken.Auto,
			StringToken.Custom,
			StringToken.Inherit
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[3] { 876, 877, 875 };
	}
}
