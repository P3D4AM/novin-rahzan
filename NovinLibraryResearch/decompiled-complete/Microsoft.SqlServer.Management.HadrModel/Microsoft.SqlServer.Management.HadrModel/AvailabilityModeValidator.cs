using System;
using System.Linq;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AvailabilityModeValidator : Validator
{
	private readonly AvailabilityGroupData availabilityGroupData;

	public AvailabilityModeValidator(AvailabilityGroupData availabilityGroupData)
		: base(Resource.ValidatingAvailabilityMode)
	{
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		this.availabilityGroupData = availabilityGroupData;
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		policy.Expired = true;
		if (availabilityGroupData.AvailabilityGroupReplicas.First((AvailabilityGroupReplica replica) => replica.AvailabilityGroupReplicaData.InitialRole == ReplicaRole.Primary).AvailabilityGroupReplicaData.AvailabilityMode == AvailabilityReplicaAvailabilityMode.AsynchronousCommit && availabilityGroupData.AvailabilityGroupReplicas.Any((AvailabilityGroupReplica replica) => replica.AvailabilityGroupReplicaData.AvailabilityMode == AvailabilityReplicaAvailabilityMode.SynchronousCommit))
		{
			throw new AvailabilityModeIncompatibleException();
		}
	}
}
