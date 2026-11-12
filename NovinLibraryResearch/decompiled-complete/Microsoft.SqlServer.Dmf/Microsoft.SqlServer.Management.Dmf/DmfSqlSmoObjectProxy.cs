using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.Dmf;

[STraceConfiguration(SkipAutoTrace = true)]
internal sealed class DmfSqlSmoObjectProxy : SqlSmoObject
{
	internal static Microsoft.SqlServer.Management.Smo.Server GetServerObj(SqlSmoObject smoObj)
	{
		return smoObj.GetServerObject();
	}
}
