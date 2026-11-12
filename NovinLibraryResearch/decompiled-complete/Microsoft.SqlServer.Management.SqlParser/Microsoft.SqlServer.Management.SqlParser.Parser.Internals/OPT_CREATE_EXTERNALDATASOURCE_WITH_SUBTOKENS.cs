using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_CREATE_EXTERNALDATASOURCE_WITH_SUBTOKENS : OptLookup
{
	public OPT_CREATE_EXTERNALDATASOURCE_WITH_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[14]
		{
			StringToken.ConnectionOptions,
			StringToken.ResourceManagerLocation,
			StringToken.ShardMapManager,
			StringToken.Credential,
			StringToken.Hadoop,
			StringToken.JobTrackerLocation,
			StringToken.PushDown,
			StringToken.Location,
			StringToken.Rdbms,
			StringToken.ShardMapName,
			StringToken.SharedMemory,
			StringToken.BlobStorage,
			StringToken.Type,
			StringToken.DatabaseName
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[14]
		{
			800, 796, 804, 798, 802, 795, 799, 794, 803, 801,
			805, 806, 793, 797
		};
	}
}
