using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_FILESTREAM_DB_OPT_SUBTOKENS : OptLookup
{
	public OPT_FILESTREAM_DB_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.DirectoryName,
			StringToken.NonTransactedAccess
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 692, 691 };
	}
}
