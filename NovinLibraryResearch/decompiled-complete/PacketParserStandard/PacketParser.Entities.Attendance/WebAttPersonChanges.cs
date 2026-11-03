using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;

namespace PacketParser.Entities.Attendance;

public class WebAttPersonChanges
{
	private static string apiUrl = HttpHelpers.ApiUrl + "api/Attendance/PersonChanges/";

	public Guid PersonGuid { get; set; }

	public string PropertyName { get; set; }

	public string Value { get; set; }

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string userName, string password, Guid personGuid, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			LoginInfo loginInfo = new LoginInfo
			{
				UserName = userName,
				Password = password,
				Source = ENSource.TimeAttendance
			};
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"PersonGuid",
				personGuid.ToString()
			} };
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await HttpHelpers.PostToApiAsync<bool, bool>(item: true, apiUrl + "RemoveChanges", loginInfo, headers, 15000, 5, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 465);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 469);
			res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 470);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebAttPersonChanges>>> GetAllAsync(string userName, string password, ENSource source = ENSource.TimeAttendance, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<WebAttPersonChanges>> result = new ReturnedSaveFuncInfoWithValue<List<WebAttPersonChanges>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<ServerCheckLogin> returnedSaveFuncInfoWithValue = await DepClient.CheckLoginAsync(userName, password, source, token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 480);
			if (result.HasError)
			{
				return result;
			}
			if (returnedSaveFuncInfoWithValue.value == null)
			{
				result.AddError("دپارتمان مورد نظر معتبر نمی باشد", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 484);
				return result;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string> { 
			{
				"Authorization",
				(returnedSaveFuncInfoWithValue.value.RestToken ?? "") ?? ""
			} };
			string url = apiUrl + "GetAll";
			Dictionary<string, string> headers = dictionary;
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebAttPersonChanges>>> returnedSaveFuncInfoWithValue2 = await url.GetFromApi<List<WebAttPersonChanges>>(token, 3, headers);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 489);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue2.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 495);
			result.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 496);
		}
		return result;
	}
}
