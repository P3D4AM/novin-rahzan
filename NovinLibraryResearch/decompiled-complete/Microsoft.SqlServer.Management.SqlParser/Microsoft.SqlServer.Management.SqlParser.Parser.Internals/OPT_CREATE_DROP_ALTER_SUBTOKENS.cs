using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_CREATE_DROP_ALTER_SUBTOKENS : OptLookup
{
	public OPT_CREATE_DROP_ALTER_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[46]
		{
			StringToken.Policy,
			StringToken.Pool,
			StringToken.Audit,
			StringToken.Resource,
			StringToken.Xml,
			StringToken.Route,
			StringToken.Classification,
			StringToken.Target,
			StringToken.Assembly,
			StringToken.Credential,
			StringToken.Aggregate,
			StringToken.Master,
			StringToken.Session,
			StringToken.Search,
			StringToken.Message,
			StringToken.Server,
			StringToken.Role,
			StringToken.Signature,
			StringToken.Cryptographic,
			StringToken.Application,
			StringToken.Partition,
			StringToken.Contract,
			StringToken.Login,
			StringToken.Event,
			StringToken.Synonym,
			StringToken.Counter,
			StringToken.Type,
			StringToken.Asymmetric,
			StringToken.Remote,
			StringToken.ColumnStore,
			StringToken.Spatial,
			StringToken.Symmetric,
			StringToken.Governor,
			StringToken.Service,
			StringToken.Selective,
			StringToken.FullText,
			StringToken.Certificate,
			StringToken.Sensitivity,
			StringToken.Broker,
			StringToken.Sequence,
			StringToken.Queue,
			StringToken.Notification,
			StringToken.Availability,
			StringToken.EndPoint,
			StringToken.Security,
			StringToken.Workload
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[46]
		{
			449, 439, 447, 437, 423, 422, 451, 435, 407, 429,
			408, 426, 433, 444, 413, 436, 419, 428, 443, 418,
			410, 421, 417, 409, 415, 431, 406, 430, 424, 446,
			441, 427, 440, 412, 442, 411, 425, 450, 432, 416,
			420, 434, 445, 414, 448, 438
		};
	}
}
