using System.Security.Permissions;
using Microsoft.Data.ProviderBase;

namespace Microsoft.Data.SqlClient;

[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
internal sealed class SqlPerformanceCounters : DbConnectionPoolCounters
{
	private const string CategoryName = ".NET Data Provider for SqlServer";

	private const string CategoryHelp = "Counters for Microsoft.Data.SqlClient";

	public static readonly SqlPerformanceCounters SingletonInstance = new SqlPerformanceCounters();

	private SqlPerformanceCounters()
		: base(".NET Data Provider for SqlServer", "Counters for Microsoft.Data.SqlClient")
	{
	}
}
