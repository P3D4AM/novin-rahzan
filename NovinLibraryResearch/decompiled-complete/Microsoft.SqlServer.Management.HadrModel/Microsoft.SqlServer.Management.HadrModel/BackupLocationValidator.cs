using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class BackupLocationValidator : Validator
{
	private Microsoft.SqlServer.Management.Smo.Server primaryServer;

	private List<Microsoft.SqlServer.Management.Smo.Server> onPremiseSecondaryServers;

	private const string DatabaseNameFormat = "BackupLocDb_{0}";

	private string testDatabaseName;

	public AvailabilityGroupData AvailabilityGroupData { get; private set; }

	public BackupLocationValidator(AvailabilityGroupData data)
		: base(Resource.ValidatingBackupLocation)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		AvailabilityGroupData = data;
		Initialize(data);
	}

	private void Initialize(AvailabilityGroupData data)
	{
		primaryServer = data.PrimaryServer;
		onPremiseSecondaryServers = new List<Microsoft.SqlServer.Management.Smo.Server>();
		foreach (AvailabilityGroupReplica availabilityGroupReplica in data.AvailabilityGroupReplicas)
		{
			if (availabilityGroupReplica.AvailabilityGroupReplicaData.State == AvailabilityObjectState.Creating && availabilityGroupReplica.AvailabilityGroupReplicaData.InitialRole == ReplicaRole.Secondary)
			{
				onPremiseSecondaryServers.Add(availabilityGroupReplica.GetServer());
			}
		}
		if (string.IsNullOrWhiteSpace(data.BackupLocationInWindowsFormat) && string.IsNullOrWhiteSpace(data.BackupLocationInLinuxFormat))
		{
			throw new ArgumentException(Resource.BackupLocationNotProvidedErrorMessage);
		}
		testDatabaseName = $"BackupLocDb_{Guid.NewGuid()}";
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		policy.Expired = true;
		string text = Path.ChangeExtension(testDatabaseName, ".bak");
		Database database = null;
		try
		{
			database = CreateTemporaryDatabaseOnPrimary();
			BackupTemporaryDatabase(database, text);
			ValidateBackupOnSecondaries(text);
		}
		finally
		{
			if (database != null)
			{
				database.DropBackupHistory();
				database.Drop();
			}
			try
			{
				if (!string.IsNullOrWhiteSpace(AvailabilityGroupData.BackupLocationInWindowsFormat))
				{
					File.Delete(PathWrapper.Combine(AvailabilityGroupData.BackupLocationInWindowsFormat, text));
				}
			}
			catch (DirectoryNotFoundException)
			{
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}
	}

	private Database CreateTemporaryDatabaseOnPrimary()
	{
		UpdateStatus(new ValidatorEventArgs("BackupLocationValidator", "CreateTemporaryDatabaseOnPrimary", "Started"));
		Database database = new Database(primaryServer, testDatabaseName);
		database.RecoveryModel = RecoveryModel.Full;
		database.UserAccess = DatabaseUserAccess.Restricted;
		database.Create();
		UpdateStatus(new ValidatorEventArgs("BackupLocationValidator", "CreateTemporaryDatabaseOnPrimary", "Completed"));
		return database;
	}

	private void BackupTemporaryDatabase(Database database, string backupFileName)
	{
		string text = PathWrapper.Combine(AvailabilityGroupData.GetBackupPathForServer(primaryServer), backupFileName);
		Backup backup = new Backup
		{
			Action = BackupActionType.Database,
			Database = database.Name,
			CompressionOption = BackupCompressionOptions.On,
			Initialize = true,
			SkipTapeHeader = true,
			FormatMedia = true
		};
		backup.Devices.AddDevice(text, DeviceType.File);
		try
		{
			backup.SqlBackup(primaryServer);
		}
		catch (Exception innerException)
		{
			throw new PrimaryCannotWriteToLocationException(primaryServer.ConnectionContext.TrueName, text, innerException);
		}
	}

	private void ValidateBackupOnSecondaries(string backupFileName)
	{
		foreach (Microsoft.SqlServer.Management.Smo.Server onPremiseSecondaryServer in onPremiseSecondaryServers)
		{
			string text = PathWrapper.Combine(AvailabilityGroupData.GetBackupPathForServer(onPremiseSecondaryServer), backupFileName);
			Restore restore = new Restore();
			restore.Devices.AddDevice(text, DeviceType.File);
			DataTable dataTable = null;
			try
			{
				if (onPremiseSecondaryServer != null)
				{
					dataTable = restore.ReadBackupHeader(onPremiseSecondaryServer);
					goto IL_006a;
				}
			}
			catch (Exception innerException)
			{
				throw new SecondaryCannotReadLocationException(onPremiseSecondaryServer.ConnectionContext.TrueName, text, innerException);
			}
			continue;
			IL_006a:
			if (dataTable.Rows.Count != 1 || dataTable.Rows[0]["DatabaseName"].ToString() != testDatabaseName)
			{
				throw new SecondaryCannotReadLocationException(onPremiseSecondaryServer.ConnectionContext.TrueName, text);
			}
		}
	}
}
