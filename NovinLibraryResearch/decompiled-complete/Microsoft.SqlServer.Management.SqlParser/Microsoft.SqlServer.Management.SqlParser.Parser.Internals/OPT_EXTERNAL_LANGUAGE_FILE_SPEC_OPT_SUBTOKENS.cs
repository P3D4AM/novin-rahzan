using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_EXTERNAL_LANGUAGE_FILE_SPEC_OPT_SUBTOKENS : OptLookup
{
	public OPT_EXTERNAL_LANGUAGE_FILE_SPEC_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[5]
		{
			StringToken.Content,
			StringToken.Platform,
			StringToken.EnvironmentVariables,
			StringToken.Parameters,
			StringToken.FileName
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[5] { 937, 940, 941, 939, 938 };
	}
}
