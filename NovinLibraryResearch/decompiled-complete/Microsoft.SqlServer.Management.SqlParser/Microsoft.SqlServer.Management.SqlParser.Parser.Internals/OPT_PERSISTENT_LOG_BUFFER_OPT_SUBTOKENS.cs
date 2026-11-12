using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_PERSISTENT_LOG_BUFFER_OPT_SUBTOKENS : OptLookup
{
	public OPT_PERSISTENT_LOG_BUFFER_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[1] { StringToken.DirectoryName };
	}

	private static int[] GetTokenIds()
	{
		return new int[1] { 918 };
	}
}
