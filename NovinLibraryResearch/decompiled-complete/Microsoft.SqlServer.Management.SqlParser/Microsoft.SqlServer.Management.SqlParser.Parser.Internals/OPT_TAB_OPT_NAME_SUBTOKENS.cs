using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_TAB_OPT_NAME_SUBTOKENS : OptLookup
{
	public OPT_TAB_OPT_NAME_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[6]
		{
			StringToken.FileTableDirectory,
			StringToken.LockEscalation,
			StringToken.SystemVersioning,
			StringToken.FileStreamOn,
			StringToken.DataDeletion,
			StringToken.RemoteDataArchive
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[6] { 535, 534, 536, 533, 538, 537 };
	}
}
