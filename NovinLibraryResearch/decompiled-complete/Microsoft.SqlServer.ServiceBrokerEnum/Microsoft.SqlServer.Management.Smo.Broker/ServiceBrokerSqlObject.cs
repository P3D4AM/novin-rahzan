using System.Reflection;

namespace Microsoft.SqlServer.Management.Smo.Broker;

internal class ServiceBrokerSqlObject : SqlObject
{
	public override Assembly ResourceAssembly => Assembly.GetExecutingAssembly();
}
