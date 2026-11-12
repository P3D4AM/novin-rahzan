using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class CreateLoginTask : HadrTask
{
	private readonly IEnumerable<string> windowsLogins;

	private readonly AvailabilityGroupReplica replica;

	public CreateLoginTask(AvailabilityGroupReplica replica, IEnumerable<string> windowsLogins)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.CreateLoginText, replica.AvailabilityGroupReplicaData.ReplicaName))
	{
		if (replica == null)
		{
			throw new ArgumentNullException("replica");
		}
		if (windowsLogins == null)
		{
			throw new ArgumentNullException("windowsLogins");
		}
		this.replica = replica;
		this.windowsLogins = windowsLogins;
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		UpdateStatus(new TaskEventArgs(base.Name, string.Format(Resource.CreatingLoginsOnReplica, string.Join(",", windowsLogins), replica.AvailabilityGroupReplicaData.ReplicaName), TaskEventStatus.Running));
		policy.Expired = true;
		replica.CreateLogins(windowsLogins);
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
