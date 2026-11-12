using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_AFTER_WITH_SUBTOKENS : OptLookup
{
	public OPT_AFTER_WITH_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), 467)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.XmlNamespaces,
			StringToken.ChangeTrackingContext
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 465, 466 };
	}
}
