using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.AssistanceClasses;
using BackUpDLL.Assistance;
using BackUpDLL.Zip;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Attendance;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities;
using SmartAssembly.Attributes;

namespace BackUpDLL;

[Serializable]
[DoNotPruneType]
public class DataBaseAction : BaseDataBaseAction
{
	private static CancellationTokenSource _deleteTempFilesToken;

	public new static DataBaseAction Instance { get; } = new DataBaseAction();


	public static event EventHandler<CreateBackupArgs> OnCreateBackup;

	public string CreateFileName(string connectionString, string version, string startup, string directory = "")
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		string result = "";
		try
		{
			FileInfo fileInfo = new FileInfo(directory);
			string path;
			if (!string.IsNullOrEmpty(fileInfo.Extension))
			{
				directory = fileInfo.DirectoryName;
				path = ((!string.IsNullOrEmpty(version) && !fileInfo.FullName.Contains("version")) ? (fileInfo.FullName.Replace(fileInfo.Extension, "") + "-" + version + fileInfo.Extension) : fileInfo.Name);
			}
			else
			{
				SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(connectionString);
				path = (string.IsNullOrEmpty(version) ? string.Format("{0}__{1}__{2}-{3}-{4}.NP", val.InitialCatalog, DateTime.Now.MiladiToCalture().RemoveNoNumbers("-"), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) : string.Format("{0}__{1}__{2}-{3}-{4}-{5}.NP", val.InitialCatalog, DateTime.Now.MiladiToCalture().RemoveNoNumbers("-"), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, version));
			}
			if (string.IsNullOrEmpty(directory))
			{
				directory = Path.Combine(startup, "TempBackUp");
			}
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			result = Path.Combine(directory, path);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateFileName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 58);
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> BackupDbAsync(BackUpConfig config, ISettingRepo repo, CancellationToken token = default(CancellationToken), IBackUpLog_GetUserName userNameInterFace = null)
	{
		int line = 0;
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		bool isAutomatic = string.IsNullOrEmpty(config.Path);
		string newPath = "";
		try
		{
			if ((config?.Source ?? ENSource.None) == ENSource.Baje)
			{
				return ret;
			}
			FilePermissionsUtility access = new FilePermissionsUtility();
			await SqlBackUpLogMaster.InitAsync(repo, config.ConnectionString, Guid.NewGuid(), config.Source, EnBackUpType.BackUp, config.UserAccGuid, userNameInterFace);
			if (!isAutomatic && !access.HasAccess(config.Path, FileSystemRights.Write))
			{
				ret.AddError("کاربر گرامی، شما دسترسی به پوشه مقصد برای ایجاد فایل پشتیبان را ندارید. \r\n" + config.Path, "BackupDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 75);
				return ret;
			}
			BackUpConfig backUpConfig = config;
			Guid? guid = backUpConfig.Guid;
			guid.GetValueOrDefault();
			if (!guid.HasValue)
			{
				Guid value = Guid.NewGuid();
				backUpConfig.Guid = value;
			}
			line = 1;
			if (string.IsNullOrEmpty(config.RefrenceGuid))
			{
				config.RefrenceGuid = config.Guid.ToString();
			}
			line = 10;
			DataBaseAction.OnCreateBackup?.Invoke(null, new CreateBackupArgs
			{
				Message = "",
				State = CreateBackupState.Start
			});
			token.ThrowIfCancellationRequested();
			_deleteTempFilesToken?.Cancel();
			line = 20;
			if (isAutomatic)
			{
				config.Path = CreateFileName(config.ConnectionString, config.VersionString, config.StartUpPath);
			}
			if (!string.IsNullOrEmpty(config.Path))
			{
				config.Path = CreateFileName(config.ConnectionString, config.VersionString, config.StartUpPath, config.Path);
			}
			line = 30;
			await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Pending, "آغاز فرآیند پشتیبان گیری", "فرآیند پشتیبان گیری در تاریخ " + DateTime.Now.MiladiToCalture() + " و در ساعت " + DateTime.Now.ToShortTimeString() + " آغاز شد", config.Path, config.ConnectionString);
			_deleteTempFilesToken?.Cancel();
			token.ThrowIfCancellationRequested();
			await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Pending, "تهیه اولین فایل پشتیبان", "تهیه اولین فایل پشتیبان با پسوند .bak آغاز شد", config.Path, config.ConnectionString);
			line = 40;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await CreateSqlServerBackupFileAsync(config.Path, config.ConnectionString, config.RefrenceGuid);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "BackupDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 106);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Error, "خطا در ایجاد اولین نسخه پشتیبان", returnedSaveFuncInfoWithValue.GetErrorMessage(), config.Path, config.ConnectionString);
				if (ret.HasError)
				{
					return ret;
				}
			}
			line = 50;
			await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Success, "تهیه اولین فایل پشتیبان", "اولین نسخه پشتیبان در مسیر " + config.Path + " ایجاد شد", config.Path, config.ConnectionString);
			_deleteTempFilesToken?.Cancel();
			token.ThrowIfCancellationRequested();
			line = 60;
			ReturnedSaveFuncInfoWithValue<string> pathForZipDirectory = MasterZip.Move2Temp(config.Path, config.StartUpPath, 10);
			ret.AddReturnedValue(pathForZipDirectory, ReturnedState.None, "BackupDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 120);
			if (pathForZipDirectory.HasError)
			{
				await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Error, "خطا در انتقال فایل پشتیبان به پوشه Temp", pathForZipDirectory.GetErrorMessage(), pathForZipDirectory.value, config.ConnectionString);
				if (ret.HasError)
				{
					return ret;
				}
			}
			line = 70;
			await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Success, "انتقال فایل به پوشه Temp", "نسخه اولیه پشتیبان به محل " + pathForZipDirectory.value + " منتقل شد", pathForZipDirectory.value, config.ConnectionString);
			_deleteTempFilesToken?.Cancel();
			token.ThrowIfCancellationRequested();
			line = 80;
			try
			{
				await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Pending, "آغاز فرآیند فشرده سازی پشتیبان", "آغاز عملیات فشرده سازی فایل پشتیبان در ساعت " + DateTime.Now.ToShortTimeString(), pathForZipDirectory.value, config.ConnectionString);
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = await GZip.CompressFileInstance.CompressFileAsync(repo, pathForZipDirectory.value, config.Path, config.CompressMethod, token, config.RefrenceGuid, config.ConnectionString);
				line = 90;
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "BackupDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 142);
				newPath = ((!ret.HasError) ? returnedSaveFuncInfoWithValue2.value : config.Path?.ToLower().Replace(".npz2", ".np").Replace(".npz", ".np")
					.Replace(".np", ".npz2"));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BackupDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 149);
				ret.AddReturnedValue(ex, "", "BackupDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 150);
				await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Error, "خطا در فشرده سازی پشتیبان", ex.Message, config.Path, config.ConnectionString);
			}
			line = 100;
			ret.value = config.Path.ToLower().Replace(".npz2", ".np").Replace(".npz", ".np")
				.Replace(".np", ".npz2");
			if (ret.HasError)
			{
				EventLogger.NewInstance(EventLoggerType.Backup, "فراید پشتیبانگیری بدون فشرده سازی", ReturnedState.Success, "", config.RefrenceGuid);
				line = 110;
				string? sourceFileName = Directory.GetFiles(pathForZipDirectory.value).FirstOrDefault();
				newPath = config.Path.ToLower().Replace(".npz2", ".np").Replace(".npz", ".np");
				File.Move(sourceFileName, newPath);
				line = 115;
				await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Success, "انتقال نسخه پشتیبان به محل ذخیره سازی اولیه", "انتقال نسخه پشتیبان به محل ذخیره سازی اولیه با موفقیت انجام شد", config.Path.ToLower().Replace(".npz2", ".np").Replace(".npz", ".np"), config.ConnectionString);
			}
			DataBaseAction.OnCreateBackup?.Invoke(null, new CreateBackupArgs
			{
				Message = "",
				State = CreateBackupState.End
			});
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "BackupDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 171);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"Error in Line {line}", "BackupDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 174);
			EventLogger.NewInstance(EventLoggerType.Backup, "خطا در فشرده سازی", ReturnedState.Error, "", config.RefrenceGuid, ex2.Message);
			ret.AddReturnedValue(ex2, "", "BackupDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 176);
			await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Error, "خطا در ایجاد فایل پشتیبان", ex2.Message, config.Path, config.ConnectionString);
		}
		finally
		{
			_deleteTempFilesToken?.Cancel();
			_deleteTempFilesToken = new CancellationTokenSource();
			CancellationToken tkn = _deleteTempFilesToken.Token;
			Task.Run(() => DeleteTempsAsync(config.StartUpPath, tkn));
		}
		await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Success, "اتمام ایجاد نسخه پشتیبان", "فایل پشتیبان در ساعت " + DateTime.Now.ToShortTimeString() + " با موفقیت ایجاد شد", newPath, config.ConnectionString);
		return ret;
	}

	private async Task DeleteTempsAsync(string startup, CancellationToken token)
	{
		try
		{
			await Task.Delay(60000);
			string path = Path.Combine(startup, "Temp");
			if (!Directory.Exists(path))
			{
				return;
			}
			try
			{
				Directory.Delete(path, recursive: true);
			}
			catch (Exception ex)
			{
				if (!ex.Message.Contains("because it is being used by another process"))
				{
					throw;
				}
			}
		}
		catch (IOException ex2)
		{
			if (!ex2.Message.Contains("because it is being used by another process"))
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DeleteTempsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 205);
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex5)
		{
			WebErrorLog.Instance.StartLog(ex5, "", "DeleteTempsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 208);
		}
	}

	private async Task<ReturnedSaveFuncInfoWithValue<string>> CreateSqlServerBackupFileAsync(string path, string connectionString, string refrenceGuid = null)
	{
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		SqlConnection cn = new SqlConnection(connectionString);
		try
		{
			_ = 1;
			try
			{
				Console.WriteLine($"{DateTime.Now}) CreateSqlServerBackupFileAsync started....");
				FileInfo fileInfo = new FileInfo(path);
				if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fileInfo.Name) || !Directory.Exists(fileInfo.DirectoryName))
				{
					ret.AddReturnedValue(ReturnedState.Error, "آدرس " + path + " موجود یا معتبر نیست.", "", "CreateSqlServerBackupFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 222);
					EventLogger.NewInstance(EventLoggerType.Backup, "آدرس " + path + " موجود یا معتبر نیست.", ReturnedState.Error, "", refrenceGuid);
					return ret;
				}
				ret.value = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
				SqlCommand val = new SqlCommand("Backup database [" + ret.value + "] to disk='" + path + "'", cn);
				((DbCommand)val).CommandType = CommandType.Text;
				SqlCommand cmd = val;
				try
				{
					((DbCommand)(object)cmd).CommandTimeout = 300000;
					EventLogger.NewInstance(EventLoggerType.Backup, "شروع پشتیبانگیری در sql از دیتابیس:" + ((DbConnection)(object)cn).Database, ReturnedState.Success, "", refrenceGuid);
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
					((DbConnection)(object)cn).Close();
					EventLogger.NewInstance(EventLoggerType.Backup, "پایان پشتیبانگیری در sql از دیتابیس:" + ((DbConnection)(object)cn).Database, ReturnedState.Success, "", refrenceGuid);
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			catch (SqlException val2)
			{
				SqlException val3 = val2;
				WebErrorLog.Instance.StartLog((Exception)(object)val3, "", "CreateSqlServerBackupFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 239);
				EventLogger.NewInstance(EventLoggerType.Backup, "خطا در پشتیبانگیری  sql از دیتابیس:" + ((DbConnection)(object)cn)?.Database, ReturnedState.Error, "", refrenceGuid, ((Exception)(object)val3).Message);
				string text = "خطا در تهیه نسخه پشتیبان اطلاعات \r\n";
				text += (((Exception)(object)val3).Message.Contains("There is not enough space on the disk") ? "فضای کافی روی دیسک وجود ندارد\r\n" : "حساب کاربری اس کیو ال دسترسی به مسیر نصب برنامه ندارد\r\n");
				ret.AddReturnedValue(ReturnedState.Error, text + "\r\n " + ((Exception)(object)val3).Message + " \r\n " + ((Exception)(object)val3).InnerException?.Message, "", "CreateSqlServerBackupFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 245);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CreateSqlServerBackupFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 249);
				ret.AddReturnedValue(ex, "", "CreateSqlServerBackupFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 250);
			}
			finally
			{
				Console.WriteLine($"{DateTime.Now}) CreateSqlServerBackupFileAsync Finished.\r\n" + $"ResultHasError:{ret.HasError}");
			}
			return ret;
		}
		finally
		{
			((IDisposable)cn)?.Dispose();
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> ReStoreDbAsync(ISettingRepo repo, string startup, string connectionString, string pathf, bool autoBackup, ENSource source)
	{
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		ReturnedSaveFuncInfoWithValue<string> result;
		try
		{
			int num = 0;
			object obj = default(object);
			try
			{
				RunScript.Instance.ResetInitializeList();
				GC.Collect();
				SqlConnection.ClearAllPools();
				if (!autoBackup)
				{
					goto IL_02b1;
				}
				string text = Path.Combine(startup, ResourceManagement.TextBrandResourceManagement.DefaultBackUPFolderName);
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string text2 = Path.GetFileName(Assembly.GetEntryAssembly()?.Location)?.Replace(".exe", "__");
				string text3 = DateTime.Now.MiladiToCalture().Replace("/", "_");
				text3 = text3 + "__" + DateTime.Now.Hour + " - " + DateTime.Now.Minute;
				text2 += text3;
				string path = text + "\\" + text2 + ".Bak";
				BackUpConfig config = new BackUpConfig("")
				{
					StartUpPath = startup,
					ConnectionString = connectionString,
					Source = source,
					Path = path,
					Guid = Guid.NewGuid()
				};
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await BackupDbAsync(config, repo), ReturnedState.None, "ReStoreDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 291);
				if (!ret.HasError)
				{
					goto IL_02b1;
				}
				result = ret;
				goto end_IL_00a1;
				IL_02b1:
				if (pathf.EndsWith(".NPZ2") || pathf.EndsWith(".npz2") || pathf.EndsWith(".NPZ") || pathf.EndsWith(".npz"))
				{
					ret.AddReturnedValue(await new MasterZip().ExtractTempDirAsync(pathf, startup), ReturnedState.None, "ReStoreDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 298);
					pathf = ret.value;
				}
				await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Pending, "آغاز عملیات بازگردانی اطلاعات", "عملیات بازگردانی اطلاعات آغاز شد " + DateTime.Now.MiladiToCalture() + " - " + DateTime.Now.ToShortTimeString(), pathf, connectionString);
				SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(connectionString);
				string text4 = connectionString.Replace("Initial Catalog=" + csb.InitialCatalog, "Initial Catalog=");
				SqlConnection val = new SqlConnection(text4);
				((DbConnection)val).ConnectionString = text4;
				SqlConnection cn = val;
				try
				{
					SqlCommand cmd = new SqlCommand("RESTORE DATABASE [dbName] FROM  DISK = N'bkPath' WITH  FILE = 1,  MOVE N'logicalName' TO N'dbMDFPath',  MOVE N'logicalName_log' TO N'dbLDFPath',   NOUNLOAD,  REPLACE,  STATS = 5", cn);
					try
					{
						CLSServerDBs dbInfo = await CLSServerDBs.GetAsync(connectionString);
						ReturnedSaveFuncInfoWithValue<DataBaseBackUpInfo> returnedSaveFuncInfoWithValue2 = await DataBaseBackUpInfo.GetAsync(pathf, ((DbConnection)(object)cn).ConnectionString);
						ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "ReStoreDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 314);
						if (!ret.HasError)
						{
							((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("dbName", csb.InitialCatalog);
							((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("bkPath", pathf);
							((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("logicalName", returnedSaveFuncInfoWithValue2.value.LogicalName);
							((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("logicalName_log", returnedSaveFuncInfoWithValue2.value.LogicalName + "_log.ldf");
							((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("dbMDFPath", dbInfo.FileName);
							((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("dbLDFPath", dbInfo.FileName.ToLower().Replace(".mdf", "_log.ldf"));
							((DbCommand)(object)cmd).CommandTimeout = 300000;
							await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
							string temp = ((DbCommand)(object)cmd).CommandText;
							((DbCommand)(object)cmd).CommandText = "ALTER DATABASE [" + csb.InitialCatalog + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
							await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
							((DbCommand)(object)cmd).CommandText = temp;
							await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
							((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("dbName", csb.InitialCatalog);
							await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
							((DbCommand)(object)cmd).CommandText = "ALTER DATABASE [" + csb.InitialCatalog + "] SET Multi_USER";
							await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
							((DbConnection)(object)cn).Close();
							SqlConnection.ClearAllPools();
							goto end_IL_0520;
						}
						result = ret;
						goto end_IL_00a1;
						end_IL_0520:;
					}
					finally
					{
						((IDisposable)cmd)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)cn)?.Dispose();
				}
				goto IL_0a33;
				end_IL_00a1:;
			}
			catch (SqlException val2)
			{
				SqlException val3 = val2;
				obj = val3;
				num = 1;
				goto IL_0a33;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ReStoreDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 355);
				ret.AddReturnedValue(ex, "", "ReStoreDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 356);
				await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Success, "خطا در عملیات بازگردانی اطلاعات", ex.Message, pathf, connectionString);
				goto IL_0d24;
			}
			goto end_IL_006b;
			IL_0a33:
			if (num == 1)
			{
				SqlException val4 = (SqlException)obj;
				WebErrorLog.Instance.StartLog((Exception)(object)val4, "", "ReStoreDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 346);
				ret.AddReturnedValue(ReturnedState.Error, "حساب کاربری SQL دسترسی به مسیر نصب برنامه ندارد\r\n" + ((Exception)(object)val4).Message + "//" + ((Exception)(object)val4).InnerException?.Message, "", "ReStoreDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 347);
				await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Success, "خطا در عملیات بازگردانی اطلاعات", "حساب کاربری SQL دسترسی به مسیر نصب برنامه ندارد\r\n" + ((Exception)(object)val4).Message + "//" + ((Exception)(object)val4).InnerException?.Message, pathf, connectionString);
			}
			goto IL_0d24;
			end_IL_006b:;
		}
		finally
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await SetMultiUserAsync(connectionString), ReturnedState.None, "ReStoreDbAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 359);
		}
		return result;
		IL_0d24:
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SetMultiUserAsync(string connectionString)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(connectionString);
			string text = connectionString.Replace("Initial Catalog=" + val.InitialCatalog, "Initial Catalog=");
			SqlConnection cn = new SqlConnection(text);
			try
			{
				SqlCommand cmd = new SqlCommand("ALTER DATABASE [" + val.InitialCatalog + "] SET Multi_USER", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.Text;
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
					((DbConnection)(object)cn).Close();
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SetMultiUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 380);
			WebErrorLog.Instance.StartLog(ex, "", "SetMultiUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 381);
		}
		return ret;
	}

	public async Task SetDatabaseUserModeAsync(string dbName, string connectionString, DatabaseUserMode mode)
	{
		_ = 1;
		try
		{
			GC.Collect();
			SqlConnection.ClearAllPools();
			if (string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(connectionString))
			{
				return;
			}
			string text = "";
			switch (mode)
			{
			case DatabaseUserMode.SingleUser:
				text = "ALTER DATABASE [DBName] SET SINGLE_USER";
				break;
			case DatabaseUserMode.MultiUser:
				text = "ALTER DATABASE [DBName] SET MULTI_USER";
				break;
			case DatabaseUserMode.ReadOnly:
				text = "ALTER DATABASE [DBName] SET READ_ONLY WITH NO_WAIT";
				break;
			case DatabaseUserMode.ReadWrite:
				text = "ALTER DATABASE [DBName] SET READ_WRITE WITH NO_WAIT";
				break;
			}
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				SqlCommand cmd = new SqlCommand(text, cn);
				try
				{
					((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("DBName", dbName);
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDatabaseUserModeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 417);
		}
	}

	public async Task<float> GetBackUpVersionAsync(ISettingRepo repo, string connctionString, string backUpFileName)
	{
		float ver = 0f;
		int line = 0;
		try
		{
			SqlConnection cn = new SqlConnection(connctionString);
			try
			{
				line = 11;
				if (string.IsNullOrEmpty(backUpFileName))
				{
					return ver;
				}
				line = 12;
				string text = "RESTORE HEADERONLY FROM DISK ='" + backUpFileName + "'";
				line = 13;
				SqlCommand sqlCommand = new SqlCommand(text, cn);
				try
				{
					line = 14;
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					line = 15;
					SqlDataReader dr = await sqlCommand.ExecuteReaderAsync();
					try
					{
						line = 16;
						while (await ((DbDataReader)(object)dr).ReadAsync())
						{
							line = 17;
							Console.WriteLine();
							line = 18;
							string text2 = string.Format("{0}", ((DbDataReader)(object)dr)["DatabaseVersion"]);
							line = 19;
							if (!string.IsNullOrEmpty(text2))
							{
								line = 20;
								ver = text2 switch
								{
									"406" => 6f, 
									"408" => 6.5f, 
									"515" => 7f, 
									"539" => 2000f, 
									"611" => 2005f, 
									"612" => 2005f, 
									"655" => 2008f, 
									"661" => 2008f, 
									"706" => 2012f, 
									"782" => 2014f, 
									"869" => 2017f, 
									"895" => 2019f, 
									"852" => 2016f, 
									_ => 0f, 
								};
								line = 21;
								await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Success, "دریافت نسخه فایل پشتیبان", $":نسخه فایل پشتیبان {ver}", backUpFileName, connctionString);
								return ver;
							}
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
					}
					finally
					{
						((IDisposable)dr)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)sqlCommand)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Error In Line{line} With ConnectionString:{connctionString} AND BackUpFileName:{backUpFileName}", "GetBackUpVersionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 474);
			ver = 0f;
			await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Error, "خطا در دریافت نسخه فایل پشتیبان", ex.Message + "\r\nConnectionString:{connctionString} AND BackUpFileName:{backUpFileName}", backUpFileName, connctionString);
		}
		return ver;
	}

	public Task<EnSqlAuthenticationMode> GetSqlAuthenticationModeAsync(string connectionString)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Expected O, but got Unknown
		try
		{
			EnSqlAuthenticationMode result = EnSqlAuthenticationMode.WindowsAuthentication;
			SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(connectionString);
			if (!string.IsNullOrEmpty(val.UserID) && !string.IsNullOrEmpty(val.Password))
			{
				result = EnSqlAuthenticationMode.SqlServerAuthentication;
			}
			return Task.FromResult(result);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSqlAuthenticationModeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseAction.cs", 495);
			throw;
		}
	}
}
