using System.Globalization;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.XEvent;

namespace Microsoft.SqlServer.Management.XEventDbScoped;

internal class DatabaseSessionProvider : SessionProviderBase
{
	public DatabaseSessionProvider(Session session)
		: base(session, "EVENT SESSION " + SfcTsqlProcFormatter.MakeSqlBracket(session.Name) + " ON DATABASE ", (string sessionName) => string.Format(CultureInfo.InvariantCulture, "CREATE EVENT SESSION {0} ON DATABASE ", SfcTsqlProcFormatter.MakeSqlBracket(sessionName)))
	{
	}
}
