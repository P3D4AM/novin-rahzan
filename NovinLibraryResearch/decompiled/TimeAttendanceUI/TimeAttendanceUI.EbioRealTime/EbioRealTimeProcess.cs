using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Assistance.Net;
using PacketParser.Assistance.Net.Ebio;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceUI.EbioRealTime;

public static class EbioRealTimeProcess
{
	private static CancellationTokenSource TokenSourceCalcTaradods = new CancellationTokenSource();

	private static CancellationTokenSource _tokenSourceCalcTaradods;

	private static CancellationTokenSource TokenPing = new CancellationTokenSource();

	private static CancellationTokenSource TokenGetAndCheckDevice = new CancellationTokenSource();

	public static string ContextGuid;

	public static Dictionary<string, DeviceInfo> deviceDictionary2 = new Dictionary<string, DeviceInfo>();

	private static ConcurrentDictionary<int, EbioServer> _dicEbioServers = new ConcurrentDictionary<int, EbioServer>();

	private static int GetIdTerafic(string status)
	{
		int result = 0;
		try
		{
			switch (status)
			{
			case "Duty On":
				result = 1;
				break;
			case "Duty Off":
				result = 257;
				break;
			case "Overtime Off":
				result = 769;
				break;
			case "Overtime On":
				result = 513;
				break;
			case "In":
				result = 1025;
				break;
			case "Out":
				result = 1281;
				break;
			default:
				// [SANITIZED] ارسال رشتهٔ وضعیت دستگاه به ربات تلگرام حذف شد
				result = 0;
				break;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfo> OnTimeLogAsync(object sender, TimeLogEventLog e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			string serialNumber = e.serialNumber;
			int deviceNo = e.termId;
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"line 76 method OnTimeLogAsync in {DateTime.Now}");
			TokenGetAndCheckDevice?.Cancel();
			TokenGetAndCheckDevice = new CancellationTokenSource();
			CancellationToken token = TokenGetAndCheckDevice.Token;
			ReturnedSaveFuncInfoWithValue<DeviceInfo> val = await DeviceInfo.CheckAndGetDeviceInfoAsync(ContextGuid, DeviceInfo.spamSerialList, deviceDictionary2, serialNumber, (short)deviceNo, token);
			if (((ReturnedSaveFuncInfo)val).HasError)
			{
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "OnTimeLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\EbioRealTime\\EbioRealTimeProcess.cs", 85);
				return ret;
			}
			DeviceInfo deviceInfo = val.value;
			int idTerafic = GetIdTerafic(e.attendStatus);
			DateTime logTime = e.TimeLog;
			int personNo = (int)e.userID;
			sb.AppendLine($"line 88 in {DateTime.Now}=> logtime{logTime},personNo:{personNo}");
			SaveLogonPersonelResult obj = await SaveTaradodAsync(sb, personNo, idTerafic, logTime, deviceInfo);
			if (obj == null || obj.CountSavedLogs <= 0)
			{
				await ((HysoonTcpClientConnection)sender).SendReplyAsync(true, e.transId);
			}
			else
			{
				await ((HysoonTcpClientConnection)sender).SendReplyAsync(true, e.transId);
				Person obj2 = await Person.GetbyDeviceIdPersonNoAsync(ContextGuid, deviceInfo.Id, personNo, token);
				string text = ((obj2 != null) ? ((PersonBase)obj2).FullName : null);
				string[] obj3 = new string[6]
				{
					" دریافت تردد از : ",
					Text.FixForTelegram(text, "_"),
					"\r\n زمان: ",
					Calendar.ToShamsiDateTime(logTime),
					"\r\n دستگاه: ",
					null
				};
				obj3[5] = ((deviceInfo != null) ? Text.FixForTelegram(deviceInfo.DeviceName, "_") : null) ?? " ";
				string msg = string.Concat(obj3);
				Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)3, msg, "", 3);
			}
			DeviceModelEnumType val2 = WebAttendanceDeviceInfo.DetectDeviceType(e.termType);
			DeviceEventsEntity.SaveEventMessage(((object)(DeviceModelEnumType)(ref val2)).ToString(), deviceNo, serialNumber, e.transId.ToString(), logTime, personNo, "taradodLog");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return ret;
	}

	private static async Task<SaveLogonPersonelResult> SaveTaradodAsync(StringBuilder sb, int personNo, int idTerafic, DateTime logTime, DeviceInfo device)
	{
		sb.AppendLine($"in {DateTime.Now} method saveTaradod line 130");
		SaveLogonPersonelResult saveLogonPersonelResult = null;
		try
		{
			DateTime dateTimeLogM = new DateTime(logTime.Year, logTime.Month, logTime.Day, logTime.Hour, logTime.Minute, 0);
			List<DeviceLogOnPerson> plst = new List<DeviceLogOnPerson>
			{
				new DeviceLogOnPerson(ContextGuid)
				{
					PersonNo = personNo,
					DeviceNo = device.DeviceNo,
					IoMode = (short)idTerafic,
					DateTimeLogM = dateTimeLogM
				}
			};
			TokenSourceCalcTaradods?.Cancel();
			TokenSourceCalcTaradods = new CancellationTokenSource();
			CancellationToken token = TokenSourceCalcTaradods.Token;
			saveLogonPersonelResult = await LogOnPersonel.SaveTaradodsNewAsync(ContextGuid, "EbioRealTimeProcess.OnTimeLogAsync", (EnSystem)0, sb, plst, token, device);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveTaradodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\EbioRealTime\\EbioRealTimeProcess.cs", 141);
		}
		return saveLogonPersonelResult;
	}

	public static Task<ReturnedSaveFuncInfo> OnAlarmLogAsync(object sender, AlarmEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		return Task.FromResult<ReturnedSaveFuncInfo>(new ReturnedSaveFuncInfo());
	}

	public static async Task<ReturnedSaveFuncInfo> OnPingAsync(object sender, PingEventLog e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			TokenPing?.Cancel();
			TokenPing = new CancellationTokenSource();
			CancellationToken token = TokenPing.Token;
			ReturnedSaveFuncInfoWithValue<DeviceInfo> val = await DeviceInfo.CheckAndGetDeviceInfoAsync(ContextGuid, DeviceInfo.spamSerialList, deviceDictionary2, e.serialNumber, (short)e.termId, token);
			if (((ReturnedSaveFuncInfo)val).HasError)
			{
				ret.AddError(((ReturnedSaveFuncInfo)val).ErrorMessage, "", "OnPingAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\EbioRealTime\\EbioRealTimeProcess.cs", 163, (StringBuilder)null);
				return ret;
			}
			((EbioTcpClientConnection)sender).SendReplyAsync("KeptAlive", (long)e.transId);
			DeviceModelEnumType val2 = WebAttendanceDeviceInfo.DetectDeviceType(e.termType);
			DeviceEventsEntity.SaveEventMessage(((object)(DeviceModelEnumType)(ref val2)).ToString(), e.termId, e.serialNumber, e.transId.ToString(), e.pingTime, 0L, "KeptAlive");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			ret.AddReturnedValue(ex, "", "OnPingAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\EbioRealTime\\EbioRealTimeProcess.cs", 173);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> OnAdminLogAsync(object sender, AdminLogEventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			await ((EbioTcpClientConnection)sender).SendReplyAsync("UploadedLog", (long)e.transId);
			DeviceModelEnumType val = WebAttendanceDeviceInfo.DetectDeviceType(e.termType);
			DeviceEventsEntity.SaveEventMessage(((object)(DeviceModelEnumType)(ref val)).ToString(), e.transId, e.serialNumber, e.transId.ToString(), e.Logtime, e.userID, "AdminLog");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			ret.AddReturnedValue(ex, "", "OnAdminLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\EbioRealTime\\EbioRealTimeProcess.cs", 191);
		}
		return ret;
	}

	public static void Dispose()
	{
		try
		{
			TokenPing?.Cancel();
			TokenPing?.Dispose();
			TokenPing = null;
			TokenGetAndCheckDevice?.Cancel();
			TokenGetAndCheckDevice?.Dispose();
			TokenGetAndCheckDevice = null;
			_tokenSourceCalcTaradods?.Cancel();
			_tokenSourceCalcTaradods?.Dispose();
			_tokenSourceCalcTaradods = null;
			TokenSourceCalcTaradods?.Cancel();
			TokenSourceCalcTaradods?.Dispose();
			TokenSourceCalcTaradods = null;
			_dicEbioServers = null;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	internal static void Start(string contextGuid, List<int> ebioPorts)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ContextGuid = contextGuid;
			if (ebioPorts == null || !ebioPorts.Any())
			{
				return;
			}
			foreach (int ebioPort in ebioPorts)
			{
				EbioServer instance = new EbioServerInstanceHolder().GetInstance(ebioPort, Log.Logger);
				instance.AdminLog += OnAdminLogAsync;
				instance.TimeLog += OnTimeLogAsync;
				instance.AlarmLog += OnAlarmLogAsync;
				instance.PingLog += OnPingAsync;
				_dicEbioServers?.TryAdd(ebioPort, instance);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	internal static void Stop(List<int> ebioPorts)
	{
		try
		{
			if (ebioPorts == null || !ebioPorts.Any())
			{
				return;
			}
			foreach (int ebioPort in ebioPorts)
			{
				EbioServer value = null;
				_dicEbioServers?.TryGetValue(ebioPort, out value);
				if (value != null)
				{
					value.AdminLog -= OnAdminLogAsync;
					value.TimeLog -= OnTimeLogAsync;
					value.AlarmLog -= OnAlarmLogAsync;
					value.PingLog -= OnPingAsync;
					((TcpServerBaseConnection<EbioTcpClientConnection>)(object)value).Dispose();
					_dicEbioServers.TryRemove(ebioPort, out value);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}
}
