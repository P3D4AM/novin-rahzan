using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_CREATE_EXTERNALSTREAM_WITH_SUBTOKENS : OptLookup
{
	public OPT_CREATE_EXTERNALSTREAM_WITH_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[5]
		{
			StringToken.OutputOptions,
			StringToken.Location,
			StringToken.FileFormat,
			StringToken.DataSource,
			StringToken.InputOptions
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[5] { 840, 837, 838, 836, 839 };
	}
}
