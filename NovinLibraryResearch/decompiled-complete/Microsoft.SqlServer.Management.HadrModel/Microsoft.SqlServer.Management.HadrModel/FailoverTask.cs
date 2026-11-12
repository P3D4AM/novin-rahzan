using System;
using System.Globalization;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class FailoverTask : HadrTask
{
	private FailoverData failoverData;

	public FailoverTask(FailoverData failoverData)
		: base((failoverData.TargetReplicaFailoverCategory == FailoverCategory.FailoverWithDataLoss) ? string.Format(CultureInfo.InvariantCulture, Resource.ForcedFailoverTaskText, failoverData.TargetAvailabilityReplica.Name) : string.Format(CultureInfo.InvariantCulture, Resource.ManualFailoverTaskText, failoverData.TargetAvailabilityReplica.Name))
	{
		if (failoverData == null)
		{
			throw new ArgumentNullException("failoverData");
		}
		this.failoverData = failoverData;
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		if (failoverData.TargetReplicaFailoverCategory == FailoverCategory.FailoverWithoutDataLoss)
		{
			failoverData.TargetAvailabilityGroup.Failover();
		}
		else
		{
			failoverData.TargetAvailabilityGroup.FailoverWithPotentialDataLoss();
		}
		if (failoverData.AvailabilityGroup.ClusterTypeWithDefault == AvailabilityGroupClusterType.None && failoverData.PrimaryServer != null)
		{
			failoverData.PrimaryServer.AvailabilityGroups[failoverData.AvailabilityGroupName]?.DemoteAsSecondary();
		}
		policy.Expired = true;
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
