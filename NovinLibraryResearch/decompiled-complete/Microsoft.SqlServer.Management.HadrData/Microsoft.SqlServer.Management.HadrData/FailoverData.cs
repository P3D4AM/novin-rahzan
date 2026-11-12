using System;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrData;

public class FailoverData
{
	private Microsoft.SqlServer.Management.Smo.Server entryPointServer;

	private AvailabilityReplicaCollection replicas;

	public ServerConnection EntryPointServerConnection { get; set; }

	public Microsoft.SqlServer.Management.Smo.Server EntryPointServer
	{
		get
		{
			if (entryPointServer == null)
			{
				entryPointServer = FailoverUtilities.GetServerWithInitFieldsSetting(EntryPointServerConnection);
			}
			return entryPointServer;
		}
	}

	public string EntryPointServerTrueName => EntryPointServer.ConnectionContext.TrueName;

	public string PrimaryServerInstanceName => AvailabilityGroup.PrimaryReplicaServerName;

	public AvailabilityGroup AvailabilityGroup => EntryPointServer.AvailabilityGroups[AvailabilityGroupName] ?? throw new ArgumentNullException();

	public AvailabilityReplicaCollection AvailabilityReplicas
	{
		get
		{
			if (replicas == null)
			{
				replicas = AvailabilityGroup.AvailabilityReplicas;
				replicas.Refresh();
			}
			return replicas;
		}
	}

	public AvailabilityReplica PrimaryReplica => AvailabilityReplicas[PrimaryServerInstanceName];

	public string AvailabilityGroupName { get; set; }

	public FailoverCategory TargetReplicaFailoverCategory { get; set; }

	public Microsoft.SqlServer.Management.Smo.Server TargetServer { get; set; }

	public Microsoft.SqlServer.Management.Smo.Server PrimaryServer { get; set; }

	public ServerConnection TargetServerConnection { get; set; }

	public AvailabilityGroup TargetAvailabilityGroup => TargetServer.AvailabilityGroups[AvailabilityGroupName] ?? throw new InvalidOperationException(string.Format(Resource.AvailabilityGroupNotExistError, AvailabilityGroupName, TargetServer.Name));

	public string TargetReplicaName { get; set; }

	public AvailabilityReplica TargetAvailabilityReplica => TargetAvailabilityGroup.AvailabilityReplicas[TargetReplicaName] ?? throw new InvalidOperationException(Resource.FailoverTargetReplicaNotExistError);

	protected void SetEntryPointServerToNull()
	{
		entryPointServer = null;
		replicas = null;
	}

	public void ResetTargetServer()
	{
		TargetServer = null;
	}
}
