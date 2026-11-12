using System.Reflection;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.RegisteredServers;

internal sealed class RegisteredServersEnumerator : SqlObject, ISupportVersions
{
	public override Assembly ResourceAssembly => Assembly.GetExecutingAssembly();

	public ServerVersion GetServerVersion(object conn)
	{
		return ExecuteSql.GetServerVersion(conn);
	}

	protected override void BeforeStatementExecuted(string levelName)
	{
		if (!levelName.Equals("RegisteredServersStore") && string.IsNullOrEmpty(base.StatementBuilder.SqlPostfix))
		{
			base.StatementBuilder.AddPostfix("option (hash join, force order)");
		}
	}
}
