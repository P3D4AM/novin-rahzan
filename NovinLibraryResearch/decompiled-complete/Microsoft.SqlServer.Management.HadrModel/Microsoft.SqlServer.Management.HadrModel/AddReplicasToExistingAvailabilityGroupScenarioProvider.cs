using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AddReplicasToExistingAvailabilityGroupScenarioProvider : ScenarioProvider<IValidatorProvider, ITasksProvider>
{
	public AvailabilityGroupData AvailabilityGroupData { get; private set; }

	public AddReplicasToExistingAvailabilityGroupScenarioProvider(AvailabilityGroupData data)
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
		foreach (AvailabilityGroupReplica availabilityGroupReplica in AvailabilityGroupData.AvailabilityGroupReplicas)
		{
			if (availabilityGroupReplica.AvailabilityGroupReplicaData.InitialRole == ReplicaRole.Secondary && availabilityGroupReplica.AvailabilityGroupReplicaData.State == AvailabilityObjectState.Creating)
			{
				list.Add(new FreeDiskSpaceValidator(AvailabilityGroupData, availabilityGroupReplica));
				list.Add(new AddReplicaDatabaseExistenceValidator(AvailabilityGroupData, availabilityGroupReplica));
				list.Add(new AddReplicaDatabaseFileExistenceValidator(AvailabilityGroupData, availabilityGroupReplica));
				list.Add(new AddReplicaDatabaseFileCompatibilityValidator(AvailabilityGroupData, availabilityGroupReplica));
			}
		}
		list.Add(new CompatibleEncryptionValidator(AvailabilityGroupData));
		if (AvailabilityGroupData.WillPerformBackupRestore)
		{
			list.Add(new BackupLocationValidator(AvailabilityGroupData));
		}
		list.Add(new AvailabilityModeValidator(AvailabilityGroupData));
		if (AvailabilityGroupData.IsBasic)
		{
			list.Add(new BasicAvailabilityGroupValidator(AvailabilityGroupData));
		}
		if (FailoverUtilities.HasDbNeedToBeDecrypted(AvailabilityGroupData.PrimaryServer, AvailabilityGroupData.ExistingAvailabilityDatabases))
		{
			list.Add(new DatabaseMasterKeyValidator(AvailabilityGroupData)
			{
				ValidateExistingDbMode = true
			});
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
			if (availabilityGroupReplica.AvailabilityGroupReplicaData.State == AvailabilityObjectState.Creating)
			{
				list.Add(new StartAlwaysOnXeventSessionTask(availabilityGroupReplica));
			}
		}
		list.Add(new AddReplicaTask(AvailabilityGroupData));
		list.Add(new JoinSecondariesTask(AvailabilityGroupData));
		if (AvailabilityGroupData.DataSecondaries.Any() && AvailabilityGroupData.PerformDataSynchronization != DataSynchronizationOption.AutomaticSeeding)
		{
			foreach (PrimaryDatabaseData existingAvailabilityDatabase in AvailabilityGroupData.ExistingAvailabilityDatabases)
			{
				list.Add(new BackupDatabaseTask(existingAvailabilityDatabase.Name, AvailabilityGroupData));
				foreach (AvailabilityGroupReplica dataSecondary in AvailabilityGroupData.DataSecondaries)
				{
					if (dataSecondary.AvailabilityGroupReplicaData.State == AvailabilityObjectState.Creating)
					{
						list.Add(new RestoreDatabaseTask(existingAvailabilityDatabase.Name, AvailabilityGroupData, dataSecondary));
					}
				}
				list.Add(new BackupLogTask(existingAvailabilityDatabase.Name, AvailabilityGroupData));
				Database database = AvailabilityGroupData.PrimaryServer.Databases[existingAvailabilityDatabase.Name];
				bool flag = database.IsAccessible && database.MasterKey != null && existingAvailabilityDatabase.DBMKPassword != null;
				foreach (AvailabilityGroupReplica dataSecondary2 in AvailabilityGroupData.DataSecondaries)
				{
					if (dataSecondary2.AvailabilityGroupReplicaData.State != AvailabilityObjectState.Creating)
					{
						continue;
					}
					list.Add(new RestoreLogTask(existingAvailabilityDatabase.Name, AvailabilityGroupData, dataSecondary2));
					if (AvailabilityGroupData.PerformDataSynchronization == DataSynchronizationOption.JoinOnly)
					{
						list.Add(new JoinDatabaseToAvailabilityGroupTask(existingAvailabilityDatabase.Name, AvailabilityGroupData, dataSecondary2));
						if (flag)
						{
							list.Add(new AddDBCredentialTask(existingAvailabilityDatabase.Name, AvailabilityGroupData, dataSecondary2)
							{
								UseExistingAvailabilityDatabases = true
							});
						}
					}
				}
			}
		}
		return list;
	}
}
