using System;
using System.Collections.Generic;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using ZDC2911;

namespace TimeAttendanceDevices.FLRZDC2911;

public class ZDC2911Realand
{
	private Realand HardWare;

	public string ContextGuid { get; set; }

	public ZDC2911Realand(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public ZDC2911Realand(string contextGuid, bool isGetDataFromLan, int DeviceNO, string CommunicationPass, string IP, int TCPPort)
	{
		ContextGuid = contextGuid;
		if (isGetDataFromLan)
		{
			HardWare = new Realand(DeviceNO, CommunicationPass, IP, TCPPort);
		}
		else
		{
			HardWare = new Realand(DeviceNO, CommunicationPass, IP, TCPPort);
		}
	}

	public List<DeviceLogOnPerson> GetAll()
	{
		List<DeviceLogOnPerson> result = new List<DeviceLogOnPerson>();
		try
		{
			HardWare.Connect();
			List<Records> aLLGlog = HardWare.GetALLGlog();
			if (aLLGlog != null)
			{
				result = RecordAllDevice.GetAllFromZdc2911(ContextGuid, aLLGlog);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRZDC2911\\ZDC2911Realand.cs", 43);
		}
		return result;
	}

	public bool TestConnection()
	{
		try
		{
			if (!HardWare.Connect())
			{
				return false;
			}
			HardWare.Disconnect();
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TestConnection", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRZDC2911\\ZDC2911Realand.cs", 64);
			return false;
		}
	}
}
