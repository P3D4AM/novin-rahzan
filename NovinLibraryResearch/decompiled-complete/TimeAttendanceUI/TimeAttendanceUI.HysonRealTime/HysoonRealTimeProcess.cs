using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using AxRealSvrOcxTcpLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceUI.HysonRealTime;

public static class HysoonRealTimeProcess
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static _DRealSvrOcxTcpEvents_OnReceiveGLogDataEventHandler _003C0_003E__AxRealSvrOcxTcp1_OnReceiveGLogData;

		public static _DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEventHandler _003C1_003E__AxRealSvrOcxTcp1_OnReceiveGLogDataExtend;

		public static _DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEventHandler _003C2_003E__AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage;

		public static _DRealSvrOcxTcpEvents_OnReceiveGLogTextEventHandler _003C3_003E__AxRealSvrOcxTcp1_OnReceiveGLogText;

		public static _DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEventHandler _003C4_003E__AxRealSvrOcxTcp1_OnReceiveGLogTextOnDoorOpen;
	}

	private static CancellationTokenSource _tokenSourceCalcTaradods;

	private static CancellationTokenSource _tokenOnTimeLog;

	private static string ContextGuid;

	public static void AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage(object sender, _DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEvent e)
	{
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			JObject val = JObject.Parse(e.astrLogText);
			string text = ((object)val["io_time"])?.ToString();
			if (text != null)
			{
				string text2 = ((object)val["serial_number"])?.ToString();
				if (string.IsNullOrEmpty(text2))
				{
					text2 = ((object)val["SerialNo"])?.ToString();
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 = "111111111111";
				}
				DateTime logTime = DateTime.Parse(text.Substring(0, 4) + "-" + text.Substring(4, 2) + "-" + text.Substring(6, 2) + " " + text.Substring(8, 2) + ":" + text.Substring(10, 2) + ":" + text.Substring(12, 2));
				AttendanceRecord attendanceRecord = new AttendanceRecord(ContextGuid)
				{
					transId = long.Parse(((object)val["log_id"])?.ToString() ?? string.Empty),
					personNo = int.Parse(((object)val["user_id"])?.ToString() ?? string.Empty),
					deviceNo = int.Parse(((object)val["fk_device_id"])?.ToString() ?? string.Empty),
					ioMode = ((object)val["io_mode"])?.ToString(),
					logTime = logTime,
					serialNumber = text2,
					deviceType = "NP587_N",
					deviceIp = e.astrClientIP,
					devicePort = e.anClientPort,
					Sender = sender
				};
				JObject val2 = new JObject();
				val2.Add("log_id", JToken.op_Implicit(attendanceRecord.transId.ToString()));
				val2.Add("result", JToken.op_Implicit("OK"));
				JObject val3 = val2;
				int num = ((AxRealSvrOcxTcp)sender).SendResponse(e.astrClientIP, e.anClientPort, ((JToken)val3).ToString((Formatting)1, Array.Empty<JsonConverter>()));
				if (num != 0)
				{
					throw new InvalidOperationException($"device response({num}) not valid.");
				}
				AttendanceRecord._bufferBlock.Post(attendanceRecord);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public static void AxRealSvrOcxTcp1_OnReceiveGLogDataExtend(object sender, _DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEvent e)
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AxRealSvrOcxTcp1_OnReceiveGLogDataExtend", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\HysonRealTime\\HysoonRealTimeProcess.cs", 74);
		}
	}

	public static void AxRealSvrOcxTcp1_OnReceiveGLogData(object sender, _DRealSvrOcxTcpEvents_OnReceiveGLogDataEvent e)
	{
		try
		{
			AttendanceRecord item = new AttendanceRecord(ContextGuid)
			{
				transId = 0L,
				personNo = e.anSEnrollNumber,
				deviceNo = e.anDeviceID,
				ioMode = e.anInOutMode.ToString(),
				logTime = e.anLogDate,
				serialNumber = e.astrSerialNo,
				deviceType = "NP587_O",
				deviceIp = e.astrDeviceIP,
				devicePort = e.anDevicePort,
				Sender = sender,
				VerifyMode = e.anVerifyMode
			};
			AttendanceRecord._bufferBlock.Post(item);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public static void AxRealSvrOcxTcp1_OnReceiveGLogText(object sender, _DRealSvrOcxTcpEvents_OnReceiveGLogTextEvent e)
	{
		Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, "تردد دریافت شده از سمت دستگاه ، پردازش نشد" + e.astrLogText, "", 3);
	}

	public static void AxRealSvrOcxTcp1_OnReceiveGLogTextOnDoorOpen(object sender, _DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEvent e)
	{
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			JObject val = JObject.Parse(e.astrLogText);
			int deviceNo = int.Parse(((object)val["fk_device_id"])?.ToString() ?? string.Empty);
			string ioMode = ((object)val["io_mode"])?.ToString();
			string text = ((object)val["io_time"])?.ToString();
			((object)val["is_support_string_id"])?.ToString();
			if (text == null)
			{
				return;
			}
			DateTime logTime = DateTime.Parse(text.Substring(0, 4) + "-" + text.Substring(4, 2) + "-" + text.Substring(6, 2) + " " + text.Substring(8, 2) + ":" + text.Substring(10, 2) + ":" + text.Substring(12, 2));
			long transId = long.Parse(((object)val["log_id"])?.ToString() ?? string.Empty);
			string deviceType = "NP395";
			string serialNumber = deviceNo.ToString();
			try
			{
				string text2 = ((object)val["serial_number"])?.ToString() ?? string.Empty;
				if (!string.IsNullOrEmpty(text2))
				{
					serialNumber = text2;
					deviceType = "756PLUS";
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
			}
			int personNo = int.Parse(((object)val["user_id"])?.ToString() ?? string.Empty);
			int verifyMode = 0;
			switch (((object)val["verify_mode"]).ToString())
			{
			case "fp":
				verifyMode = 1;
				break;
			case "password":
				verifyMode = 2;
				break;
			case "face":
				verifyMode = 3;
				break;
			}
			_ = e.astrClientIP;
			_ = e.anClientPort;
			AttendanceRecord attendanceRecord = new AttendanceRecord(ContextGuid)
			{
				transId = transId,
				personNo = personNo,
				deviceNo = deviceNo,
				ioMode = ioMode,
				logTime = logTime,
				serialNumber = serialNumber,
				deviceType = deviceType,
				deviceIp = e.astrClientIP,
				devicePort = e.anClientPort,
				Sender = sender,
				VerifyMode = verifyMode
			};
			JObject val2 = new JObject();
			val2.Add("log_id", JToken.op_Implicit(attendanceRecord.transId.ToString()));
			val2.Add("result", JToken.op_Implicit("OK"));
			val2.Add("mode", JToken.op_Implicit("nothing"));
			JObject val3 = val2;
			int num = ((AxRealSvrOcxTcp)sender).SendRtLogResponseV3(e.astrClientIP, e.anClientPort, ((object)val3).ToString());
			if (num != 0)
			{
				throw new InvalidOperationException($"device response({num}) not valid.");
			}
			AttendanceRecord._bufferBlock.Post(attendanceRecord);
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	public static void Start(string contextGuid, AxRealSvrOcxTcp ocx)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		try
		{
			ContextGuid = contextGuid;
			object obj = _003C_003EO._003C0_003E__AxRealSvrOcxTcp1_OnReceiveGLogData;
			if (obj == null)
			{
				_DRealSvrOcxTcpEvents_OnReceiveGLogDataEventHandler val = AxRealSvrOcxTcp1_OnReceiveGLogData;
				_003C_003EO._003C0_003E__AxRealSvrOcxTcp1_OnReceiveGLogData = val;
				obj = (object)val;
			}
			ocx.OnReceiveGLogData += (_DRealSvrOcxTcpEvents_OnReceiveGLogDataEventHandler)obj;
			object obj2 = _003C_003EO._003C1_003E__AxRealSvrOcxTcp1_OnReceiveGLogDataExtend;
			if (obj2 == null)
			{
				_DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEventHandler val2 = AxRealSvrOcxTcp1_OnReceiveGLogDataExtend;
				_003C_003EO._003C1_003E__AxRealSvrOcxTcp1_OnReceiveGLogDataExtend = val2;
				obj2 = (object)val2;
			}
			ocx.OnReceiveGLogDataExtend += (_DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEventHandler)obj2;
			object obj3 = _003C_003EO._003C2_003E__AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage;
			if (obj3 == null)
			{
				_DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEventHandler val3 = AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage;
				_003C_003EO._003C2_003E__AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage = val3;
				obj3 = (object)val3;
			}
			ocx.OnReceiveGLogTextAndImage += (_DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEventHandler)obj3;
			object obj4 = _003C_003EO._003C3_003E__AxRealSvrOcxTcp1_OnReceiveGLogText;
			if (obj4 == null)
			{
				_DRealSvrOcxTcpEvents_OnReceiveGLogTextEventHandler val4 = AxRealSvrOcxTcp1_OnReceiveGLogText;
				_003C_003EO._003C3_003E__AxRealSvrOcxTcp1_OnReceiveGLogText = val4;
				obj4 = (object)val4;
			}
			ocx.OnReceiveGLogText += (_DRealSvrOcxTcpEvents_OnReceiveGLogTextEventHandler)obj4;
			object obj5 = _003C_003EO._003C4_003E__AxRealSvrOcxTcp1_OnReceiveGLogTextOnDoorOpen;
			if (obj5 == null)
			{
				_DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEventHandler val5 = AxRealSvrOcxTcp1_OnReceiveGLogTextOnDoorOpen;
				_003C_003EO._003C4_003E__AxRealSvrOcxTcp1_OnReceiveGLogTextOnDoorOpen = val5;
				obj5 = (object)val5;
			}
			ocx.OnReceiveGLogTextOnDoorOpen += (_DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEventHandler)obj5;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public static void Dispose()
	{
		try
		{
			_tokenSourceCalcTaradods?.Cancel();
			_tokenSourceCalcTaradods?.Dispose();
			_tokenSourceCalcTaradods = null;
			_tokenOnTimeLog?.Cancel();
			_tokenOnTimeLog?.Dispose();
			_tokenOnTimeLog = null;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}
}
