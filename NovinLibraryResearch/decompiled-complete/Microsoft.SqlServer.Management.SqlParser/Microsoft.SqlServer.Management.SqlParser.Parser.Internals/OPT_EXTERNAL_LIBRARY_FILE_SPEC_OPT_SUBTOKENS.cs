using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_EXTERNAL_LIBRARY_FILE_SPEC_OPT_SUBTOKENS : OptLookup
{
	public OPT_EXTERNAL_LIBRARY_FILE_SPEC_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.Content,
			StringToken.Platform
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 948, 949 };
	}
}
