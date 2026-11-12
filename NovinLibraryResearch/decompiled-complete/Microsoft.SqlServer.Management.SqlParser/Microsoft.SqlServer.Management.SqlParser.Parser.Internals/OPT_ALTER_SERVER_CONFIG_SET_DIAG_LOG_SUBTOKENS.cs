using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_ALTER_SERVER_CONFIG_SET_DIAG_LOG_SUBTOKENS : OptLookup
{
	public OPT_ALTER_SERVER_CONFIG_SET_DIAG_LOG_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[5]
		{
			StringToken.MaxSize,
			StringToken.Path,
			StringToken.MaxFiles,
			StringToken.On,
			StringToken.Off
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[5] { 675, 674, 676, 677, 678 };
	}
}
