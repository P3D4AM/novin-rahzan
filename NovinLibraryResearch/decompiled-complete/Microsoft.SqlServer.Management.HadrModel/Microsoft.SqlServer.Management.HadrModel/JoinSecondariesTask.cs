using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class JoinSecondariesTask : HadrTask, IScriptableTask
{
	private AvailabilityGroupData availabilityGroupData;

	public List<ServerConnection> ScriptingConnections { get; private set; }

	public JoinSecondariesTask(AvailabilityGroupData availabilityGroupData)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.JoinSecondariesText, availabilityGroupData.GroupName))
	{
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		this.availabilityGroupData = availabilityGroupData;
		ScriptingConnections = new List<ServerConnection>();
		foreach (AvailabilityGroupReplica secondary in this.availabilityGroupData.Secondaries)
		{
			ScriptingConnections.Add(secondary.AvailabilityGroupReplicaData.Connection);
		}
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		foreach (AvailabilityGroupReplica secondary in availabilityGroupData.Secondaries)
		{
			if (secondary.AvailabilityGroupReplicaData.State != AvailabilityObjectState.Creating)
			{
				continue;
			}
			Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(secondary.AvailabilityGroupReplicaData.Connection);
			server.JoinAvailabilityGroup(availabilityGroupData.GroupName, availabilityGroupData.ClusterType);
			if (availabilityGroupData.WillPerformAutomaticSeeding && secondary.AvailabilityGroupReplicaData.AvailabilityMode != AvailabilityReplicaAvailabilityMode.ConfigurationOnly)
			{
				server.GrantAvailabilityGroupCreateDatabasePrivilege(availabilityGroupData.GroupName);
				if (server.ConnectionContext.SqlExecutionModes != SqlExecutionModes.CaptureSql)
				{
					AvailabilityReplica availabilityReplica = availabilityGroupData.PrimaryServer.AvailabilityGroups[availabilityGroupData.GroupName].AvailabilityReplicas[secondary.AvailabilityGroupReplicaData.ReplicaName];
					availabilityReplica.SeedingMode = AvailabilityReplicaSeedingMode.Automatic;
					availabilityReplica.Alter();
				}
			}
		}
		policy.Expired = true;
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
