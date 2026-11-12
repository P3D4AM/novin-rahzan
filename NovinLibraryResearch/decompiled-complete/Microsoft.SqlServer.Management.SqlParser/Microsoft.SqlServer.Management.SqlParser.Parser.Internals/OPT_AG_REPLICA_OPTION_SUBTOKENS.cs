using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_AG_REPLICA_OPTION_SUBTOKENS : OptLookup
{
	public OPT_AG_REPLICA_OPTION_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), 662)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.SecondaryRole,
			StringToken.PrimaryRole
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 663, 663 };
	}
}
