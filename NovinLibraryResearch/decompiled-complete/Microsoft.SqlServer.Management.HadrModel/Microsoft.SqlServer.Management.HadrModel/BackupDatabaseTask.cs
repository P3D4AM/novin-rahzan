using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class BackupDatabaseTask : HadrTask, IScriptableTask
{
	private readonly AvailabilityGroupData _availabilityGroupData;

	private readonly string _databaseName;

	private Backup _backup;

	public List<ServerConnection> ScriptingConnections { get; private set; }

	public AvailabilityGroupData AvailabilityGroupData => _availabilityGroupData;

	public BackupDatabaseTask(string databaseName, AvailabilityGroupData availabilityGroupData)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.BackupDatabaseText, databaseName))
	{
		if (string.IsNullOrEmpty(databaseName))
		{
			throw new ArgumentNullException("databaseName");
		}
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		_databaseName = databaseName;
		_availabilityGroupData = availabilityGroupData;
		ScriptingConnections = new List<ServerConnection> { _availabilityGroupData.PrimaryServer.ConnectionContext };
	}

	public void Abort()
	{
		if (_backup != null)
		{
			_backup.Abort();
		}
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		ServerConnection connectionContext = _availabilityGroupData.PrimaryServer.ConnectionContext;
		int statementTimeout = connectionContext.StatementTimeout;
		connectionContext.StatementTimeout = 0;
		policy.Expired = true;
		try
		{
			Microsoft.SqlServer.Management.Smo.Server newSmoServerObject = HadrModelUtilities.GetNewSmoServerObject(connectionContext);
			_backup = new Backup
			{
				Action = BackupActionType.Database,
				Database = _databaseName,
				CompressionOption = BackupCompressionOptions.On,
				Initialize = true,
				SkipTapeHeader = true,
				FormatMedia = true,
				CopyOnly = true
			};
			PercentCompleteHandler percentCompleteHandler = new PercentCompleteHandler(base.Name);
			percentCompleteHandler.TaskProgressEventHandler += base.TaskUpdateEventHandler;
			_backup.PercentCompleteNotification = 5;
			_backup.PercentComplete += percentCompleteHandler.percentCompleteHandler;
			_backup.Devices.AddDevice(AvailabilityGroupData.GetDatabaseBackupFileFullName(newSmoServerObject, _databaseName), DeviceType.File);
			_backup.SqlBackup(newSmoServerObject);
			percentCompleteHandler.TaskProgressEventHandler -= base.TaskUpdateEventHandler;
		}
		catch (FailedOperationException ex)
		{
			SqlException sqlException = GetSqlException(ex);
			if (sqlException == null || sqlException.Number != 5133)
			{
				throw;
			}
			throw new BackupDatabaseTaskException(_databaseName, ex);
		}
		finally
		{
			_backup = null;
			connectionContext.StatementTimeout = statementTimeout;
		}
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}

	private SqlException GetSqlException(Exception ex)
	{
		for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
		{
			if (ex2 is SqlException)
			{
				return (SqlException)ex2;
			}
		}
		return null;
	}
}
