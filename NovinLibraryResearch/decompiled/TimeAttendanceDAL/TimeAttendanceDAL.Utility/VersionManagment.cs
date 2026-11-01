using System;
using PacketParser;
using PacketParser.Entities;
using TimeAttendanceDAL.Utility.Settings;

namespace TimeAttendanceDAL.Utility;

public class VersionManagment
{
	public static string Version { get; set; }

	public static string AssemblyVersion
	{
		get
		{
			try
			{
				return (Version.Length >= 5) ? Version.Substring(0, 5) : Version;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AssemblyVersion", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\VersionManagment.cs", 22);
			}
			return "";
		}
	}

	public static string App4CharacterVersion
	{
		get
		{
			string text = AssemblyVersion.Replace(".", "");
			switch (text.Length)
			{
			case 1:
				text += "000";
				break;
			case 2:
				text += "00";
				break;
			default:
				if (text.Length > 3)
				{
					text = text.Substring(0, 3);
				}
				break;
			}
			return text;
		}
	}

	public static string LkSerial
	{
		get
		{
			try
			{
				string text = ClsRegistry.Read("LKSerial").Replace("-", "");
				if (string.IsNullOrEmpty(text))
				{
					text = LkOperations.GetLkOperations("").LkSerial();
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LkSerial", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\VersionManagment.cs", 78);
				return "";
			}
		}
		set
		{
			try
			{
				if (value == null)
				{
					ClsRegistry.Write("LKSerial", "");
				}
				else
				{
					ClsRegistry.Write("LKSerial", value.Replace("-", ""));
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LkSerial", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\VersionManagment.cs", 93);
			}
		}
	}

	public static string SoftwareSerial
	{
		get
		{
			try
			{
				string text = ClsRegistry.Read("SoftwareSerial");
				if (!(text.Substring(0, 4) == AssemblyVersion3Num().ToString()) && text.Length > 0)
				{
					text = AssemblyVersion3Num() + text.Substring(text.Length - 10, 10);
				}
				Count();
				if (text.Length < 2)
				{
					text = GenerateSoftwareSerial();
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SoftwareSerial", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\VersionManagment.cs", 120);
				return "";
			}
		}
		set
		{
			try
			{
				ClsRegistry.Write("SoftwareSerial", value);
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SoftwareSerial", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\VersionManagment.cs", 132);
			}
		}
	}

	public static int AssemblyVersion3Num()
	{
		int result = 0;
		try
		{
			int.TryParse(AssemblyVersion.Substring(0, 5).Replace(".", ""), out result);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AssemblyVersion3Num", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\VersionManagment.cs", 38);
		}
		return result;
	}

	public static void Count()
	{
		try
		{
			int num = Utilities.ParseToInt(ClsRegistry.Read("X125"), 0);
			if (num > 1000 || num < 0)
			{
				ClsRegistry.Write("X125", 1);
				SoftwareSerial = GenerateSoftwareSerial();
			}
			else
			{
				ClsRegistry.Write("X125", num + 1);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Count", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\VersionManagment.cs", 154);
		}
	}

	private static string GenerateSoftwareSerial()
	{
		try
		{
			ClsRegistry.Write("X125", 1);
			string inStr = Guid.NewGuid().ToString();
			inStr = Conv2Number(inStr).ToString();
			int length = inStr.Length;
			inStr = inStr.Substring(length - 10);
			return AssemblyVersion3Num() + inStr;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GenerateSoftwareSerial", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\VersionManagment.cs", 171);
		}
		return "";
	}

	public static long Conv2Number(string InStr)
	{
		try
		{
			InStr = InStr.ToUpper();
			InStr = InStr.Replace("A", Convert.ToString(20));
			InStr = InStr.Replace("B", Convert.ToString(12));
			InStr = InStr.Replace("C", Convert.ToString(0));
			InStr = InStr.Replace("D", Convert.ToString(18));
			InStr = InStr.Replace("E", Convert.ToString(2));
			InStr = InStr.Replace("F", Convert.ToString(0));
			InStr = InStr.Replace("G", Convert.ToString(9));
			InStr = InStr.Replace("H", Convert.ToString(7));
			InStr = InStr.Replace("I", Convert.ToString(5));
			InStr = InStr.Replace("J", Convert.ToString(23));
			InStr = InStr.Replace("K", Convert.ToString(6));
			InStr = InStr.Replace("L", Convert.ToString(3));
			InStr = InStr.Replace("M", Convert.ToString(4));
			InStr = InStr.Replace("N", Convert.ToString(6));
			InStr = InStr.Replace("O", Convert.ToString(7));
			InStr = InStr.Replace("P", Convert.ToString(8));
			InStr = InStr.Replace("Q", Convert.ToString(9));
			InStr = InStr.Replace("R", Convert.ToString(3));
			InStr = InStr.Replace("S", Convert.ToString(2));
			InStr = InStr.Replace("T", Convert.ToString(1));
			InStr = InStr.Replace("U", Convert.ToString(2));
			InStr = InStr.Replace("V", Convert.ToString(33));
			InStr = InStr.Replace("W", Convert.ToString(20));
			InStr = InStr.Replace("X", Convert.ToString(3));
			InStr = InStr.Replace("Y", Convert.ToString(3));
			InStr = InStr.Replace("Z", Convert.ToString(2));
			InStr = InStr.Replace("-", "");
			if (InStr.Length > 10)
			{
				InStr = InStr.Substring(0, 10);
			}
			return Utilities.ParseToInt(InStr, 0);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Conv2Number", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\VersionManagment.cs", 214);
		}
		return 0L;
	}
}
