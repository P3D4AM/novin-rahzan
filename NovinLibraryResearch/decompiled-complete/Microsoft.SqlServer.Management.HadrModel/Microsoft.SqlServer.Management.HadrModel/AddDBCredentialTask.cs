using System;
using System.Globalization;
using System.Linq;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AddDBCredentialTask : HadrTask
{
	private bool useExistingAvailabilityDatabases;

	private AvailabilityGroupData availabilityGroupData;

	private readonly string databaseName;

	private readonly AvailabilityGroupReplica replica;

	public AvailabilityGroupData AvailabilityGroupData => availabilityGroupData;

	public AvailabilityGroupReplica ReplicaData => replica;

	public bool UseExistingAvailabilityDatabases
	{
		get
		{
			return useExistingAvailabilityDatabases;
		}
		set
		{
			useExistingAvailabilityDatabases = value;
		}
	}

	public AddDBCredentialTask(string databaseName, AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica replica)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.AddDatabaseCredentialText, databaseName, replica.AvailabilityGroupReplicaData.ReplicaName))
	{
		if (string.IsNullOrEmpty(databaseName))
		{
			throw new ArgumentNullException("databaseName");
		}
		this.databaseName = databaseName;
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		if (replica == null)
		{
			throw new ArgumentNullException("replica");
		}
		this.replica = replica;
		this.availabilityGroupData = availabilityGroupData;
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		Microsoft.SqlServer.Management.Smo.Server newSmoServerObject = HadrModelUtilities.GetNewSmoServerObject(replica.AvailabilityGroupReplicaData.Connection);
		PrimaryDatabaseData primaryDatabaseData = (useExistingAvailabilityDatabases ? AvailabilityGroupData.ExistingAvailabilityDatabases : AvailabilityGroupData.NewAvailabilityDatabases).FirstOrDefault((PrimaryDatabaseData c) => c.Name.Equals(databaseName));
		if (primaryDatabaseData != null && primaryDatabaseData.DBMKPassword != null)
		{
			FailoverUtilities.ProvisionCredential(newSmoServerObject, databaseName, primaryDatabaseData.DBMKPassword);
		}
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
