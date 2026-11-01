using System;
using System.Collections.Generic;
using PacketParser.Entities;
using Riss.Devices;
using ZDC2911.UI;

namespace ZDC2911;

public class Realand
{
	private CMForm frm = null;

	public Realand(int DeviceNumber, string Password, string IP, int Port)
	{
		frm = new CMForm(DeviceNumber, Password, IP, Port);
	}

	public bool Connect()
	{
		return frm.Connect();
	}

	public bool Disconnect()
	{
		return frm.DisConnect();
	}

	public List<Records> GetALLGlog()
	{
		List<Record> aLLGlog = frm.GetALLGlog();
		if (aLLGlog == null)
		{
			return null;
		}
		List<Records> list = new List<Records>();
		try
		{
			foreach (Record item in aLLGlog)
			{
				Records records = new Records();
				records.Action = item.Action;
				records.Antipassback = item.Antipassback;
				records.Clock = item.Clock;
				records.DIN = item.DIN;
				records.DN = item.DN;
				records.DoorStatus = item.DoorStatus;
				records.JobCode = item.JobCode;
				records.MDIN = item.MDIN;
				records.Remark = item.Remark;
				records.Verify = item.Verify;
				list.Add(records);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetALLGlog", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\ZDC2911\\Realand.cs", 93);
		}
		return list;
	}
}
