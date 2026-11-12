using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrData;

public class AvailabilityGroupReplicaData
{
	[Bindable(false)]
	public ServerConnection Connection { get; set; }

	public string ReplicaName { get; set; }

	public ReplicaRole InitialRole { get; set; }

	public bool AutomaticFailover { get; set; }

	public AvailabilityReplicaAvailabilityMode AvailabilityMode { get; set; }

	public AvailabilityReplicaConnectionModeInSecondaryRole ReadableSecondaryRole { get; set; }

	public IList<IList<string>> ReadOnlyRoutingList { get; set; }

	public string ReadOnlyRoutingUrl { get; set; }

	public string EndpointUrl { get; set; }

	public int EndpointPortNumber { get; set; }

	public string EndpointName { get; set; }

	public EndpointEncryption EndpointEncryption { get; set; }

	public EndpointEncryptionAlgorithm EndpointEncryptionAlgorithm { get; set; }

	public string EndpointServiceAccount { get; set; }

	public byte[] EndpointServiceAccountSid { get; set; }

	public bool EndpointPresent { get; set; }

	public bool EndpointCanBeConfigured { get; set; }

	public Endpoint Endpoint { get; set; }

	public int BackupPriority { get; set; }

	public AvailabilityObjectState State { get; set; }

	public bool IsClustered { get; set; }

	public AvailabilityReplicaSeedingMode SeedingMode { get; set; }

	public AvailabilityGroupReplicaData()
	{
		BackupPriority = 50;
		EndpointCanBeConfigured = true;
		EndpointPortNumber = 5022;
		EndpointServiceAccount = null;
		EndpointEncryption = EndpointEncryption.Required;
		EndpointServiceAccountSid = null;
		Endpoint = null;
		EndpointEncryptionAlgorithm = EndpointEncryptionAlgorithm.Aes;
		ReadableSecondaryRole = AvailabilityReplicaConnectionModeInSecondaryRole.AllowNoConnections;
	}

	public AvailabilityReplicaFailoverMode GetFailoverMode(AvailabilityGroupClusterType clusterType)
	{
		if (clusterType != AvailabilityGroupClusterType.External)
		{
			if (!AutomaticFailover)
			{
				return AvailabilityReplicaFailoverMode.Manual;
			}
			return AvailabilityReplicaFailoverMode.Automatic;
		}
		return AvailabilityReplicaFailoverMode.External;
	}
}
