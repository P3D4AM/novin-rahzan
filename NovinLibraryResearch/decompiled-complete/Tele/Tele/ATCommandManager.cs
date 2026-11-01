using System;
using PacketParser;
using PacketParser.Entities;

namespace Tele;

public class ATCommandManager
{
	public static string GetInputNumber(string Input)
	{
		try
		{
			Input = Input.Replace("H0", "").Replace("H1", "");
			for (int i = 0; i < 10; i++)
			{
				Input = Input.Replace(i + "+", "");
				Input = Input.Replace("+" + i, "");
				Input = Input.Replace(i + "=", "");
				Input = Input.Replace("=" + i, "");
			}
			return Text.RemoveNoNumbers(Input, "");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetInputNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\ATCommandManager.cs", 30);
			return null;
		}
	}

	public static string RemoveNotValidCharacters(string input)
	{
		try
		{
			string text = "";
			for (int i = 0; i < input.Length - 1; i++)
			{
				string text2 = input.ToCharArray()[i].ToString();
				if ("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRTTUVWXYZ\r\n;+ !-+*%#&".Contains(text2))
				{
					text += text2;
				}
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RemoveNotValidCharacters", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\ATCommandManager.cs", 52);
			return null;
		}
	}

	public static bool IsNoResponseCommand(string Command)
	{
		try
		{
			if (Command.ToUpper() == "ATZ")
			{
				return false;
			}
			if (Command.StartsWith("ATD"))
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "IsNoResponseCommand", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\ATCommandManager.cs", 70);
			return false;
		}
	}
}
