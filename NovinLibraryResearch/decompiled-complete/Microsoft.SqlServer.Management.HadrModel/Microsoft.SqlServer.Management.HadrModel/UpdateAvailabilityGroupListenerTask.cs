using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class UpdateAvailabilityGroupListenerTask : HadrTask, IScriptableTask
{
	private readonly AvailabilityGroupData availabilityGroupData;

	public List<ServerConnection> ScriptingConnections { get; private set; }

	public UpdateAvailabilityGroupListenerTask(AvailabilityGroupData availabilityGroupData)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.UpdateAvailabilityGroupListenerText, availabilityGroupData.AvailabilityGroupListener.ListenerName))
	{
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		this.availabilityGroupData = availabilityGroupData;
		ScriptingConnections = new List<ServerConnection> { this.availabilityGroupData.PrimaryServer.ConnectionContext };
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		UpdateStatus(new TaskEventArgs(base.Name, string.Format(Resource.CreatingAvailabilityGroup, availabilityGroupData.GroupName), TaskEventStatus.Running));
		policy.Expired = true;
		if (availabilityGroupData.SqlAvailabilityGroup == null)
		{
			throw new InvalidAvailabilityGroupException(availabilityGroupData.GroupName, availabilityGroupData.PrimaryServer.ConnectionContext.TrueName);
		}
		AvailabilityGroupListener availabilityGroupListener = new AvailabilityGroupListener(availabilityGroupData.SqlAvailabilityGroup, availabilityGroupData.AvailabilityGroupListener.ListenerName);
		availabilityGroupListener.PortNumber = availabilityGroupData.AvailabilityGroupListener.ListenerPortNumber;
		switch (availabilityGroupData.AvailabilityGroupListener.NetworkMode)
		{
		case AGListenerNetworkMode.DHCP:
		{
			AvailabilityGroupListenerIPAddress availabilityGroupListenerIPAddress2 = new AvailabilityGroupListenerIPAddress(availabilityGroupListener)
			{
				IsDHCP = true,
				SubnetIP = availabilityGroupData.AvailabilityGroupListener.DhcpSubnetConfigurations.SubnetIP,
				SubnetMask = availabilityGroupData.AvailabilityGroupListener.DhcpSubnetConfigurations.SubnetMask
			};
			availabilityGroupListener.AvailabilityGroupListenerIPAddresses.Add(availabilityGroupListenerIPAddress2);
			break;
		}
		case AGListenerNetworkMode.StaticIP:
			foreach (AvailabilityGroupListenerSubnet staticIpSubnetConfiguration in availabilityGroupData.AvailabilityGroupListener.StaticIpSubnetConfigurations)
			{
				AvailabilityGroupListenerIPAddress availabilityGroupListenerIPAddress = new AvailabilityGroupListenerIPAddress(availabilityGroupListener)
				{
					IsDHCP = false,
					SubnetIP = staticIpSubnetConfiguration.SubnetIP,
					SubnetMask = staticIpSubnetConfiguration.SubnetMask,
					IPAddress = staticIpSubnetConfiguration.IPAddress
				};
				availabilityGroupListener.AvailabilityGroupListenerIPAddresses.Add(availabilityGroupListenerIPAddress);
			}
			break;
		}
		availabilityGroupListener.Alter();
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
