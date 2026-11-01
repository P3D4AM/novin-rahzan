using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence;
using AccSqlServerPersistence.AssistanceClasses;
using ClientHub;
using EntityCache;
using EntityCache.Interfaces.Sync;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Connection;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.EventArgs;
using Serilog;
using Serilog.Core;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Logger;
using TimeAttendanceDAL.Properties;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.Settings;

namespace TimeAttendanceDAL.Settings;

[Serializable]
[DoNotPruneType]
[DoNotObfuscate]
public class EntityCacheConfig
{
	private static ClsCache _cacheInstance;

	private static bool _initialized;

	public static CancellationTokenSource ConnecToken = new CancellationTokenSource();

	private static Buffer _bufferDevice;

	public AggregateSqlRepository CacheRepository { get; protected set; }

	public static ClsCache CacheInstance
	{
		get
		{
			if (_cacheInstance == null)
			{
				throw new NullReferenceException(" _CacheInstance is null");
			}
			return _cacheInstance;
		}
	}

	public string ImagePath { get; protected set; }

	public static Buffer BufferDevice
	{
		get
		{
			return _bufferDevice;
		}
		set
		{
			try
			{
				if (value != null)
				{
					if (_bufferDevice != null)
					{
						_bufferDevice.Connected -= Buffer_Connected;
						_bufferDevice.Disconnected -= Buffer_Disconnected;
						_bufferDevice.Connecting -= Buffer_Connecting;
						_bufferDevice.LoggedIn -= Buffer_LoggedIn;
					}
					_bufferDevice = value;
					_bufferDevice.Connected += Buffer_Connected;
					_bufferDevice.Disconnected += Buffer_Disconnected;
					_bufferDevice.Connecting += Buffer_Connecting;
					_bufferDevice.LoggedIn += Buffer_LoggedIn;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Buffer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 97);
			}
		}
	}

	public event EventNotification OnDisconnected;

	public event Func<object, EventArgs, Task> BufferConnecting;

	private static Task Buffer_Connecting(object sender, EventArgs e)
	{
		return Task.CompletedTask;
	}

	private static async Task<Guid> GetDataBaseGuidAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		Guid dbGuid = Guid.NewGuid();
		try
		{
			Guid.TryParse(await SettingAttendance.GetValueAsync(contextGuid, "DbGuid", 0, token), out dbGuid);
			if (Guid.Empty == dbGuid)
			{
				dbGuid = Guid.NewGuid();
				await SettingAttendance.SetValueAsync(contextGuid, "DbGuid", dbGuid.ToString(), 0, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetDataBaseGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 99);
		}
		return dbGuid;
	}

	public static async Task<ReturnedSaveFuncInfo> InitAsync(string contextGuid, string connectionString, string systemInfo, string version, EnConnectionType connectionType, bool connectSignalR = true, CancellationToken token = default(CancellationToken))
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo val = ret;
			val.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "RemoveNoNeedTables", true, (ENSource)5, Resources.RemoveNoNeedTables, connectionString, 30, default(CancellationToken)), (ReturnedState)0, "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 107);
			EnCacheConfig[] array = (EnCacheConfig[])(object)new EnCacheConfig[2]
			{
				default(EnCacheConfig),
				(EnCacheConfig)3
			};
			string text = Path.Combine(Application.StartupPath, "CacheImages_" + ClsConnection.GetDbName(connectionString));
			UserInterfaceAggregator userInterfaceAggregator = new UserInterfaceAggregator
			{
				ExeVersion = version
			};
			ReturnedSaveFuncInfoWithValue<ClsCache> val2 = await new Cache().GetCache((IEnumerable<EnCacheConfig>)array, contextGuid, connectionString, Path.Combine(Application.StartupPath, "sqlite.db"), text, 5, (ENSource)5, (IUserInterfaceAggregator)(object)userInterfaceAggregator);
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 118);
			_cacheInstance = val2.value;
			_ = _cacheInstance;
			if (_cacheInstance == null)
			{
				throw new Exception("Cache is null after call GetCache() in timeAttendance .........");
			}
			if (connectSignalR)
			{
				ConnectSignalRAsync(contextGuid, systemInfo, version, connectionType, token);
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 135);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "systemInfo:" + systemInfo + "//version:" + version, "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 136);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>> ConnectSignalRAsync(string contextGuid, string systemInfo, string version, EnConnectionType connectionType, CancellationToken token)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		int line = 0;
		ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> ret = new ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>();
		try
		{
			if (_cacheInstance == null)
			{
				return ret.AddError((EnSystem)2, "cache is null!!", "ConnectSignalRAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 146);
			}
			UserAttSetting userAttSetting = UserAttendance.SelectedUser?.UserSetting;
			if (userAttSetting == null || !userAttSetting.EnableCloudSignalR)
			{
				return ret;
			}
			string usernameSr = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token)).WebServiceKey.NPACCUserName;
			string passwordSr = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token)).WebServiceKey.NPACCPassword;
			string url = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token)).WebServiceKey.URL;
			Guid dbGuid = await GetDataBaseGuidAsync(contextGuid);
			if (string.IsNullOrEmpty(usernameSr) || string.IsNullOrEmpty(passwordSr) || string.IsNullOrEmpty(url))
			{
				TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)5, "چون در صفحه تنظیمات نرم افزار، نام کاربری یا رمز عبور اتصال یا لینک سرور خالی است امکان ارتباط با سرور وجود ندارد", "", 3);
				return ret.AddError((EnSystem)2, "در صفحه تنظیمات ارتباط سیگنال آر نام کاربری و رمز تنظیم نشده است", "ConnectSignalRAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 158);
			}
			line = 1;
			line = 2;
			_initialized = true;
			line = 3;
			long lkSerial = Utilities.ParseToLong(LkOperations.GetLkOperations(contextGuid).LkSerial(), 0L);
			line = 4;
			List<ClientConfig> list = new List<ClientConfig>
			{
				new ClientConfig
				{
					UserName = usernameSr,
					Password = passwordSr,
					Source = (ENSource)5,
					DbGuid = dbGuid,
					Version = version,
					CompressedMode = (ENCompressedMode)1,
					LkSerial = lkSerial,
					CpuSerial = systemInfo,
					Url = url
				}
			};
			line = 5;
			_cacheInstance.Context.LoginExtention.OnLoginResponse -= Instance_OnLoginResponse;
			_cacheInstance.Context.LoginExtention.OnLoginResponse += Instance_OnLoginResponse;
			_cacheInstance.BufferConnected -= Buffer_Connected;
			_cacheInstance.BufferConnected += Buffer_Connected;
			_cacheInstance.BufferDisconnected -= Buffer_Disconnected;
			_cacheInstance.BufferDisconnected += Buffer_Disconnected;
			_cacheInstance.BufferLoggedIn -= Buffer_LoggedIn;
			_cacheInstance.BufferLoggedIn += Buffer_LoggedIn;
			ClsCache.GetCache(contextGuid).Context.WebAttendanceLogOnPerson.OnReadResponse -= WebAttendanceLogOnPerson_OnReadResponse;
			ClsCache.GetCache(contextGuid).Context.WebAttendanceLogOnPerson.OnReadResponse += WebAttendanceLogOnPerson_OnReadResponse;
			ClsCache.GetCache(contextGuid).Context.WebAttendanceLogOnPerson.OnSync -= WebAttendanceLogOnPerson_OnSync;
			ClsCache.GetCache(contextGuid).Context.WebAttendanceLogOnPerson.OnSync += WebAttendanceLogOnPerson_OnSync;
			ConnecToken?.Cancel();
			ConnecToken = new CancellationTokenSource();
			CancellationToken token2 = ConnecToken.Token;
			ClsCache cacheInstance = _cacheInstance;
			ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> val = await ((cacheInstance != null) ? cacheInstance.ConnectAsync(connectionType, (IEnumerable<ClientConfig>)list, token2, true) : null);
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "ConnectSignalRAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 207);
			EnLoginResult? obj;
			if (val == null)
			{
				obj = null;
			}
			else
			{
				LoginResponseEventArgs value = val.value;
				obj = ((value != null) ? new EnLoginResult?(value.Status) : null);
			}
			EnLoginResult? val2 = obj;
			if ((int)val2.GetValueOrDefault((EnLoginResult)16) == 1)
			{
				TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)1, "ارتباط با سرور برقرار شد", "", 3);
			}
			else
			{
				TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)4, "عدم امکان برقراری ارتباط با سرور", "", 3);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, string.Format("Connecting to signalR has error. Line:{0}//systemInfo:{1}//version:{2}", line, systemInfo ?? "", version ?? ""), "ConnectSignalRAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 215);
			ret.AddReturnedValue(ex, "", "ConnectSignalRAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 216);
		}
		return ret;
	}

	private static Task Buffer_LoggedIn(object arg1, LoginResponseEventArgs arg2)
	{
		try
		{
			TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)1, "بافر لاگین شد", "", 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Buffer_LoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 228);
		}
		return Task.CompletedTask;
	}

	private static async Task WebAttendanceLogOnPerson_OnSync(object sender, ConnectionEventArgs<WebAttendanceLogOnPerson> e)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine($"method WebAttendanceLogOnPerson_OnSync in {DateTime.Now}");
		try
		{
			TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)9, "شروع عملیات سینک", "", 3);
			ReturnedSaveFuncInfoWithValue<(int, List<string>)> val = await SaveTaradod(stringBuilder, ((BaseEventArgs)e).Connection.Context.Guid, e.ConnectionData, ((BaseEventArgs)e).Connection.cancellationToken);
			if (((BaseEventArgs)e).Connection.cancellationToken.IsCancellationRequested)
			{
				return;
			}
			((ReturnedSaveFuncInfo)val).throwExceptionIfError();
			if (!((ReturnedSaveFuncInfo)val).HasError)
			{
				if (val.value.Item1 > 0)
				{
					TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)7, string.Format("اتمام عملیات سینک\r\n [تعداد ترددهای دریافت شده]: {0} از\r\n {1}", val.value.Item1, string.Join(",", val.value.Item2.Distinct())), "[اتمام عملیات سینک]", 10);
				}
				if (!((ReturnedSaveFuncInfo)val).HasError && InstanceOnSyncManagement.PacketManager(((BaseEventArgs)e).Part, ((BaseEventArgs)e).TotalParts, ((BaseEventArgs)e).PacketReferenceId, true))
				{
					ReturnedSaveFuncInfo val2 = new ReturnedSaveFuncInfo();
					Packet val3 = Sync.GenerateSyncDone((EntityPart)69, e.PartitionId, val2);
					await CacheInstance.SendData(val3, (Priority)2);
				}
			}
			else
			{
				TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)4, ((ReturnedSaveFuncInfo)val).ErrorMessage, "[خطا در عملیات سینک]", 5);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "WebAttendanceLogOnPerson_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 256);
		}
	}

	private static void RemoveTaradodsUnrelatedToUs(string contextGuid, DeviceInfo deviceInfo, string deviceSerial, List<WebAttendanceLogOnPerson> logs)
	{
		try
		{
			if (logs.Count > 0 && deviceInfo == null)
			{
				// [SANITIZED] ارسال تله‌متری (سریال دستگاه/قفل، IP، نام کاربر ویندوز) به ربات تلگرام حذف شد
				TimeAttendanceDAL.Utility.Utility.MessageBoxShow("دستگاهی با شماره سریال " + deviceSerial + " روی سیستم شما تعریف نشده است. لذا اگر دستگاه مذکور متعلق به شماست آنرا در قسمت دستگاه ها تعریف نمائید در غیر اینصورت جهت رفع این مورد، موضوع را به پشتیبان شرکت نوین پرداز اطلاع دهید", contextGuid);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	private static async Task<ReturnedSaveFuncInfoWithValue<(int CountTaradod, List<string> deviceNames)>> SaveTaradod(StringBuilder sb, string contextGuid, List<WebAttendanceLogOnPerson> items, CancellationToken token)
	{
		sb.AppendLine($"in {DateTime.Now} method SaveTaradod");
		ReturnedSaveFuncInfoWithValue<(int CountTaradod, List<string> deviceNames)> ret = new ReturnedSaveFuncInfoWithValue<(int, List<string>)>
		{
			value = (0, new List<string>())
		};
		try
		{
			int countNewRecords = 0;
			List<DeviceLogOnPerson> res = LogOnPersonel.MapDataToRecordAllDevice(contextGuid, items);
			var list = (from p in items
				group p by p.DeviceGuid into x
				select new
				{
					Device = x.Key,
					Records = x.ToList()
				}).ToList();
			foreach (var group in list)
			{
				string deviceSerial = group.Records.Select((WebAttendanceLogOnPerson p) => p.DeviceInfo.SerialIdString).Distinct().SingleOrDefault();
				DeviceInfo deviceInfo = await DeviceInfo.GetBySerialAsync(contextGuid, deviceSerial ?? "", token);
				RemoveTaradodsUnrelatedToUs(contextGuid, deviceInfo, deviceSerial, group.Records);
				if (deviceInfo != null)
				{
					List<DeviceLogOnPerson> plst = res.Where((DeviceLogOnPerson P) => P.DeviceSerial == deviceSerial).ToList();
					countNewRecords += (await LogOnPersonel.SaveTaradodsNewAsync(contextGuid, "entityCacheConfig.WebAttendaceLogOnPerson_onSync", (EnSystem)1, sb, plst, token, deviceInfo)).CountSavedLogs;
				}
			}
			ret.value = (countNewRecords, items.Select((WebAttendanceLogOnPerson x) => x.DeviceInfo.Name).Distinct().ToList());
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveTaradod", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 320);
			ret.AddReturnedValue(ex, "", "SaveTaradod", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 321);
		}
		return ret;
	}

	private static async Task WebAttendanceLogOnPerson_OnReadResponse(object sender, ConnectionEventArgs<WebAttendanceLogOnPerson> e)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("method ");
		try
		{
			TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)9, "شروع دریافت تردد آنی", "", 2);
			ReturnedSaveFuncInfoWithValue<(int, List<string>)> val = await SaveTaradod(stringBuilder, ((BaseEventArgs)e).Connection.Context.Guid, e.ConnectionData, ((BaseEventArgs)e).Connection.cancellationToken);
			if (val.value.Item1 > 0)
			{
				TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)7, $"تعداد تردد های جدید: {val.value.Item1}", "[دریافت ترددهای آنی]", 5);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "WebAttendanceLogOnPerson_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 336);
		}
	}

	private static Task Buffer_Disconnected(object sender, BaseEventArgs e)
	{
		try
		{
			TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)5, "ارتباط بافر قطع شد", "", 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Buffer_Disconnected", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 346);
		}
		return Task.CompletedTask;
	}

	private static Task Buffer_Connected(object arg1, BaseEventArgs arg2)
	{
		try
		{
			TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)10, "در حال اتصال به بافر ...", "", 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Buffer_Connected", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\EntityCachConfig.cs", 358);
		}
		return Task.CompletedTask;
	}

	private static async Task Instance_OnLoginResponse(object sender, LoginResponseEventArgs e)
	{
		_ = 1;
		try
		{
			Logger loggerUi = AttLogger.LoggerUi;
			WebUserInformation userInfo = e.UserInfo;
			loggerUi.Information("Begin Of Login Cache and arg2.UserInfo?.UserName=" + ((userInfo != null) ? userInfo.UserName : null));
			if ((int)e.Status == 1)
			{
				WebUserInformation userInfo2 = e.UserInfo;
				TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)1, "UserName = " + (((userInfo2 != null) ? userInfo2.UserName : null) ?? "---"), $"وضعیت ارتباط با سرور: {e.Status}", 2);
			}
			else
			{
				WebUserInformation userInfo3 = e.UserInfo;
				TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)4, "UserName = " + (((userInfo3 != null) ? userInfo3.UserName : null) ?? "---"), $"وضعیت ارتباط با سرور: {e.Status}", 2);
			}
			AttLogger.LoggerUi.Information("After Of Login Cache");
			string contextGuid = e.connection.ContextGuid;
			WebUserInformation userInfo4 = e.UserInfo;
			await LogUsers.RegisterNewLogAsync(contextGuid, string.Format("ارتباط سیگنال آر - UserName= {0}, وضعیت ارتباط با سرور= {1}", ((userInfo4 != null) ? userInfo4.UserName : null) ?? "---", e.Status), "Taradod.TaradodReciveDataFromInternet");
			await Task.Delay(1);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
