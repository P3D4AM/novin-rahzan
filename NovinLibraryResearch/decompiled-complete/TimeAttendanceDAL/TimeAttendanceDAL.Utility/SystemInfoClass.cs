using System;
using System.Collections;
using Microsoft.VisualBasic;

namespace TimeAttendanceDAL.Utility;

public class SystemInfoClass
{
	private static string CpuId()
	{
		try
		{
			string text = ".";
			dynamic @object = Interaction.GetObject("winmgmts:{impersonationLevel=impersonate}!\\\\" + text + "\\root\\cimv2", (string)null);
			dynamic obj = @object.ExecQuery("Select * from Win32_Processor");
			string text2 = "";
			foreach (dynamic item in (IEnumerable)(object)obj)
			{
				text2 = text2 + ", " + item.ProcessorId;
			}
			if (text2.Length > 0)
			{
				text2 = text2.Substring(2);
			}
			return text2;
		}
		catch (Exception)
		{
			return "13HTHT67";
		}
	}

	public static string SystemInfo()
	{
		try
		{
			string text = Convert.ToString(CpuId() + HardId() + MotherboardID());
			text = text.Replace(" ", "");
			if (text.Length > 45)
			{
				text = text.Substring(0, 45);
			}
			return text ?? "";
		}
		catch
		{
			return "13HTHT67";
		}
	}

	private static string MotherboardID()
	{
		try
		{
			string text = ".";
			dynamic @object = Interaction.GetObject("winmgmts:{impersonationLevel=impersonate}!\\\\" + text + "\\root\\cimv2", (string)null);
			object obj = @object.ExecQuery("Select * from Win32_MotherboardDevice");
			string text2 = "";
			foreach (dynamic item in (IEnumerable)obj)
			{
				text2 = text2 + ", " + item.DeviceID;
			}
			if (text2.Length > 0)
			{
				text2 = text2.Substring(2);
			}
			string text3 = "";
			foreach (dynamic item2 in (IEnumerable)obj)
			{
				text3 = text3 + ", " + item2.PNPDeviceID;
			}
			if (text3.Length > 0)
			{
				text3 = text3.Substring(2);
			}
			return text2 + text3;
		}
		catch (Exception)
		{
			return "13HTHT67";
		}
	}

	private static string HardId()
	{
		try
		{
			string text = ".";
			dynamic @object = Interaction.GetObject("winmgmts:{impersonationLevel=impersonate}!\\\\" + text + "\\root\\cimv2", (string)null);
			dynamic obj = @object.ExecQuery("Select * from Win32_DiskDrive");
			string text2 = "";
			foreach (dynamic item in (IEnumerable)(object)obj)
			{
				text2 = text2 + ", " + item.SerialNumber;
			}
			if (text2.Length > 0)
			{
				text2 = text2.Substring(2);
			}
			return text2;
		}
		catch (Exception)
		{
			return "13HTHT67";
		}
	}
}
