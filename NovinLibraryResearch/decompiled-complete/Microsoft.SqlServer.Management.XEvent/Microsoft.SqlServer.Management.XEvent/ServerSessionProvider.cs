using System.Globalization;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

internal class ServerSessionProvider : SessionProviderBase
{
	public ServerSessionProvider(Session session)
		: base(session, "EVENT SESSION " + SfcTsqlProcFormatter.MakeSqlBracket(session.Name) + " ON SERVER ", (string sessionName) => string.Format(CultureInfo.InvariantCulture, "CREATE EVENT SESSION {0} ON SERVER ", SfcTsqlProcFormatter.MakeSqlBracket(sessionName)))
	{
	}
}
