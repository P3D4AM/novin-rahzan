using System;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AddNewAvailabilityGroupListenerTask : HadrTask
{
	private AvailabilityGroup availabilityGroup;

	private AvailabilityGroupListenerConfiguration availabilityGroupListenerConfiguration;

	public AddNewAvailabilityGroupListenerTask(string name, AvailabilityGroup AvailabilityGroup, AvailabilityGroupListenerConfiguration AvailabilityGroupListenerConfiguration)
		: base(name)
	{
		if (AvailabilityGroup == null)
		{
			throw new ArgumentNullException("AvailabilityGroup");
		}
		availabilityGroup = AvailabilityGroup;
		if (AvailabilityGroupListenerConfiguration == null)
		{
			throw new ArgumentNullException("AvailabilityGroupListenerConfiguration");
		}
		availabilityGroupListenerConfiguration = AvailabilityGroupListenerConfiguration;
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		AvailabilityGroupListener availabilityGroupListener = new AvailabilityGroupListener(availabilityGroup, availabilityGroupListenerConfiguration.ListenerName);
		availabilityGroupListener.PortNumber = availabilityGroupListenerConfiguration.ListenerPortNumber;
		AvailabilityGroupListenerIPAddressCollection availabilityGroupListenerIPAddresses = availabilityGroupListener.AvailabilityGroupListenerIPAddresses;
		if (availabilityGroupListenerConfiguration.NetworkMode == AGListenerNetworkMode.DHCP)
		{
			AvailabilityGroupListenerIPAddress availabilityGroupListenerIPAddress = new AvailabilityGroupListenerIPAddress(availabilityGroupListener);
			availabilityGroupListenerIPAddress.IsDHCP = true;
			availabilityGroupListenerIPAddress.SubnetIP = availabilityGroupListenerConfiguration.DhcpSubnetConfigurations.IPAddress;
			availabilityGroupListenerIPAddress.SubnetMask = availabilityGroupListenerConfiguration.DhcpSubnetConfigurations.SubnetMask;
			availabilityGroupListenerIPAddresses.Add(availabilityGroupListenerIPAddress);
		}
		else
		{
			foreach (AvailabilityGroupListenerSubnet staticIpSubnetConfiguration in availabilityGroupListenerConfiguration.StaticIpSubnetConfigurations)
			{
				AvailabilityGroupListenerIPAddress availabilityGroupListenerIPAddress2 = new AvailabilityGroupListenerIPAddress(availabilityGroupListener);
				availabilityGroupListenerIPAddress2.IsDHCP = false;
				availabilityGroupListenerIPAddress2.SubnetIP = staticIpSubnetConfiguration.SubnetIP;
				availabilityGroupListenerIPAddress2.SubnetMask = staticIpSubnetConfiguration.SubnetMask;
				availabilityGroupListenerIPAddress2.IPAddress = staticIpSubnetConfiguration.IPAddress;
				availabilityGroupListenerIPAddresses.Add(availabilityGroupListenerIPAddress2);
			}
		}
		availabilityGroupListener.Create();
		policy.Expired = true;
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
