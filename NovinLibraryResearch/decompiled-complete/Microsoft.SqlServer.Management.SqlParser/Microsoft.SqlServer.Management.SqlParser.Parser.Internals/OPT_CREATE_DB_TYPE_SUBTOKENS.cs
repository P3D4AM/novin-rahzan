using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_CREATE_DB_TYPE_SUBTOKENS : OptLookup
{
	public OPT_CREATE_DB_TYPE_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.Copy,
			StringToken.Snapshot
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 483, 482 };
	}
}
