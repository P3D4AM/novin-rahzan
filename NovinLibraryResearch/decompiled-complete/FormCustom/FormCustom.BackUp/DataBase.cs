using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence;
using AccSqlServerPersistence.AssistanceClasses;
using BackUpDLL;
using BackUpDLL.Assistance;
using BackUpDLL.Zip;
using Microsoft.Win32;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Attendance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.BackUp;

public class DataBase
{
	public static bool FinishEvent;

	public static string ConnectionString
	{
		get
		{
			return Settings.ConnectionString;
		}
		set
		{
			Settings.ConnectionString = value;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> BackUpStartAsync(string contextGuid, IWin32Window owner, BackUpConfig config, CancellationToken token = default(CancellationToken), IBackUpLog_GetUserName userNameInterFace = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			token.ThrowIfCancellationRequested();
			if (string.IsNullOrEmpty(config.Path))
			{
				token.ThrowIfCancellationRequested();
				SaveFileDialog val = new SaveFileDialog
				{
					Title = " پشتیبان گیری اطلاعات "
				};
				token.ThrowIfCancellationRequested();
				string text = Path.GetFileName(Assembly.GetEntryAssembly()?.Location)?.Replace(".exe", "__");
				string text2 = DateTime.Now.MiladiToCalture().Replace("/", "_");
				text2 = text2 + "__" + DateTime.Now.Hour + " - " + DateTime.Now.Minute;
				text += text2;
				text = text.Replace(" ", "");
				((FileDialog)val).FileName = text;
				token.ThrowIfCancellationRequested();
				if (config.CompressMethod == BackUpMethod.GZip)
				{
					((FileDialog)val).Filter = "*.Bak|*.Bak";
				}
				else if (config.CompressMethod == BackUpMethod.SevenZip)
				{
					((FileDialog)val).Filter = "*.NPZ|*.NPZ";
				}
				token.ThrowIfCancellationRequested();
				token.ThrowIfCancellationRequested();
				if ((int)((CommonDialog)val).ShowDialog(owner) != 1)
				{
					ret.AddReturnedValue(ReturnedState.Warning, "لغو  توسط کاربر. عدم انتخاب آدرس ذخیره سازی.", "", "BackUpStartAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 50);
					return ret;
				}
				config.Path = ((FileDialog)val).FileName;
			}
			token.ThrowIfCancellationRequested();
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await DataBaseAction.Instance.BackupDbAsync(config, BackupSettingsRepo.GetInstance(contextGuid), token, userNameInterFace), ReturnedState.None, "BackUpStartAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 57);
		}
		catch (ThreadAbortException exception)
		{
			ret.AddReturnedValue(exception, "", "BackUpStartAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 59);
		}
		catch (OperationCanceledException exception2)
		{
			ret.AddReturnedValue(exception2, "", "BackUpStartAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 60);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BackUpStartAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 63);
			FinishEvent = true;
			ret.AddReturnedValue(ex, "", "BackUpStartAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 65);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> ReStoreStartAsync(string contextGuid, IWin32Window owner, string connectionString, ENSource source, string path = "", bool autoBackup = true, Guid? userAccGuid = null, IBackUpLog_GetUserName userNameInterFace = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			await SqlBackUpLogMaster.InitAsync(BackupSettingsRepo.GetInstance(contextGuid), connectionString, Guid.NewGuid(), source, EnBackUpType.Restore, userAccGuid, userNameInterFace);
			if (string.IsNullOrEmpty(path))
			{
				OpenFileDialog val = new OpenFileDialog
				{
					Multiselect = false,
					Filter = "Backup Files(*.NPZ;*.NPZ2;*.BAK)|*.NPZ;*.NPZ2;*.BAK",
					Title = "فایل حاوی اطلاعات پشتیبانی نرم افزار را انتخاب نمائید"
				};
				if ((int)((CommonDialog)val).ShowDialog(owner) != 1)
				{
					ret.AddReturnedValue(ReturnedState.Warning, "بازگردانی اطلاعات توسط کاربر لغو شد. عدم انتخاب آدرس ذخیره سازی فایل.", "", "ReStoreStartAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 90);
					return ret;
				}
				path = ((FileDialog)val).FileName;
			}
			await SqlBackUpLogMaster.SaveAsync(BackupSettingsRepo.GetInstance(contextGuid), EnBackUpStatus.Pending, "آغاز فرآیند Extract پشتیبان", "فرآیند بازیابی در تاریخ " + DateTime.Now.MiladiToCalture() + " و در ساعت " + DateTime.Now.ToShortTimeString() + " آغاز شد", path, connectionString);
			if (path.ToLower().EndsWith("npz") || path.ToLower().EndsWith("npz2"))
			{
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new MasterZip().ExtractTempDirAsync(path, Application.StartupPath);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "ReStoreStartAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 106);
				path = returnedSaveFuncInfoWithValue.value;
			}
			if (ret.HasError)
			{
				await SqlBackUpLogMaster.SaveAsync(BackupSettingsRepo.GetInstance(contextGuid), EnBackUpStatus.Error, "خطا در Extract کردن فایل پشتیبان", ret.GetErrorMessage(), path, connectionString);
				return ret;
			}
			await SqlBackUpLogMaster.SaveAsync(BackupSettingsRepo.GetInstance(contextGuid), EnBackUpStatus.Success, " اتمام عملیات Extract کردن فایل پشتیبان", "عملیات Extract کردن پشتیبان با موفقیت به اتمام رسید", path, connectionString);
			float backUpVersion = await DataBaseAction.Instance.GetBackUpVersionAsync(BackupSettingsRepo.GetInstance(contextGuid), connectionString, path);
			int num = await DataBaseAction.Instance.GetDataBaseVersionAsync(BackupSettingsRepo.GetInstance(contextGuid), connectionString, saveLog: true);
			if (backUpVersion > (float)num)
			{
				ret.AddReturnedValue(ReturnedState.Error, $"{backUpVersion} نسخه فایل پشتیبان" + " \r\n" + $"{num} نسخه دیتابیس" + "\r\nبدلیل بالاتر بودن نسخه پشتیبان نسبت به دیتابیس، امکان بازگردانی وجود ندارد", "", "ReStoreStartAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 123);
				await SqlBackUpLogMaster.SaveAsync(BackupSettingsRepo.GetInstance(contextGuid), EnBackUpStatus.Error, "ناهماهنگی نسخه فایل پشتیبان و نسخه دیتابیس", $"{backUpVersion} نسخه فایل پشتیبان" + " \r\n" + $"{num} نسخه دیتابیس" + "\r\nبدلیل بالاتر بودن نسخه پشتیبان نسبت به دیتابیس، امکان بازگردانی وجود ندارد", path, connectionString);
				return ret;
			}
			SqlConnection.ClearAllPools();
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await DataBaseAction.Instance.ReStoreDbAsync(BackupSettingsRepo.GetInstance(contextGuid), Application.StartupPath, connectionString, path, autoBackup, source), ReturnedState.None, "ReStoreStartAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 139);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReStoreStartAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 144);
			ret.AddReturnedValue(ex, "", "ReStoreStartAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 145);
		}
		return ret;
	}

	public static string GetServerConnectionString()
	{
		try
		{
			return Settings.ServerConnectionsString;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetServerConnectionString", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 162);
		}
		return "";
	}

	public static async Task<CreateDatabaseResult> CreateDatabase(string contextGuid, string dataBaseName, string serverName)
	{
		CreateDatabaseResult ret = new CreateDatabaseResult();
		try
		{
			if (dataBaseName == "")
			{
				ret.Status = EnCreateDataBase.databaseNameEmpty;
				ret.ReturnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "نام بانک اطلاعاتی خالی است.", "", "CreateDatabase", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 181);
				return ret;
			}
			if (string.IsNullOrEmpty(serverName))
			{
				ret.Status = EnCreateDataBase.serverConnectionStringError;
				ret.ReturnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "نام سروریس دهنده sql خالی است.", "", "CreateDatabase", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 187);
				return ret;
			}
			return await DataBaseAction.Instance.CreateDataBaseAsync(BackupSettingsRepo.GetInstance(contextGuid), dataBaseName, serverName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateDatabase", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 194);
			ret.ReturnedSaveFuncInfo.AddReturnedValue(ex, "", "CreateDatabase", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 195);
		}
		return ret;
	}

	public static Task<ReturnedSaveFuncInfo> SetRegistery(string sqlConnection, string name)
	{
		using (new JaegerMonitor("SetRegistery", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin\\" + name, "SQLCN", sqlConnection);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetRegistery", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 210);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetRegistery", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\DataBase.cs", 211);
			}
			return Task.FromResult(returnedSaveFuncInfo);
		}
	}
}
