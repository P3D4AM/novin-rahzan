using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_VIEW_SNAPSHOT_SUBTOKENS : OptLookup
{
	public OPT_VIEW_SNAPSHOT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[4]
		{
			StringToken.Build,
			StringToken.Store,
			StringToken.RefreshMode,
			StringToken.BuildReplica
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[4] { 922, 924, 921, 923 };
	}
}
