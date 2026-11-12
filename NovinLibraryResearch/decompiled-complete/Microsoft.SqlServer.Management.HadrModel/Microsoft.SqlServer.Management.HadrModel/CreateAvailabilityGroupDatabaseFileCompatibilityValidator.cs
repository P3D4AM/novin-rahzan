using System.Globalization;
using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class CreateAvailabilityGroupDatabaseFileCompatibilityValidator : DatabaseFileCompatibilityValidator
{
	public CreateAvailabilityGroupDatabaseFileCompatibilityValidator(AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica replica)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.ValidatingDatabaseFileLocationCompatibility, replica.AvailabilityGroupReplicaData.ReplicaName), availabilityGroupData, replica)
	{
		base.DatabasesToValidate = availabilityGroupData.NewAvailabilityDatabases;
	}
}
