using System;
using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class BasicAvailabilityGroupValidator : Validator
{
	private const int DefaultBackupPriority = 50;

	private AvailabilityGroupData availabilityGroupData;

	public BasicAvailabilityGroupValidator(AvailabilityGroupData availabilityGroupData)
		: base(Resource.ValidatingBasicAvailabilityGroupOptions)
	{
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		this.availabilityGroupData = availabilityGroupData;
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		if (!availabilityGroupData.IsBasic)
		{
			return;
		}
		if (availabilityGroupData.AvailabilityGroupReplicas.Count > 2)
		{
			throw new BasicAvailabilityGroupIncompatibleException(Resource.BasicTooManyReplicasReason);
		}
		if (availabilityGroupData.ExistingAvailabilityDatabases.Count + availabilityGroupData.NewAvailabilityDatabases.Count > 1)
		{
			throw new BasicAvailabilityGroupIncompatibleException(Resource.BasicTooManyDatabasesReason);
		}
		foreach (AvailabilityGroupReplica availabilityGroupReplica in availabilityGroupData.AvailabilityGroupReplicas)
		{
			if (availabilityGroupReplica.BackupPriority != 50)
			{
				throw new BasicAvailabilityGroupIncompatibleException(Resource.BasicBackupPriorityReason);
			}
			if (availabilityGroupReplica.ReadableSecondaryRole != 0)
			{
				throw new BasicAvailabilityGroupIncompatibleException(Resource.BasicSecondaryRoleReason);
			}
		}
	}
}
