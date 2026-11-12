using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public abstract class DatabaseExistenceValidator : Validator
{
	private readonly AvailabilityGroupReplica replica;

	public AvailabilityGroupData AvailabilityGroupData { get; private set; }

	public List<PrimaryDatabaseData> DatabasesToValidate { get; protected set; }

	protected DatabaseExistenceValidator(string name, AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica replica)
		: base(name)
	{
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		if (replica == null)
		{
			throw new ArgumentNullException("replica");
		}
		AvailabilityGroupData = availabilityGroupData;
		this.replica = replica;
		DatabasesToValidate = availabilityGroupData.NewAvailabilityDatabases;
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		policy.Expired = true;
		List<string> list = new List<string>();
		Microsoft.SqlServer.Management.Smo.Server secondaryReplica = replica.GetServer();
		list.AddRange(from PrimaryDatabaseData db in DatabasesToValidate
			where secondaryReplica.Databases.Contains(db.Name)
			select db.Name);
		if (list.Count > 0)
		{
			throw new DatabaseAlreadyExistsException(replica.AvailabilityGroupReplicaData.ReplicaName, list);
		}
	}
}
