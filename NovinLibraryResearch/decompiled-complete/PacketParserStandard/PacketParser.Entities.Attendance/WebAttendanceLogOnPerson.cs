using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Assistance.Net;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;
using PacketParser.EventArgs;

namespace PacketParser.Entities.Attendance;

public class WebAttendanceLogOnPerson : Serializable<WebAttendanceLogOnPerson>, IAttendanceLogOnPerson, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver
{
	public enum AttendanceLogOnPersonActions
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

	public WebAttendanceDeviceInfo DeviceInfo { get; set; }

	public Guid DeviceGuid { get; set; }

	public int PartitionId { get; set; }

	public long TransactionId { get; set; }

	public DateTime LogTime { get; set; }

	public long UserId { get; set; }

	public int DoorId { get; set; }

	public string AttendanceStatus { get; set; }

	public string VerifyMode { get; set; }

	public int JobCode { get; set; }

	public string AntiPassv { get; set; }

	public byte[] Photo { get; set; }

	public string IpAddress { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public int TerminalID { get; set; }

	public int ActionInt { get; set; }

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

	public event Func<object, ConnectionEventArgs<WebAttendanceLogOnPerson>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebAttendanceLogOnPerson>, Task> OnSync;

	public event Func<object, TimeLogEventLog> OnTimeLog;

	public static List<Packet> GenerateReadResponse(IEnumerable<WebAttendanceLogOnPerson> attendanceRecords, WebAttendanceDeviceInfo deviceInfo, int partition = 0, bool isSync = false)
	{
		if (attendanceRecords == null || attendanceRecords.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs"))
		{
			try
			{
				if (attendanceRecords == null || attendanceRecords.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				foreach (WebAttendanceLogOnPerson attendanceRecord in attendanceRecords)
				{
					if (attendanceRecord.DeviceInfo == null)
					{
						attendanceRecord.DeviceInfo = deviceInfo;
					}
				}
				object result;
				if (attendanceRecords == null)
				{
					result = null;
				}
				else
				{
					result = attendanceRecords.GeneratePackets(PacketType.AttendanceLogOnPersonel, 6, null, isSync, null, partition, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs", 42);
				return null;
			}
		}
	}

	public static int SetActionInt(string ActionString)
	{
		try
		{
			int num = ActionString.ParseToInt();
			if (num > 0)
			{
				return num;
			}
			switch (ActionString)
			{
			case "Duty On":
				return 1;
			case "Duty Off":
				return 257;
			case "Overtime Off":
				return 769;
			case "Overtime On":
				return 0;
			case "In":
				return 1025;
			case "Out":
				return 1281;
			default:
				WebTelegramMessage.GetAttendanceRealTimeMonitoring_bot().Sent2web("❌❌ActionString IS=> " + ActionString + " ❌❌", null, "SetActionInt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs", 116);
				return 0;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetActionInt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs", 122);
			return 0;
		}
	}

	public ReturnedSaveFuncInfo CheckValidtion(long TransactionId)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (TransactionId < 0)
			{
				returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, $"شماره تراکنش دستگاه {TransactionId} معتبر نمیباشد.", "TransactionId", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs", 134);
			}
			returnedSaveFuncInfo.throwExceptionIfError();
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs", 139);
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs", 140);
		}
		return returnedSaveFuncInfo;
	}

	public new void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs", 160);
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs"))
		{
			_ = 1;
			try
			{
				if (pack.PacketActions != 6 || this.OnReadResponse == null)
				{
					return;
				}
				ConnectionEventArgs<WebAttendanceLogOnPerson> args = new ConnectionEventArgs<WebAttendanceLogOnPerson>(pack, ConnectionUniqueId, Connection);
				try
				{
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs");
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs", 201);
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs", 207);
			}
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((AttendanceLogOnPersonActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"action:{action}", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs", 218);
			return $"Error({action})";
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			AttendanceLogOnPersonActions attendanceLogOnPersonActions = (AttendanceLogOnPersonActions)action;
			return attendanceLogOnPersonActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"action:{action}", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnPerson.cs", 231);
			return $"Error({action})";
		}
	}
}
