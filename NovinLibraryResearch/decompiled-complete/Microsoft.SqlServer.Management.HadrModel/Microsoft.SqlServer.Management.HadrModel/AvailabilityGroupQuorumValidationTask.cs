using System;
using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AvailabilityGroupQuorumValidationTask : HadrTask
{
	private AvailabilityGroupData availabilityGroupData;

	public AvailabilityGroupQuorumValidationTask(AvailabilityGroupData availabilityGroupData)
		: base(Resource.QuorumValidationTaskText)
	{
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		this.availabilityGroupData = availabilityGroupData;
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		policy.Expired = true;
		try
		{
			if (!QuorumHelper.ValidateQuorumVoteConfiguration(availabilityGroupData.SqlAvailabilityGroup))
			{
				throw new AvailabilityGroupQuorumValidationTaskException(availabilityGroupData.SqlAvailabilityGroup.Name);
			}
		}
		catch (QuorumHelperException inner)
		{
			throw new AvailabilityGroupQuorumValidationTaskException(availabilityGroupData.SqlAvailabilityGroup.Name, inner);
		}
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
