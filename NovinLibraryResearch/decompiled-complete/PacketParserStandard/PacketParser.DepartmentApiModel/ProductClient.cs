using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel.RequestModel;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class ProductClient
{
	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebPrdGroup>>> GetAllPrdGroupAsync(string pUserDep, string pUserDepPassword, ENSource source, CancellationToken token)
	{
		using (new JaegerMonitor("GetAllPrdGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ProductClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<WebPrdGroup>> ret = new ReturnedSaveFuncInfoWithValue<List<WebPrdGroup>>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<WebPrdGroup>>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "api/ProductGroup/list?PageSize=10000&Page=1").GetFromApi<PagedResponseModel<WebPrdGroup>>(pUserDep, pUserDepPassword, source, token, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllPrdGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ProductClient.cs", 22);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data.Results;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllPrdGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ProductClient.cs", 28);
				ret.AddError(EnSystem.None, ex.Message, "GetAllPrdGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ProductClient.cs", 29);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebProduct>> GetProductAsync(string depUsername, string depPass, ENSource source, Guid guidOrder, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<WebProduct> ret = new ReturnedSaveFuncInfoWithValue<WebProduct>();
		using (new JaegerMonitor("GetProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ProductClient.cs"))
		{
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<WebProduct>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "api/Product/" + guidOrder).GetFromApi<WebProduct>(depUsername, depPass, source, token, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ProductClient.cs", 41);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ProductClient.cs", 46);
				ret.AddReturnedValue(ex, "", "GetProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ProductClient.cs", 47);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebProduct>>> GetAllAsync(string pUserDep, string pPass, ProductRequestModel model, CancellationToken token)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ProductClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<WebProduct>> ret = new ReturnedSaveFuncInfoWithValue<List<WebProduct>>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<WebProduct>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiByAdminToken<PagedResponseModel<WebProduct>, ProductRequestModel>(model, HttpHelpers.ApiUrl + "api/Product/list", pUserDep, pPass, ENSource.None, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ProductClient.cs", 60);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data.Results;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ProductClient.cs", 66);
				ret.AddError(EnSystem.None, ex.Message, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\ProductClient.cs", 67);
			}
			return ret;
		}
	}
}
