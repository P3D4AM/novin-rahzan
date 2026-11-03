using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;

namespace PacketParser.Entities.Attendance;

public class WebAttPersonCompact
{
	private static string apiUrl = HttpHelpers.ApiUrl + "api/Attendance/PersonCompact/";

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string Family { get; set; }

	public string FullName { get; set; }

	public string FatherName { get; set; }

	public DateTime DateMBirthday { get; set; }

	public string CodeMeli { get; set; }

	public string Address { get; set; }

	public string ZipCode { get; set; }

	public string Mob1 { get; set; }

	public string Tel1 { get; set; }

	public string Description { get; set; }

	public string UserName { get; set; }

	public string NoBimeh { get; set; }

	public string Shsh { get; set; }

	public string SerialSh { get; set; }

	public string MadrakTahsili { get; set; }

	public string SodorSh { get; set; }

	public DateTime DateMStartWork { get; set; }

	public string DateShStartWork => DateMStartWork.MiladiToCalture();

	public async Task<ReturnedSaveFuncInfo> SaveAsync(string depUser_UserName, string depUser_Password, CancellationToken token = default(CancellationToken), ENSource source = ENSource.TimeAttendance)
	{
		ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<ServerCheckLogin> returnedSaveFuncInfoWithValue = await DepClient.CheckLoginAsync(depUser_UserName, depUser_Password, source, token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 104);
			if (result.HasError)
			{
				return result;
			}
			if (returnedSaveFuncInfoWithValue.value == null)
			{
				result.AddError("دپارتمان مورد نظر معتبر نمی باشد", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 108);
				return result;
			}
			LoginInfo loginInfo = new LoginInfo
			{
				UserName = returnedSaveFuncInfoWithValue.value.DepartmentUserName,
				Password = returnedSaveFuncInfoWithValue.value.DepartmentPassword,
				Source = ENSource.TimeAttendance
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = result;
			returnedSaveFuncInfo.AddReturnedValue(await HttpHelpers.PostToApiAsync<bool, WebAttPersonCompact>(this, apiUrl + "Save", loginInfo, null, 15000, 5, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 117);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 121);
			result.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 122);
		}
		return result;
	}
}
