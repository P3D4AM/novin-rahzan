using System;
using System.Globalization;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class FailoverWaitRoleChangeValidator : Validator
{
	private FailoverData failoverData;

	public FailoverWaitRoleChangeValidator(FailoverData failoverData)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.WaitForRoleChange, failoverData.TargetAvailabilityReplica.Name))
	{
		if (failoverData == null)
		{
			throw new ArgumentNullException("failoverData");
		}
		this.failoverData = failoverData;
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		failoverData.TargetAvailabilityReplica.Refresh();
		if (failoverData.TargetAvailabilityReplica.Role == AvailabilityReplicaRole.Primary)
		{
			policy.Expired = true;
		}
	}
}
