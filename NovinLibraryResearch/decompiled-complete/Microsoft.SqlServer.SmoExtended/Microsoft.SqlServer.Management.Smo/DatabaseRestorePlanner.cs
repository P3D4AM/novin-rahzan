using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Common;

namespace Microsoft.SqlServer.Management.Smo;

public sealed class DatabaseRestorePlanner
{
	public delegate void CreateRestorePlanEventHandler(object sender, CreateRestorePlanEventArgs e);

	private class BackupDeviceCollection : ICollection<BackupDeviceItem>, IEnumerable<BackupDeviceItem>, IEnumerable
	{
		public bool Dirty = true;

		private ICollection<BackupDeviceItem> items = new List<BackupDeviceItem>();

		public int Count => items.Count;

		public bool IsReadOnly => items.IsReadOnly;

		public void Add(BackupDeviceItem item)
		{
			if ((from BackupDeviceItem bkdev in items
				where bkdev.Name.Equals(item.Name) && bkdev.DeviceType == item.DeviceType
				select bkdev).Count() == 0)
			{
				Dirty = true;
				items.Add(item);
			}
		}

		public void Clear()
		{
			Dirty = true;
			items.Clear();
		}

		public bool Remove(BackupDeviceItem item)
		{
			Dirty = true;
			return items.Remove(item);
		}

		public bool Contains(BackupDeviceItem item)
		{
			return items.Contains(item);
		}

		public void CopyTo(BackupDeviceItem[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}

		public IEnumerator<BackupDeviceItem> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)(List<BackupDeviceItem>)items).GetEnumerator();
		}
	}

	private Server server;

	private string databaseName;

	public bool IncludeSnapshotBackups;

	private bool readHeaderFromMedia;

	private BackupDeviceCollection backupMediaList = new BackupDeviceCollection();

	private BackupSetCollection backupSets;

	private List<Exception> backupDeviceReadErrors = new List<Exception>();

	public Server Server
	{
		get
		{
			return server;
		}
		set
		{
			if (server != value)
			{
				server = value;
				backupSets = null;
			}
		}
	}

	public string DatabaseName
	{
		get
		{
			return databaseName;
		}
		set
		{
			if (value == null || !value.Equals(databaseName))
			{
				databaseName = value;
				backupSets = null;
			}
		}
	}

	public bool RestoreToLastBackup { get; set; }

	public DateTime RestoreToPointInTime { get; set; }

	public bool ReadHeaderFromMedia
	{
		get
		{
			return readHeaderFromMedia;
		}
		set
		{
			if (readHeaderFromMedia != value)
			{
				readHeaderFromMedia = value;
				backupSets = null;
			}
		}
	}

	public ICollection<BackupDeviceItem> BackupMediaList => backupMediaList;

	public bool BackupTailLog { get; set; }

	public bool TailLogWithNoRecovery { get; set; }

	public string TailLogBackupFile { get; set; }

	public BackupSetCollection BackupSets
	{
		get
		{
			if ((backupSets == null && ReadHeaderFromMedia) || (ReadHeaderFromMedia && backupMediaList.Dirty))
			{
				backupSets = GetBackupSetFromDevices();
				backupMediaList.Dirty = false;
			}
			else if (backupSets == null && !ReadHeaderFromMedia)
			{
				backupSets = GetBackupSetFromMSDB();
			}
			return backupSets;
		}
	}

	public event CreateRestorePlanEventHandler CreateRestorePlanUpdates;

	public DatabaseRestorePlanner(Server server)
	{
		if (server == null)
		{
			throw new ArgumentNullException("Server");
		}
		Server = server;
		RestoreToLastBackup = true;
	}

	public DatabaseRestorePlanner(Server server, string databaseName)
		: this(server)
	{
		if (databaseName == null)
		{
			throw new ArgumentNullException("DatabaseName");
		}
		DatabaseName = databaseName;
		RestoreToLastBackup = true;
	}

	public DatabaseRestorePlanner(Server server, string databaseName, DateTime pointInTime, string tailLogBackupFile)
		: this(server, databaseName)
	{
		RestoreToLastBackup = false;
		RestoreToPointInTime = pointInTime;
		TailLogBackupFile = tailLogBackupFile;
	}

	public RestorePlan CreateRestorePlan()
	{
		return CreateRestorePlan(new RestoreOptions());
	}

	public RestorePlan CreateRestorePlan(RestoreOptions ro)
	{
		RestorePlan restorePlan = new RestorePlan(Server);
		restorePlan.RestoreAction = RestoreActionType.Database;
		if (DatabaseName == null)
		{
			throw new SmoException(ExceptionTemplatesImpl.PropertyNotSetExceptionText("DatabaseName"));
		}
		restorePlan.DatabaseName = DatabaseName;
		if (Server.Version.Major < 9)
		{
			restorePlan.AddRestoreOperation(createShilohPlan(BackupSets));
			if (restorePlan.RestoreOperations.Count > 0 && !RestoreToLastBackup)
			{
				Restore restore = restorePlan.RestoreOperations.Last();
				if (restore.Action == RestoreActionType.Log)
				{
					restore.ToPointInTime = SqlSmoObject.SqlDateString(RestoreToPointInTime);
				}
			}
		}
		else
		{
			SelectBackupSetsForPlan(restorePlan);
		}
		if (BackupTailLog && restorePlan.TailLogBackupOperation == null)
		{
			TakeTailLogBackup(restorePlan);
		}
		restorePlan.SetRestoreOptions(ro);
		return restorePlan;
	}

	private List<BackupSet> createShilohPlan(BackupSetCollection backupsets)
	{
		List<BackupSet> list = new List<BackupSet>();
		IEnumerable<BackupSet> source = ((!RestoreToLastBackup) ? (from BackupSet bkset in backupsets
			where bkset.BackupSetType == BackupSetType.Database && bkset.BackupStartDate <= RestoreToPointInTime
			orderby bkset.BackupStartDate descending, bkset.ID descending
			select bkset).Take(1) : (from BackupSet bkset in backupsets
			where bkset.BackupSetType == BackupSetType.Database
			orderby bkset.BackupStartDate descending, bkset.ID descending
			select bkset).Take(1));
		if (source.Count() == 0)
		{
			return list;
		}
		BackupSet backupSet = source.ElementAt(0);
		DateTime lastBkStartDate = backupSet.BackupStartDate;
		list.Add(backupSet);
		source = ((!RestoreToLastBackup) ? (from BackupSet bkset in backupsets
			where bkset.BackupSetType == BackupSetType.Differential && bkset.BackupStartDate <= RestoreToPointInTime && bkset.BackupStartDate >= lastBkStartDate
			orderby bkset.BackupStartDate descending, bkset.ID descending
			select bkset).Take(1) : (from BackupSet bkset in backupsets
			where bkset.BackupSetType == BackupSetType.Differential && bkset.BackupStartDate >= lastBkStartDate
			orderby bkset.BackupStartDate descending, bkset.ID descending
			select bkset).Take(1));
		if (source.Count() > 0)
		{
			BackupSet backupSet2 = source.ElementAt(0);
			lastBkStartDate = backupSet2.BackupStartDate;
			list.Add(backupSet2);
		}
		source = ((!RestoreToLastBackup) ? (from BackupSet bkset in backupsets
			where bkset.BackupSetType == BackupSetType.Log && bkset.BackupStartDate <= RestoreToPointInTime && bkset.BackupStartDate >= lastBkStartDate
			orderby bkset.BackupStartDate, bkset.ID
			select bkset) : (from BackupSet bkset in backupsets
			where bkset.BackupSetType == BackupSetType.Log && bkset.BackupStartDate >= lastBkStartDate
			orderby bkset.BackupStartDate, bkset.ID
			select bkset));
		foreach (BackupSet item in source)
		{
			list.Add(item);
			lastBkStartDate = item.BackupStartDate;
		}
		if (!RestoreToLastBackup && RestoreToPointInTime > lastBkStartDate)
		{
			source = (from BackupSet bkset in backupsets
				where bkset.BackupSetType == BackupSetType.Log && bkset.BackupStartDate > RestoreToPointInTime
				orderby bkset.BackupStartDate, bkset.ID
				select bkset).Take(1);
			if (source.Count() > 0)
			{
				list.Add(source.ElementAt(0));
			}
		}
		return list;
	}

	private void SelectBackupSetsForPlan(RestorePlan plan)
	{
		List<BackupSet> list = new List<BackupSet>();
		BackupSet lastSelBackupSet = null;
		bool flag = false;
		if (this.CreateRestorePlanUpdates != null)
		{
			this.CreateRestorePlanUpdates(null, new CreateRestorePlanEventArgs(DatabaseRestorePlannerSR.SelectingBackupSets));
		}
		if (!RestoreToLastBackup)
		{
			IEnumerable<BackupSet> enumerable = (from BackupSet b in BackupSets
				where b.BackupStartDate == RestoreToPointInTime && b.BackupSetType != BackupSetType.FileOrFileGroup && b.BackupSetType != BackupSetType.FileOrFileGroupDifferential
				orderby b.LastLsn descending, b.ID descending, b.BackupStartDate descending
				select b).Take(1);
			if (enumerable != null && enumerable.Count() > 0)
			{
				lastSelBackupSet = enumerable.ElementAt(0);
				lastSelBackupSet.StopAtLsn = default(decimal);
				list.Add(lastSelBackupSet);
			}
		}
		if (lastSelBackupSet == null)
		{
			DateTime? dateTime = TailLogStartTime();
			if (!ReadHeaderFromMedia && !RestoreToLastBackup && dateTime.HasValue)
			{
				DateTime restoreToPointInTime = RestoreToPointInTime;
				DateTime? dateTime2 = dateTime;
				if (restoreToPointInTime > dateTime2)
				{
					flag = true;
					goto IL_036b;
				}
			}
			if (!RestoreToLastBackup)
			{
				BackupSet lastLsn = (from BackupSet b in BackupSets
					where b.BackupStartDate < RestoreToPointInTime
					orderby b.LastLsn descending, b.ID descending, b.BackupStartDate descending
					select b).FirstOrDefault();
				if (lastLsn != null)
				{
					BackupSet[] array = (from BackupSet b in BackupSets
						where b.BackupSetType == BackupSetType.Log && b.BackupStartDate > RestoreToPointInTime && b.FirstLsn <= lastLsn.LastLsn
						orderby b.LastLsn descending, b.ID, b.BackupStartDate
						select b).Take(4).ToArray();
					if (array.Length != 0)
					{
						DateTime? dateTime3 = null;
						for (int i = 0; i < array.Length; i++)
						{
							if (lastSelBackupSet != null)
							{
								break;
							}
							dateTime3 = LogStartTime(array[i]);
							if (dateTime3.HasValue && dateTime3 < RestoreToPointInTime)
							{
								lastSelBackupSet = array[i];
								lastSelBackupSet.StopAtLsn = default(decimal);
								list.Add(lastSelBackupSet);
							}
						}
					}
				}
			}
			goto IL_036b;
		}
		goto IL_04e1;
		IL_04e1:
		BackupSet backupSet = lastSelBackupSet;
		HashSet<Guid> visitedBksetGuids = new HashSet<Guid>();
		visitedBksetGuids.Add(backupSet.BackupSetGuid);
		while (lastSelBackupSet.BackupSetType != BackupSetType.Database && visitedBksetGuids.Count < BackupSets.Count)
		{
			BackupSet backupSet2 = (from BackupSet b in BackupSets
				where b.LastLsn <= lastSelBackupSet.LastLsn && !visitedBksetGuids.Contains(b.BackupSetGuid) && b.ID <= lastSelBackupSet.ID
				orderby b.LastLsn descending, b.ID descending, b.BackupStartDate descending
				select b).FirstOrDefault();
			if (backupSet2 == null)
			{
				throw new SmoException(ExceptionTemplatesImpl.UnableToCreateRestoreSequence);
			}
			backupSet2.StopAtLsn = default(decimal);
			visitedBksetGuids.Add(backupSet2.BackupSetGuid);
			decimal stopAtLsn = default(decimal);
			if (BackupSet.IsBackupSetsInSequence(backupSet2, lastSelBackupSet, ref stopAtLsn))
			{
				if (stopAtLsn != 0m)
				{
					backupSet2.StopAtLsn = stopAtLsn;
				}
				lastSelBackupSet = backupSet2;
				list.Add(lastSelBackupSet);
			}
		}
		list.Reverse();
		for (int j = 1; j < list.Count; j++)
		{
			if (list[j].StopAtLsn != 0m && list[j - 1].LastLsn == list[j].StopAtLsn)
			{
				list.RemoveAt(j);
				break;
			}
		}
		plan.AddRestoreOperation(list);
		if (!RestoreToLastBackup && plan.RestoreOperations.Count() > 0 && plan.RestoreOperations.Last().Action == RestoreActionType.Log && plan.RestoreOperations.Last().BackupSet.BackupStartDate > RestoreToPointInTime)
		{
			plan.RestoreOperations.Last().ToPointInTime = SqlSmoObject.SqlDateString(RestoreToPointInTime);
		}
		if (flag)
		{
			BackupTailLog = true;
			TakeTailLogBackup(plan);
			TakeTailLogRestore(plan);
		}
		return;
		IL_036b:
		if (lastSelBackupSet == null)
		{
			BackupSet backupSet3 = ((!RestoreToLastBackup) ? (from BackupSet b in BackupSets
				where b.BackupSetType != BackupSetType.FileOrFileGroup && b.BackupSetType != BackupSetType.FileOrFileGroupDifferential && b.BackupStartDate <= RestoreToPointInTime
				orderby b.LastLsn descending, b.ID descending, b.BackupStartDate descending
				select b).FirstOrDefault() : (from BackupSet b in BackupSets
				where b.BackupSetType != BackupSetType.FileOrFileGroup && b.BackupSetType != BackupSetType.FileOrFileGroupDifferential
				orderby b.LastLsn descending, b.ID descending, b.BackupStartDate descending
				select b).FirstOrDefault());
			if (backupSet3 == null)
			{
				return;
			}
			lastSelBackupSet = backupSet3;
			lastSelBackupSet.StopAtLsn = default(decimal);
			list.Add(lastSelBackupSet);
		}
		goto IL_04e1;
	}

	public bool IsTailLogBackupPossible(string databaseName)
	{
		if (string.IsNullOrEmpty(databaseName))
		{
			return false;
		}
		if (Server.Version.Major < 9 || string.IsNullOrEmpty(DatabaseName))
		{
			return false;
		}
		Database database = Server.Databases[databaseName];
		if (database == null)
		{
			return false;
		}
		database.Refresh();
		if (database.Status != DatabaseStatus.Normal && database.Status != DatabaseStatus.Suspect && database.Status != DatabaseStatus.EmergencyMode)
		{
			return false;
		}
		if (database.RecoveryModel == RecoveryModel.Full || database.RecoveryModel == RecoveryModel.BulkLogged)
		{
			return true;
		}
		return false;
	}

	public bool IsTailLogBackupWithNoRecoveryPossible(string databaseName)
	{
		if (string.IsNullOrEmpty(databaseName))
		{
			return false;
		}
		if (!IsTailLogBackupPossible(databaseName))
		{
			return false;
		}
		Database database = Server.Databases[databaseName];
		if (database == null)
		{
			return false;
		}
		if (database.ServerVersion.Major > 10 && database.DatabaseEngineType == DatabaseEngineType.Standalone && !string.IsNullOrEmpty(database.AvailabilityGroupName))
		{
			return false;
		}
		if (database.DatabaseEngineType == DatabaseEngineType.Standalone && database.IsMirroringEnabled)
		{
			return false;
		}
		return true;
	}

	private DateTime? GetLastRestoreDateTime()
	{
		string query = string.Format(SmoApplication.DefaultCulture, "SELECT TOP(1) restore_date FROM msdb.dbo.restorehistory WHERE destination_database_name = N'{0}' ORDER BY restore_date DESC", SqlSmoObject.SqlString(DatabaseName));
		DateTime? result = null;
		DataSet dataSet = Server.ExecutionManager.ExecuteWithResults(query);
		if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0 && !(dataSet.Tables[0].Rows[0]["restore_date"] is DBNull))
		{
			result = (DateTime)dataSet.Tables[0].Rows[0]["restore_date"];
		}
		return result;
	}

	internal DateTime? LogStartTime(BackupSet bkset)
	{
		if (bkset.ForkPointLsn == 0m)
		{
			IEnumerable<DateTime> enumerable = (from BackupSet b in BackupSets
				where BackupSet.IsBackupSetsInSequence(b, bkset)
				orderby b.BackupStartDate, b.ID
				select b.BackupStartDate).Take(1);
			if (enumerable != null && enumerable.Count() > 0)
			{
				return enumerable.ElementAt(0);
			}
			return null;
		}
		if (ReadHeaderFromMedia)
		{
			IEnumerable<DateTime> enumerable2 = (from BackupSet b in BackupSets
				where b.BackupStartDate <= bkset.BackupStartDate && b.BackupSetGuid != bkset.BackupSetGuid
				orderby b.BackupStartDate descending, b.ID descending
				select b.BackupStartDate).Take(1);
			if (enumerable2 != null && enumerable2.Count() > 0)
			{
				return enumerable2.ElementAt(0);
			}
			return null;
		}
		string query = string.Format(SmoApplication.DefaultCulture, "SELECT TOP(1) restore_date FROM msdb.dbo.restorehistory WHERE destination_database_name = N'{0}' AND restore_date < N'{1}' ORDER BY restore_date DESC", SqlSmoObject.SqlString(DatabaseName), bkset.BackupStartDate);
		DataSet dataSet = Server.ExecutionManager.ExecuteWithResults(query);
		if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0 && !(dataSet.Tables[0].Rows[0]["restore_date"] is DBNull))
		{
			return (DateTime)dataSet.Tables[0].Rows[0]["restore_date"];
		}
		return null;
	}

	public DateTime? TailLogStartTime()
	{
		if (IsTailLogBackupPossible(DatabaseName) && !ReadHeaderFromMedia)
		{
			DateTime? lastRestoreDateTime = GetLastRestoreDateTime();
			if (!lastRestoreDateTime.HasValue)
			{
				lastRestoreDateTime = DateTime.MinValue;
			}
			IEnumerable<DateTime> source = (from bkset in BackupSets.Cast<BackupSet>().Where(delegate(BackupSet bkset)
				{
					DateTime backupStartDate2 = bkset.BackupStartDate;
					DateTime? dateTime2 = lastRestoreDateTime;
					return backupStartDate2 >= dateTime2 && bkset.BackupSetType == BackupSetType.Log;
				})
				orderby bkset.BackupStartDate descending
				select bkset.BackupStartDate).Take(1);
			if (source.Count() == 0)
			{
				source = (from bkset in BackupSets.Cast<BackupSet>().Where(delegate(BackupSet bkset)
					{
						DateTime backupStartDate = bkset.BackupStartDate;
						DateTime? dateTime = lastRestoreDateTime;
						return dateTime.HasValue && backupStartDate >= dateTime.GetValueOrDefault() && (bkset.BackupSetType == BackupSetType.Differential || bkset.BackupSetType == BackupSetType.Database) && !bkset.IsCopyOnly;
					})
					orderby bkset.BackupStartDate
					select bkset.BackupStartDate).Take(1);
			}
			if (source.Count() > 0)
			{
				return source.ElementAt(0);
			}
		}
		return null;
	}

	private void TakeTailLogBackup(RestorePlan plan)
	{
		if (string.IsNullOrEmpty(TailLogBackupFile))
		{
			throw new PropertyNotSetException("TailLogBackupFile");
		}
		Backup backup = new Backup();
		backup.Database = DatabaseName;
		backup.Action = BackupActionType.Log;
		backup.NoRecovery = TailLogWithNoRecovery;
		backup.BackupSetName = Path.GetFileNameWithoutExtension(TailLogBackupFile);
		if (IsBackupDeviceUrl())
		{
			backup.Devices.Add(new BackupDeviceItem(TailLogBackupFile, DeviceType.Url));
		}
		else
		{
			backup.Devices.Add(new BackupDeviceItem(TailLogBackupFile, DeviceType.File));
		}
		backup.NoRewind = true;
		plan.TailLogBackupOperation = backup;
	}

	private void TakeTailLogRestore(RestorePlan plan)
	{
		if (string.IsNullOrEmpty(TailLogBackupFile))
		{
			throw new PropertyNotSetException("TailLogBackupFile");
		}
		Restore restore = new Restore();
		restore.Database = DatabaseName;
		restore.Action = RestoreActionType.Log;
		if (IsBackupDeviceUrl())
		{
			restore.Devices.Add(new BackupDeviceItem(TailLogBackupFile, DeviceType.Url));
		}
		else
		{
			restore.Devices.Add(new BackupDeviceItem(TailLogBackupFile, DeviceType.File));
		}
		restore.ToPointInTime = SqlSmoObject.SqlDateString(RestoreToPointInTime);
		plan.RestoreOperations.Add(restore);
	}

	private bool IsBackupDeviceUrl()
	{
		if (Uri.TryCreate(TailLogBackupFile, UriKind.Absolute, out Uri result))
		{
			if (!(result.Scheme == Uri.UriSchemeHttps))
			{
				return result.Scheme == Uri.UriSchemeHttp;
			}
			return true;
		}
		return false;
	}

	private BackupSetCollection GetBackupSetFromMSDB()
	{
		return new BackupSetCollection(Server, DatabaseName, PopulateFromMsdb: true, IncludeSnapshotBackups);
	}

	public Exception GetBackupDeviceReadErrors()
	{
		if (backupDeviceReadErrors.Count() == 0)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (Exception backupDeviceReadError in backupDeviceReadErrors)
		{
			stringBuilder.Append(backupDeviceReadError.Message + "\t");
		}
		return new Exception(stringBuilder.ToString());
	}

	private BackupSetCollection GetBackupSetFromDevices()
	{
		backupDeviceReadErrors.Clear();
		BackupSetCollection backupSetCollection = new BackupSetCollection(Server, DatabaseName, PopulateFromMsdb: false, IncludeSnapshotBackups);
		new List<BackupMediaSet>();
		Dictionary<Guid, List<BackupDeviceItem>> dictionary = new Dictionary<Guid, List<BackupDeviceItem>>();
		foreach (BackupDeviceItem backupMedia in BackupMediaList)
		{
			try
			{
				if (this.CreateRestorePlanUpdates != null)
				{
					string arg = ((backupMedia.Name != null) ? backupMedia.Name : string.Empty);
					this.CreateRestorePlanUpdates(null, new CreateRestorePlanEventArgs(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", DatabaseRestorePlannerSR.IdentifyingMediaSets, arg)));
				}
				Guid guid = GetMediaSetGuid(backupMedia);
				if (guid == default(Guid))
				{
					guid = Guid.NewGuid();
					while (dictionary.ContainsKey(guid))
					{
						guid = Guid.NewGuid();
					}
				}
				if (dictionary.ContainsKey(guid))
				{
					dictionary[guid].Add(backupMedia);
					continue;
				}
				List<BackupDeviceItem> list = new List<BackupDeviceItem>();
				list.Add(backupMedia);
				dictionary.Add(guid, list);
			}
			catch (ExecutionFailureException item)
			{
				backupDeviceReadErrors.Add(item);
			}
		}
		foreach (List<BackupDeviceItem> value in dictionary.Values)
		{
			try
			{
				List<BackupMedia> list2 = backupMediaObjectList(value);
				BackupMediaSet backupMediaSet = new BackupMediaSet(Server, list2);
				if (this.CreateRestorePlanUpdates != null)
				{
					string arg2 = ((list2.Count > 0 && list2[0].MediaName != null) ? list2[0].MediaName : string.Empty);
					this.CreateRestorePlanUpdates(null, new CreateRestorePlanEventArgs(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", DatabaseRestorePlannerSR.ReadingBackupSetHeader, arg2)));
				}
				foreach (BackupSet item2 in backupMediaSet.ReadBackupSetHeader())
				{
					if (Server.GetStringComparer().Compare(item2.DatabaseName, DatabaseName) == 0)
					{
						backupSetCollection.backupsetList.Add(item2);
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is BackupMediaSet.IncompleteBackupMediaSetException)
				{
					backupDeviceReadErrors.Add(ex);
				}
			}
		}
		return backupSetCollection;
	}

	private List<BackupMedia> backupMediaObjectList(List<BackupDeviceItem> list)
	{
		List<BackupMedia> list2 = new List<BackupMedia>();
		foreach (BackupDeviceItem item in list)
		{
			try
			{
				list2.Add(item.BackupMedia);
			}
			catch (ExecutionFailureException)
			{
			}
		}
		return list2;
	}

	internal Guid GetMediaSetGuid(BackupDeviceItem bkDeviceItem)
	{
		Guid result = default(Guid);
		DataTable dataTable = bkDeviceItem.DeviceLabel(Server);
		if (dataTable != null && dataTable.Rows.Count > 0)
		{
			if (dataTable.Rows[0]["MediaSetId"] is Guid)
			{
				return (Guid)dataTable.Rows[0]["MediaSetId"];
			}
			if (dataTable.Rows[0]["MediaSetId"] is string)
			{
				result = new Guid(dataTable.Rows[0]["MediaSetId"] as string);
			}
		}
		return result;
	}

	public bool RefreshBackupSets()
	{
		backupSets = null;
		if (BackupSets != null)
		{
			return true;
		}
		return false;
	}
}
