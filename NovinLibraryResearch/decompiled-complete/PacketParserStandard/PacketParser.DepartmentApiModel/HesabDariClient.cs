using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel.RequestModel;
using PacketParser.Entities;
using PacketParser.Entities.Department;

namespace PacketParser.DepartmentApiModel;

public class HesabDariClient
{
	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebTafsil>>> GetAllTafsil(string depUsername, string depPass, TafsilRequestModel model)
	{
		ReturnedSaveFuncInfoWithValue<List<WebTafsil>> ret = new ReturnedSaveFuncInfoWithValue<List<WebTafsil>>
		{
			value = new List<WebTafsil>()
		};
		using (new JaegerMonitor("GetAllTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs"))
		{
			try
			{
				new Dictionary<string, string>();
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<WebTafsil>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiByAdminToken<PagedResponseModel<WebTafsil>, TafsilRequestModel>(model, HttpHelpers.ApiUrl + "api/Tafsil/list", depUsername, depPass, ENSource.None, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 24);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data.Results;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 29);
				ret.AddReturnedValue(ex, "", "GetAllTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 30);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebPardakht>>> GetAllPardakhtAsync(string depUsername, string depPass, PaymentRequestModelcs model)
	{
		ReturnedSaveFuncInfoWithValue<List<WebPardakht>> ret = new ReturnedSaveFuncInfoWithValue<List<WebPardakht>>
		{
			value = new List<WebPardakht>()
		};
		using (new JaegerMonitor("GetAllPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs"))
		{
			try
			{
				new Dictionary<string, string>();
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<WebPardakht>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiByAdminToken<PagedResponseModel<WebPardakht>, PaymentRequestModelcs>(model, HttpHelpers.ApiUrl + "api/Payment/list", depUsername, depPass, ENSource.None, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 42);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data.Results;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 47);
				ret.AddReturnedValue(ex, "", "GetAllPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 48);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebOrder>>> GetAllOrderCompactNewAsync(string depUsername, string depPass, OrderRequestModelcs model)
	{
		ReturnedSaveFuncInfoWithValue<List<WebOrder>> ret = new ReturnedSaveFuncInfoWithValue<List<WebOrder>>
		{
			value = new List<WebOrder>()
		};
		using (new JaegerMonitor("GetAllOrderCompactNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs"))
		{
			int line = 0;
			try
			{
				new Dictionary<string, string>();
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<WebOrder>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiByAdminToken<PagedResponseModel<WebOrder>, OrderRequestModelcs>(model, HttpHelpers.ApiUrl + "api/Order/listAsync", depUsername, depPass, ENSource.None, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllOrderCompactNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 62);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue?.value?.Data?.Results;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetAllOrderCompactNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 67);
				ret.AddReturnedValue(ex, "", "GetAllOrderCompactNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 68);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<T>>> GetAllOrderCompactAsync<T>(string depUsername, string depPass, OrderRequestModelcs model) where T : WebOrderCompact
	{
		ReturnedSaveFuncInfoWithValue<List<T>> ret = new ReturnedSaveFuncInfoWithValue<List<T>>
		{
			value = new List<T>()
		};
		using (new JaegerMonitor("GetAllOrderCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs"))
		{
			int line = 0;
			try
			{
				new Dictionary<string, string>();
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<T>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiByAdminToken<PagedResponseModel<T>, OrderRequestModelcs>(model, HttpHelpers.ApiUrl + "api/OrderCompact/list", depUsername, depPass, ENSource.None, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllOrderCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 82);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue?.value?.Data?.Results;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetAllOrderCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 87);
				ret.AddReturnedValue(ex, "", "GetAllOrderCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 88);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebReception>>> GetAllReceptionCompactNewAsync(string depUsername, string depPass, RecptionRequestModelcs model)
	{
		ReturnedSaveFuncInfoWithValue<List<WebReception>> ret = new ReturnedSaveFuncInfoWithValue<List<WebReception>>
		{
			value = new List<WebReception>()
		};
		using (new JaegerMonitor("GetAllReceptionCompactNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs"))
		{
			int line = 0;
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<WebReception>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiByAdminToken<PagedResponseModel<WebReception>, RecptionRequestModelcs>(model, HttpHelpers.ApiUrl + "api/Reception/listAsync", depUsername, depPass, ENSource.None, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllReceptionCompactNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 101);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data.Results;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetAllReceptionCompactNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 107);
				ret.AddReturnedValue(ex, "", "GetAllReceptionCompactNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 108);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<T>>> GetAllPaymentCompactAsync<T>(string depUsername, string depPass, PaymentRequestModelcs model)
	{
		ReturnedSaveFuncInfoWithValue<List<T>> ret = new ReturnedSaveFuncInfoWithValue<List<T>>
		{
			value = new List<T>()
		};
		using (new JaegerMonitor("GetAllPaymentCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs"))
		{
			int line = 0;
			try
			{
				new Dictionary<string, string>();
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<T>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiByAdminToken<PagedResponseModel<T>, PaymentRequestModelcs>(model, HttpHelpers.ApiUrl + "api/Payment/list", depUsername, depPass, ENSource.None, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllPaymentCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 122);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data.Results;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetAllPaymentCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 128);
				ret.AddReturnedValue(ex, "", "GetAllPaymentCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 129);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebPaymentCompact>>> GetAllPaymentCompactNewAsync(string depUsername, string depPass, PaymentRequestModelcs model)
	{
		ReturnedSaveFuncInfoWithValue<List<WebPaymentCompact>> ret = new ReturnedSaveFuncInfoWithValue<List<WebPaymentCompact>>
		{
			value = new List<WebPaymentCompact>()
		};
		using (new JaegerMonitor("GetAllPaymentCompactNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs"))
		{
			int line = 0;
			try
			{
				new Dictionary<string, string>();
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<WebPaymentCompact>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiByAdminToken<PagedResponseModel<WebPaymentCompact>, PaymentRequestModelcs>(model, HttpHelpers.ApiUrl + "api/Payment/listAsync", depUsername, depPass, ENSource.None, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllPaymentCompactNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 143);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data.Results;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetAllPaymentCompactNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 148);
				ret.AddReturnedValue(ex, "", "GetAllPaymentCompactNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 149);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<T>> GetOrderAsync<T>(string depUsername, string depPass, ENSource source, Guid guidOrder, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<T> ret = new ReturnedSaveFuncInfoWithValue<T>();
		using (new JaegerMonitor("GetOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs"))
		{
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<T>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "api/Order/" + guidOrder).GetFromApi<T>(depUsername, depPass, source, token, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 162);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 167);
				ret.AddReturnedValue(ex, "", "GetOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 168);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebHesabTreeCompact>>> GetAllHesabtreeCompactAsync(CancellationToken token, string depId, HesabType hType = HesabType.All)
	{
		using (new JaegerMonitor("GetAllHesabtreeCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<WebHesabTreeCompact>> ret = new ReturnedSaveFuncInfoWithValue<List<WebHesabTreeCompact>>
			{
				value = new List<WebHesabTreeCompact>()
			};
			try
			{
				Dictionary<string, string> obj = new Dictionary<string, string>
				{
					{
						"UserName",
						DepAdmin.UserName
					},
					{
						"Password",
						DepAdmin.Password
					},
					{ "DepId", depId }
				};
				short num = (short)hType;
				obj.Add("Htype", num.ToString());
				Dictionary<string, string> headers = obj;
				ReturnedSaveFuncInfoWithValue<WebResponse<PagedResponseModel<WebHesabTreeCompact>>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "api/HesabTreeCompact/list").GetFromApi<PagedResponseModel<WebHesabTreeCompact>>(token, 3, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllHesabtreeCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 186);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data.Results;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllHesabtreeCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 191);
				ret.AddReturnedValue(ex, "", "GetAllHesabtreeCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\HesabDariClient.cs", 192);
			}
			return ret;
		}
	}
}
