using System;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.Assistance;

public class SystemVariableManager
{
	public static void SetValue(string key, string value)
	{
		try
		{
			value = value.EncryptData();
			Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Machine);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetValue", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Assistance\\SystemVariableManager.cs", 19);
			throw;
		}
	}

	public static string GetValue(string key)
	{
		try
		{
			string environmentVariable = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
			if (string.IsNullOrEmpty(environmentVariable))
			{
				return null;
			}
			environmentVariable = environmentVariable.DecryptString();
			if (string.IsNullOrEmpty(environmentVariable))
			{
				return null;
			}
			return environmentVariable;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetValue", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Assistance\\SystemVariableManager.cs", 35);
			throw;
		}
	}
}
