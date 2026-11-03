using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel.RequestModel;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class CustomerClient
{
	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebCusCompact>>> GetAllCustomerCompact(string DepUsername, string DepPass, CustomerRequestModel Model, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebCusCompact>> ret = new ReturnedSaveFuncInfoWithValue<List<WebCusCompact>>
		{
			value = new List<WebCusCompact>()
		};
		using (new JaegerMonitor("GetAllCustomerCompact", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\CustomerClient.cs"))
		{
			try
			{
				string url = HttpHelpers.ApiUrl + "api/CustomerCompact/list";
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<WebCusCompact>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<PagedResponseModel<WebCusCompact>, CustomerRequestModel>(Model, url, DepUsername, DepPass, ENSource.None, null, strongDeserialize: false, 15000, 3);
				if (returnedSaveFuncInfoWithValue.HasError)
				{
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllCustomerCompact", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\CustomerClient.cs", 24);
				}
				else
				{
					ret.value = returnedSaveFuncInfoWithValue.value?.Data?.Results ?? new List<WebCusCompact>();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllCustomerCompact", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\CustomerClient.cs", 31);
				ret.AddReturnedValue(ex, "", "GetAllCustomerCompact", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\CustomerClient.cs", 32);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebCusGroup>>> GetAllCusGroupAsync(string DepUsername, string DepPass, ENSource source, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebCusGroup>> ret = new ReturnedSaveFuncInfoWithValue<List<WebCusGroup>>
		{
			value = new List<WebCusGroup>()
		};
		using (new JaegerMonitor("GetAllCusGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\CustomerClient.cs"))
		{
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<WebCusGroup>>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "api/CustomerGroup/list?PageSize=10000&Page=1").GetFromApi<PagedResponseModel<WebCusGroup>>(DepUsername, DepPass, source, token, 3);
				if (returnedSaveFuncInfoWithValue.HasError)
				{
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllCusGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\CustomerClient.cs", 45);
				}
				else
				{
					ret.value = returnedSaveFuncInfoWithValue.value?.Data?.Results ?? new List<WebCusGroup>();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllCusGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\CustomerClient.cs", 51);
				ret.AddReturnedValue(ex, "", "GetAllCusGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\CustomerClient.cs", 52);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebCus>>> GetAllCustomerAsync(string DepUsername, string DepPass, CustomerRequestModel Model, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebCus>> ret = new ReturnedSaveFuncInfoWithValue<List<WebCus>>
		{
			value = new List<WebCus>()
		};
		using (new JaegerMonitor("GetAllCustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\CustomerClient.cs"))
		{
			try
			{
				string url = HttpHelpers.ApiUrl + "api/Customers/list";
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<WebCus>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<PagedResponseModel<WebCus>, CustomerRequestModel>(Model, url, DepUsername, DepPass, ENSource.None, null, strongDeserialize: false, 15000, 3);
				if (returnedSaveFuncInfoWithValue.HasError)
				{
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllCustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\CustomerClient.cs", 67);
				}
				else
				{
					ret.value = returnedSaveFuncInfoWithValue.value?.Data?.Results ?? new List<WebCus>();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllCustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\CustomerClient.cs", 74);
				ret.AddReturnedValue(ex, "", "GetAllCustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\CustomerClient.cs", 75);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<WebCus>>> GetCustomerAsync(Guid CusGuid, string depUserName, ENSource source, string depPass)
	{
		return await (HttpHelpers.ApiUrl + $"api/Customers/{CusGuid}").GetFromApi<WebCus>(depUserName, depPass, source, default(CancellationToken), 3);
	}
}
