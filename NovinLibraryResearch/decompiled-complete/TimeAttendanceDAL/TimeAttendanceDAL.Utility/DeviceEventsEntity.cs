using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.User;

namespace TimeAttendanceDAL.Utility;

public class DeviceEventsEntity
{
	public static CancellationTokenSource tokenLoad = new CancellationTokenSource();

	private static Queue<DeviceEventsEntity> _eventsEntities = new Queue<DeviceEventsEntity>(UserAttendance.SelectedUser.UserSetting.CountReportDevices);

	public string termType { get; set; }

	public int DeviceNo { get; set; }

	public string serialNumber { get; set; }

	public string transactionID { get; set; }

	public DateTime logTime { get; set; }

	public DateTime ModifiedDate { get; set; }

	public long userID { get; set; }

	public string LogType { get; set; }

	public static void SaveEventMessage(DeviceEventsEntity entity)
	{
		try
		{
			int countReportDevices = UserAttendance.SelectedUser.UserSetting.CountReportDevices;
			if (_eventsEntities == null)
			{
				_eventsEntities = new Queue<DeviceEventsEntity>(countReportDevices);
			}
			Queue<DeviceEventsEntity> eventsEntities = _eventsEntities;
			if (eventsEntities != null && eventsEntities.Count >= countReportDevices)
			{
				while (eventsEntities.Count >= countReportDevices)
				{
					_eventsEntities.Dequeue();
				}
			}
			_eventsEntities.Enqueue(entity);
			tokenLoad?.Cancel();
			tokenLoad = new CancellationTokenSource();
			Utility.ReloadDgridReportDevicesAsync(tokenLoad.Token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveEventMessage", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\DeviceEventsEntity.cs", 40);
		}
	}

	public static void SaveEventMessage(string terminalType, int deviceNo, string serialNumber, string transactionID, DateTime logTime, long userID, string LogType)
	{
		try
		{
			SaveEventMessage(new DeviceEventsEntity
			{
				logTime = logTime,
				DeviceNo = deviceNo,
				userID = userID,
				serialNumber = serialNumber,
				termType = terminalType,
				transactionID = transactionID,
				LogType = LogType,
				ModifiedDate = DateTime.Now
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveEventMessage", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\DeviceEventsEntity.cs", 63);
		}
	}

	public static List<DeviceEventsEntity> GetEventMessages()
	{
		List<DeviceEventsEntity> result = new List<DeviceEventsEntity>();
		try
		{
			result = _eventsEntities?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetEventMessages", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\DeviceEventsEntity.cs", 76);
		}
		return result;
	}

	public static void reset()
	{
		_eventsEntities = null;
	}
}
