using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;

namespace PacketParser.Entities;

public class WebMessageSession : IDisposable
{
	private static string apiUrl = HttpHelpers.ApiUrl;

	private bool IsDispose;

	public long Sender { get; set; }

	public short DepID { get; set; }

	public string Path { get; set; }

	public ENTelegramSessionStatus LastStatus { get; set; }

	public string LastStatusAsString => LastStatus.GetDisplay();

	public DateTime CreationDate { get; set; }

	public string CreationDateSH => CreationDate.MiladiToCalture();

	public DateTime Modified { get; set; }

	public string ModifiedSH => Modified.MiladiToCalture();

	public static WebMessageSession Factory(long sender, short depID, string path)
	{
		try
		{
			return new WebMessageSession
			{
				CreationDate = DateTime.Now,
				DepID = depID,
				LastStatus = ENTelegramSessionStatus.None,
				Modified = DateTime.Now,
				Path = path,
				Sender = sender
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Factory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 40);
			return null;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> GenrateTelegramSession(long mobileNumber, string userName, string password, string apiAddress = null, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo response = new ReturnedSaveFuncInfo();
		try
		{
			if (mobileNumber < 9000000000L || mobileNumber > 9999999999L)
			{
				await response.AddErrorAsync("شماره موبایل نامعتبر می باشد", "", "GenrateTelegramSession", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 53);
				return response;
			}
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{ "UserName", userName },
				{ "Password", password }
			};
			ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfo>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiAsync<ReturnedSaveFuncInfo, long>(mobileNumber, (apiAddress ?? apiUrl) + "Message/GenerateSession", LoginInfo.CreateLoginInfo(userName, password, ENSource.None), headers, 15000, 5, token);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				await response.AddErrorAsync(returnedSaveFuncInfoWithValue.ErrorMessage, "", "GenrateTelegramSession", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 64);
				return response;
			}
			response.AddReturnedValue(returnedSaveFuncInfoWithValue.value.Data, ReturnedState.None, "GenrateTelegramSession", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 67);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "خطا در هنگام ارسال درخواست ساخت سشن برای تلگرام", "GenrateTelegramSession", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 71);
			await response.AddErrorAsync(ex.Message, "", "GenrateTelegramSession", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 72);
		}
		return response;
	}

	public static async Task<ReturnedSaveFuncInfo> VarifiTelegram(long mobile, string code, string password, string userName, string deppassword, string apiAddress = null, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo response = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfo>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiAsync<ReturnedSaveFuncInfo, VerifiTelegramEntity>(new VerifiTelegramEntity
			{
				VerificationCode = code,
				Mobile = mobile,
				Password = password
			}, headers: new Dictionary<string, string>
			{
				{ "UserName", userName },
				{ "Password", password }
			}, loginInfo: LoginInfo.CreateLoginInfo(userName, deppassword, ENSource.None), url: (apiAddress ?? apiUrl) + "Message/VerifiCode", timeOutMilliSeconds: 15000, tryCount: 5, token: token);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				await response.AddErrorAsync(returnedSaveFuncInfoWithValue.ErrorMessage, "", "VarifiTelegram", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 101);
				return response;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "خطا در هنگام ارسال درخواست  تایید سشن برای تلگرام", "VarifiTelegram", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 107);
			await response.AddErrorAsync(ex.Message, "", "VarifiTelegram", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 108);
		}
		return response;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<string>> GetLastStatusOf_TelegramLogin(long mobile, string userName, string password, string apiAddress = null, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<string> response = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(ENSource.None, userName, password).GetAdminToken(token);
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"Mobile",
					mobile.ToString()
				},
				{ "UserName", userName },
				{ "Password", password },
				{ "Authorization", returnedSaveFuncInfoWithValue.value }
			};
			ReturnedSaveFuncInfoWithValue<WebResponse<string>> returnedSaveFuncInfoWithValue2 = await ((apiAddress ?? apiUrl) + "Message/LoginLastStatus").GetFromApi<string>(token, 10, headers);
			if (returnedSaveFuncInfoWithValue2.HasError)
			{
				await response.AddErrorAsync(returnedSaveFuncInfoWithValue2.ErrorMessage, "", "GetLastStatusOf_TelegramLogin", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 131);
				return response;
			}
			response.value = returnedSaveFuncInfoWithValue2.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "خطا در هنگام دریافت آخرین وضعیت لاگین تلگرام", "GetLastStatusOf_TelegramLogin", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 138);
			await response.AddErrorAsync(ex.Message, "", "GetLastStatusOf_TelegramLogin", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 139);
		}
		return response;
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteSessionAsync(long mobile, string userName, string password, string apiAddress = null, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{ "UserName", userName },
				{ "Password", password }
			};
			ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfo>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiAsync<ReturnedSaveFuncInfo, long>(mobile, (apiAddress ?? apiUrl) + "Message/DeleteSession", LoginInfo.CreateLoginInfo(userName, password, ENSource.None), headers, 15000, 5, token);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				await ret.AddErrorAsync(returnedSaveFuncInfoWithValue.ErrorMessage, "", "DeleteSessionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 156);
				return ret;
			}
			ret = returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteSessionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 163);
			await ret.AddErrorAsync(ex.Message, "", "DeleteSessionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 164);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebMessageSession>>> GetSessionsAsync(string userName, string password, string apiAddress = null, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<WebMessageSession>> response = new ReturnedSaveFuncInfoWithValue<List<WebMessageSession>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(ENSource.None, userName, password).GetAdminToken(token);
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{ "Authorization", returnedSaveFuncInfoWithValue.value },
				{ "UserName", userName },
				{ "Password", password }
			};
			ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfoWithValue<List<WebMessageSession>>>> returnedSaveFuncInfoWithValue2 = await ((apiAddress ?? apiUrl) + "Message/GetSession").GetFromApi<ReturnedSaveFuncInfoWithValue<List<WebMessageSession>>>(token, 10, headers);
			if (returnedSaveFuncInfoWithValue2.HasError)
			{
				await response.AddErrorAsync(returnedSaveFuncInfoWithValue2.ErrorMessage, "", "GetSessionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 183);
				return response;
			}
			response = returnedSaveFuncInfoWithValue2.value.Data;
			if (response == null)
			{
				if (response == null)
				{
					response = new ReturnedSaveFuncInfoWithValue<List<WebMessageSession>>();
				}
				response.value = new List<WebMessageSession>();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "خطا در هنگام دریافت لیست سشن ها", "GetSessionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 195);
			await response.AddErrorAsync(ex.Message, "", "GetSessionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 196);
		}
		return response;
	}

	public async Task<ENSessionStatus?> ChekSessionStatusAsync(string userName, string password, string apiAddress = null, CancellationToken token = default(CancellationToken))
	{
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"mobileNumber",
				Sender.ToString()
			} };
			ReturnedSaveFuncInfoWithValue<WebResponse<short>> returnedSaveFuncInfoWithValue = await ((apiAddress ?? apiUrl) + "Message/Session/GetStatus").GetFromApi<short>(userName, password, ENSource.None, token, 3, headers);
			if (returnedSaveFuncInfoWithValue.value.Data == 0)
			{
				return null;
			}
			return (ENSessionStatus)returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChekSessionStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageSession.cs", 215);
			return null;
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!IsDispose)
		{
			IsDispose = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
