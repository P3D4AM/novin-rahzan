using System;
using System.Globalization;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.XEvent;

namespace Microsoft.SqlServer.Management.HadrModel;

public class StartAlwaysOnXeventSessionTask : HadrTask
{
	public const string AlwaysOnHealthSessionName = "AlwaysOn_health";

	private XEStore store;

	private SqlStoreConnection storeConnection;

	public AvailabilityGroupReplica Replica { get; private set; }

	public StartAlwaysOnXeventSessionTask(AvailabilityGroupReplica replica)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.StartAlwaysOnXeventSessionText, "AlwaysOn_health", replica.AvailabilityGroupReplicaData.ReplicaName))
	{
		if (replica == null)
		{
			throw new ArgumentNullException("replica");
		}
		Replica = replica;
		storeConnection = new SqlStoreConnection(replica.AvailabilityGroupReplicaData.Connection.SqlConnectionObject);
		store = new XEStore(storeConnection);
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		UpdateStatus(new TaskEventArgs(base.Name, string.Format(Resource.EnablingXeventOnReplica, "AlwaysOn_health", Replica.AvailabilityGroupReplicaData.ReplicaName), TaskEventStatus.Running));
		policy.Expired = true;
		if (store.Sessions.TryGetValue(new Session.Key("AlwaysOn_health"), out var obj))
		{
			obj.AutoStart = true;
			obj.Alter();
			if (!obj.IsRunning)
			{
				obj.Start();
			}
		}
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
