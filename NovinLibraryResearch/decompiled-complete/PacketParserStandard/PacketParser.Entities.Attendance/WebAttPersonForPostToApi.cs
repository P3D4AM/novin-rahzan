using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;

namespace PacketParser.Entities.Attendance;

public class WebAttPersonForPostToApi
{
	private static string apiUrl = HttpHelpers.ApiUrl + "api/Attendance/Person/";

	public Guid Guid { get; set; }

	public byte[] Data { get; set; }

	public string FullName { get; set; }

	public string UserName { get; set; }

	public byte[] HashPassword { get; set; }

	public short ClientVersion { get; private set; } = 8;


	public string HashedQueryString { get; set; }

	public Guid? DepartmentGuid { get; set; }

	internal Task<ReturnedSaveFuncInfoWithValue<WebResponse<bool>>> PostToApiAsync(LoginInfo loginInfo, CancellationToken token)
	{
		return HttpHelpers.PostToApiAsync<bool, WebAttPersonForPostToApi>(this, apiUrl + "SaveBytes", loginInfo, null, 15000, 5, token);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebAttPerson>> GetAsync(string userName, string password, string depUser_UserName, string depUser_Password, CancellationToken token = default(CancellationToken), ENSource source = ENSource.TimeAttendance)
	{
		ReturnedSaveFuncInfoWithValue<WebAttPerson> result = new ReturnedSaveFuncInfoWithValue<WebAttPerson>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(source, depUser_UserName, depUser_Password).GetToken(token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 344);
			if (result.HasError)
			{
				return result;
			}
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{ "UserName", userName },
				{ "Password", password },
				{ "Authorization", returnedSaveFuncInfoWithValue.value }
			};
			ReturnedSaveFuncInfoWithValue<WebResponse<WebAttPerson>> returnedSaveFuncInfoWithValue2 = await (apiUrl + "Get").GetFromApi<WebAttPerson>(token, 3, headers);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 354);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue2.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 360);
			result.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 361);
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebAttPerson>> GetAsync(string queryString, ENSource source, string depUser_UserName, string depUser_Password, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<WebAttPerson> result = new ReturnedSaveFuncInfoWithValue<WebAttPerson>();
		try
		{
			ReturnedSaveFuncInfoWithValue<ServerCheckLogin> returnedSaveFuncInfoWithValue = await DepClient.CheckLoginAsync(depUser_UserName, depUser_Password, source, token);
			Dictionary<string, string> obj = new Dictionary<string, string> { { "QueryString", queryString } };
			object obj2 = returnedSaveFuncInfoWithValue.value.RestToken ?? "";
			if (obj2 == null)
			{
				obj2 = "";
			}
			obj.Add("Authorization", (string)obj2);
			Dictionary<string, string> headers = obj;
			ReturnedSaveFuncInfoWithValue<WebResponse<WebAttPerson>> returnedSaveFuncInfoWithValue2 = await (apiUrl + "GetByQueryString").GetFromApi<WebAttPerson>(token, 3, headers);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 377);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue2.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 383);
			result.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 384);
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebAttPerson>>> GetAllAsync(string userName, string depUser_UserName, string depUser_Password, Guid sectionGuid, ENSource source = ENSource.TimeAttendance, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<WebAttPerson>> result = new ReturnedSaveFuncInfoWithValue<List<WebAttPerson>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<ServerCheckLogin> returnedSaveFuncInfoWithValue = await DepClient.CheckLoginAsync(depUser_UserName, depUser_Password, source, token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 394);
			if (result.HasError)
			{
				return result;
			}
			if (returnedSaveFuncInfoWithValue.value == null)
			{
				result.AddError("دپارتمان مورد نظر معتبر نمی باشد", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 398);
				return result;
			}
			Dictionary<string, string> obj = new Dictionary<string, string>
			{
				{ "UserName", userName },
				{ "IsAdmin", "yes" },
				{
					"guid",
					$"{sectionGuid}"
				}
			};
			obj.Add("Authorization", (returnedSaveFuncInfoWithValue.value.RestToken ?? "") ?? "");
			Dictionary<string, string> headers = obj;
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebAttPerson>>> returnedSaveFuncInfoWithValue2 = await (apiUrl + "GetAll").GetFromApi<List<WebAttPerson>>(token, 3, headers);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 409);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue2.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 415);
			result.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 416);
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfo> RecoveryPasswordRequestAsync(string personUserName, string depUser_UserName, string depUser_Password, string newPassword, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			LoginInfo loginInfo = new LoginInfo
			{
				UserName = depUser_UserName,
				Password = depUser_Password,
				Source = ENSource.TimeAttendance
			};
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{ "UserName", personUserName },
				{ "Password", newPassword }
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await HttpHelpers.PostToApiAsync<bool, bool>(item: false, apiUrl + "RecoveryPassword", loginInfo, headers, 15000, 5), ReturnedState.None, "RecoveryPasswordRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 436);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RecoveryPasswordRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 440);
			res.AddReturnedValue(ex, "", "RecoveryPasswordRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 441);
		}
		return res;
	}
}
