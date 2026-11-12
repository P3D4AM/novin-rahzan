using System;
using PacketParser;
using PacketParser.Entities;

namespace SharedActivation.LKLogics;

public class SoftwareSerialManager
{
	public static double Conv2Number(string InStr, bool flag = false)
	{
		try
		{
			if (string.IsNullOrEmpty(InStr))
			{
				InStr = "";
			}
			InStr = InStr.ToUpper();
			InStr = InStr.Replace("A", "20");
			InStr = InStr.Replace("B", "12");
			InStr = InStr.Replace("C", "0");
			InStr = InStr.Replace("D", "18");
			InStr = InStr.Replace("E", "2");
			InStr = InStr.Replace("F", "0");
			InStr = InStr.Replace("G", "9");
			InStr = InStr.Replace("H", "7");
			InStr = InStr.Replace("I", "5");
			InStr = InStr.Replace("J", "23");
			InStr = InStr.Replace("K", "6");
			InStr = InStr.Replace("L", "3");
			InStr = InStr.Replace("M", "4");
			InStr = InStr.Replace("N", "6");
			InStr = InStr.Replace("O", "7");
			InStr = InStr.Replace("P", "8");
			InStr = InStr.Replace("Q", "9");
			InStr = InStr.Replace("R", "3");
			InStr = InStr.Replace("S", "2");
			InStr = InStr.Replace("T", "1");
			InStr = InStr.Replace("U", "2");
			InStr = InStr.Replace("V", "33");
			InStr = InStr.Replace("W", "20");
			InStr = InStr.Replace("X", "3");
			InStr = InStr.Replace("Y", "3");
			InStr = InStr.Replace("Z", "2");
			InStr = InStr.Replace("-", "");
			if (flag)
			{
				InStr = Convert.ToInt32(Math.Ceiling(new Random().NextDouble() * 8.0)) + 1 + InStr;
			}
			if (InStr.Length > 10)
			{
				InStr = InStr.Substring(0, 10);
			}
			return InStr.ParseToDouble(0.0, 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Conv2Number", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\SoftwareSerialManager.cs", 53);
		}
		return 0.0;
	}
}
