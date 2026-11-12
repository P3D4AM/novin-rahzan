using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class ConfigureEndpointsTask : HadrTask, IScriptableTask
{
	private readonly AvailabilityGroupReplica replica;

	private readonly IEnumerable<string> loginNames;

	public List<ServerConnection> ScriptingConnections { get; private set; }

	public ConfigureEndpointsTask(AvailabilityGroupReplica replica, IEnumerable<string> loginNames)
		: base(Resource.ConfigureEndpointsText)
	{
		if (replica == null)
		{
			throw new ArgumentNullException("replica");
		}
		if (loginNames == null)
		{
			throw new ArgumentNullException("loginNames");
		}
		this.replica = replica;
		this.loginNames = loginNames;
		ScriptingConnections = new List<ServerConnection> { this.replica.AvailabilityGroupReplicaData.Connection };
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		policy.Expired = true;
		replica.ConfigureEndpoint();
		replica.AddGrantServiceAccount(loginNames);
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
