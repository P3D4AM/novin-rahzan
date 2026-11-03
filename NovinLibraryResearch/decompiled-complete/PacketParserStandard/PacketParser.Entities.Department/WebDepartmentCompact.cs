using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities.Interfaces.Department;

namespace PacketParser.Entities.Department;

public class WebDepartmentCompact : Serializable<WebDepartmentCompact>, IDepartmentCompact
{
	private static string apiUrl = HttpHelpers.ApiUrl + "Department/WebDepartmentCompact";

	public short Id { get; set; }

	public string Name { get; set; }

	public Guid TafsilGuid { get; set; }

	public string User { get; set; }

	public DateTime DateExpire { get; set; }

	public string URL { get; set; }

	public string StrType { get; set; }

	public short AndroidCount { get; set; }

	public short BajeCount { get; set; }

	public short PrintCount { get; set; }

	public bool HaveBKWebService { get; set; }

	public string SefareshgirLevel { get; set; }

	public string Description { get; set; }

	public string Password { get; set; }

	public bool GPS { get; set; }

	public Guid Guid { get; set; }

	public DateTime Date { get; set; }

	public string Mail { get; set; }

	public string Tel { get; set; }

	public bool RestApi { get; set; }

	public bool WebSiteWordPress { get; set; }

	public bool CloudeAttendance { get; set; }

	public bool SyncsKolMoeinTafsil { get; set; }

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<List<WebDepartmentCompact>>>> GetAllAsync(string userName, string password, ENSource source, CancellationToken token, ENSource? userType)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebDepartmentCompact>>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<List<WebDepartmentCompact>>>();
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 47);
				}
				if (userType == ENSource.None)
				{
					userType = null;
				}
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebDepartmentCompact>>> returnedSaveFuncInfoWithValue = await DepClient.WebDepartmentCompactApi(userName, password, source, token, userType);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 50);
				ret.value = returnedSaveFuncInfoWithValue.value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 55);
				ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 56);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebDepartment>> GetDeparmentAsync(string userName, string password, short id)
	{
		int line = 0;
		using (new JaegerMonitor("GetDeparmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebDepartment> ret = new ReturnedSaveFuncInfoWithValue<WebDepartment>();
			try
			{
				if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
				{
					return ret.AddError("User Name Or Password Is Empty", "GetDeparmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 67);
				}
				ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> returnedSaveFuncInfoWithValue = await GetDeparmentApiAsync(id);
				line = 1;
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetDeparmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 70);
				if (ret.HasError)
				{
					return ret;
				}
				line = 2;
				if (returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode == HttpStatusCode.OK)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
				else
				{
					ret.AddError(EnSystem.None, returnedSaveFuncInfoWithValue.value.Description, "GetDeparmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 76);
				}
				line = 3;
			}
			catch (Exception ex)
			{
				ret.AddError(EnSystem.None, ex.Message, "GetDeparmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 81);
				WebErrorLog.Instance.StartLog(ex, $"The Error Line Is : {line}", "GetDeparmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 82);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebDepartment>> GetDeparmentByTafsilGuidAsync(Guid pTafsilGuid)
	{
		using (new JaegerMonitor("GetDeparmentByTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebDepartment> ret = new ReturnedSaveFuncInfoWithValue<WebDepartment>();
			try
			{
				string url = apiUrl + "/GetTafsil";
				Dictionary<string, string> headers = new Dictionary<string, string> { 
				{
					"TafsilGuid",
					pTafsilGuid.ToString()
				} };
				ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> returnedSaveFuncInfoWithValue = await url.GetFromApi<WebDepartment>(default(CancellationToken), 2, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetDeparmentByTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 95);
				ret.value = returnedSaveFuncInfoWithValue?.value?.Data;
			}
			catch (Exception ex)
			{
				ret.AddError(EnSystem.None, ex.Message, "GetDeparmentByTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 100);
				WebErrorLog.Instance.StartLog(ex, "", "GetDeparmentByTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 101);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebDepartment>>> GetAllByTafsilGuidAsync(Guid tafsilGuid)
	{
		using (new JaegerMonitor("GetAllByTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<WebDepartment>> ret = new ReturnedSaveFuncInfoWithValue<List<WebDepartment>>();
			try
			{
				string url = apiUrl + "/GetAllTafsil";
				Dictionary<string, string> headers = new Dictionary<string, string> { 
				{
					"TafsilGuid",
					tafsilGuid.ToString()
				} };
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebDepartment>>> returnedSaveFuncInfoWithValue = await url.GetFromApi<List<WebDepartment>>(default(CancellationToken), 2, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllByTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 114);
				ret.value = returnedSaveFuncInfoWithValue?.value?.Data;
			}
			catch (Exception ex)
			{
				ret.AddError(EnSystem.None, ex.Message, "GetAllByTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 119);
				WebErrorLog.Instance.StartLog(ex, "", "GetAllByTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 120);
			}
			return ret;
		}
	}

	private static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>>> GetDeparmentApiAsync(short id)
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>>();
		using (new JaegerMonitor("GetDeparmentApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs"))
		{
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> returnedSaveFuncInfoWithValue = await (apiUrl + "/Get/" + id).GetFromApi<WebDepartment>(default(CancellationToken), 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetDeparmentApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 132);
				ret.value = returnedSaveFuncInfoWithValue.value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetDeparmentApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 137);
				ret.AddReturnedValue(ex, "", "GetDeparmentApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebDepartmentCompact.cs", 138);
			}
			return ret;
		}
	}
}
