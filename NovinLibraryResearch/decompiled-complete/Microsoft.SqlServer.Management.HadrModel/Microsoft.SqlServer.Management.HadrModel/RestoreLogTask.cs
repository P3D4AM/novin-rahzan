using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class RestoreLogTask : HadrTask, IScriptableTask
{
	private Restore restore;

	private AvailabilityGroupData availabilityGroupData;

	private readonly string databaseName;

	private readonly AvailabilityGroupReplica replica;

	public List<ServerConnection> ScriptingConnections { get; private set; }

	public AvailabilityGroupData AvailabilityGroupData => availabilityGroupData;

	public AvailabilityGroupReplica ReplicaData => replica;

	public RestoreLogTask(string databaseName, AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica replica)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.RestoreLogText, databaseName, replica.AvailabilityGroupReplicaData.ReplicaName))
	{
		if (string.IsNullOrEmpty(databaseName))
		{
			throw new ArgumentNullException("databaseName");
		}
		this.databaseName = databaseName;
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		if (replica == null)
		{
			throw new ArgumentNullException("replica");
		}
		this.replica = replica;
		this.availabilityGroupData = availabilityGroupData;
		ScriptingConnections = new List<ServerConnection> { this.replica.AvailabilityGroupReplicaData.Connection };
	}

	public void Abort()
	{
		if (restore != null)
		{
			restore.Abort();
		}
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		int statementTimeout = replica.AvailabilityGroupReplicaData.Connection.StatementTimeout;
		replica.AvailabilityGroupReplicaData.Connection.StatementTimeout = 0;
		try
		{
			Microsoft.SqlServer.Management.Smo.Server newSmoServerObject = HadrModelUtilities.GetNewSmoServerObject(replica.AvailabilityGroupReplicaData.Connection);
			restore = new Restore
			{
				Action = RestoreActionType.Log,
				Database = databaseName,
				FileNumber = 0,
				NoRecovery = true
			};
			string logBackupFileFullName = AvailabilityGroupData.GetLogBackupFileFullName(newSmoServerObject, databaseName);
			PercentCompleteHandler percentCompleteHandler = new PercentCompleteHandler(base.Name);
			percentCompleteHandler.TaskProgressEventHandler += base.TaskUpdateEventHandler;
			restore.PercentCompleteNotification = 5;
			restore.PercentComplete += percentCompleteHandler.percentCompleteHandler;
			restore.Devices.AddDevice(logBackupFileFullName, DeviceType.File);
			restore.SqlRestore(newSmoServerObject);
			percentCompleteHandler.TaskProgressEventHandler -= base.TaskUpdateEventHandler;
			policy.Expired = true;
		}
		catch (Exception inner)
		{
			throw new RestoreLogTaskException(databaseName, inner);
		}
		finally
		{
			restore = null;
			replica.AvailabilityGroupReplicaData.Connection.StatementTimeout = statementTimeout;
		}
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
