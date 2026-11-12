using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class RestoreDatabaseTask : HadrTask, IScriptableTask
{
	private Restore restore;

	private AvailabilityGroupData availabilityGroupData;

	private ServerConnection secondaryConnection;

	private string databaseName;

	private AvailabilityGroupReplica availabilityGroupReplica;

	public List<ServerConnection> ScriptingConnections { get; private set; }

	public AvailabilityGroupData AvailabilityGroupData => availabilityGroupData;

	public AvailabilityGroupReplica ReplicaData => availabilityGroupReplica;

	public RestoreDatabaseTask(string databaseName, AvailabilityGroupData availabilityGroupData, AvailabilityGroupReplica availabilityGroupReplica)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.RestoreDatabaseText, databaseName, availabilityGroupReplica.AvailabilityGroupReplicaData.ReplicaName))
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
		if (availabilityGroupReplica == null)
		{
			throw new ArgumentNullException("availabilityGroupReplica");
		}
		this.availabilityGroupReplica = availabilityGroupReplica;
		this.availabilityGroupData = availabilityGroupData;
		secondaryConnection = this.availabilityGroupReplica.AvailabilityGroupReplicaData.Connection;
		ScriptingConnections = new List<ServerConnection> { secondaryConnection };
	}

	public void Abort()
	{
		if (restore != null)
		{
			restore.Abort();
		}
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		int statementTimeout = secondaryConnection.StatementTimeout;
		secondaryConnection.StatementTimeout = 0;
		try
		{
			Microsoft.SqlServer.Management.Smo.Server newSmoServerObject = HadrModelUtilities.GetNewSmoServerObject(secondaryConnection);
			string databaseBackupFileFullName = AvailabilityGroupData.GetDatabaseBackupFileFullName(newSmoServerObject, databaseName);
			restore = new Restore
			{
				Action = RestoreActionType.Database,
				Database = databaseName,
				FileNumber = 0,
				ReplaceDatabase = false,
				NoRecovery = true
			};
			PercentCompleteHandler percentCompleteHandler = new PercentCompleteHandler(base.Name);
			percentCompleteHandler.TaskProgressEventHandler += base.TaskUpdateEventHandler;
			restore.PercentCompleteNotification = 5;
			restore.PercentComplete += percentCompleteHandler.percentCompleteHandler;
			restore.Devices.AddDevice(databaseBackupFileFullName, DeviceType.File);
			if (newSmoServerObject.HostPlatform != availabilityGroupData.PrimaryServer.HostPlatform)
			{
				Database database = AvailabilityGroupData.PrimaryServer.Databases[databaseName];
				foreach (FileGroup fileGroup in database.FileGroups)
				{
					foreach (DataFile file in fileGroup.Files)
					{
						RelocateFile value = new RelocateFile(file.Name, PathWrapper.Combine(newSmoServerObject.DefaultFile, Path.GetFileName(file.FileName)));
						restore.RelocateFiles.Add(value);
					}
				}
				foreach (LogFile logFile in database.LogFiles)
				{
					RelocateFile value2 = new RelocateFile(logFile.Name, PathWrapper.Combine(newSmoServerObject.DefaultLog, Path.GetFileName(logFile.FileName)));
					restore.RelocateFiles.Add(value2);
				}
			}
			restore.SqlRestore(newSmoServerObject);
			percentCompleteHandler.TaskProgressEventHandler -= base.TaskUpdateEventHandler;
			policy.Expired = true;
		}
		catch (Exception inner)
		{
			throw new RestoreDatabaseTaskException(databaseName, inner);
		}
		finally
		{
			restore = null;
			secondaryConnection.StatementTimeout = statementTimeout;
		}
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}
