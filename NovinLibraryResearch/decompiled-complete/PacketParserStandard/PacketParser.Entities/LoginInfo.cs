using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;

namespace PacketParser.Entities;

public class LoginInfo
{
	public string UserName { get; set; }

	public string ApiAddress { get; set; }

	public string Password { get; set; }

	public ENSource Source { get; set; }

	public static LoginInfo CreateLoginInfo(string username, string password, ENSource source, string apiAddress = "")
	{
		return new LoginInfo
		{
			UserName = username,
			Password = password,
			Source = source,
			ApiAddress = apiAddress
		};
	}

	public async Task<bool> LoginAsync(CancellationToken token)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(Source, UserName, Password).GetToken(token);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return false;
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value))
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginInfo.cs", 47);
			return false;
		}
	}
}
