using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AddReplicaDatabaseFileExistenceValidator : DatabaseFileExistenceValidator
{
	public AddReplicaDatabaseFileExistenceValidator(AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica replica)
		: base(Resource.ValidatingDatabaseFilesNotExistsOnSecondary, availabilityGroupData, replica)
	{
		base.DatabasesToValidate = availabilityGroupData.ExistingAvailabilityDatabases;
	}
}
