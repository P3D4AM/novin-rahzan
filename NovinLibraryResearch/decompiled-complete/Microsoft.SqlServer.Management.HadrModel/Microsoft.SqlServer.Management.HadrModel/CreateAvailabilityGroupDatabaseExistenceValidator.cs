using System.Globalization;
using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class CreateAvailabilityGroupDatabaseExistenceValidator : DatabaseExistenceValidator
{
	public CreateAvailabilityGroupDatabaseExistenceValidator(AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica replica)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.ValidatingDatabaseNotExistOnSecondary, replica.AvailabilityGroupReplicaData.ReplicaName), availabilityGroupData, replica)
	{
		base.DatabasesToValidate = availabilityGroupData.NewAvailabilityDatabases;
	}
}
