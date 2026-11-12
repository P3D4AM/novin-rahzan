using System;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class EditExistingAvailabilityGroupListenerTask : HadrTask
{
	private AvailabilityGroup availabilityGroup;

	private AvailabilityGroupListenerConfiguration availabilityGroupListenerConfiguration;

	public EditExistingAvailabilityGroupListenerTask(string name, AvailabilityGroup AvailabilityGroup, AvailabilityGroupListenerConfiguration AvailabilityGroupListenerConfiguration)
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
		bool flag = false;
		AvailabilityGroupListener availabilityGroupListener = availabilityGroup.AvailabilityGroupListeners[availabilityGroupListenerConfiguration.ListenerName];
		if (availabilityGroupListener.PortNumber != availabilityGroupListenerConfiguration.ListenerPortNumber)
		{
			availabilityGroupListener.PortNumber = availabilityGroupListenerConfiguration.ListenerPortNumber;
			flag = true;
		}
		AvailabilityGroupListenerIPAddressCollection availabilityGroupListenerIPAddresses = availabilityGroupListener.AvailabilityGroupListenerIPAddresses;
		if (availabilityGroupListenerConfiguration.NetworkMode == AGListenerNetworkMode.StaticIP)
		{
			foreach (AvailabilityGroupListenerSubnet staticIpSubnetConfiguration in availabilityGroupListenerConfiguration.StaticIpSubnetConfigurations)
			{
				AvailabilityGroupListenerIPAddress availabilityGroupListenerIPAddress = new AvailabilityGroupListenerIPAddress(availabilityGroupListener);
				availabilityGroupListenerIPAddress.IsDHCP = false;
				availabilityGroupListenerIPAddress.SubnetIP = staticIpSubnetConfiguration.SubnetIP;
				availabilityGroupListenerIPAddress.SubnetMask = staticIpSubnetConfiguration.SubnetMask;
				availabilityGroupListenerIPAddress.IPAddress = staticIpSubnetConfiguration.IPAddress;
				availabilityGroupListenerIPAddresses.Add(availabilityGroupListenerIPAddress);
				availabilityGroupListenerIPAddress.Create();
			}
		}
		if (flag)
		{
			availabilityGroupListener.Alter();
		}
		policy.Expired = true;
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
