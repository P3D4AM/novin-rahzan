using System;
using System.Threading;
using PacketParser.Entities;

namespace Tele;

public class CLSNumberDetector
{
	public static string GetInput(NovinPhone phone, short maxSeconds)
	{
		try
		{
			DateTime now = DateTime.Now;
			string text = "";
			string text2 = "";
			while (Math.Abs(now.Subtract(DateTime.Now).Seconds) < maxSeconds)
			{
				Thread.Sleep(100);
				text2 = phone.ReadLine();
				if (!(text2 == "") && !(text2 == "-"))
				{
					if (text2 == "*" || text2 == "#")
					{
						return text;
					}
					text += ATCommandManager.GetInputNumber(text2);
				}
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetInput", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\CLSNumberDetector.cs", 47);
			return null;
		}
	}

	public static string GetChar(NovinPhone phone, short maxSeconds)
	{
		try
		{
			DateTime now = DateTime.Now;
			string text = "";
			while (Math.Abs(now.Subtract(DateTime.Now).Seconds) < maxSeconds)
			{
				Thread.Sleep(100);
				text = phone.ReadLine();
				if (text == "" || text == "-")
				{
					continue;
				}
				return ATCommandManager.GetInputNumber(text).Substring(0, 1);
			}
			return "";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetChar", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\CLSNumberDetector.cs", 76);
			return null;
		}
	}
}
