using System;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class FreeDiskSpaceValidator : Validator
{
	private readonly AvailabilityGroupData availabilityGroupData;

	private readonly AvailabilityGroupReplica replica;

	public AvailabilityGroupData AvailabilityGroupData => availabilityGroupData;

	public FreeDiskSpaceValidator(AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica replica)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.ValidatingDiskSpace, replica.AvailabilityGroupReplicaData.ReplicaName))
	{
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		if (replica == null)
		{
			throw new ArgumentNullException("replica");
		}
		this.availabilityGroupData = availabilityGroupData;
		this.replica = replica;
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		policy.Expired = true;
		availabilityGroupData.GetPrimaryTotalDataFileAndLogSize(out var totalDataFilesSize, out var totalLogFilesSize);
		long freeSpaceOnReplicaDrive = GetFreeSpaceOnReplicaDrive(GetDataDriveLetterOfReplica());
		long freeSpaceOnReplicaDrive2 = GetFreeSpaceOnReplicaDrive(GetLogDriveLetterOfReplica());
		if ((double)freeSpaceOnReplicaDrive < totalDataFilesSize)
		{
			throw new InSufficientFreeSpaceForDatabaseFilesException(replica.AvailabilityGroupReplicaData.ReplicaName);
		}
		if ((double)freeSpaceOnReplicaDrive2 < totalLogFilesSize)
		{
			throw new InSufficientFreeSpaceForDatabaseFilesException(replica.AvailabilityGroupReplicaData.ReplicaName);
		}
	}

	private char GetDataDriveLetterOfReplica()
	{
		Microsoft.SqlServer.Management.Smo.Server server = replica.GetServer();
		string text = server.Settings.DefaultFile;
		if (string.IsNullOrEmpty(text))
		{
			text = server.MasterDBPath;
		}
		return char.ToUpper(Directory.GetDirectoryRoot(text)[0]);
	}

	private char GetLogDriveLetterOfReplica()
	{
		Microsoft.SqlServer.Management.Smo.Server server = replica.GetServer();
		string text = server.Settings.DefaultLog;
		if (string.IsNullOrEmpty(text))
		{
			text = server.MasterDBLogPath;
		}
		return char.ToUpper(Directory.GetDirectoryRoot(text)[0]);
	}

	private long GetFreeSpaceOnReplicaDrive(char driveLetter)
	{
		DataSet obj = replica.GetServer().ConnectionContext.ExecuteWithResults("exec master..xp_fixeddrives") ?? throw new DriveNotFoundOnReplicaException(driveLetter, replica.AvailabilityGroupReplicaData.ReplicaName);
		bool flag = false;
		long result = 0L;
		foreach (DataRow row in obj.Tables[0].Rows)
		{
			if (char.ToUpper(driveLetter) == char.ToUpper(Convert.ToChar(row[0])))
			{
				result = Convert.ToInt64(row[1]);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			throw new DriveNotFoundOnReplicaException(driveLetter, replica.AvailabilityGroupReplicaData.ReplicaName);
		}
		return result;
	}
}
