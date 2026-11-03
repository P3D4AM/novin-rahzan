using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Attendance;

public class WebAttendanceLogOnAdmin : Serializable<WebAttendanceLogOnAdmin>, IReceiver
{
	public enum AttendanceLogOnAdminActions
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

	public int PartitionId { get; set; }

	public short? DepId { get; set; }

	public int TransactionId { get; set; }

	public DateTime LogTime { get; set; }

	public long AdminId { get; set; }

	public long UserId { get; set; }

	public string Action { get; set; }

	public int Result { get; set; }

	public string IpAddress { get; set; }

	public Guid DeviceGuid { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public ReturnedSaveFuncInfo CheckValidtion(long transactionId)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (transactionId < 0)
			{
				returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, $"شماره تراکنش {transactionId} معتبر نمیباشد.", "transactionId", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAdmin.cs", 40);
			}
			if (returnedSaveFuncInfo.HasError)
			{
				return returnedSaveFuncInfo;
			}
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAdmin.cs", 45);
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAdmin.cs", 46);
		}
		return returnedSaveFuncInfo;
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAdmin.cs", 58);
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebAttendanceLogOnAdmin> attendanceRecord, int partition = 0, bool isSync = false)
	{
		if (attendanceRecord == null || attendanceRecord.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAdmin.cs"))
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
					result = attendanceRecord.GeneratePackets(PacketType.AttendanceLogOnPersonel, 6, null, isSync, null, partition, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAdmin.cs", 92);
				return null;
			}
		}
	}

	public Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		throw new NotImplementedException();
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			AttendanceLogOnAdminActions attendanceLogOnAdminActions = (AttendanceLogOnAdminActions)action;
			return attendanceLogOnAdminActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAdmin.cs", 111);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((AttendanceLogOnAdminActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceLogOnAdmin.cs", 124);
			return $"Error({action})";
		}
	}
}
