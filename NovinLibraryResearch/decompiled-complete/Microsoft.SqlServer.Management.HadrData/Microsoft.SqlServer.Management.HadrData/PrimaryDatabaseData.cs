using System;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrData;

public class PrimaryDatabaseData
{
	private Database smoDatabase;

	private string existingAvailabilityGroupName;

	public string Name => smoDatabase.Name;

	public double DatabaseSize => smoDatabase.Size;

	public string AvailabiliyGroupName => existingAvailabilityGroupName;

	public bool IsPartOfExistingAvailabilityGroup => !string.IsNullOrEmpty(AvailabiliyGroupName);

	public string DatabaseFullBackupFile { get; set; }

	public string DatabaseLogBackupFile { get; set; }

	public string DBMKPassword { get; set; }

	public PrimaryDatabaseData(Database database, string availabilityGroupName)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (availabilityGroupName == null)
		{
			throw new ArgumentNullException("availabilityGroupName");
		}
		smoDatabase = database;
		existingAvailabilityGroupName = availabilityGroupName;
	}

	public PrimaryDatabaseData(Database database)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		smoDatabase = database;
	}
}
