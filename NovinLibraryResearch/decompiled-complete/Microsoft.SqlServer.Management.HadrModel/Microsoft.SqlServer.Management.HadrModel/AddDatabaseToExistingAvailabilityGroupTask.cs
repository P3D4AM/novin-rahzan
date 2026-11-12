using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AddDatabaseToExistingAvailabilityGroupTask : HadrTask, IScriptableTask
{
	private readonly AvailabilityGroupData availabilityGroupData;

	public List<ServerConnection> ScriptingConnections { get; private set; }

	public AddDatabaseToExistingAvailabilityGroupTask(AvailabilityGroupData availabilityGroupData)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.AddDatabaseToExistingAvailabilityGroupText, availabilityGroupData.GroupName))
	{
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		this.availabilityGroupData = availabilityGroupData;
		ScriptingConnections = new List<ServerConnection> { this.availabilityGroupData.PrimaryServer.ConnectionContext };
		foreach (AvailabilityGroupReplica secondary in availabilityGroupData.Secondaries)
		{
			ScriptingConnections.Add(secondary.AvailabilityGroupReplicaData.Connection);
		}
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		policy.Expired = true;
		AvailabilityGroup availabilityGroup = availabilityGroupData.PrimaryServer.AvailabilityGroups[availabilityGroupData.GroupName];
		if (availabilityGroup == null)
		{
			throw new InvalidAvailabilityGroupException(availabilityGroupData.GroupName, availabilityGroupData.PrimaryServer.ConnectionContext.TrueName);
		}
		foreach (AvailabilityReplica availabilityReplica in availabilityGroup.AvailabilityReplicas)
		{
			if (availabilityReplica.Role != AvailabilityReplicaRole.Primary && availabilityReplica.IsSeedingModeSupported)
			{
				availabilityReplica.SeedingMode = ((!availabilityGroupData.WillPerformAutomaticSeeding) ? AvailabilityReplicaSeedingMode.Manual : AvailabilityReplicaSeedingMode.Automatic);
				availabilityReplica.Alter();
			}
		}
		if (availabilityGroupData.WillPerformAutomaticSeeding)
		{
			foreach (AvailabilityGroupReplica secondary in availabilityGroupData.Secondaries)
			{
				new Microsoft.SqlServer.Management.Smo.Server(secondary.AvailabilityGroupReplicaData.Connection).GrantAvailabilityGroupCreateDatabasePrivilege(availabilityGroupData.GroupName);
			}
		}
		foreach (PrimaryDatabaseData newAvailabilityDatabase in availabilityGroupData.NewAvailabilityDatabases)
		{
			AvailabilityDatabase availabilityDatabase = new AvailabilityDatabase(availabilityGroup, newAvailabilityDatabase.Name);
			availabilityGroup.AvailabilityDatabases.Add(availabilityDatabase);
			availabilityDatabase.Create();
		}
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
