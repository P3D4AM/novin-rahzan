using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public abstract class DatabaseFileExistenceValidator : Validator
{
	private readonly AvailabilityGroupData availabilityGroupData;

	private readonly AvailabilityGroupReplica replica;

	public AvailabilityGroupData AvailabilityGroupData => availabilityGroupData;

	public List<PrimaryDatabaseData> DatabasesToValidate { get; protected set; }

	protected DatabaseFileExistenceValidator(string name, AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica replica)
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
		List<string> list = new List<string>();
		Microsoft.SqlServer.Management.Smo.Server primaryServer = availabilityGroupData.PrimaryServer;
		Microsoft.SqlServer.Management.Smo.Server server = replica.GetServer();
		foreach (PrimaryDatabaseData item in DatabasesToValidate)
		{
			Database database = primaryServer.Databases[item.Name];
			foreach (FileGroup fileGroup in database.FileGroups)
			{
				foreach (DataFile file in fileGroup.Files)
				{
					if (server.FileExists(file.FileName))
					{
						list.Add(file.FileName);
					}
				}
			}
			foreach (LogFile logFile in database.LogFiles)
			{
				if (server.FileExists(logFile.FileName))
				{
					list.Add(logFile.FileName);
				}
			}
		}
		if (list.Count > 0)
		{
			throw new DatabaseFileAlreadyExistsOnReplicaException(replica.AvailabilityGroupReplicaData.ReplicaName, list);
		}
	}
}
