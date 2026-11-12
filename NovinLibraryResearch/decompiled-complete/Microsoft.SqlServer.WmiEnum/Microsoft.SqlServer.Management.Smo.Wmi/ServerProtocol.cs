using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class ServerProtocol : WmiBase
{
	private string m_whereServiceInstance;

	public ServerProtocol()
		: base("ServerNetworkProtocol")
	{
		WmiProperty wmiProperty = null;
		AddProperty(new WmiProperty("Name", "System.String")
		{
			PhysicalName = "ProtocolName"
		});
		AddProperty(new WmiProperty("InstanceName", "System.String")
		{
			ReadOnly = true,
			Usage = ObjectPropertyUsages.Reserved1
		});
		AddProperty(new WmiProperty("DisplayName", "System.String")
		{
			PhysicalName = "ProtocolDisplayName",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("IsEnabled", "System.Boolean")
		{
			PhysicalName = "Enabled"
		});
		AddProperty(new WmiProperty("HasMultiIPAddresses", "System.Boolean")
		{
			PhysicalName = "MultiIpConfigurationSupport",
			ReadOnly = true
		});
	}

	public override Request RetrieveParentRequest()
	{
		return new WmiRequest
		{
			RequestingTheWhereClause = true
		};
	}

	protected override void BuildStatementBuilder()
	{
		base.BuildStatementBuilder();
		if (m_whereServiceInstance.Length > 0)
		{
			base.StatementBuilder.AddWhere(m_whereServiceInstance);
		}
		m_whereServiceInstance = null;
	}

	public override EnumResult GetData(EnumResult erParent)
	{
		m_whereServiceInstance = ((WmiEnumResult)erParent).WhereClause;
		return base.GetData(erParent);
	}
}
