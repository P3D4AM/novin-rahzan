using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using Microsoft.EntityFrameworkCore;
using Ninject;
using Ninject.Modules;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;

namespace AccSqlServerPersistence;

public abstract class CacheBase : IDisposable
{
	public bool IsDisposed { get; set; }

	public async Task<ReturnedSaveFuncInfoWithValue<ClsCache>> GetCache(IEnumerable<EnCacheConfig> configs, string contextGuid, string connectionString, string imagePath, int cachingTimeSeconds, ENSource source, IUserInterfaceAggregator uiAggregator, CancellationToken token)
	{
		using (new JaegerMonitor("GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs"))
		{
			ReturnedSaveFuncInfoWithValue<ClsCache> ret = new ReturnedSaveFuncInfoWithValue<ClsCache>();
			int line = 0;
			Stopwatch totalSwTime = Stopwatch.StartNew();
			Stopwatch sw7 = Stopwatch.StartNew();
			StringBuilder sb = new StringBuilder(100000);
			try
			{
				_ = 11;
				try
				{
					ret.value = ClsCache.GetCache(contextGuid);
					if (ret.value != null)
					{
						return ret;
					}
					sb.AppendLine("first initiazlization. Guid:" + contextGuid);
					string assembleVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
					if (string.IsNullOrEmpty(uiAggregator.AppPathFolder))
					{
						ret.AddError("AppPathFolder is null or empty", "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 46);
						ret.throwExceptionIfError();
					}
					AccSqlServerPersistence.AssistanceClasses.ConnectionLogger.Init(assembleVersion, source, 0L, uiAggregator.AppPathFolder);
					if (uiAggregator.TransferDefault == null)
					{
						uiAggregator.TransferDefault = new TransferDefault(defaultValues: false)
						{
							Hesabs = true
						};
					}
					line = 1;
					sb.AppendLine($"Checking SqlConnection. {sw7.ElapsedMilliseconds:N0}(ms)");
					sw7 = Stopwatch.StartNew();
					ReturnedSaveFuncInfo obj = await CheckConnectionString(contextGuid, connectionString, token);
					sb.AppendLine($"checkConnectionHas been finished. {sw7.ElapsedMilliseconds:N0}(ms)");
					sw7 = Stopwatch.StartNew();
					ReturnedSaveFuncInfoWithValue<ClsCache> returnedSaveFuncInfoWithValue;
					if (obj.HasError)
					{
						await Task.Delay(15000);
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionString(contextGuid, connectionString, token), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 62);
						WebErrorLog.Instance.StartLog("create cache has error of coonection string not valid, second try has " + $"error:{ret.HasError} " + "errorIs:" + ret.GetErrorMessage() + "ConnectionString:" + connectionString, "", "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 63);
					}
					ret.throwExceptionIfError();
					line = 2;
					if (configs.Contains(EnCacheConfig.Global))
					{
						sb.AppendLine($"Start running GlobalScripts... {sw7.ElapsedMilliseconds:N0}(ms)");
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await RunScripts(contextGuid, source, new List<EnCacheConfig> { EnCacheConfig.Global }, connectionString, isCheckCrc: false, token), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 75);
						sb.AppendLine($"GlobalScripts fished. {sw7.ElapsedMilliseconds:N0}(ms)");
						sw7 = Stopwatch.StartNew();
					}
					long lkSerial = await uiAggregator.GetLkSerialAsync(contextGuid);
					sb.AppendLine($"checking exe version... {sw7.ElapsedMilliseconds:N0}(ms)");
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckExeVersionAsync(contextGuid, connectionString, uiAggregator?.ExeVersion, configs, source, lkSerial, token), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 82);
					sb.AppendLine($"Check exe Version has been finishe. {sw7.ElapsedMilliseconds:N0}(ms)");
					sw7 = Stopwatch.StartNew();
					sb.AppendLine($"checking cache version... {sw7.ElapsedMilliseconds:N0}(ms)");
					ret.throwExceptionIfError();
					ReturnedSaveFuncInfoWithValue<bool> retVersion = await CheckCacheVersionAsync(contextGuid, connectionString, uiAggregator?.ExeVersion, configs, uiAggregator.DbMigrator, source, token);
					ret.AddReturnedValue(retVersion, ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 89);
					ret.throwExceptionIfError();
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
					line = 150;
					sb.AppendLine($"getting cache... {sw7.ElapsedMilliseconds:N0}(ms)");
					line = 151;
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.value = await ClsCache.GetCacheAsync(sb, imagePath, cachingTimeSeconds, source, uiAggregator.ImageResizer, uiAggregator.GetPersistenceAggregator(connectionString, context, list), list, DI.Kernel, uiAggregator.SendData2Server, uiAggregator.ExeVersion, uiAggregator.MasterSendData2Server, uiAggregator.Notification);
					line = 152;
					sb.AppendLine($"cache has been created. {sw7.ElapsedMilliseconds:N0}(ms)");
					line = 153;
					sw7 = Stopwatch.StartNew();
					line = 154;
					ret.throwExceptionIfError();
					line = 155;
					sb.AppendLine($"starting runScripts... {sw7.ElapsedMilliseconds:N0}(ms)");
					line = 156;
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await RunScripts(contextGuid, source, configs, connectionString, isCheckCrc: true, token), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 124);
					line = 157;
					sb.AppendLine($"runScrips has been finished. {sw7.ElapsedMilliseconds:N0}(ms)");
					line = 158;
					sw7 = Stopwatch.StartNew();
					line = 159;
					ret.throwExceptionIfError();
					line = 160;
					sb.AppendLine($"adding default values... {sw7.ElapsedMilliseconds:N0}(ms)");
					line = 161;
					if (retVersion.value)
					{
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await AddDefaults.InsertDefaultDataAsync(EnSystem.None, source, context.Guid, uiAggregator.TransferDefault), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 145);
					}
					line = 162;
					ret.throwExceptionIfError();
					line = 163;
					sb.AppendLine($"default values has been finished. {sw7.ElapsedMilliseconds:N0}(ms)");
					line = 164;
					sw7 = Stopwatch.StartNew();
					line = 165;
					sb.AppendLine($"starting RunSqlScriptsAsync... {sw7.ElapsedMilliseconds:N0}(ms)");
					line = 166;
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await RunScript.Instance.RunSqlScriptsAsync(BackupSettingsRepo.GetInstance(contextGuid), connectionString, source), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 155);
					line = 167;
					sb.AppendLine($"RunSqlScriptsAsync has been finished. {sw7.ElapsedMilliseconds:N0}(ms)");
					line = 168;
					sw7 = Stopwatch.StartNew();
					line = 169;
					sb.AppendLine($"AfterCacheInitialized... {sw7.ElapsedMilliseconds:N0}(ms)");
					line = 170;
					await uiAggregator.AfterCacheInitialized();
					line = 171;
					sb.AppendLine($"AfterCacheInitialized has been finished. {sw7.ElapsedMilliseconds:N0}(ms)");
					if (!ret.HasError && source != ENSource.TimeAttendance)
					{
						Task.Run(() => ClsCache.GetCache(contextGuid).BusinessAggregator.FullTextWordsBusiness.UpdateFullTextSearchIndex(default(CancellationToken)));
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}\r\n" + $"Sb=>{sb}", "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 174);
					ret.AddReturnedValue(ex2, "", "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 176);
				}
			}
			finally
			{
				if (ret.HasError)
				{
					ReturnedSaveFuncInfoWithValue<ClsCache> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await LocalSettingPersistenceRepository.SaveAsync(contextGuid, connectionString, "AccCacheVersion", "", "", ""), ReturnedState.None, "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 181);
				}
				Console.WriteLine("_____________________________________\r\n" + $"Cache.GetCache totalTime:{totalSwTime.ElapsedMilliseconds:N0}(ms)\r\n" + $"hasError:{ret.HasError}\r\n" + $"sb:{sb}\r\n" + "_____________________________________\r\n");
			}
			return ret;
		}
	}

	protected abstract Task<ReturnedSaveFuncInfoWithValue<bool>> CheckCacheVersionAsync(string contextGuid, string connectionString, string exeVersion, IEnumerable<EnCacheConfig> configs, IDbMigrator migrator, ENSource source, CancellationToken token);

	protected abstract Task<ReturnedSaveFuncInfo> CheckExeVersionAsync(string contextGuid, string connectionString, string exeVersion, IEnumerable<EnCacheConfig> configs, ENSource source, long lkSerial, CancellationToken token);

	protected virtual async Task<ReturnedSaveFuncInfoWithValue<bool>> UpdateDatabaseSchemaAsync(string contextGuid, string connectionString, IEnumerable<EnCacheConfig> configs, IDbMigrator migrator, ENSource source, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			int timeout = 600000;
			bool isUpdateSchema = false;
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = await HasPendingMigration(configs, connectionString, timeout, token);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "UpdateDatabaseSchemaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 202);
			if (ret.HasError)
			{
				return ret;
			}
			bool flag = true;
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue2;
			if (returnedSaveFuncInfoWithValue.value && flag)
			{
				returnedSaveFuncInfoWithValue2 = ret;
				returnedSaveFuncInfoWithValue2.AddReturnedValue(await UpdateMigrationsAsync(configs, connectionString, timeout, token), ReturnedState.None, "UpdateDatabaseSchemaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 212);
				isUpdateSchema = true;
			}
			if (ret.HasError)
			{
				return ret;
			}
			returnedSaveFuncInfoWithValue2 = ret;
			returnedSaveFuncInfoWithValue2.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_DropReportGenerator", checkCrcBeforeExecution: false, source, Resources.DropReportGenerator, connectionString), ReturnedState.None, "UpdateDatabaseSchemaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 218);
			ret.value = isUpdateSchema;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateDatabaseSchemaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 224);
			ret.AddReturnedValue(ex, "", "UpdateDatabaseSchemaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 225);
		}
		return ret;
	}

	protected abstract Task<ReturnedSaveFuncInfo> RunScripts(string contextGuid, ENSource source, IEnumerable<EnCacheConfig> configs, string connectionString, bool isCheckCrc, CancellationToken token);

	protected abstract Task<ReturnedSaveFuncInfo> CheckConnectionString(string contextGuid, string con, CancellationToken token = default(CancellationToken));

	protected abstract Task<ReturnedSaveFuncInfo> CreateDatabaseAsync(string contextGuid, string connectionString, CancellationToken token);

	protected virtual async Task<ReturnedSaveFuncInfo> UpdateMigrationsAsync(IEnumerable<EnCacheConfig> configs, string connectionString, int timeout, CancellationToken token)
	{
		using (new JaegerMonitor("UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs"))
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				line = 1;
				string dbname = BaseDataBaseAction.Instance.GetDbName(connectionString);
				line = 2;
				await BaseDataBaseAction.Instance.SetNumericRoundAbortAsync(dbname, connectionString);
				line = 3;
				await BaseDataBaseAction.Instance.ChangeCompatibilityLevelAsync(dbname, 150, connectionString);
				line = 4;
				using IEnumerator<EnCacheConfig> enumerator = configs.GetEnumerator();
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case EnCacheConfig.Accounting:
						line = 10;
						await RelationalDatabaseFacadeExtensions.MigrateAsync(((DbContext)new Acc_ModelContext(connectionString, "", timeout, "", "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 255)).Database, token);
						break;
					case EnCacheConfig.Global:
						line = 15;
						await RelationalDatabaseFacadeExtensions.MigrateAsync(((DbContext)new Global_ModelContext(connectionString, "", "", timeout, "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 259)).Database, token);
						break;
					case EnCacheConfig.Advertise:
						line = 20;
						await RelationalDatabaseFacadeExtensions.MigrateAsync(((DbContext)new Adv_ModelContext(connectionString, "", "", timeout, "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 263)).Database, token);
						break;
					case EnCacheConfig.Attendance:
						line = 20;
						await RelationalDatabaseFacadeExtensions.MigrateAsync(((DbContext)new Att_ModelContext(connectionString, "", "", timeout, "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 267)).Database, token);
						break;
					case EnCacheConfig.Department:
						line = 20;
						await RelationalDatabaseFacadeExtensions.MigrateAsync(((DbContext)new Dpt_ModelContext(connectionString, "", "", timeout, "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 271)).Database, token);
						break;
					default:
						line = 25;
						throw new NotImplementedException();
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 280);
				ret.AddReturnedValue(ex, "", "UpdateMigrationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 281);
			}
			sw.Stop();
			return ret;
		}
	}

	protected virtual async Task<ReturnedSaveFuncInfoWithValue<bool>> HasPendingMigration(IEnumerable<EnCacheConfig> configs, string connectionString, int timeout, CancellationToken token)
	{
		using (new JaegerMonitor("HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs"))
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
				await BaseDataBaseAction.Instance.ChangeCompatibilityLevelAsync(dbname, 140, connectionString);
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
							Acc_ModelContext accContext = new Acc_ModelContext(connectionString, "", timeout, "", "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 312);
							IEnumerable<string> pendingMigrations = await RelationalDatabaseFacadeExtensions.GetPendingMigrationsAsync(((DbContext)accContext).Database, token);
							await accContext.DisposeAsync("HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs");
							pendingMigrations.Count();
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
							Global_ModelContext globalContext = new Global_ModelContext(connectionString, "", "", timeout, "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 326);
							IEnumerable<string> pendingMigrations = await RelationalDatabaseFacadeExtensions.GetPendingMigrationsAsync(((DbContext)globalContext).Database, token);
							await globalContext.DisposeAsync("HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs");
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
							Adv_ModelContext advertiseContext = new Adv_ModelContext(connectionString, "", "", 60000, "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 339);
							IEnumerable<string> pendingMigrations = await RelationalDatabaseFacadeExtensions.GetPendingMigrationsAsync(((DbContext)advertiseContext).Database, token);
							await advertiseContext.DisposeAsync("HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs");
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
							Att_ModelContext attendanceContext = new Att_ModelContext(connectionString, "", "", 60000, "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 350);
							IEnumerable<string> pendingMigrations = await RelationalDatabaseFacadeExtensions.GetPendingMigrationsAsync(((DbContext)attendanceContext).Database, token);
							await attendanceContext.DisposeAsync("HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs");
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
							Dpt_ModelContext dptContext = new Dpt_ModelContext(connectionString, "", "", 60000, "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 361);
							IEnumerable<string> pendingMigrations = await RelationalDatabaseFacadeExtensions.GetPendingMigrationsAsync(((DbContext)dptContext).Database, token);
							await dptContext.DisposeAsync("HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs");
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
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 378);
				ret.AddReturnedValue(ex, "", "HasPendingMigration", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 379);
			}
			return ret;
		}
	}

	public virtual void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\CacheBase.cs", 392);
		}
	}
}
