using System;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class JoinDatabaseToAvailabilityGroupTask : HadrTask
{
	private const int JoinAvailabilityGroupRetryTimes = 10;

	private const int JoinAvailabilityGroupSleepBetweenRetries = 10;

	private readonly AvailabilityGroupData availabilityGroupData;

	private readonly AvailabilityGroupReplica replica;

	private readonly string databaseName;

	public string DatabaseName => databaseName;

	public AvailabilityGroupReplica ReplicaData => replica;

	public AvailabilityGroupData AvailabilityGroupData => availabilityGroupData;

	public JoinDatabaseToAvailabilityGroupTask(string databaseName, AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica replica)
		: base(string.Format(Resource.JoinDatabaseToAvailabilityGroupText, databaseName, availabilityGroupData.GroupName, replica.AvailabilityGroupReplicaData.ReplicaName))
	{
		if (databaseName == null)
		{
			throw new ArgumentNullException("databaseName");
		}
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		if (replica == null)
		{
			throw new ArgumentNullException("replica");
		}
		this.databaseName = databaseName;
		this.availabilityGroupData = availabilityGroupData;
		this.replica = replica;
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		Microsoft.SqlServer.Management.Smo.Server newSmoServerObject = HadrModelUtilities.GetNewSmoServerObject(replica.AvailabilityGroupReplicaData.Connection);
		AvailabilityDatabase availabilityDatabase = (newSmoServerObject.AvailabilityGroups[availabilityGroupData.GroupName] ?? throw new AvailabilityGroupNotJoinedOnReplicaException(availabilityGroupData.GroupName, newSmoServerObject.Name)).AvailabilityDatabases[databaseName];
		availabilityDatabase.JoinAvailablityGroup();
		if (JoinDatabaseToAvailabilityGroup(availabilityDatabase))
		{
			policy.Expired = true;
		}
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}

	private bool JoinDatabaseToAvailabilityGroup(AvailabilityDatabase availabilityDatabase)
	{
		if (!availabilityDatabase.IsJoined)
		{
			availabilityDatabase.JoinAvailablityGroup();
			return false;
		}
		return true;
	}
}
