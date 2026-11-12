using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class ServerInstance : WmiBase
{
	public ServerInstance()
		: base("ServerSettings")
	{
		AddProperty(new WmiProperty("Name", "System.String")
		{
			PhysicalName = "InstanceName"
		});
	}

	public override EnumResult GetData(EnumResult erParent)
	{
		if (base.Request is WmiRequest { RequestingTheWhereClause: not false })
		{
			return new WmiEnumResult(((WmiEnumResult)erParent).Scope)
			{
				WhereClause = GetXpathFilter()
			};
		}
		return base.GetData(erParent);
	}
}
