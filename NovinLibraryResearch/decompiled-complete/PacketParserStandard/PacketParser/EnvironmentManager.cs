using System;
using PacketParser.Entities;

namespace PacketParser;

public class EnvironmentManager<T>
{
	public static void SetValue(string _key, T obj)
	{
		try
		{
			string value = Json.ToStringJson(obj).EncryptData();
			Environment.SetEnvironmentVariable(_key, value, EnvironmentVariableTarget.Machine);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Service\\EnvironmentManager.cs", 18);
			throw;
		}
	}

	public static T? GetValue(string _key)
	{
		try
		{
			string environmentVariable = Environment.GetEnvironmentVariable(_key, EnvironmentVariableTarget.Machine);
			if (string.IsNullOrEmpty(environmentVariable))
			{
				return default(T);
			}
			string text = environmentVariable.DecryptString();
			if (string.IsNullOrEmpty(text))
			{
				return default(T);
			}
			return text.FromJson<T>("GetValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Service\\EnvironmentManager.cs", 30);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Service\\EnvironmentManager.cs", 34);
			throw;
		}
	}
}
