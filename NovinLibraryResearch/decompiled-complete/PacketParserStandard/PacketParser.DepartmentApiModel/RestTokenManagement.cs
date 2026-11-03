using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class RestTokenManagement
{
	private static Dictionary<string, RestTokenMini> ListActiveUser = new Dictionary<string, RestTokenMini>();

	private WebUser Users { get; set; }

	private string DepUserName { get; set; } = "";


	private string DepPassword { get; set; } = "";


	private ENSource Source { get; set; }

	public RestTokenManagement(ENSource source, string depUserUsername, string depPassword)
	{
		DepUserName = depUserUsername;
		DepPassword = depPassword;
		Source = source;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetToken(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			RestTokenMini value = new RestTokenMini();
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 33);
			}
			if (!ListActiveUser.TryGetValue(DepUserName, out value))
			{
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await GetNewToken(token), ReturnedState.None, "GetToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 35);
			}
			else
			{
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await NewOrKeeptoken(value, token, isAdmin: false), ReturnedState.None, "GetToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 37);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex.Message, "", "GetToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 41);
			ret.AddReturnedValue(ex, "", "GetToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 42);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetAdminToken(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			RestTokenMini value = new RestTokenMini();
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 52);
			}
			if (!ListActiveUser.TryGetValue(DepUserName, out value))
			{
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await GetNewAdminToken(token), ReturnedState.None, "GetAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 54);
			}
			else
			{
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await NewOrKeeptoken(value, token, isAdmin: true), ReturnedState.None, "GetAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 56);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex.Message, "", "GetAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 60);
			ret.AddReturnedValue(ex, "", "GetAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 61);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<string>> GetNewAdminToken(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>
		{
			value = ""
		};
		try
		{
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetNewAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 70);
			}
			ReturnedSaveFuncInfoWithValue<WebUser> returnedSaveFuncInfoWithValue = await DepUserClient.GetUser(DepUserName, DepPassword, Source, token);
			if (ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetNewAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 72).HasError)
			{
				return ret;
			}
			AdminLoginModel item = new AdminLoginModel
			{
				Username = returnedSaveFuncInfoWithValue.value.Name,
				Password = returnedSaveFuncInfoWithValue.value.Pass,
				AdminUsername = DepAdmin.UserName,
				AdminPassword = DepAdmin.Password
			};
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetNewAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 80);
			}
			ReturnedSaveFuncInfoWithValue<WebResponse<GenericApiResponse<RestTokenMini>>> returnedSaveFuncInfoWithValue2 = await HttpHelpers.PostToApi<GenericApiResponse<RestTokenMini>, AdminLoginModel>(item, HttpHelpers.ApiUrl + "adminlogin", null, strongDeserialize: true, 15000, 3);
			if (!returnedSaveFuncInfoWithValue2.HasError)
			{
				ret.value = returnedSaveFuncInfoWithValue2.value.Data.Result?.Token;
				ListActiveUser.Add(DepUserName, returnedSaveFuncInfoWithValue2.value.Data.Result);
				ret.value = returnedSaveFuncInfoWithValue2?.value?.Data?.Result?.Token ?? "";
			}
			else
			{
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetNewAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 89);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex.Message, "", "GetNewAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 93);
			ret.AddReturnedValue(ex, "", "GetNewAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 94);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetNewToken(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>
		{
			value = ""
		};
		try
		{
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetNewToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 103);
			}
			SimpleLoginModel item = new SimpleLoginModel
			{
				Username = DepUserName,
				Password = DepPassword
			};
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetNewToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 109);
			}
			ReturnedSaveFuncInfoWithValue<WebResponse<GenericApiResponse<RestTokenMini>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<GenericApiResponse<RestTokenMini>, SimpleLoginModel>(item, HttpHelpers.ApiUrl + "login", null, strongDeserialize: false, 15000, 3);
			if (!returnedSaveFuncInfoWithValue.HasError)
			{
				ret.value = returnedSaveFuncInfoWithValue.value?.Data.Result?.Token;
				if (string.IsNullOrEmpty(returnedSaveFuncInfoWithValue?.value?.Data.Result?.Token))
				{
					return ret.AddError("مجوز دسترسی به این قسمت از سمت سرور تایید نشد", "GetNewToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 114);
				}
				ListActiveUser.Add(DepUserName, returnedSaveFuncInfoWithValue.value.Data.Result);
				ret.value = returnedSaveFuncInfoWithValue.value.Data.Result.Token;
			}
			else
			{
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetNewToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 118);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex.Message, "", "GetNewToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 122);
			ret.AddReturnedValue(ex, "", "GetNewToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 123);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<string>> NewOrKeeptoken(RestTokenMini Resttoken, CancellationToken token, bool isAdmin)
	{
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>
		{
			value = ""
		};
		try
		{
			if (token.IsCancellationRequested)
			{
				ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "NewOrKeeptoken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 132);
			}
			if (Resttoken?.ExpirationDate > DateTime.Now.AddSeconds(60.0))
			{
				ret.value = Resttoken?.Token;
			}
			else if (ListActiveUser.Remove(DepUserName))
			{
				if (!isAdmin)
				{
					ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await GetNewToken(token), ReturnedState.None, "NewOrKeeptoken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 138);
				}
				else
				{
					ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await GetNewAdminToken(token), ReturnedState.None, "NewOrKeeptoken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 140);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex.Message, "", "NewOrKeeptoken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 145);
			ret.AddReturnedValue(ex, "", "NewOrKeeptoken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\RestTokenManagement.cs", 146);
		}
		return ret;
	}
}
