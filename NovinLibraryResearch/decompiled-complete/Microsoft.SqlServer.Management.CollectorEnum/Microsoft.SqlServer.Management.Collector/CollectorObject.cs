using System.Reflection;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Collector;

internal class CollectorObject : SqlObject, ISupportVersions
{
	public override Assembly ResourceAssembly => Assembly.GetExecutingAssembly();

	public ServerVersion GetServerVersion(object conn)
	{
		return ExecuteSql.GetServerVersion(conn);
	}
}
