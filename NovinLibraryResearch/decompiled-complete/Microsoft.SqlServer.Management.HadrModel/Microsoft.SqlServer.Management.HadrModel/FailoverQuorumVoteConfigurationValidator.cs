using System;
using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class FailoverQuorumVoteConfigurationValidator : Validator
{
	private FailoverData failoverData;

	public FailoverQuorumVoteConfigurationValidator(FailoverData failoverData)
		: base(Resource.ValidatingWSFCQuorumConfiguration)
	{
		if (failoverData == null)
		{
			throw new ArgumentNullException("failoverData");
		}
		this.failoverData = failoverData;
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		if (QuorumHelper.ValidateQuorumVoteConfiguration(failoverData.TargetAvailabilityReplica.Parent))
		{
			policy.Expired = true;
		}
	}
}
