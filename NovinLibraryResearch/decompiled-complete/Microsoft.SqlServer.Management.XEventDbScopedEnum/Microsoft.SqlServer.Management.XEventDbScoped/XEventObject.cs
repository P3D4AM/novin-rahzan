using System.Reflection;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEventDbScoped;

internal sealed class XEventObject : SqlObject, ISupportVersions, ISupportDatabaseEngineTypes
{
	public override Assembly ResourceAssembly => Assembly.GetExecutingAssembly();

	public ServerVersion GetServerVersion(object conn)
	{
		return ExecuteSql.GetServerVersion(conn);
	}

	public DatabaseEngineType GetDatabaseEngineType(object conn)
	{
		return ExecuteSql.GetDatabaseEngineType(conn);
	}
}
