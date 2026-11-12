using System;
using System.Globalization;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class FailoverValidator : Validator
{
	private FailoverData failoverData;

	public FailoverValidator(FailoverData failoverData)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.ValidatingFailoverSettings, failoverData.TargetAvailabilityReplica.Name))
	{
		if (failoverData == null)
		{
			throw new ArgumentNullException("failoverData");
		}
		this.failoverData = failoverData;
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		if (failoverData.TargetReplicaFailoverCategory == FailoverCategory.FailoverWithoutDataLoss)
		{
			if (failoverData.TargetAvailabilityReplica.Role == AvailabilityReplicaRole.Primary)
			{
				throw new FailoverValidationException(failoverData.TargetAvailabilityReplica.Name, failoverData.TargetAvailabilityReplica.Role.ToString());
			}
		}
		else if (failoverData.TargetAvailabilityReplica.Role == AvailabilityReplicaRole.Primary)
		{
			throw new FailoverValidationException(failoverData.TargetAvailabilityReplica.Name, failoverData.TargetAvailabilityReplica.Role.ToString());
		}
		policy.Expired = true;
	}
}
