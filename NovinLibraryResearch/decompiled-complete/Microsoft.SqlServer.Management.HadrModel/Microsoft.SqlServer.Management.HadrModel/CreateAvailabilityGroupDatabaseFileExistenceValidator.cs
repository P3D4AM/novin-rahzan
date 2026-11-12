using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class CreateAvailabilityGroupDatabaseFileExistenceValidator : DatabaseFileExistenceValidator
{
	public CreateAvailabilityGroupDatabaseFileExistenceValidator(AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica replica)
		: base(Resource.ValidatingDatabaseFilesNotExistsOnSecondary, availabilityGroupData, replica)
	{
		base.DatabasesToValidate = availabilityGroupData.NewAvailabilityDatabases;
	}
}
