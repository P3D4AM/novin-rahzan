using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class DepotClient
{
	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebDepot>>> GetAllDepotAsync(string pUserDep, string pUserDepPassword, ENSource source, CancellationToken token)
	{
		using (new JaegerMonitor("GetAllDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepotClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<WebDepot>> ret = new ReturnedSaveFuncInfoWithValue<List<WebDepot>>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<WebDepot>>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "api/Depot/list?PageSize=10000&Page=1").GetFromApi<PagedResponseModel<WebDepot>>(pUserDep, pUserDepPassword, source, token, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepotClient.cs", 20);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data.Results;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepotClient.cs", 26);
				ret.AddError(EnSystem.None, ex.Message, "GetAllDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepotClient.cs", 27);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebDepot>> GetDepotAsync(string depUsername, string depPass, ENSource source, Guid guidOrder, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<WebDepot> ret = new ReturnedSaveFuncInfoWithValue<WebDepot>();
		using (new JaegerMonitor("GetDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepotClient.cs"))
		{
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<WebDepot>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "api/Depot/" + guidOrder).GetFromApi<WebDepot>(depUsername, depPass, source, token, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepotClient.cs", 38);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepotClient.cs", 43);
				ret.AddReturnedValue(ex, "", "GetDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepotClient.cs", 44);
			}
			return ret;
		}
	}
}
