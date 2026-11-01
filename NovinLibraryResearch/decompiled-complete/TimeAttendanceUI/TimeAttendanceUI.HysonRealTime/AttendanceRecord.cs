using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using LK;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceUI.HysonRealTime;

public class AttendanceRecord
{
	[JsonIgnore]
	internal static BufferBlock<AttendanceRecord> _bufferBlock;

	[JsonIgnore]
	private static CancellationTokenSource _saveToken;

	public string deviceType { get; set; }

	public int deviceNo { get; set; }

	public string serialNumber { get; set; }

	public long transId { get; set; }

	public DateTime logTime { get; set; }

	public int personNo { get; set; }

	public string ioMode { get; set; }

	public string deviceIp { get; set; }

	public int devicePort { get; set; }

	[JsonIgnore]
	public object Sender { get; internal set; }

	[JsonIgnore]
	public TaskCompletionSource<ReturnedSaveFuncInfo> SaveResult { get; private set; } = new TaskCompletionSource<ReturnedSaveFuncInfo>();


	public int VerifyMode { get; internal set; }

	public string ContextGuid { get; private set; }

	public AttendanceRecord()
	{
	}

	public AttendanceRecord(string contextGuid)
	{
		AttendanceRecord attendanceRecord = this;
		if (_bufferBlock == null)
		{
			_bufferBlock = new BufferBlock<AttendanceRecord>();
			_saveToken?.Cancel();
			_saveToken = new CancellationTokenSource();
			CancellationToken token = _saveToken.Token;
			Task.Run(() => attendanceRecord.SaveAsync(token));
		}
		ContextGuid = contextGuid;
	}

	private async Task SaveAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			while (!token.IsCancellationRequested)
			{
				try
				{
					await SaveLogAsync(await _bufferBlock.ReceiveAsync(token), token);
				}
				catch (Exception ex)
				{
					Log.Error(ex, "");
				}
			}
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private static async Task<ReturnedSaveFuncInfo> SaveLogAsync(AttendanceRecord nextRecord, CancellationToken token, int tryCount = 10)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		StringBuilder sb = new StringBuilder();
		new JaegerMonitor("SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\HysonRealTime\\AttendanceRecord.cs", 60000);
		try
		{
			try
			{
				File.AppendAllText(Path.Combine(".", $"logRealTimeDevice{nextRecord.deviceNo}.txt"), "\r\n___________________________________\r\n" + $"{DateTime.Now}\r\n" + Json.ToStringJson((object)nextRecord, true));
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
			}
			ReturnedSaveFuncInfoWithValue<DeviceInfo> val = await DeviceInfo.CheckAndGetDeviceInfoAsync(nextRecord.ContextGuid, nextRecord.serialNumber, (short)nextRecord.deviceNo, token);
			if (((ReturnedSaveFuncInfo)val).HasError)
			{
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\HysonRealTime\\AttendanceRecord.cs", 113);
				return ret;
			}
			DeviceInfo deviceInfo = val.value;
			if (deviceInfo == null)
			{
				Log.Error($"دستگاهی با شماره سریال {nextRecord.serialNumber} و یا شناسه دستگاه {nextRecord.deviceNo} در سیستم یافت نشد. لذا تردد ثبت نخواهد شد");
				return ret;
			}
			if (deviceInfo.DeviceNo != nextRecord.deviceNo)
			{
				string path = Path.Combine(".", $"ErrorGetDevice{nextRecord.deviceNo}.txt");
				string text = $"\r\n------------------------------\r\n deviceNo from device({deviceInfo.DeviceNo})<>{nextRecord.deviceNo}\r\n-------------------\r\n{Json.ToStringJson((object)nextRecord, true)}\r\n -------------------------\r\n{Json.ToStringJson((object)deviceInfo, true)}";
				File.AppendAllText(path, text);
				throw new InvalidOperationException(text);
			}
			DateTime dateTimeLogM = new DateTime(nextRecord.logTime.Year, nextRecord.logTime.Month, nextRecord.logTime.Day, nextRecord.logTime.Hour, nextRecord.logTime.Minute, 0);
			int num = 1;
			if (nextRecord.ioMode.ToLower() == "in")
			{
				num = 1;
			}
			List<DeviceLogOnPerson> plst = new List<DeviceLogOnPerson>
			{
				new DeviceLogOnPerson(nextRecord.ContextGuid)
				{
					PersonNo = nextRecord.personNo,
					DeviceNo = nextRecord.deviceNo,
					IoMode = num,
					DateTimeLogM = dateTimeLogM,
					DeviceSerial = nextRecord.serialNumber
				}
			};
			if ((await LogOnPersonel.SaveTaradodsNewAsync(nextRecord.ContextGuid, "AttendanceRecord.SaveLogAsync", (EnSystem)0, sb, plst, token, deviceInfo)).CountSavedLogs > 0)
			{
				string text2 = string.Format("سریال قفل:{0} \r\nنسخه نرم افزار: {1}\r\n  کد شخص: {2}\r\n زمان: {3}\r\n نوع تردد: {4}\r\n  دستگاه: {5}", Text.FixForTelegram(LK.GetSerialNumber(MainFormOwner.owner, (Package)2).ToString(), "_"), VersionManagment.AssemblyVersion3Num(), nextRecord.personNo, Calendar.ToShamsiDateTime(nextRecord.logTime), nextRecord.ioMode, Text.FixForTelegram(deviceInfo.DeviceName, "_") ?? " ");
				Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)3, text2, "", 3);
				// [SANITIZED] بلوک ارسال ترددهای زنده (کد شخص، زمان، دستگاه) به ربات‌های
				// [SANITIZED] پایش سازنده حذف شد؛ اعلان محلی کاربر در خط قبل باقی است.
			}
			DeviceEventsEntity.SaveEventMessage(nextRecord.deviceType + " " + nextRecord.ioMode, nextRecord.deviceNo, nextRecord.serialNumber, nextRecord.transId.ToString(), nextRecord.logTime, nextRecord.personNo, "taradodLog");
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			ret.AddReturnedValue(ex2, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\HysonRealTime\\AttendanceRecord.cs", 180);
		}
		if (ret.HasError && tryCount > 0)
		{
			await Task.Delay(new Random().Next(10000));
			int num2 = tryCount - 1;
			tryCount = num2;
			ret = await SaveLogAsync(nextRecord, token, num2);
		}
		nextRecord.SaveResult?.TrySetResult(ret);
		return ret;
	}

	public static void Dispose()
	{
		try
		{
			_saveToken?.Cancel();
			_saveToken?.Dispose();
			_saveToken = null;
			_bufferBlock = null;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}
}
