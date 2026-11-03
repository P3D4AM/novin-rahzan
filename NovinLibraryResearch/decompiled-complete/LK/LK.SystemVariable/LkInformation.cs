using System;
using PacketParser;
using PacketParser.Entities;

namespace LK.SystemVariable;

public class LkInformation
{
	private static readonly string _key = "NovinPardaz";

	public DateTime X12Mx { get; set; }

	public void SetValue()
	{
		try
		{
			string value = Json.ToStringJson(this).EncryptData();
			Environment.SetEnvironmentVariable(_key, value, EnvironmentVariableTarget.Machine);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetValue", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SystemVariable\\LkInformation.cs", 23);
			throw;
		}
	}

	public static LkInformation GetValue()
	{
		try
		{
			string environmentVariable = Environment.GetEnvironmentVariable(_key, EnvironmentVariableTarget.Machine);
			if (string.IsNullOrEmpty(environmentVariable))
			{
				return null;
			}
			environmentVariable = environmentVariable.DecryptString();
			if (string.IsNullOrEmpty(environmentVariable))
			{
				return null;
			}
			return environmentVariable.FromJson<LkInformation>("GetValue", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SystemVariable\\LkInformation.cs", 35);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetValue", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SystemVariable\\LkInformation.cs", 39);
			throw;
		}
	}
}
