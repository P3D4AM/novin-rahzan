using System.Data;
using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class ServerProtocolIPAddress : WmiBase
{
	private DataTable m_dataParent;

	public ServerProtocolIPAddress()
		: base("ServerNetworkProtocolIPAddress")
	{
		AddProperty(new WmiProperty("Name", "System.String")
		{
			PhysicalName = "IpAddressName"
		});
		AddProperty(new WmiProperty("InstanceName", "System.String")
		{
			ReadOnly = true,
			Usage = ObjectPropertyUsages.Reserved1
		});
		AddProperty(new WmiProperty("ProtocolName", "System.String")
		{
			ReadOnly = true,
			Usage = ObjectPropertyUsages.Reserved1
		});
	}

	public override Request RetrieveParentRequest()
	{
		WmiRequest wmiRequest = new WmiRequest();
		wmiRequest.Fields = new string[2] { "Name", "InstanceName" };
		return wmiRequest;
	}

	protected override void BuildStatementBuilder()
	{
		base.BuildStatementBuilder();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataRow row in m_dataParent.Rows)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" OR ");
			}
			stringBuilder.AppendFormat("(ProtocolName=\"{0}\" and InstanceName=\"{1}\")", row[0], row[1]);
		}
		m_dataParent = null;
		base.StatementBuilder.AddWhere(stringBuilder.ToString());
	}

	public override EnumResult GetData(EnumResult erParent)
	{
		m_dataParent = erParent;
		return base.GetData(erParent);
	}
}
