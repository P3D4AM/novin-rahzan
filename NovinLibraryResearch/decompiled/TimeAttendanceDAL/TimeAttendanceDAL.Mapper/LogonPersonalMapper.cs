using System;
using System.Collections.Generic;
using System.Threading;
using PacketParser.Attendance.Entities;
using PacketParser.Attendance.SDK.LAN_VisibleLight;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendanceDAL.Mapper;

internal class LogonPersonalMapper : IItemMapper<WebAttendanceLogOnPerson, DeviceLogOnPerson>, IItemMapper<DeviceLogOnPerson, WebAttendanceLogOnPerson>, IItemMapper<LogOnPersonel, WebAttLogOnPersonel>
{
	public static LogonPersonalMapper Instance { get; } = new LogonPersonalMapper();


	public DeviceLogOnPerson Map(string contextGuid, WebAttendanceLogOnPerson source, DeviceLogOnPerson destination = null)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (source == null)
			{
				destination = null;
				return null;
			}
			if (destination == null)
			{
				destination = new DeviceLogOnPerson(contextGuid);
			}
			destination.DateTimeLogM = source.LogTime;
			destination.PersonNo = (int)source.UserId;
			destination.DeviceNo = source.TerminalID;
			destination.IdTraffic = (short)source.ActionInt;
			destination.Guid = source.Guid;
			DeviceLogOnPerson obj = destination;
			WebAttendanceDeviceInfo deviceInfo = source.DeviceInfo;
			obj.DeviceSerial = ((deviceInfo != null) ? deviceInfo.SerialIdString : null) ?? "";
			WebAttendanceDeviceInfo deviceInfo2 = source.DeviceInfo;
			if (deviceInfo2 != null)
			{
				_ = deviceInfo2.TerminalType;
				if (true)
				{
					DeviceLogOnPerson obj2 = destination;
					WebAttendanceDeviceInfo deviceInfo3 = source.DeviceInfo;
					obj2.TerminalType = ((deviceInfo3 != null) ? new DeviceModelEnumType?(deviceInfo3.TerminalType) : null).Value;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Mapper\\LogonPersonalMapper.cs", 105);
		}
		return destination;
	}

	public WebAttendanceLogOnPerson Map(string contextGuid, DeviceLogOnPerson source, WebAttendanceLogOnPerson destination = null)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		try
		{
			if (source == null)
			{
				destination = null;
				return null;
			}
			if (destination == null)
			{
				destination = new WebAttendanceLogOnPerson();
			}
			destination.LogTime = source.DateTimeLogM;
			destination.UserId = source.PersonNo;
			destination.TerminalID = source.DeviceNo;
			destination.ActionInt = (short)source.IdTraffic;
			destination.DoorId = 0;
			destination.TransactionId = 0L;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Mapper\\LogonPersonalMapper.cs", 128);
		}
		return destination;
	}

	public DeviceLogOnPerson Map(string contextGuid, WebAttendanceLogOnPerson source, DeviceLogOnPerson destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public List<DeviceLogOnPerson> MapList(string contextGuid, IEnumerable<WebAttendanceLogOnPerson> source, List<DeviceLogOnPerson> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public WebAttendanceLogOnPerson Map(string contextGuid, DeviceLogOnPerson source, WebAttendanceLogOnPerson destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public List<WebAttendanceLogOnPerson> MapList(string contextGuid, IEnumerable<DeviceLogOnPerson> source, List<WebAttendanceLogOnPerson> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public WebAttLogOnPersonel Map(string contextGuid, LogOnPersonel source, WebAttLogOnPersonel destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		try
		{
			if (source == null)
			{
				destination = null;
				return null;
			}
			if (destination == null)
			{
				destination = new WebAttLogOnPersonel();
			}
			destination.TypeTaradod = source.TypeTaradod;
			destination.PersonId = source.PersonId;
			destination.PersonNo = source.PersonNo;
			destination.DeviceId_ = source.DeviceId_;
			destination.DateTimeLogM = source.DateTimeLogM;
			destination.Duration = source.Duration;
			destination.SmsRefrenceIDs = source.SmsRefrenceIDs;
			destination.Id = source.Id;
			destination.TrafficId = source.TrafficId;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return destination;
	}

	public LogOnPersonel Map(string contextGuid, WebAttLogOnPerson source, LogOnPersonel destination = null)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (source == null)
			{
				destination = null;
				return null;
			}
			if (destination == null)
			{
				destination = new LogOnPersonel(contextGuid);
			}
			destination.TypeTaradod = source.TypeTaradod;
			destination.PersonId = source.PersonId;
			destination.PersonNo = source.PersonNo;
			destination.DeviceId_ = source.DeviceId_;
			destination.DateTimeLogM = source.DateTimeLogM;
			destination.Duration = source.Duration;
			destination.Id = source.Id;
			destination.TrafficId = source.TrafficId;
			destination.DeviceSerial_ = source.DeviceSerial_;
			destination.IsVirtual = (SavedTaradodEnumType)1;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return destination;
	}

	public DeviceLogOnPerson Map(string contextGuid, Logdata source, DeviceLogOnPerson destination = null)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		try
		{
			if (source == null)
			{
				destination = null;
				return null;
			}
			if (destination == null)
			{
				destination = new DeviceLogOnPerson(contextGuid);
			}
			destination.TerminalType = (DeviceModelEnumType)9309;
			destination.PersonNo = source.accNo;
			destination.IoMode = source.readId;
			destination.DateTimeLogM = DateTime.Parse(source.passTime);
			destination.DeviceModel = "761P GROUPS";
			destination.DeviceSerial = source.deviceKey;
			destination.IdTraffic = long.Parse(source.reqSeqNo);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return destination;
	}
}
