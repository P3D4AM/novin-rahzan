using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_ALTER_EXTERNALDATASOURCE_WITH_SUBTOKENS : OptLookup
{
	public OPT_ALTER_EXTERNALDATASOURCE_WITH_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[6]
		{
			StringToken.ShardMapName,
			StringToken.Location,
			StringToken.ResourceManagerLocation,
			StringToken.DatabaseName,
			StringToken.PushDown,
			StringToken.Credential
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[6] { 831, 827, 828, 830, 833, 829 };
	}
}
