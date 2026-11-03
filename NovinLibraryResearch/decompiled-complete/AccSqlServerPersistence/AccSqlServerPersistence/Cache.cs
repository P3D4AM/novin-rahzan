using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.AssistanceClasses;
using AccSqlServerPersistence.Ertegha;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories;
using AccSqlServerPersistence.Properties;
using ClientHub;
using EntityCache;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Interfaces.Sync;
using Microsoft.EntityFrameworkCore;
using Ninject;
using Ninject.Modules;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Settings;

namespace AccSqlServerPersistence;

public class Cache : CacheBase
{
	private static Cache cache = new Cache();

	public static Cache Instance => cache;

	public async Task<ReturnedSaveFuncInfoWithValue<ClsCache>> GetCache(IEnumerable<EnCacheConfig> configs, string contextGuid, string connectionString, string sqliteDbPath, string imagePath, int cachingTimeSeconds, ENSource source, IUserInterfaceAggregator uiAggregator)
	{
		using (new JaegerMonitor("GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs"))
		{
			ReturnedSaveFuncInfoWithValue<ClsCache> ret = new ReturnedSaveFuncInfoWithValue<ClsCache>();
			int line = 0;
			Stopwatch totalSwTime = Stopwatch.StartNew();
			Stopwatch sw7 = Stopwatch.StartNew();
			StringBuilder sb = new StringBuilder(100000);
			ReturnedSaveFuncInfoWithValue<ClsCache> result;
			try
			{
				_ = 12;
				try
				{
					if (!(configs?.Any() ?? false))
					{
						configs = new List<EnCacheConfig>
						{
							EnCacheConfig.Attendance,
							EnCacheConfig.Accounting,
							EnCacheConfig.Global
						};
					}
					ret.value = ClsCache.GetCache(contextGuid);
					if (ret.value != null)
					{
						result = ret;
						goto IL_118a;
					}
					sb.AppendLine("first initiazlization. Guid:" + contextGuid);
					string exeVersionString = Assembly.GetExecutingAssembly().GetName().Version.ToString();
					FileInfo fileInfo = new FileInfo(Assembly.GetEntryAssembly()?.Location);
					AccSqlServerPersistence.AssistanceClasses.ConnectionLogger.Init(exeVersionString, source, 0L, fileInfo.DirectoryName);
					if (uiAggregator != null)
					{
						if (uiAggregator.TransferDefault == null)
						{
							uiAggregator.TransferDefault = new TransferDefault(defaultValues: false)
							{
								Hesabs = true
							};
						}
					}
					line = 1;
					sb.AppendLine($"Checking SqlConnection. {sw7.ElapsedMilliseconds:N0}(ms)");
					sw7 = Stopwatch.StartNew();
					ReturnedSaveFuncInfo obj = await CheckConnectionString(contextGuid, connectionString);
					sb.AppendLine($"checkConnectionHas been finished. {sw7.ElapsedMilliseconds:N0}(ms)");
					sw7 = Stopwatch.StartNew();
					ReturnedSaveFuncInfoWithValue<ClsCache> returnedSaveFuncInfoWithValue;
					if (obj.HasError)
					{
						await Task.Delay(15000);
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionString(contextGuid, connectionString), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 75);
						WebErrorLog.Instance.StartLog("create cache has error of coonection string not valid, second try has " + $"error:{ret.HasError} " + "errorIs:" + ret.GetErrorMessage() + "ConnectionString:" + connectionString, "", "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 76);
					}
					ret.throwExceptionIfError();
					line = 2;
					if (configs.Contains(EnCacheConfig.Global))
					{
						sb.AppendLine($"Start running GlobalScripts... {sw7.ElapsedMilliseconds:N0}(ms)");
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await RunScripts(contextGuid, source, new List<EnCacheConfig> { EnCacheConfig.Global }, connectionString, isCheckCrc: false, default(CancellationToken)), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 87);
						sb.AppendLine($"GlobalScripts fished. {sw7.ElapsedMilliseconds:N0}(ms)");
						sw7 = Stopwatch.StartNew();
					}
					long lkSerial = await uiAggregator.GetLkSerialAsync(contextGuid);
					sb.AppendLine($"checking exe version... {sw7.ElapsedMilliseconds:N0}(ms)");
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckExeVersionAsync(contextGuid, connectionString, uiAggregator?.ExeVersion, configs, source, lkSerial, default(CancellationToken)), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 93);
					sb.AppendLine($"Check exe Version has been finishe. {sw7.ElapsedMilliseconds:N0}(ms)");
					sw7 = Stopwatch.StartNew();
					sb.AppendLine($"checking cache version... {sw7.ElapsedMilliseconds:N0}(ms)");
					ret.throwExceptionIfError();
					ReturnedSaveFuncInfoWithValue<bool> retVersion = await CheckCacheVersionAsync(contextGuid, connectionString, exeVersionString, configs, uiAggregator.DbMigrator, source, default(CancellationToken));
					ret.AddReturnedValue(retVersion, ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 100);
					if (ret.HasError)
					{
						result = ret;
						goto IL_118a;
					}
					sb.AppendLine($"Check cache Version has been finishe. {sw7.ElapsedMilliseconds:N0}(ms)");
					sw7 = Stopwatch.StartNew();
					line = 10;
					WebContextHolder context = WebContextHolder.GetWebContextHolder(contextGuid);
					List<IDatabaseManager> list = new List<IDatabaseManager>();
					foreach (EnCacheConfig config in configs)
					{
						sb.AppendLine($"initializing cache context({config})... {sw7.ElapsedMilliseconds:N0}(ms)");
						list.Add(new ContextHolder(config, connectionString, context));
						sb.AppendLine($"context({config}) has been initialized. {sw7.ElapsedMilliseconds:N0}(ms)");
						sw7 = Stopwatch.StartNew();
					}
					NinjectSettings settings = new NinjectSettings
					{
						LoadExtensions = false
					};
					if (DI.Kernel == null)
					{
						DI.Kernel = new NinjectLoggerKernel((INinjectSettings)(object)settings, (INinjectModule)AccSqlServerPersistence.AssistanceClasses.SignalrNinjectModule.Instance);
					}
					line = 15;
					sb.AppendLine($"getting cache... {sw7.ElapsedMilliseconds:N0}(ms)");
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.value = await ClsCache.GetCacheAsync(sb, imagePath, cachingTimeSeconds, source, uiAggregator.ImageResizer, uiAggregator.GetPersistenceAggregator(connectionString, context, list), list, DI.Kernel, uiAggregator.SendData2Server, uiAggregator.ExeVersion, uiAggregator.MasterSendData2Server, uiAggregator.Notification);
					sb.AppendLine($"cache has been created. {sw7.ElapsedMilliseconds:N0}(ms)");
					sw7 = Stopwatch.StartNew();
					ret.throwExceptionIfError();
					sb.AppendLine($"starting runScripts... {sw7.ElapsedMilliseconds:N0}(ms)");
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await RunScripts(contextGuid, source, configs, connectionString, isCheckCrc: true, default(CancellationToken)), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 131);
					sb.AppendLine($"runScrips has been finished. {sw7.ElapsedMilliseconds:N0}(ms)");
					sw7 = Stopwatch.StartNew();
					ret.throwExceptionIfError();
					line = 16;
					sb.AppendLine($"adding default values... {sw7.ElapsedMilliseconds:N0}(ms)");
					if (retVersion.value)
					{
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await AddDefaults.InsertDefaultDataAsync(EnSystem.None, source, context.Guid, uiAggregator.TransferDefault), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 139);
					}
					if (configs?.Contains(EnCacheConfig.Global) ?? false)
					{
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await AddDefaults.InsertDefaultGlobalDataAsync(contextGuid), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 142);
					}
					ret.throwExceptionIfError();
					sb.AppendLine($"default values has been finished. {sw7.ElapsedMilliseconds:N0}(ms)");
					sw7 = Stopwatch.StartNew();
					sb.AppendLine($"starting RunSqlScriptsAsync... {sw7.ElapsedMilliseconds:N0}(ms)");
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await RunScript.Instance.RunSqlScriptsAsync(BackupSettingsRepo.GetInstance(contextGuid), connectionString, source), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 150);
					sb.AppendLine($"RunSqlScriptsAsync has been finished. {sw7.ElapsedMilliseconds:N0}(ms)");
					sw7 = Stopwatch.StartNew();
					sb.AppendLine($"AfterCacheInitialized... {sw7.ElapsedMilliseconds:N0}(ms)");
					await uiAggregator.AfterCacheInitialized();
					sb.AppendLine($"AfterCacheInitialized has been finished. {sw7.ElapsedMilliseconds:N0}(ms)");
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 161);
					ret.AddReturnedValue(ex, "", "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 162);
				}
			}
			finally
			{
				if (ret.HasError)
				{
					ReturnedSaveFuncInfoWithValue<ClsCache> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await LocalSettingPersistenceRepository.SaveAsync(contextGuid, connectionString, "AccCacheVersion", "", "", ""), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 167);
				}
				Console.WriteLine("_____________________________________\r\n" + $"Cache.GetCache totalTime:{totalSwTime.ElapsedMilliseconds:N0}(ms)\r\n" + $"hasError:{ret.HasError}\r\n" + $"sb:{sb}\r\n" + "_____________________________________\r\n");
				if (!ret.HasError && source != ENSource.TimeAttendance)
				{
					Task.Run(() => ClsCache.GetCache(contextGuid).BusinessAggregator.FullTextWordsBusiness.UpdateFullTextSearchIndex(default(CancellationToken)));
				}
			}
			return ret;
			IL_118a:
			return result;
		}
	}

	protected override async Task<ReturnedSaveFuncInfoWithValue<bool>> CheckCacheVersionAsync(string contextGuid, string connectionString, string exeVersion, IEnumerable<EnCacheConfig> configs, IDbMigrator migrator, ENSource source, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<bool> res = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			res.value = false;
			long num = ((await new LocalSettingPersistenceRepository(new ContextHolder(configs.FirstOrDefault(), connectionString, new WebContextHolder())).GetCacheVersionAsync(connectionString, token)) ?? new VersionModel()).Version.VersionStringToLong(removeMinorPart: true);
			if (exeVersion.VersionStringToLong(removeMinorPart: true) < num)
			{
				return res;
			}
			if (exeVersion.VersionStringToLong(removeMinorPart: true) == num)
			{
				return res;
			}
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = await UpdateDatabaseSchemaAsync(contextGuid, connectionString, configs, migrator, source, token);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CheckCacheVersionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 193);
			if (res.HasError)
			{
				return res;
			}
			if (returnedSaveFuncInfoWithValue.value)
			{
				ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue2 = res;
				returnedSaveFuncInfoWithValue2.AddReturnedValue(await LocalSettingPersistenceRepository.SaveAsync(contextGuid, connectionString, "AccCacheVersion", exeVersion, "", "", token), ReturnedState.None, "CheckCacheVersionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 196);
			}
			res.value = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckCacheVersionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 201);
			res.AddReturnedValue(ex, "", "CheckCacheVersionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 202);
		}
		return res;
	}

	protected override async Task<ReturnedSaveFuncInfo> CheckExeVersionAsync(string contextGuid, string connectionString, string exeVersion, IEnumerable<EnCacheConfig> configs, ENSource source, long lkSerial, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			List<ISqlServerTableInformation> obj = await SqlServerTableInformation.GetAllAsync(connectionString, "Acc_LocalSetting");
			line = 1;
			if (!(obj?.Any() ?? false))
			{
				return res;
			}
			line = 2;
			LocalSettingPersistenceRepository localSettingPersistenceRepository = new LocalSettingPersistenceRepository(new ContextHolder(configs.FirstOrDefault(), connectionString, new WebContextHolder()));
			line = 3;
			VersionModel obj2 = (await localSettingPersistenceRepository.GetExeVersionAsync(connectionString, token)) ?? new VersionModel();
			line = 4;
			long num = obj2.Version.VersionStringToLong(removeMinorPart: true);
			line = 5;
			if (exeVersion.VersionStringToLong(removeMinorPart: true) < num)
			{
				line = 6;
				string message = "کاربر گرامی نسخه نرم افزار".GetTranslate() + "  " + source.GetDisplay().GetTranslate() + " " + " شما".GetTranslate() + " " + $" {exeVersion?.VersionStringToLong(removeMinorPart: true)}" + " میباشد که قادر به باز کردن بانک اطلاعاتی با نسخه".GetTranslate() + " " + $" {num}" + "  نمیباشد.\r\n" + "لطفا آخرین نسخه نرم افزار را از آدرس ذیل دانلود کنید.".GetTranslate() + "\r\n " + ResourceManagement.TextBrandResourceManagement?.Url1;
				res.AddError(EnSystem.None, message, "", "CheckExeVersionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 228);
				return res;
			}
			line = 7;
			if (exeVersion.VersionStringToLong(removeMinorPart: true) == num)
			{
				return res;
			}
			line = 8;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await LocalSettingPersistenceRepository.SaveAsync(contextGuid, connectionString, "AccExeVersion", exeVersion, "", "", token), ReturnedState.None, "CheckExeVersionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 234);
			if (lkSerial > 0)
			{
				Task.Run(() => SendVersionUsageToServerAsync(contextGuid, connectionString, lkSerial, source, exeVersion, token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "CheckExeVersionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 240);
			res.AddReturnedValue(ex, "", "CheckExeVersionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 241);
		}
		return res;
	}

	protected override async Task<ReturnedSaveFuncInfoWithValue<bool>> UpdateDatabaseSchemaAsync(string contextGuid, string connectionString, IEnumerable<EnCacheConfig> configs, IDbMigrator migrator, ENSource source, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			int timeout = 600000;
			bool isUpdateSchema = false;
			ReturnedSaveFuncInfoWithValue<bool> hasPendingMigration = await HasPendingMigration(configs, connectionString, timeout, token);
			ret.AddReturnedValue(hasPendingMigration, ReturnedState.None, "UpdateDatabaseSchemaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 253);
			if (ret.HasError)
			{
				return ret;
			}
			bool isMigrationAllowed = true;
			string dbname = BaseDataBaseAction.Instance.GetDbName(connectionString);
			await BaseDataBaseAction.Instance.SetNumericRoundAbortAsync(dbname, connectionString);
			await BaseDataBaseAction.Instance.ChangeCompatibilityLevelAsync(dbname, 150, connectionString);
			await BaseDataBaseAction.Instance.SetScalarUdfInliningAsync(connectionString);
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue;
			if (hasPendingMigration.value && isMigrationAllowed)
			{
				returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await UpdateMigrationsAsync(configs, connectionString, timeout, token), ReturnedState.None, "UpdateDatabaseSchemaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 267);
				isUpdateSchema = true;
			}
			if (ret.HasError)
			{
				return ret;
			}
			returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_DropReportGenerator", checkCrcBeforeExecution: true, source, Resources.DropReportGenerator, connectionString), ReturnedState.None, "UpdateDatabaseSchemaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 273);
			ret.value = isUpdateSchema;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateDatabaseSchemaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 278);
			ret.AddReturnedValue(ex, "", "UpdateDatabaseSchemaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 279);
		}
		return ret;
	}

	protected override async Task<ReturnedSaveFuncInfo> RunScripts(string contextGuid, ENSource source, IEnumerable<EnCacheConfig> configs, string connectionString, bool isCheckCrc, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			long crc = 0L;
			ILocalSettingRepository localSettingRepository = ClsCache.GetCache(contextGuid)?.CacheRepository?.ILocalSetting;
			if (isCheckCrc && localSettingRepository != null)
			{
				long num = LocalSettingBusiness.Get(contextGuid, "ResourceCrc", token).ParseToLong(0L);
				ReturnedSaveFuncInfoWithValue<long> returnedSaveFuncInfoWithValue = ResourceComparison.CheckCrc(configs, source);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "RunScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 294);
				if (ret.HasError)
				{
					return ret;
				}
				crc = returnedSaveFuncInfoWithValue.value;
				if (num == crc)
				{
					return ret;
				}
			}
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
				((DbConnection)(object)cn).Close();
				if (configs.Any((EnCacheConfig x) => x == EnCacheConfig.Accounting))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_Acc_sp", checkCrcBeforeExecution: false, source, Resources.Acc_sp, connectionString), ReturnedState.None, "RunScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 305);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_FixAccountingData", checkCrcBeforeExecution: false, source, Resources.FixAccountingData, connectionString), ReturnedState.None, "RunScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 306);
					if (source == ENSource.Accounting)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_DropConstraintForClientsOfAccounting", checkCrcBeforeExecution: false, source, Resources.DropConstraintForClientsOfAccounting, connectionString), ReturnedState.None, "RunScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 308);
					}
					else
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_ErteghaFuctions_Baje", checkCrcBeforeExecution: false, source, Resources.ErteghaFuctions_Baje, connectionString), ReturnedState.None, "RunScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 311);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_DropConstraintForClientsOfSignalR", checkCrcBeforeExecution: false, source, Resources.DropConstraintForClientsOfSignalR, connectionString), ReturnedState.None, "RunScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 312);
					}
				}
				if (configs.Any((EnCacheConfig x) => x == EnCacheConfig.Advertise))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_Advertise_sp", checkCrcBeforeExecution: false, source, Resources.Advertise_sp, connectionString), ReturnedState.None, "RunScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 317);
				}
				if (configs.Any((EnCacheConfig x) => x == EnCacheConfig.Global))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_Global_sp", checkCrcBeforeExecution: false, source, Resources.Global_sp, connectionString), ReturnedState.None, "RunScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 320);
				}
				if (configs.Any((EnCacheConfig x) => x == EnCacheConfig.Attendance))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_Attendance_sp", checkCrcBeforeExecution: false, source, Resources.Attendance_sp, connectionString), ReturnedState.None, "RunScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 323);
				}
				if (isCheckCrc)
				{
					await LocalSettingPersistenceRepository.SaveAsync(contextGuid, connectionString, "ResourceCrc", crc.ToString(), "", "", token);
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RunScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 330);
			ret.AddReturnedValue(ex, "", "RunScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 331);
		}
		return ret;
	}

	protected override async Task<ReturnedSaveFuncInfo> CheckConnectionString(string contextGuid, string con, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("CheckConnectionString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				SqlConnection cn = new SqlConnection(con);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					((DbConnection)(object)cn).Close();
				}
				finally
				{
					((IDisposable)cn)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "CheckConnectionString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 347);
				WebErrorLog.Instance.StartLog(ex, "ConnectionString:" + con, "CheckConnectionString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 348);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CreateDatabaseAsync(contextGuid, con, token), ReturnedState.None, "CheckConnectionString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 349);
			}
			return ret;
		}
	}

	protected override async Task<ReturnedSaveFuncInfo> CreateDatabaseAsync(string contextGuid, string connectionString, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue((await BaseDataBaseAction.Instance.CreateDataBaseAsync(BackupSettingsRepo.GetInstance(contextGuid), BaseDataBaseAction.Instance.GetDbName(connectionString), connectionString)).ReturnedSaveFuncInfo, ReturnedState.None, "CreateDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 358);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "connectionString:" + connectionString, "CreateDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 362);
			ret.AddReturnedValue(ex, "", "CreateDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 363);
		}
		return ret;
	}

	protected override async Task<ReturnedSaveFuncInfo> UpdateMigrationsAsync(IEnumerable<EnCacheConfig> configs, string connectionString, int timeout, CancellationToken token)
	{
		using (new JaegerMonitor("UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs"))
		{
			Stopwatch sw = Stopwatch.StartNew();
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				using IEnumerator<EnCacheConfig> enumerator = configs.GetEnumerator();
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case EnCacheConfig.Accounting:
						line = 10;
						await RelationalDatabaseFacadeExtensions.MigrateAsync(((DbContext)new Acc_ModelContext(connectionString, "", timeout, "", "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 380)).Database, token);
						break;
					case EnCacheConfig.Global:
						line = 15;
						await RelationalDatabaseFacadeExtensions.MigrateAsync(((DbContext)new Global_ModelContext(connectionString, "", "", timeout, "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 384)).Database, token);
						break;
					case EnCacheConfig.Advertise:
						line = 20;
						await RelationalDatabaseFacadeExtensions.MigrateAsync(((DbContext)new Adv_ModelContext(connectionString, "", "", timeout, "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 388)).Database, token);
						break;
					case EnCacheConfig.Attendance:
						line = 20;
						await RelationalDatabaseFacadeExtensions.MigrateAsync(((DbContext)new Att_ModelContext(connectionString, "", "", timeout, "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 392)).Database, token);
						break;
					case EnCacheConfig.Department:
						line = 20;
						await RelationalDatabaseFacadeExtensions.MigrateAsync(((DbContext)new Dpt_ModelContext(connectionString, "", "", timeout, "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 396)).Database, token);
						break;
					default:
						line = 25;
						throw new NotImplementedException();
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 405);
				ret.AddReturnedValue(ex, "", "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 406);
			}
			sw.Stop();
			return ret;
		}
	}

	protected override async Task<ReturnedSaveFuncInfoWithValue<bool>> HasPendingMigration(IEnumerable<EnCacheConfig> configs, string connectionString, int timeout, CancellationToken token)
	{
		using (new JaegerMonitor("HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs"))
		{
			ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
			int line = 0;
			try
			{
				line = 1;
				string dbname = BaseDataBaseAction.Instance.GetDbName(connectionString);
				line = 2;
				await BaseDataBaseAction.Instance.SetNumericRoundAbortAsync(dbname, connectionString);
				line = 3;
				await BaseDataBaseAction.Instance.ChangeCompatibilityLevelAsync(dbname, 150, connectionString);
				line = 5;
				new List<string>();
				using (IEnumerator<EnCacheConfig> enumerator = configs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						switch (enumerator.Current)
						{
						case EnCacheConfig.Accounting:
						{
							line = 10;
							Acc_ModelContext accContext = new Acc_ModelContext(connectionString, "", timeout, "", "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 435);
							IEnumerable<string> pendingMigrations = await RelationalDatabaseFacadeExtensions.GetPendingMigrationsAsync(((DbContext)accContext).Database, token);
							await accContext.DisposeAsync("HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs");
							if (pendingMigrations.Any())
							{
								ret.value = true;
								return ret;
							}
							break;
						}
						case EnCacheConfig.Global:
						{
							line = 15;
							Global_ModelContext globalContext = new Global_ModelContext(connectionString, "", "", 60000, "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 446);
							IEnumerable<string> pendingMigrations = await RelationalDatabaseFacadeExtensions.GetPendingMigrationsAsync(((DbContext)globalContext).Database, token);
							await globalContext.DisposeAsync("HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs");
							if (pendingMigrations.Any())
							{
								ret.value = true;
								return ret;
							}
							break;
						}
						case EnCacheConfig.Advertise:
						{
							line = 20;
							Adv_ModelContext advertiseContext = new Adv_ModelContext(connectionString, "", "", 60000, "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 457);
							IEnumerable<string> pendingMigrations = await RelationalDatabaseFacadeExtensions.GetPendingMigrationsAsync(((DbContext)advertiseContext).Database, token);
							await advertiseContext.DisposeAsync("HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs");
							if (pendingMigrations.Any())
							{
								ret.value = true;
								return ret;
							}
							break;
						}
						case EnCacheConfig.Attendance:
						{
							line = 20;
							Att_ModelContext attendanceContext = new Att_ModelContext(connectionString, "", "", 60000, "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 468);
							IEnumerable<string> pendingMigrations = await RelationalDatabaseFacadeExtensions.GetPendingMigrationsAsync(((DbContext)attendanceContext).Database, token);
							await attendanceContext.DisposeAsync("HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs");
							if (pendingMigrations.Any())
							{
								ret.value = true;
								return ret;
							}
							break;
						}
						case EnCacheConfig.Department:
						{
							line = 20;
							Dpt_ModelContext dptContext = new Dpt_ModelContext(connectionString, "", "", 60000, "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 479);
							IEnumerable<string> pendingMigrations = await RelationalDatabaseFacadeExtensions.GetPendingMigrationsAsync(((DbContext)dptContext).Database, token);
							await dptContext.DisposeAsync("HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs");
							if (pendingMigrations.Any())
							{
								ret.value = true;
								return ret;
							}
							break;
						}
						default:
							line = 25;
							throw new NotImplementedException();
						}
					}
				}
				line = 30;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 496);
				ret.AddReturnedValue(ex, "", "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 497);
			}
			return ret;
		}
	}

	private async Task SendVersionUsageToServerAsync(string contextGuid, string connectionString, long lkSerial, ENSource source, string version, CancellationToken token)
	{
		_ = 2;
		try
		{
			string key = "Version_" + version;
			if (!((await LocalSettingPersistenceRepository.GetValueAsync(contextGuid, connectionString, key, token))?.ParseToBoolean() ?? false) && !(await WebVersionUsages.SendAsync(lkSerial, source, version)).HasError)
			{
				await LocalSettingPersistenceRepository.SaveAsync(contextGuid, connectionString, key, true.ToString(), "", "", token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendVersionUsageToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Cache.cs", 514);
		}
	}
}
