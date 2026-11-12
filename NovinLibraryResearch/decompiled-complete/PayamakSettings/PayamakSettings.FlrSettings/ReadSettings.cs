using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace PayamakSettings.FlrSettings;

public class ReadSettings
{
	public static void ResetConnectionStringValid()
	{
	}

	public static async Task<string> GetValueAsync(string propertyName, CancellationToken token = default(CancellationToken))
	{
		return "";
	}

	public static async Task<bool> GetBoolValueAsync(string propertyName, bool def, CancellationToken token = default(CancellationToken))
	{
		try
		{
			string text = await GetValueAsync(propertyName, token);
			if (string.IsNullOrEmpty(text))
			{
				return def;
			}
			if (text.Trim() == "")
			{
				return def;
			}
			bool.TryParse(text, out var result);
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBoolValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\FlrSettings\\ReadSettings.cs", 46);
			return false;
		}
	}

	public static async Task<bool> SetValueAsync(string name, string value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\FlrSettings\\ReadSettings.cs", 72);
			throw;
		}
	}

	internal static void SetValue(string v1, string v2)
	{
		throw new NotImplementedException();
	}
}
