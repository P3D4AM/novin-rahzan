using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_OPT_SXI_PATH_OPTION_SUBTOKENS : OptLookup
{
	public OPT_OPT_SXI_PATH_OPTION_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.MaxLength,
			StringToken.Singleton
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 755, 756 };
	}
}
