using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class CreateAvailabilityGroupTask : HadrTask, IScriptableTask
{
	private readonly AvailabilityGroupData availabilityGroupData;

	public List<ServerConnection> ScriptingConnections { get; private set; }

	public CreateAvailabilityGroupTask(AvailabilityGroupData availabilityGroupData)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.CreateAvailabilityGroupText, availabilityGroupData.GroupName))
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
		AvailabilityGroup availabilityGroup = new AvailabilityGroup(availabilityGroupData.PrimaryServer, availabilityGroupData.GroupName);
		availabilityGroup.AutomatedBackupPreference = availabilityGroupData.BackupPreference;
		if (Utils.IsSql13OrLater(availabilityGroupData.PrimaryServer.VersionMajor))
		{
			availabilityGroup.BasicAvailabilityGroup = availabilityGroupData.IsBasic;
			availabilityGroup.DatabaseHealthTrigger = availabilityGroupData.IsDatabaseHealthTriggerOn;
			availabilityGroup.DtcSupportEnabled = availabilityGroupData.IsDtcSupportEnabled;
		}
		if (Utils.IsSql14OrLater(availabilityGroupData.PrimaryServer.VersionMajor))
		{
			availabilityGroup.ClusterType = availabilityGroupData.ClusterType;
			availabilityGroup.RequiredSynchronizedSecondariesToCommit = availabilityGroupData.RequiredSynchronizedSecondariesToCommit;
		}
		foreach (PrimaryDatabaseData newAvailabilityDatabase in availabilityGroupData.NewAvailabilityDatabases)
		{
			availabilityGroup.AvailabilityDatabases.Add(new AvailabilityDatabase(availabilityGroup, newAvailabilityDatabase.Name));
		}
		foreach (AvailabilityGroupReplica availabilityGroupReplica in availabilityGroupData.AvailabilityGroupReplicas)
		{
			AvailabilityReplica availabilityReplica = new AvailabilityReplica(availabilityGroup, availabilityGroupReplica.AvailabilityGroupReplicaData.ReplicaName);
			availabilityReplica.EndpointUrl = availabilityGroupReplica.AvailabilityGroupReplicaData.EndpointUrl;
			availabilityReplica.FailoverMode = availabilityGroupReplica.AvailabilityGroupReplicaData.GetFailoverMode(availabilityGroupData.ClusterType);
			availabilityReplica.AvailabilityMode = availabilityGroupReplica.AvailabilityGroupReplicaData.AvailabilityMode;
			availabilityReplica.ConnectionModeInSecondaryRole = availabilityGroupReplica.ReadableSecondaryRole;
			availabilityReplica.BackupPriority = availabilityGroupReplica.BackupPriority;
			availabilityReplica.ReadonlyRoutingConnectionUrl = availabilityGroupReplica.AvailabilityGroupReplicaData.ReadOnlyRoutingUrl ?? string.Empty;
			availabilityReplica.SetLoadBalancedReadOnlyRoutingList(availabilityGroupReplica.AvailabilityGroupReplicaData.ReadOnlyRoutingList);
			if (Utils.IsSql13OrLater(availabilityGroupData.PrimaryServer.VersionMajor))
			{
				availabilityReplica.SeedingMode = ((availabilityGroupData.PerformDataSynchronization != DataSynchronizationOption.AutomaticSeeding) ? AvailabilityReplicaSeedingMode.Manual : AvailabilityReplicaSeedingMode.Automatic);
			}
			availabilityGroup.AvailabilityReplicas.Add(availabilityReplica);
		}
		availabilityGroup.Create();
		availabilityGroupData.SqlAvailabilityGroup = availabilityGroup;
		policy.Expired = true;
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
