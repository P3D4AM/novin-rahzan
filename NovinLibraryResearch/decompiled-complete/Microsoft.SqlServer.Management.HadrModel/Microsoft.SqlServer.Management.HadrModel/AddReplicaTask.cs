using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AddReplicaTask : HadrTask, IScriptableTask
{
	private readonly AvailabilityGroupData availabilityGroupData;

	public List<ServerConnection> ScriptingConnections { get; private set; }

	public AddReplicaTask(AvailabilityGroupData availabilityGroupData)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.AddReplicaText, availabilityGroupData.GroupName))
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
		AvailabilityGroup availabilityGroup = new Microsoft.SqlServer.Management.Smo.Server(availabilityGroupData.PrimaryServer.ConnectionContext).AvailabilityGroups[availabilityGroupData.GroupName];
		List<AvailabilityGroupReplica> list = availabilityGroupData.AvailabilityGroupReplicas.Where((AvailabilityGroupReplica ar) => ar.AvailabilityGroupReplicaData.State == AvailabilityObjectState.Creating).ToList();
		List<AvailabilityGroupReplica> list2 = availabilityGroupData.AvailabilityGroupReplicas.Where((AvailabilityGroupReplica ar) => ar.AvailabilityGroupReplicaData.State != AvailabilityObjectState.Creating).ToList();
		foreach (AvailabilityGroupReplica item in list)
		{
			AvailabilityReplica availabilityReplica = new AvailabilityReplica(availabilityGroup, item.AvailabilityGroupReplicaData.ReplicaName);
			availabilityReplica.EndpointUrl = item.AvailabilityGroupReplicaData.EndpointUrl;
			availabilityReplica.FailoverMode = item.AvailabilityGroupReplicaData.GetFailoverMode(availabilityGroupData.ClusterType);
			availabilityReplica.AvailabilityMode = item.AvailabilityGroupReplicaData.AvailabilityMode;
			availabilityReplica.ConnectionModeInSecondaryRole = item.ReadableSecondaryRole;
			availabilityReplica.BackupPriority = item.BackupPriority;
			availabilityReplica.ReadonlyRoutingConnectionUrl = item.AvailabilityGroupReplicaData.ReadOnlyRoutingUrl ?? string.Empty;
			availabilityReplica.SetLoadBalancedReadOnlyRoutingList(item.AvailabilityGroupReplicaData.ReadOnlyRoutingList);
			availabilityGroup.AvailabilityReplicas.Add(availabilityReplica);
			if (availabilityGroupData.PerformDataSynchronization == DataSynchronizationOption.AutomaticSeeding)
			{
				availabilityReplica.SeedingMode = AvailabilityReplicaSeedingMode.Automatic;
			}
			availabilityReplica.Create();
		}
		foreach (AvailabilityGroupReplica item2 in list2)
		{
			if (availabilityGroup.AvailabilityReplicas.Contains(item2.AvailabilityGroupReplicaData.ReplicaName))
			{
				AvailabilityReplica availabilityReplica2 = availabilityGroup.AvailabilityReplicas[item2.AvailabilityGroupReplicaData.ReplicaName];
				availabilityReplica2.SetLoadBalancedReadOnlyRoutingList(item2.AvailabilityGroupReplicaData.ReadOnlyRoutingList);
				availabilityReplica2.Alter();
			}
		}
		availabilityGroupData.SqlAvailabilityGroup = availabilityGroup;
		policy.Expired = true;
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
