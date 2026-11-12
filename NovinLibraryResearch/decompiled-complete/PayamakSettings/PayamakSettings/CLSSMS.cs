using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Entities;
using PayamakSettings.FlrSettings;

namespace PayamakSettings;

public class CLSSMS
{
	private static bool? _gsmEnable;

	public static bool GsmEnable
	{
		get
		{
			try
			{
				if (!_gsmEnable.HasValue)
				{
					string text = Registry.Read("GS_D_002");
					_gsmEnable = text != "" && bool.Parse(text);
				}
				return _gsmEnable.Value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GsmEnable", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\CLSSMS.cs", 31);
				return false;
			}
		}
		set
		{
			try
			{
				_gsmEnable = value;
				Registry.Write("GS_D_002", value);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GsmEnable", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\CLSSMS.cs", 42);
			}
		}
	}

	public static Task<bool> GetDeliveryEnableAsync()
	{
		return ReadSettings.GetBoolValueAsync("GS_D_001", def: false);
	}

	public static Task SetDeliveryEnableAsync(bool value)
	{
		return ReadSettings.SetValueAsync("GS_D_001", value.ToString());
	}

	public static Task<bool> GetRejectIncommingCallsAsync()
	{
		return ReadSettings.GetBoolValueAsync("GS_D_003", def: false);
	}

	public static Task SetRejectIncommingCallsAsync(bool value)
	{
		return ReadSettings.SetValueAsync("GS_D_003", value.ToString());
	}

	public static Task<bool> GetHolidaySmsEnableAsync()
	{
		return ReadSettings.GetBoolValueAsync("Hol_enable", def: true);
	}

	public static Task SetHolidaySmsEnableAsync(bool value)
	{
		return ReadSettings.SetValueAsync("Hol_enable", value.ToString());
	}

	public static async Task<int> GetCountSentAsync()
	{
		try
		{
			string text = await ReadSettings.GetValueAsync("Count_Sent");
			return (text == "") ? 1 : int.Parse(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCountSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\CLSSMS.cs", 60);
			return 1;
		}
	}

	public static Task SetCountSentAsync(int value)
	{
		return ReadSettings.SetValueAsync("Count_Sent", value.ToString());
	}

	public static async Task<DateTime> GetSmsFromTimeAsync()
	{
		try
		{
			string text = await ReadSettings.GetValueAsync("SFTN");
			return (text == "") ? DateTime.Now.Date.AddHours(9.0) : DateTime.Parse(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSmsFromTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\CLSSMS.cs", 75);
			return DateTime.Now;
		}
	}

	public static Task SetSmsFromTimeAsync(DateTime value)
	{
		return ReadSettings.SetValueAsync("SFTN", value.ToString("yyyy/MM/dd HH:mm"));
	}

	public static async Task<DateTime> GetSmsToTimeAsync()
	{
		try
		{
			string text = await ReadSettings.GetValueAsync("STTN");
			return (text == "") ? DateTime.Now.Date.AddHours(22.0) : DateTime.Parse(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSmsToTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\CLSSMS.cs", 91);
			return DateTime.Now;
		}
	}

	public static Task SetSmsToTimeAsync(DateTime value)
	{
		return ReadSettings.SetValueAsync("STTN", value.ToString("yyyy/MM/dd HH:mm"));
	}

	public static EnCellPhoneOperatorType CheckNumberOperator(string Number)
	{
		try
		{
			Number = Number.Substring(0, 3);
			List<string> obj = new List<string>
			{
				"900", "901", "902", "903", "904", "905", "930", "933", "935", "936",
				"937", "938", "939"
			};
			List<string> list = new List<string>
			{
				"915", "912", "910", "911", "913", "914", "915", "916", "917", "918",
				"919"
			};
			List<string> list2 = new List<string> { "921" };
			if (obj.Contains(Number))
			{
				return EnCellPhoneOperatorType.IranCell;
			}
			if (list.Contains(Number))
			{
				return EnCellPhoneOperatorType.HamraheAval;
			}
			if (list2.Contains(Number))
			{
				return EnCellPhoneOperatorType.Rightel;
			}
			return EnCellPhoneOperatorType.None;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckNumberOperator", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\CLSSMS.cs", 121);
		}
		return EnCellPhoneOperatorType.None;
	}
}
