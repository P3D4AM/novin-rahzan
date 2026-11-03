using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;

namespace PacketParser.Entities.Attendance;

public class WebAttTempVacations
{
	private static string apiUrl = HttpHelpers.ApiUrl + "api/Attendance/Vacation/";

	public Guid Guid { get; set; }

	public Guid PersonGuid { get; set; }

	public string PersonName { get; set; }

	public string Reason { get; set; }

	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }

	public EnPersonLeaveType Type { get; set; }

	public EnPersonLeaveKind Kind { get; set; }

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebAttTempVacations>>> GetAllAsync(Guid? personGuid, string depUser_UserName, string depUser_Password, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<WebAttTempVacations>> result = new ReturnedSaveFuncInfoWithValue<List<WebAttTempVacations>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(ENSource.TimeAttendance, depUser_UserName, depUser_Password).GetToken(token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttTempVacations.cs", 30);
			if (result.HasError)
			{
				return result;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string> { 
			{
				"Authorization",
				(returnedSaveFuncInfoWithValue.value ?? "") ?? ""
			} };
			if ((personGuid ?? Guid.Empty) != Guid.Empty)
			{
				dictionary.Add("PersonGuid", personGuid?.ToString() ?? "");
			}
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebAttTempVacations>>> returnedSaveFuncInfoWithValue2 = await (apiUrl + "GetAll").GetFromApi<List<WebAttTempVacations>>(token, 3, dictionary);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttTempVacations.cs", 37);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue2.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttTempVacations.cs", 43);
			result.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttTempVacations.cs", 44);
		}
		return result;
	}
}
