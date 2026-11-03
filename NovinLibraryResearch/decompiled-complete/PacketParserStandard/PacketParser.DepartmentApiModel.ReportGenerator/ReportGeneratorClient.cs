using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PacketParser.DepartmentApiModel.ReportGenerator;

public class ReportGeneratorClient
{
	private static string Url = HttpHelpers.ApiUrl + "Report/ReportGenerator/";

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebReportGenerator>>> GetAllAsync(ReportGeneratorFilter model, string userName, string password, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebReportGenerator>> ret = new ReturnedSaveFuncInfoWithValue<List<WebReportGenerator>>();
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs"))
		{
			try
			{
				string url = Url + "GetAll";
				ReturnedSaveFuncInfoWithValue<WebResponse<ApiResponseModel<List<WebReportGenerator>>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<ApiResponseModel<List<WebReportGenerator>>, ReportGeneratorFilter>(model, url, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 24);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 30);
				ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 31);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebReportGenerator>> GetAsync(Guid pGuid)
	{
		ReturnedSaveFuncInfoWithValue<WebReportGenerator> ret = new ReturnedSaveFuncInfoWithValue<WebReportGenerator>();
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs"))
		{
			try
			{
				string url = Url + "Get";
				Dictionary<string, string> headers = new Dictionary<string, string> { 
				{
					"Guid",
					pGuid.ToString()
				} };
				ReturnedSaveFuncInfoWithValue<WebResponse<WebReportGenerator>> returnedSaveFuncInfoWithValue = await url.GetFromApi<WebReportGenerator>(default(CancellationToken), 3, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 44);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 50);
				ret.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 51);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveAsync(WebReportGenerator item, ENSource source, string userName, string password, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs"))
		{
			try
			{
				if (token.IsCancellationRequested)
				{
					ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 61);
				}
				if (ret.HasError)
				{
					return ret;
				}
				string url = Url + "Save";
				ret.AddReturnedValue(await HttpHelpers.PostToApiByAdminToken<SaveResponse, WebReportGenerator>(item, url, userName, password, source, null, strongDeserialize: false, 15000, 3), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 65);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 69);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 70);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, string userName, string pass)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			string url = Url + "Delete";
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"Guid",
				guid.ToString()
			} };
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await url.GetFromApi<bool>(userName, pass, ENSource.Accounting, default(CancellationToken), 3, headers), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 81);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 85);
			ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ReportGenerator\\ReportGeneratorClientcs.cs", 86);
		}
		return ret;
	}
}
