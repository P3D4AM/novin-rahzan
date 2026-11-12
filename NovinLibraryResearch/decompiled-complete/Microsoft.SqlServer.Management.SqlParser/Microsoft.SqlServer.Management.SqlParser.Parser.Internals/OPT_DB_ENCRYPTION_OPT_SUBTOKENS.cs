using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_DB_ENCRYPTION_OPT_SUBTOKENS : OptLookup
{
	public OPT_DB_ENCRYPTION_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[1] { StringToken.WorkloadGroup };
	}

	private static int[] GetTokenIds()
	{
		return new int[1] { 934 };
	}
}
