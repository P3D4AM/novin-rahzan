using System;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PayamakSettings.FlrSettings;

namespace PayamakSettings;

public class Db
{
	private static string _db = "";

	private static string _cn = "";

	private static string _conn = "";

	public static int Sqlcndef = 1;

	public static string Version { get; set; }

	public static DateTime DateM { get; set; }

	public static bool NewMessage
	{
		get
		{
			return Registry.ReadBool("NewMessage");
		}
		set
		{
			Registry.Write("NewMessage", value);
		}
	}

	public static bool NewCall
	{
		get
		{
			return Registry.ReadBool("NewCall");
		}
		set
		{
			Registry.Write("NewCall", value);
		}
	}

	public static bool NewDelivery
	{
		get
		{
			return Registry.ReadBool("NewDelivery");
		}
		set
		{
			Registry.Write("NewDelivery", value);
		}
	}

	public static bool SentMessage
	{
		get
		{
			return Registry.ReadBool("SentMessage");
		}
		set
		{
			Registry.Write("SentMessage", value);
		}
	}

	public static Task<string> GetHesabdariConnectionStringAsync()
	{
		return ReadSettings.GetValueAsync("CN_Hesabdari");
	}

	public static Task SetHesabdariConnectionString(string value)
	{
		return ReadSettings.SetValueAsync("CN_Hesabdari", value);
	}

	public static Task<bool> GetHesabdariAsync()
	{
		return ReadSettings.GetBoolValueAsync("chbh", def: true);
	}

	public static Task SetHesabdariAsync(bool value)
	{
		return ReadSettings.SetValueAsync("chbh", value.ToString());
	}

	public static Task<bool> GetAutoConnectAsync()
	{
		return ReadSettings.GetBoolValueAsync("AutoConnect", def: true);
	}

	public static Task SetAutoConnectAsync(bool value)
	{
		return ReadSettings.SetValueAsync("AutoConnect", value.ToString());
	}

	public static Task<bool> GetIsAutoAnswerAsync()
	{
		return ReadSettings.GetBoolValueAsync("AutoAnswer", def: true);
	}

	public static Task SetIsAutoAnswerAsync(bool value)
	{
		return ReadSettings.SetValueAsync("AutoAnswer", value.ToString());
	}

	public static Task<string> GetAutoAnswerFileAsync()
	{
		return ReadSettings.GetValueAsync("AutoAnswerFile");
	}

	public static Task SetAutoAnswerFileAsync(string value)
	{
		return ReadSettings.SetValueAsync("AutoAnswerFile", value);
	}

	public static async Task<short> GetNumberPortAsync()
	{
		return (await ReadSettings.GetValueAsync("PortNumber")).ParseToShort(0);
	}

	public static Task SetNumberPortAsync(short value)
	{
		return ReadSettings.SetValueAsync("PortNumber", value.ToString());
	}

	public static async Task<long> GetBaudRateAsync()
	{
		try
		{
			string text = await ReadSettings.GetValueAsync("BaudRate");
			return (text == "") ? 115200 : long.Parse(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBaudRateAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\Db.cs", 39);
			return 115200L;
		}
	}

	public static Task SetBaudRateAsync(long value)
	{
		return ReadSettings.SetValueAsync("BaudRate", value.ToString());
	}
}
