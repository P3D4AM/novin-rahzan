using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_CREATE_DB_OPT_SUBTOKENS : OptLookup
{
	public OPT_CREATE_DB_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), 478)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[3]
		{
			StringToken.PersistentLogBuffer,
			StringToken.BackupStorageRedundancy,
			StringToken.FileStream
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[3] { 477, 479, 474 };
	}
}
