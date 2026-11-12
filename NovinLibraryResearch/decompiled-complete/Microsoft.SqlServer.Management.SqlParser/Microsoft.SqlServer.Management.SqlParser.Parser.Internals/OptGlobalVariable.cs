using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OptGlobalVariable : OptLookup
{
	public OptGlobalVariable()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[36]
		{
			StringToken.Timeticks,
			StringToken.Error,
			StringToken.Servicename,
			StringToken.Cpu_busy,
			StringToken.Idle,
			StringToken.Langid,
			StringToken.Cursor_rows,
			StringToken.Total_write,
			StringToken.Max_connections,
			StringToken.Remserver,
			StringToken.Language,
			StringToken.Pack_received,
			StringToken.Version,
			StringToken.Def_sortorder_id,
			StringToken.Trancount,
			StringToken.Microsoftversion,
			StringToken.Io_busy,
			StringToken.TextSize,
			StringToken.Lock_timeout,
			StringToken.Max_precision,
			StringToken.RowCount,
			StringToken.Total_read,
			StringToken.Datefirst,
			StringToken.Nestlevel,
			StringToken.Total_errors,
			StringToken.Identity,
			StringToken.Default_langid,
			StringToken.Servername,
			StringToken.Procid,
			StringToken.Connections,
			StringToken.Fetch_status,
			StringToken.Options,
			StringToken.Pack_sent,
			StringToken.Dbts,
			StringToken.Packet_errors,
			StringToken.Spid
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[36]
		{
			183, 183, 183, 183, 183, 183, 183, 183, 183, 183,
			183, 183, 183, 183, 183, 183, 183, 183, 183, 183,
			183, 183, 183, 183, 183, 183, 183, 183, 183, 183,
			183, 183, 183, 183, 183, 183
		};
	}
}
