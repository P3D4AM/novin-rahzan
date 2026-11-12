using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrData;

public class AvailabilityGroupListenerConfiguration
{
	private int listenerPortNumber = 1433;

	private readonly List<AvailabilityGroupListenerSubnet> subnetConfigurations;

	public int ListenerPortNumber
	{
		get
		{
			return listenerPortNumber;
		}
		set
		{
			listenerPortNumber = value;
		}
	}

	public AGListenerNetworkMode NetworkMode { get; set; }

	public string ListenerName { get; set; }

	public IList<AvailabilityGroupListenerSubnet> SubnetConfigurations => subnetConfigurations;

	public AvailabilityGroupListenerSubnet DhcpSubnetConfigurations => subnetConfigurations.FirstOrDefault((AvailabilityGroupListenerSubnet s) => s.IsDHCP);

	public IList<AvailabilityGroupListenerSubnet> StaticIpSubnetConfigurations => subnetConfigurations.Where((AvailabilityGroupListenerSubnet s) => !s.IsDHCP).ToList().AsReadOnly();

	public AvailabilityGroupListenerConfiguration()
	{
		ListenerName = string.Empty;
		ListenerPortNumber = 0;
		NetworkMode = AGListenerNetworkMode.StaticIP;
		subnetConfigurations = new List<AvailabilityGroupListenerSubnet>();
	}

	public AvailabilityGroupListenerConfiguration(AvailabilityGroupListener ag)
	{
		ListenerName = ag.Name;
		ListenerPortNumber = ag.PortNumber;
		NetworkMode = ((!ag.AvailabilityGroupListenerIPAddresses[0].IsDHCP) ? AGListenerNetworkMode.StaticIP : AGListenerNetworkMode.DHCP);
		subnetConfigurations = new List<AvailabilityGroupListenerSubnet>();
		foreach (AvailabilityGroupListenerIPAddress availabilityGroupListenerIPAddress in ag.AvailabilityGroupListenerIPAddresses)
		{
			subnetConfigurations.Add(new AvailabilityGroupListenerSubnet
			{
				IsDHCP = availabilityGroupListenerIPAddress.IsDHCP,
				SubnetIP = availabilityGroupListenerIPAddress.SubnetIP,
				SubnetMask = availabilityGroupListenerIPAddress.SubnetMask,
				IPAddress = availabilityGroupListenerIPAddress.IPAddress
			});
		}
	}

	public void AddSubnetConfiguration(AvailabilityGroupListenerSubnet subnetConfiguration)
	{
		if (subnetConfiguration == null)
		{
			throw new ArgumentNullException("subnetConfiguration");
		}
		if (subnetConfiguration.IsDHCP && subnetConfigurations.FirstOrDefault((AvailabilityGroupListenerSubnet s) => s.IsDHCP) != null)
		{
			throw new ArgumentException("Listener configuration already contains a DHCP subnet.");
		}
		subnetConfigurations.Add(subnetConfiguration);
	}
}
