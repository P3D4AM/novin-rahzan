using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_EXTERNAL_TYPE_SUBTOKENS : OptLookup
{
	public OPT_EXTERNAL_TYPE_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[11]
		{
			StringToken.Pool,
			StringToken.Language,
			StringToken.File,
			StringToken.Resource,
			StringToken.Data,
			StringToken.Format,
			StringToken.Url,
			StringToken.Source,
			StringToken.Table,
			StringToken.Library,
			StringToken.Stream
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[11]
		{
			850, 852, 846, 848, 844, 847, 849, 845, 843, 851,
			853
		};
	}
}
