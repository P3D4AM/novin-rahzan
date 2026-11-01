using System;
using System.Collections.Generic;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using Riss.Devices;
using Serilog;
using TimeAttendanceDevices.FLRHysoon;
using TimeAttendanceDevices.FLRZk;
using ZDC2911;

namespace TimeAttendanceDevices;

[Serializable]
public class RecordAllDevice
{
	public static List<DeviceLogOnPerson> GetAllFromRiss(string contextGuid, List<Record> plst)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		List<DeviceLogOnPerson> list = new List<DeviceLogOnPerson>();
		try
		{
			foreach (Record item2 in plst)
			{
				DeviceLogOnPerson item = new DeviceLogOnPerson(contextGuid)
				{
					DeviceNo = item2.DN,
					DateTimeLogM = item2.Clock,
					IdTraffic = (short)item2.Verify,
					IoMode = (int)item2.MDIN,
					PersonNo = (int)item2.DIN
				};
				list.Add(item);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAllFromRiss", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\RecordAllDevice.cs", 38);
		}
		return list;
	}

	public static List<DeviceLogOnPerson> GetAllFromZdc2911(string contextGuid, List<Records> plst)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		List<DeviceLogOnPerson> list = new List<DeviceLogOnPerson>();
		try
		{
			foreach (Records item2 in plst)
			{
				DeviceLogOnPerson item = new DeviceLogOnPerson(contextGuid)
				{
					DeviceNo = item2.DN,
					DateTimeLogM = item2.Clock,
					IdTraffic = (short)item2.Verify,
					IoMode = (int)item2.MDIN,
					PersonNo = (int)item2.DIN
				};
				list.Add(item);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAllFromZdc2911", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\RecordAllDevice.cs", 65);
		}
		return list;
	}

	public static List<DeviceLogOnPerson> GetAllFromZk(string contextGuid, List<ZkRecord> plst)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		List<DeviceLogOnPerson> list = new List<DeviceLogOnPerson>();
		try
		{
			foreach (ZkRecord item2 in plst)
			{
				DeviceLogOnPerson item = new DeviceLogOnPerson(contextGuid)
				{
					PersonNo = int.Parse(item2.sdwEnrollNumber),
					DeviceNo = item2.iMachineNumber,
					DateTimeLogM = item2.DateM,
					IdTraffic = (short)item2.idwInOutMode
				};
				list.Add(item);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAllFromZk", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\RecordAllDevice.cs", 86);
		}
		return list;
	}

	public static List<DeviceLogOnPerson> GetAllFromClslog(string contextGuid, List<CLSLOG> plst)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		List<DeviceLogOnPerson> list = new List<DeviceLogOnPerson>();
		try
		{
			foreach (CLSLOG item2 in plst)
			{
				DeviceLogOnPerson item = new DeviceLogOnPerson(contextGuid)
				{
					PersonNo = item2.PersonNo,
					DeviceNo = item2.TerminalNo,
					IoMode = item2.Mode,
					IdTraffic = item2.Mode,
					DateTimeLogM = item2.DateM
				};
				list.Add(item);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAllFromClslog", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\RecordAllDevice.cs", 109);
		}
		return list;
	}

	public static List<DeviceLogOnPerson> GetAllFromDynamicHysoon(string contextGuid, List<DynamicHysoonInfo.Logs> plst, int deviceNo)
	{
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		List<DeviceLogOnPerson> list = new List<DeviceLogOnPerson>();
		try
		{
			foreach (DynamicHysoonInfo.Logs item2 in plst)
			{
				int personNo = int.Parse(item2.userId);
				DateTime dateTimeLogM = DateTime.Parse(item2.time.Substring(0, 4) + "/" + item2.time.Substring(4, 2) + "/" + item2.time.Substring(6, 2) + " " + item2.time.Substring(8, 2) + ":" + item2.time.Substring(10, 2));
				int num = 1;
				if (item2.inOut != null)
				{
					string text = item2.inOut.ToLower();
					if (!(text == "in"))
					{
						if (text == "out")
						{
							num = 2;
						}
						else
						{
							try
							{
								short.Parse(item2.inOut);
							}
							catch (Exception ex)
							{
								Log.Error(ex, "terafic mode is :" + item2.inOut);
							}
						}
					}
					else
					{
						num = 1;
					}
				}
				DeviceLogOnPerson item = new DeviceLogOnPerson(contextGuid)
				{
					PersonNo = personNo,
					DeviceNo = deviceNo,
					DateTimeLogM = dateTimeLogM,
					IdTraffic = (short)num
				};
				list.Add(item);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetAllFromDynamicHysoon", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\RecordAllDevice.cs", 150);
		}
		return list;
	}

	public static DeviceLogOnPerson GetFromDynamicHysoonRealTime(string contextGuid, DynamicHysoonInfo.MonitoringLogs log)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		try
		{
			int personNo = int.Parse(log.userId);
			DateTime dateTimeLogM = DateTime.Parse(log.time.Substring(0, 4) + "/" + log.time.Substring(4, 2) + "/" + log.time.Substring(6, 2) + " " + log.time.Substring(8, 2) + ":" + log.time.Substring(10, 2));
			int num = 1;
			if (log.inOut != null)
			{
				num = short.Parse(log.inOut);
			}
			return new DeviceLogOnPerson(contextGuid)
			{
				PersonNo = personNo,
				DateTimeLogM = dateTimeLogM,
				IdTraffic = (short)num,
				IoMode = log.ioMode
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetFromDynamicHysoonRealTime", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\RecordAllDevice.cs", 174);
		}
		return null;
	}
}
