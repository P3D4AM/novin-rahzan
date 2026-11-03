using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Sms;

public class SMS_WhiteList
{
	private static string ApiUrl = HttpHelpers.ApiUrl + "api/sms/";

	public string UserName { get; set; }

	public string Domain { get; set; }

	public DateTime CreateDate { get; set; }

	public string CreateDateSH => CreateDate.MiladiToCalture();

	public DateTime ExpirationDate { get; set; }

	public string ExpirationDateSH => ExpirationDate.MiladiToCalture();

	public static async Task<List<SMS_WhiteList>> GetAllAsync(CancellationToken token)
	{
		List<SMS_WhiteList> response = new List<SMS_WhiteList>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<SMS_WhiteList>>> returnedSaveFuncInfoWithValue = await (ApiUrl + "GetAllWhiteList").GetFromApi<List<SMS_WhiteList>>(token, 3);
			if (!returnedSaveFuncInfoWithValue.HasError || !returnedSaveFuncInfoWithValue.value.Data.Any())
			{
				response = returnedSaveFuncInfoWithValue.value.Data;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 32);
		}
		return response;
	}

	public static SMS_WhiteList FactoryMethod(DateTime expirtionDate, string username, string domain)
	{
		try
		{
			return new SMS_WhiteList
			{
				CreateDate = DateTime.Now,
				ExpirationDate = expirtionDate,
				UserName = username,
				Domain = domain
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FactoryMethod", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 48);
			return null;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> UpdateSMS_WhiteList(DateTime expirtionDate, string username, string domain, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<bool> result = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			ExpirationDate = expirtionDate;
			UserName = username;
			Domain = domain;
			LoginInfo loginInfo = LoginInfo.CreateLoginInfo(DepAdmin.UserName, DepAdmin.Password, ENSource.Activation);
			result = await SaveAsync(loginInfo, token);
		}
		catch (Exception ex)
		{
			result.AddError(ex.Message, "UpdateSMS_WhiteList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 64);
			WebErrorLog.Instance.StartLog(ex, "", "UpdateSMS_WhiteList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 65);
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> DoExtensionOrNot(bool isExtension, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<bool> result = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			if (isExtension)
			{
				ExpirationDate = ExpirationDate.AddYears(1);
			}
			else
			{
				ExpirationDate = CreateDate.AddDays(-1.0);
			}
			LoginInfo loginInfo = LoginInfo.CreateLoginInfo(DepAdmin.UserName, DepAdmin.Password, ENSource.Activation);
			result = await SaveAsync(loginInfo, token);
		}
		catch (Exception ex)
		{
			result.AddError(ex.Message, "DoExtensionOrNot", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 84);
			WebErrorLog.Instance.StartLog(ex, "", "DoExtensionOrNot", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 85);
		}
		return result;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<bool>> SaveAsync(LoginInfo loginInfo, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<bool> response = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
			ReturnedSaveFuncInfoWithValue<WebResponse<bool>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiAsync<bool, SMS_WhiteList>(this, ApiUrl + "SaveAtWhiteList", loginInfo, headers, 60000, 5);
			if (returnedSaveFuncInfoWithValue.HasError || returnedSaveFuncInfoWithValue.value == null)
			{
				returnedSaveFuncInfoWithValue.ErrorList.ToList().ForEach(delegate(string error)
				{
					response.AddError(error, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 99);
				});
				response.value = false;
			}
			response.value = returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			await response.AddErrorAsync(ex.Message, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 106);
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 107);
		}
		return response;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> DeleteAsync(CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<bool> response = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			LoginInfo loginInfo = LoginInfo.CreateLoginInfo(DepAdmin.UserName, DepAdmin.Password, ENSource.Activation);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			ReturnedSaveFuncInfoWithValue<WebResponse<bool>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiAsync<bool, SMS_WhiteList>(this, ApiUrl + "DeleteFromWhiteList", loginInfo, headers, 60000, 5);
			if (returnedSaveFuncInfoWithValue.HasError || returnedSaveFuncInfoWithValue.value == null)
			{
				returnedSaveFuncInfoWithValue.ErrorList.ToList().ForEach(delegate(string error)
				{
					response.AddError(error, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 122);
				});
				response.value = false;
			}
			if (!returnedSaveFuncInfoWithValue.value.Data)
			{
				await response.AddErrorAsync("علمیات حذف با شکست مواجه شد", "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 128);
			}
			else
			{
				response.value = returnedSaveFuncInfoWithValue.value.Data;
			}
		}
		catch (Exception ex)
		{
			await response.AddErrorAsync(ex.Message, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 132);
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sms\\SMS_WhiteList.cs", 133);
		}
		return response;
	}
}
