using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public abstract class DatabaseFileCompatibilityValidator : Validator
{
	private readonly AvailabilityGroupData availabilityGroupData;

	private readonly AvailabilityGroupReplica replica;

	public AvailabilityGroupData AvailabilityGroupData => availabilityGroupData;

	public List<PrimaryDatabaseData> DatabasesToValidate { get; protected set; }

	protected DatabaseFileCompatibilityValidator(string name, AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica replica)
		: base(name)
	{
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		if (replica == null)
		{
			throw new ArgumentNullException("replica");
		}
		this.availabilityGroupData = availabilityGroupData;
		this.replica = replica;
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		policy.Expired = true;
		if (availabilityGroupData.PerformDataSynchronization != DataSynchronizationOption.AutomaticSeeding && availabilityGroupData.PerformDataSynchronization != 0)
		{
			return;
		}
		Microsoft.SqlServer.Management.Smo.Server primaryServer = availabilityGroupData.PrimaryServer;
		Microsoft.SqlServer.Management.Smo.Server secondaryServer = replica.GetServer();
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		foreach (PrimaryDatabaseData item in DatabasesToValidate)
		{
			Database database = primaryServer.Databases[item.Name];
			foreach (FileGroup fileGroup in database.FileGroups)
			{
				list2.AddRange(from DataFile df in fileGroup.Files
					select df.FileName);
			}
			list3.AddRange(from LogFile lf in database.LogFiles
				select lf.FileName);
		}
		if (availabilityGroupData.PerformDataSynchronization == DataSynchronizationOption.AutomaticSeeding)
		{
			string[] array = list2.Where((string file) => !file.StartsWith(primaryServer.DefaultFile)).ToArray();
			string[] array2 = list3.Where((string file) => !file.StartsWith(primaryServer.DefaultLog)).ToArray();
			if (AvailabilityGroupData.IsCrossPlatform && (array2.Any() || array.Any()))
			{
				if (replica.AvailabilityGroupReplicaData.InitialRole == ReplicaRole.Primary)
				{
					throw new DatabaseFileNotInDefaultDirectoryException(primaryServer.DefaultFile, primaryServer.DefaultLog, array.Union(array2));
				}
			}
			else if (Utils.IsSql14OrLater(primaryServer.VersionMajor) && !primaryServer.IsTraceFlagOn(9571, isGlobalTraceFlag: true))
			{
				list2 = list2.Select((string file) => file.Replace(primaryServer.DefaultFile, secondaryServer.DefaultFile)).ToList();
				list3 = list3.Select((string file) => file.Replace(primaryServer.DefaultLog, secondaryServer.DefaultLog)).ToList();
			}
		}
		foreach (string item2 in list2.Union(list3))
		{
			string directoryName = PathWrapper.GetDirectoryName(item2);
			if (!list.Contains(directoryName) && !secondaryServer.ParentDirectoryExists(item2))
			{
				list.Add(directoryName);
			}
		}
		if (list.Count > 0)
		{
			throw new DatabaseFileLocationMissingOnReplicaException(replica.AvailabilityGroupReplicaData.ReplicaName, list);
		}
	}
}
