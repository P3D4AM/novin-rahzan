using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AddDatabaseToExistingAgScenarioProvider : ScenarioProvider<IValidatorProvider, ITasksProvider>
{
	public AvailabilityGroupData AvailabilityGroupData { get; private set; }

	public AddDatabaseToExistingAgScenarioProvider(AvailabilityGroupData data)
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
		if (AvailabilityGroupData.WillPerformBackupRestore)
		{
			list.Add(new BackupLocationValidator(AvailabilityGroupData));
		}
		if (AvailabilityGroupData.IsBasic)
		{
			list.Add(new BasicAvailabilityGroupValidator(AvailabilityGroupData));
		}
		if (FailoverUtilities.HasDbNeedToBeDecrypted(AvailabilityGroupData.PrimaryServer, AvailabilityGroupData.NewAvailabilityDatabases))
		{
			list.Add(new DatabaseMasterKeyValidator(AvailabilityGroupData));
		}
		foreach (AvailabilityGroupReplica availabilityGroupReplica in AvailabilityGroupData.AvailabilityGroupReplicas)
		{
			if (availabilityGroupReplica.AvailabilityGroupReplicaData.InitialRole == ReplicaRole.Secondary)
			{
				list.Add(new FreeDiskSpaceValidator(AvailabilityGroupData, availabilityGroupReplica));
				list.Add(new CreateAvailabilityGroupDatabaseExistenceValidator(AvailabilityGroupData, availabilityGroupReplica));
				list.Add(new CreateAvailabilityGroupDatabaseFileExistenceValidator(AvailabilityGroupData, availabilityGroupReplica));
				list.Add(new CreateAvailabilityGroupDatabaseFileCompatibilityValidator(AvailabilityGroupData, availabilityGroupReplica));
			}
		}
		return list;
	}

	public override List<HadrTask> Tasks()
	{
		List<HadrTask> list = new List<HadrTask>();
		list.Add(new AddDatabaseToExistingAvailabilityGroupTask(AvailabilityGroupData));
		if (AvailabilityGroupData.Secondaries.Any() && AvailabilityGroupData.PerformDataSynchronization != DataSynchronizationOption.AutomaticSeeding)
		{
			foreach (PrimaryDatabaseData newAvailabilityDatabase in AvailabilityGroupData.NewAvailabilityDatabases)
			{
				if (AvailabilityGroupData.WillPerformBackupRestore)
				{
					list.Add(new BackupDatabaseTask(newAvailabilityDatabase.Name, AvailabilityGroupData));
					foreach (AvailabilityGroupReplica secondary in AvailabilityGroupData.Secondaries)
					{
						list.Add(new RestoreDatabaseTask(newAvailabilityDatabase.Name, AvailabilityGroupData, secondary));
					}
				}
				list.Add(new BackupLogTask(newAvailabilityDatabase.Name, AvailabilityGroupData));
				Database database = AvailabilityGroupData.PrimaryServer.Databases[newAvailabilityDatabase.Name];
				bool flag = database.IsAccessible && database.MasterKey != null && newAvailabilityDatabase.DBMKPassword != null;
				foreach (AvailabilityGroupReplica secondary2 in AvailabilityGroupData.Secondaries)
				{
					if (AvailabilityGroupData.WillPerformBackupRestore || AvailabilityGroupData.WillPerformDatabaseJoin)
					{
						list.Add(new RestoreLogTask(newAvailabilityDatabase.Name, AvailabilityGroupData, secondary2));
						list.Add(new JoinDatabaseToAvailabilityGroupTask(newAvailabilityDatabase.Name, AvailabilityGroupData, secondary2));
						if (flag)
						{
							list.Add(new AddDBCredentialTask(newAvailabilityDatabase.Name, AvailabilityGroupData, secondary2));
						}
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

	public override List<HadrTask> RollbackTasks()
	{
		return null;
	}
}
