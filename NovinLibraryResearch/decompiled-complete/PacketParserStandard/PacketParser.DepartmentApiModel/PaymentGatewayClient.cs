using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class PaymentGatewayClient
{
	private static string Url => HttpHelpers.ApiUrl;

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebPaymentGateway>>> GetAllAsync(bool status, string depUser, string depPass, ENSource source, CancellationToken token)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\PaymentGatewayClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<WebPaymentGateway>> res = new ReturnedSaveFuncInfoWithValue<List<WebPaymentGateway>>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebPaymentGateway>>> returnedSaveFuncInfoWithValue = await (Url + $"api/PaymentGatewayGetAllAsync/{status}").GetFromApi<List<WebPaymentGateway>>(depUser, depPass, source, token, 3);
				res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\PaymentGatewayClient.cs", 22);
				res.value = returnedSaveFuncInfoWithValue.value.Data;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "apiUrl:" + HttpHelpers.ApiUrl, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\PaymentGatewayClient.cs", 27);
				res.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\PaymentGatewayClient.cs", 28);
			}
			return res;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebPaymentGateway>> GetAsync(string depUser, string depPass, Guid guid, ENSource source, CancellationToken token)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\PaymentGatewayClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebPaymentGateway> res = new ReturnedSaveFuncInfoWithValue<WebPaymentGateway>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<WebPaymentGateway>> returnedSaveFuncInfoWithValue = await (Url + $"api/PaymentGateway/{guid}").GetFromApi<WebPaymentGateway>(depUser, depPass, source, token, 3);
				res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\PaymentGatewayClient.cs", 39);
				res.value = returnedSaveFuncInfoWithValue.value.Data;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "apiUrl:" + HttpHelpers.ApiUrl, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\PaymentGatewayClient.cs", 44);
				res.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\PaymentGatewayClient.cs", 45);
			}
			return res;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveAsync(WebPaymentGateway cls, string depUser, string depPass, ENSource source, CancellationToken token)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\PaymentGatewayClient.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				new Dictionary<string, string>();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await HttpHelpers.PostToApi(cls, Url + "api/PaymentGatewaySaveAsync", depUser, depPass, source, null, strongDeserialize: false, 15000, 3), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\PaymentGatewayClient.cs", 57);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "apiUrl:" + HttpHelpers.ApiUrl, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\PaymentGatewayClient.cs", 61);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\PaymentGatewayClient.cs", 62);
			}
			return res;
		}
	}
}
