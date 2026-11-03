using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;

namespace PacketParser.Entities.Attendance;

public class WebAttendanceLogOnAlarm : Serializable<WebAttendanceLogOnAlarm>, IAttendanceLogOnAlarm, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver
{
	public enum AttendanceLogOnAlarmActions
	{
		[PersianNameAttribute.PersianName("نامشخص")]
		Unknown = 0,
		[PersianNameAttribute.PersianName("درخواست ذخیره")]
		SaveRequest = 1,
		[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
		SaveResponse = 2,
		[PersianNameAttribute.PersianName("درخواست خواندن")]
		ReadRequest = 5,
		[PersianNameAttribute.PersianName("سینک")]
		ReadResponse = 6,
		[PersianNameAttribute.PersianName("ریست")]
		Reset = 7
	}

	private bool _isDisposed;

	public Guid DeviceGuid { get; set; }

	public int PartitionId { get; set; }

	public int TransactionId { get; set; }

	public DateTime LogTime { get; set; }

	public long UserId { get; set; }

	public int DoorId { get; set; }

	public string AlarmType { get; set; }

	public string IpAddress { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public ReturnedSaveFuncInfo CheckValidtion(long transactionId)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (transactionId < 0)
			{
				returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, $"شماره تراکنش دستگاه {transactionId} معتبر نمیباشد.", "transactionId", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAlarm.cs", 34);
			}
			if (returnedSaveFuncInfo.HasError)
			{
				return returnedSaveFuncInfo;
			}
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAlarm.cs", 39);
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAlarm.cs", 40);
		}
		return returnedSaveFuncInfo;
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebAttendanceLogOnAlarm> attendanceRecord, int partition = 0, bool isSync = false)
	{
		if (attendanceRecord == null || attendanceRecord.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAlarm.cs"))
		{
			try
			{
				if (attendanceRecord == null || attendanceRecord.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				object result;
				if (attendanceRecord == null)
				{
					result = null;
				}
				else
				{
					result = attendanceRecord.GeneratePackets(PacketType.AttendanceLogOnAlarm, 6, null, isSync, null, partition, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAlarm.cs", 76);
				return null;
			}
		}
	}

	public override void Dispose()
	{
		Dispose(isDisposing: true);
	}

	public override void Dispose(bool isDisposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				base.Dispose(isDisposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAlarm.cs", 98);
		}
	}

	public Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		throw new NotImplementedException();
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((AttendanceLogOnAlarmActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAlarm.cs", 114);
			return $"Error({action})";
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			AttendanceLogOnAlarmActions attendanceLogOnAlarmActions = (AttendanceLogOnAlarmActions)action;
			return attendanceLogOnAlarmActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAlarm.cs", 127);
			return $"Error({action})";
		}
	}
}
