using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_CREATE_TAB_WITH_DATA_DELETION_OPT_SUBTOKENS : OptLookup
{
	public OPT_CREATE_TAB_WITH_DATA_DELETION_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[10]
		{
			StringToken.Week,
			StringToken.RetentionPeriod,
			StringToken.Day,
			StringToken.Second,
			StringToken.Month,
			StringToken.Year,
			StringToken.Infinite,
			StringToken.FilterColumn,
			StringToken.Minute,
			StringToken.Hour
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[10] { 733, 727, 732, 729, 734, 735, 728, 726, 730, 731 };
	}
}
