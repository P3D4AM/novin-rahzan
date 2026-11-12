using System.Globalization;
using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AddReplicaDatabaseExistenceValidator : DatabaseExistenceValidator
{
	public AddReplicaDatabaseExistenceValidator(AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica replica)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.ValidatingDatabaseNotExistOnSecondary, replica.AvailabilityGroupReplicaData.ReplicaName), availabilityGroupData, replica)
	{
		base.DatabasesToValidate = availabilityGroupData.ExistingAvailabilityDatabases;
	}
}
