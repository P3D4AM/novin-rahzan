namespace Microsoft.SqlServer.Management.HadrData;

public class AvailabilityGroupListenerSubnet
{
	public bool IsDHCP { get; set; }

	public string SubnetIP { get; set; }

	public string SubnetMask { get; set; }

	public string IPAddress { get; set; }
}
