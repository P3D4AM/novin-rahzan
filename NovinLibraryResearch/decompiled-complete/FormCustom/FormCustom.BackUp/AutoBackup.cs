using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence;
using BackUpDLL;
using BackUpDLL.Assistance;
using EntityCache;
using EntityCache.Business;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.BackUp;

public class AutoBackup
{
	public delegate void TakeBackupEventHandler(string fileName);

	public delegate void BackupStateChangedEventHandler(BKThreadStatus oldsTate, BKThreadStatus newState);

	public enum BKThreadStatus : short
	{
		NotStarted,
		Busy,
		WaitForNext
	}

	private static CancellationTokenSource _backupToken = new CancellationTokenSource();

	private static TakeBackupEventHandler _takeBackupEvent;

	private static BackupStateChangedEventHandler _backupStateChangedEvent;

	public static BKThreadStatus AutoBkStatus { get; private set; } = BKThreadStatus.NotStarted;


	public static event TakeBackupEventHandler TakeBackup
	{
		add
		{
			_takeBackupEvent = (TakeBackupEventHandler)Delegate.Combine(_takeBackupEvent, value);
		}
		remove
		{
			_takeBackupEvent = (TakeBackupEventHandler)Delegate.Remove(_takeBackupEvent, value);
		}
	}

	public static event BackupStateChangedEventHandler BackupStateChanged
	{
		add
		{
			_backupStateChangedEvent = (BackupStateChangedEventHandler)Delegate.Combine(_backupStateChangedEvent, value);
		}
		remove
		{
			_backupStateChangedEvent = (BackupStateChangedEventHandler)Delegate.Remove(_backupStateChangedEvent, value);
		}
	}

	public static void Abort()
	{
		try
		{
			_backupToken?.Cancel();
			_backupToken = new CancellationTokenSource();
			_backupStateChangedEvent?.Invoke(AutoBkStatus, BKThreadStatus.NotStarted);
			AutoBkStatus = BKThreadStatus.NotStarted;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Abort", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\AutoBackup.cs", 49);
		}
	}

	public static async Task StartThreadAsync(string contextGuid, ENSource source, string path, string version, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (AutoBkStatus == BKThreadStatus.NotStarted)
			{
				_backupStateChangedEvent?.Invoke(BKThreadStatus.NotStarted, BKThreadStatus.WaitForNext);
				await _AutoBackupThread(contextGuid, source, path, version, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StartThreadAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\AutoBackup.cs", 73);
		}
	}

	private static async Task _AutoBackupThread(string contextGuid, ENSource source, string path, string version, CancellationToken token)
	{
		if (string.IsNullOrEmpty(path))
		{
			throw new NullReferenceException("path");
		}
		try
		{
			await Task.Delay(20000, _backupToken.Token);
			while (true)
			{
				bool flag = false;
				ENSource? eNSource = ClsCache.GetCache(contextGuid)?.Source;
				if (eNSource == ENSource.Accounting || eNSource == ENSource.Baje)
				{
					flag = (await LocalSettingBusiness.GetSettingAsync(contextGuid)).Others.BackUpSettings.EnableAutoBackup;
				}
				if (!flag)
				{
					await Task.Delay(60000);
					continue;
				}
				_backupStateChangedEvent?.Invoke(AutoBkStatus, BKThreadStatus.Busy);
				AutoBkStatus = BKThreadStatus.Busy;
				BackUpConfig config = new BackUpConfig(version)
				{
					StartUpPath = Application.StartupPath,
					ConnectionString = contextGuid.GetSqlConnectionString(),
					Source = source,
					Path = path,
					Guid = Guid.NewGuid()
				};
				DataBaseAction.Instance.BackupDbAsync(config, BackupSettingsRepo.GetInstance(contextGuid), _backupToken.Token);
				_backupStateChangedEvent?.Invoke(AutoBkStatus, BKThreadStatus.WaitForNext);
				AutoBkStatus = BKThreadStatus.WaitForNext;
				await Task.Delay((await LocalSettingBusiness.GetSettingAsync(contextGuid, token)).Others.BackUpSettings.BackupTimeBetween * 60000, _backupToken.Token);
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "_AutoBackupThread", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\AutoBackup.cs", 109);
		}
	}

	public static async Task<string> FileNameAsync(string contextGuid, string defaultBackupPath)
	{
		try
		{
			SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(contextGuid.GetSqlConnectionString());
			string text = DateManagment.DateM.Date.MiladiToCalture();
			text = text.Replace("/", "-");
			text = text.Replace("\\", "-");
			string text2 = val.InitialCatalog + "__" + text + "__" + DateManagment.DateM.Hour + "-" + DateManagment.DateM.Minute + ".NP";
			Path.Combine(defaultBackupPath, text2);
			return text2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FileNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\AutoBackup.cs", 188);
			return "";
		}
	}
}
