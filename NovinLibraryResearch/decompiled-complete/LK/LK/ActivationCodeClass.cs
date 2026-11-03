using System;
using PacketParser.Entities;

namespace LK;

public class ActivationCodeClass
{
	public static string ActivateCode;

	public static string SerialCode;

	public static string HardWareSerial;

	public static string Days;

	public static string CountOFScale;

	public static string ClientNetNumber;

	public static string ExpiredTime;

	public static string SoftType;

	public static string ReturnActiveCode(string ActivationCode)
	{
		try
		{
			if (ActivationCode.Length < 10)
			{
				return null;
			}
			if (ActivationCode.Length > 12)
			{
				if (ActivationCode.Length == 16)
				{
					SoftType = Convert.ToString(ActivationCode.Substring(ActivationCode.Length - 1, 1) + ActivationCode.Substring(0, 1));
					ActivationCode = ActivationCode.Remove(0, 1);
					ActivationCode = ActivationCode.Remove(ActivationCode.Length - 1, 1);
				}
				else
				{
					SoftType = "00";
				}
				ClientNetNumber = Convert.ToString(ActivationCode.Substring(ActivationCode.Length - 1, 1) + ActivationCode.Substring(0, 1));
				ActivationCode = ActivationCode.Remove(0, 1);
				ActivationCode = ActivationCode.Remove(ActivationCode.Length - 1, 1);
				Days = Convert.ToString(ActivationCode.Substring(11, 1) + ActivationCode.Substring(4, 1));
				ActivationCode = Convert.ToString(ActivationCode.Remove(4, 1).Remove(10, 1));
				return ActivationCode;
			}
			Days = "00";
			ClientNetNumber = "00";
			return ActivationCode;
		}
		catch
		{
			return "";
		}
	}

	public static string ReturnScaleActiveCode(string ActivationCode)
	{
		try
		{
			if (ActivationCode.Length <= 10)
			{
				return null;
			}
			ClientNetNumber = Convert.ToString(ActivationCode.Substring(0, 1) + ActivationCode.Substring(ActivationCode.Length - 1, 1));
			ActivationCode = ActivationCode.Remove(0, 1);
			ActivationCode = ActivationCode.Remove(ActivationCode.Length - 1, 1);
			CountOFScale = Convert.ToString(ActivationCode.Substring(12, 1) + ActivationCode.Substring(4, 1));
			Days = Convert.ToString(ActivationCode.Substring(13, 1) + ActivationCode.Substring(6, 1));
			ActivationCode = Convert.ToString(ActivationCode.Remove(4, 1).Remove(5, 1).Remove(10, 1)
				.Remove(10, 1));
			return ActivationCode;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReturnScaleActiveCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ActivationCodeClass.cs", 76);
			return "";
		}
	}
}
