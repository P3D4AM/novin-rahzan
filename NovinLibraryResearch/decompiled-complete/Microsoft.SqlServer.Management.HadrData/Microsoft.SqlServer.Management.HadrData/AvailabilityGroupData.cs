using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrData;

public class AvailabilityGroupData
{
	public AvailabilityGroupListenerConfiguration AvailabilityGroupListener { get; set; }

	public Microsoft.SqlServer.Management.Smo.Server PrimaryServer { get; private set; }

	public string GroupName { get; set; }

	public AvailabilityGroupClusterType ClusterType { get; set; }

	public int RequiredSynchronizedSecondariesToCommit { get; set; }

	public string BackupLocationInWindowsFormat { get; set; }

	public string BackupLocationInLinuxFormat { get; set; }

	public List<PrimaryDatabaseData> ExistingAvailabilityDatabases { get; private set; }

	public List<PrimaryDatabaseData> NewAvailabilityDatabases { get; private set; }

	public IList<AvailabilityGroupReplica> AvailabilityGroupReplicas { get; set; }

	public IEnumerable<AvailabilityGroupReplica> Secondaries => AvailabilityGroupReplicas.Where((AvailabilityGroupReplica replica) => replica.AvailabilityGroupReplicaData.InitialRole == ReplicaRole.Secondary);

	public IEnumerable<AvailabilityGroupReplica> DataSecondaries => AvailabilityGroupReplicas.Where((AvailabilityGroupReplica replica) => replica.AvailabilityGroupReplicaData.InitialRole == ReplicaRole.Secondary && replica.AvailabilityGroupReplicaData.AvailabilityMode != AvailabilityReplicaAvailabilityMode.ConfigurationOnly);

	public AvailabilityGroupAutomatedBackupPreference BackupPreference { get; set; }

	public bool IsBasic { get; set; }

	public bool IsDatabaseHealthTriggerOn { get; set; }

	public bool IsDtcSupportEnabled { get; set; }

	public DataSynchronizationOption PerformDataSynchronization { get; set; }

	public bool WillPerformBackupRestore => PerformDataSynchronization == DataSynchronizationOption.Full;

	public bool WillPerformDatabaseJoin
	{
		get
		{
			if (PerformDataSynchronization != 0 && PerformDataSynchronization != DataSynchronizationOption.JoinOnly)
			{
				return PerformDataSynchronization == DataSynchronizationOption.AutomaticSeeding;
			}
			return true;
		}
	}

	public bool WillPerformDatabaseInitialization
	{
		get
		{
			if (PerformDataSynchronization != 0)
			{
				return PerformDataSynchronization == DataSynchronizationOption.AutomaticSeeding;
			}
			return true;
		}
	}

	public bool WillPerformAutomaticSeeding => PerformDataSynchronization == DataSynchronizationOption.AutomaticSeeding;

	public string ClusterName { get; set; }

	public bool IsPrimaryInQuorum
	{
		get
		{
			ClusterQuorumState clusterQuorumState = PrimaryServer.ClusterQuorumState;
			if (clusterQuorumState != ClusterQuorumState.NotApplicable)
			{
				return clusterQuorumState != ClusterQuorumState.UnknownQuorumState;
			}
			return false;
		}
	}

	public AvailabilityObjectState AvailabilityGroupState { get; private set; }

	public bool IsConnectedToPrimary
	{
		get
		{
			if (PrimaryServer != null && PrimaryServer.ConnectionContext != null)
			{
				AvailabilityGroup availabilityGroup = PrimaryServer.AvailabilityGroups[GroupName];
				if (availabilityGroup != null)
				{
					string trueName = PrimaryServer.ConnectionContext.TrueName;
					return string.Equals(availabilityGroup.PrimaryReplicaServerName, trueName, StringComparison.OrdinalIgnoreCase);
				}
			}
			return false;
		}
	}

	public AvailabilityGroup SqlAvailabilityGroup { get; set; }

	public bool IsCrossPlatform => Secondaries.Any((AvailabilityGroupReplica replica) => replica.GetServer().HostPlatform != PrimaryServer.HostPlatform);

	public AvailabilityGroupData()
	{
		BackupPreference = AvailabilityGroupAutomatedBackupPreference.Secondary;
		IsBasic = false;
		IsDatabaseHealthTriggerOn = true;
		IsDtcSupportEnabled = true;
		AvailabilityGroupReplicas = null;
		PerformDataSynchronization = DataSynchronizationOption.Full;
		RequiredSynchronizedSecondariesToCommit = 0;
	}

	public AvailabilityGroupData(ServerConnection primaryConnection)
		: this()
	{
		if (primaryConnection == null)
		{
			throw new ArgumentNullException("primaryConnection");
		}
		PrimaryServer = new Microsoft.SqlServer.Management.Smo.Server(primaryConnection);
		AvailabilityGroupState = AvailabilityObjectState.Creating;
		AvailabilityGroupReplicas = new AvailabilityGroupReplicaCollection();
		ExistingAvailabilityDatabases = new List<PrimaryDatabaseData>();
		NewAvailabilityDatabases = new List<PrimaryDatabaseData>();
		AvailabilityGroupListener = null;
		RequiredSynchronizedSecondariesToCommit = 0;
	}

	public bool HasViewServerStatePermission(Microsoft.SqlServer.Management.Smo.Server server)
	{
		return HasPermissionOnServer(server, "VIEW SERVER STATE");
	}

	public IEnumerable<KeyValuePair<string, bool>> GetLoginNames(AvailabilityGroupReplica replica)
	{
		if (replica.AvailabilityGroupReplicaData.Connection.HostPlatform != "Windows")
		{
			return Enumerable.Empty<KeyValuePair<string, bool>>();
		}
		return replica.GetLoginsForRemoteReplicas(GetUniqueServiceAccountSids(replica));
	}

	public void GetPrimaryTotalDataFileAndLogSize(out double totalDataFilesSize, out double totalLogFilesSize)
	{
		totalDataFilesSize = 0.0;
		totalLogFilesSize = 0.0;
		List<string> list = new List<string>();
		if (NewAvailabilityDatabases.Count != 0)
		{
			foreach (PrimaryDatabaseData newAvailabilityDatabase in NewAvailabilityDatabases)
			{
				list.Add(newAvailabilityDatabase.Name);
			}
		}
		else
		{
			AvailabilityGroup availabilityGroup = PrimaryServer.AvailabilityGroups[GroupName];
			if (availabilityGroup == null)
			{
				throw new InvalidOperationException(string.Format(Resource.AvailabilityGroupNotExistError, GroupName, PrimaryServer.Name));
			}
			foreach (AvailabilityDatabase availabilityDatabase in availabilityGroup.AvailabilityDatabases)
			{
				list.Add(availabilityDatabase.Name);
			}
		}
		foreach (string item in list)
		{
			double num = 0.0;
			double num2 = 0.0;
			Database database = PrimaryServer.Databases[item];
			foreach (FileGroup fileGroup in database.FileGroups)
			{
				num += fileGroup.Size / 1024.0;
			}
			totalDataFilesSize += num;
			num2 = database.Size - num;
			totalLogFilesSize += num2;
		}
	}

	public void AddAvailabilityDatabase(PrimaryDatabaseData database)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		NewAvailabilityDatabases.Add(database);
	}

	public void AddExistingAvailabilityDatabase(PrimaryDatabaseData database)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		ExistingAvailabilityDatabases.Add(database);
	}

	public string GetBackupPathForServer(Microsoft.SqlServer.Management.Smo.Server server)
	{
		if (!(server.HostPlatform == "Windows"))
		{
			return BackupLocationInLinuxFormat;
		}
		return BackupLocationInWindowsFormat;
	}

	private IEnumerable<KeyValuePair<byte[], string>> GetUniqueServiceAccountSids(AvailabilityGroupReplica replica)
	{
		Dictionary<byte[], string> dictionary = new Dictionary<byte[], string>();
		foreach (AvailabilityGroupReplica additionalReplica in AvailabilityGroupReplicas)
		{
			if (replica != additionalReplica && additionalReplica.AvailabilityGroupReplicaData.EndpointServiceAccountSid != null && !dictionary.Any((KeyValuePair<byte[], string> x) => x.Key.SequenceEqual(additionalReplica.AvailabilityGroupReplicaData.EndpointServiceAccountSid)))
			{
				dictionary.Add(additionalReplica.AvailabilityGroupReplicaData.EndpointServiceAccountSid, additionalReplica.AvailabilityGroupReplicaData.EndpointServiceAccount);
			}
		}
		return dictionary;
	}

	private static bool HasPermissionOnServer(Microsoft.SqlServer.Management.Smo.Server server, string permissionName)
	{
		return Convert.ToBoolean(server.ConnectionContext.ExecuteScalar(string.Format(CultureInfo.InvariantCulture, "SELECT HAS_PERMS_BY_NAME(null, null, '{0}');", permissionName)));
	}
}
