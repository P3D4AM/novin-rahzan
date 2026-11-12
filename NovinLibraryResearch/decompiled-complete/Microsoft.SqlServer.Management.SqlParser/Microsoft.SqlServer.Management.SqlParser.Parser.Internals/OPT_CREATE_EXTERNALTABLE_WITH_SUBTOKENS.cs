using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_CREATE_EXTERNALTABLE_WITH_SUBTOKENS : OptLookup
{
	public OPT_CREATE_EXTERNALTABLE_WITH_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[16]
		{
			StringToken.Value,
			StringToken.ObjectName,
			StringToken.Distribution,
			StringToken.RejectValue,
			StringToken.SchemaName,
			StringToken.RejectedRowLocation,
			StringToken.FileFormat,
			StringToken.RejectType,
			StringToken.Location,
			StringToken.Sharded,
			StringToken.RoundRobin,
			StringToken.DataSource,
			StringToken.Hash,
			StringToken.RejectSampleValue,
			StringToken.Replicated,
			StringToken.Percentage
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[16]
		{
			815, 823, 817, 813, 822, 824, 811, 812, 810, 818,
			820, 809, 821, 814, 819, 816
		};
	}
}
