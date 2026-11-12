using System.Reflection;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

internal sealed class PolicyObject : SqlObject, ISupportVersions
{
	public override Assembly ResourceAssembly => Assembly.GetExecutingAssembly();

	public ServerVersion GetServerVersion(object conn)
	{
		return ExecuteSql.GetServerVersion(conn);
	}
}
