using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class CreateAvailabilityGroupScenarioProvider : ScenarioProvider<IValidatorProvider, ITasksProvider>
{
	public AvailabilityGroupData AvailabilityGroupData { get; private set; }

	public CreateAvailabilityGroupScenarioProvider(AvailabilityGroupData data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (!data.PrimaryServer.IsHadrEnabled)
		{
			throw new ArgumentException(string.Format(Resource.PrimaryServerNotHadrEnabled, data.PrimaryServer.Name));
		}
		if (!data.HasViewServerStatePermission(data.PrimaryServer))
		{
			throw new ArgumentException(Resource.UserDoesNotHaveViewServerStatePermission);
		}
		if (!data.IsPrimaryInQuorum)
		{
			throw new ArgumentException(Resource.WizardLaunchFailureQuorumLoss);
		}
		AvailabilityGroupData = data;
	}

	public override List<Validator> Validators()
	{
		List<Validator> list = new List<Validator>();
		foreach (AvailabilityGroupReplica dataSecondary in AvailabilityGroupData.DataSecondaries)
		{
			list.Add(new FreeDiskSpaceValidator(AvailabilityGroupData, dataSecondary));
			list.Add(new CreateAvailabilityGroupDatabaseExistenceValidator(AvailabilityGroupData, dataSecondary));
			list.Add(new CreateAvailabilityGroupDatabaseFileExistenceValidator(AvailabilityGroupData, dataSecondary));
			list.Add(new CreateAvailabilityGroupDatabaseFileCompatibilityValidator(AvailabilityGroupData, dataSecondary));
		}
		list.Add(new CompatibleEncryptionValidator(AvailabilityGroupData));
		if (AvailabilityGroupData.WillPerformBackupRestore)
		{
			list.Add(new BackupLocationValidator(AvailabilityGroupData));
		}
		list.Add(new AvailabilityModeValidator(AvailabilityGroupData));
		if (FailoverUtilities.HasDbNeedToBeDecrypted(AvailabilityGroupData.PrimaryServer, AvailabilityGroupData.NewAvailabilityDatabases))
		{
			list.Add(new DatabaseMasterKeyValidator(AvailabilityGroupData));
		}
		if (AvailabilityGroupData.IsBasic)
		{
			list.Add(new BasicAvailabilityGroupValidator(AvailabilityGroupData));
		}
		list.Add(new ListenerConfigurationValidator(AvailabilityGroupData));
		return list;
	}

	public override List<HadrTask> RollbackTasks()
	{
		return null;
	}

	public override List<HadrTask> Tasks()
	{
		List<HadrTask> list = new List<HadrTask>();
		foreach (AvailabilityGroupReplica availabilityGroupReplica in AvailabilityGroupData.AvailabilityGroupReplicas)
		{
			List<KeyValuePair<string, bool>> source = AvailabilityGroupData.GetLoginNames(availabilityGroupReplica).ToList();
			List<string> list2 = (from pair in source
				where !pair.Value
				select pair.Key).ToList();
			if (list2.Any())
			{
				list.Add(new CreateLoginTask(availabilityGroupReplica, list2));
			}
			list.Add(new ConfigureEndpointsTask(availabilityGroupReplica, source.Select((KeyValuePair<string, bool> x) => x.Key).ToList()));
			list.Add(new StartAlwaysOnXeventSessionTask(availabilityGroupReplica));
		}
		list.Add(new CreateAvailabilityGroupTask(AvailabilityGroupData));
		list.Add(new WaitForAvailabilityGroupOnlineTask(AvailabilityGroupData));
		if (AvailabilityGroupData.AvailabilityGroupListener != null)
		{
			list.Add(new CreateAvailabilityGroupListenerTask(AvailabilityGroupData));
		}
		list.Add(new JoinSecondariesTask(AvailabilityGroupData));
		if (AvailabilityGroupData.ClusterType == AvailabilityGroupClusterType.Wsfc)
		{
			list.Add(new AvailabilityGroupQuorumValidationTask(AvailabilityGroupData));
		}
		if (AvailabilityGroupData.DataSecondaries.Any() && AvailabilityGroupData.PerformDataSynchronization != DataSynchronizationOption.AutomaticSeeding)
		{
			foreach (PrimaryDatabaseData newAvailabilityDatabase in AvailabilityGroupData.NewAvailabilityDatabases)
			{
				list.Add(new BackupDatabaseTask(newAvailabilityDatabase.Name, AvailabilityGroupData));
				foreach (AvailabilityGroupReplica dataSecondary in AvailabilityGroupData.DataSecondaries)
				{
					list.Add(new RestoreDatabaseTask(newAvailabilityDatabase.Name, AvailabilityGroupData, dataSecondary));
				}
				list.Add(new BackupLogTask(newAvailabilityDatabase.Name, AvailabilityGroupData));
				Database database = AvailabilityGroupData.PrimaryServer.Databases[newAvailabilityDatabase.Name];
				bool flag = database.IsAccessible && database.MasterKey != null && newAvailabilityDatabase.DBMKPassword != null;
				foreach (AvailabilityGroupReplica dataSecondary2 in AvailabilityGroupData.DataSecondaries)
				{
					list.Add(new RestoreLogTask(newAvailabilityDatabase.Name, AvailabilityGroupData, dataSecondary2));
					list.Add(new JoinDatabaseToAvailabilityGroupTask(newAvailabilityDatabase.Name, AvailabilityGroupData, dataSecondary2));
					if (flag)
					{
						list.Add(new AddDBCredentialTask(newAvailabilityDatabase.Name, AvailabilityGroupData, dataSecondary2));
					}
				}
				if (flag)
				{
					AvailabilityGroupReplica replica2 = AvailabilityGroupData.AvailabilityGroupReplicas.Single((AvailabilityGroupReplica replica) => replica.AvailabilityGroupReplicaData.InitialRole == ReplicaRole.Primary);
					list.Add(new AddDBCredentialTask(newAvailabilityDatabase.Name, AvailabilityGroupData, replica2));
				}
			}
		}
		return list;
	}
}
