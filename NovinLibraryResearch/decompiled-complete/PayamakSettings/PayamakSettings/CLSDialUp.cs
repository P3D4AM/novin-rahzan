using System;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PayamakSettings.FlrSettings;

namespace PayamakSettings;

public class CLSDialUp
{
	public static async Task<bool> GetAutoConnectAsync()
	{
		try
		{
			string text = (await ReadSettings.GetBoolValueAsync("AutoConnectDialUP", def: true)).ToString();
			return text != "" && bool.Parse(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAutoConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\CLSDialUp.cs", 21);
			return false;
		}
	}

	public static void SetAutoConnect(bool value)
	{
		ReadSettings.SetValue("AutoConnectDialUP", value.ToString());
	}

	public static async Task<bool> GetIsAutoAnswerAsync()
	{
		try
		{
			string text = (await ReadSettings.GetBoolValueAsync("AutoAnswerDialUP", def: true)).ToString();
			return text != "" && bool.Parse(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetIsAutoAnswerAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\CLSDialUp.cs", 35);
			return false;
		}
	}

	public static void SetIsAutoAnswer(bool value)
	{
		ReadSettings.SetValue("AutoAnswerDialUP", value.ToString());
	}

	public static async Task<short> GetNumberPortAsync()
	{
		return (await ReadSettings.GetValueAsync("PortNumberDialUP")).ParseToShort(0);
	}

	public static void SetNumberPort(short value)
	{
		ReadSettings.SetValue("PortNumberDialUP", value.ToString());
	}

	public static async Task<long> GetSpeedPortAsync()
	{
		try
		{
			string text = await ReadSettings.GetValueAsync("BaudRateDialUP");
			return (text == "") ? 115200 : long.Parse(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSpeedPortAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\CLSDialUp.cs", 52);
			return 115200L;
		}
	}

	public static void SetSpeedPort(long value)
	{
		ReadSettings.SetValue("BaudRateDialUp", value.ToString());
	}

	public static async Task<int> GetNumberAnswerAutoAsync()
	{
		try
		{
			string text = await ReadSettings.GetValueAsync("NumAutoAnswer");
			return (text == "") ? 10 : int.Parse(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNumberAnswerAutoAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\CLSDialUp.cs", 71);
			return 10;
		}
	}

	public static void SetNumberAnswerAuto(int value)
	{
		ReadSettings.SetValue("NumAutoAnswer", value.ToString());
	}
}
