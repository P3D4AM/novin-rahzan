using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Global;

namespace PacketParser.Global.PushNotification;

public class WebPushNotificationClient
{
	private static string Url = HttpHelpers.ApiUrl + "WebPushNotification/";

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebPushNotification>>> GetFromApi(DateTime ModifiedDate, ENSource Source)
	{
		ReturnedSaveFuncInfoWithValue<List<WebPushNotification>> ret = new ReturnedSaveFuncInfoWithValue<List<WebPushNotification>>
		{
			value = new List<WebPushNotification>()
		};
		using (new JaegerMonitor("GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Global\\WebPushNotificationClient.cs"))
		{
			_ = 1;
			try
			{
				using HttpClient clientHttp = new HttpClient
				{
					BaseAddress = new Uri(Url)
				};
				HttpRequestHeaders defaultRequestHeaders = clientHttp.DefaultRequestHeaders;
				short num = (short)Source;
				defaultRequestHeaders.Add("Source", num.ToString());
				clientHttp.DefaultRequestHeaders.Add("ModifiedDate", ModifiedDate.ToString());
				clientHttp.DefaultRequestHeaders.Add("userName", DepAdmin.UserName);
				clientHttp.DefaultRequestHeaders.Add("password", DepAdmin.Password);
				HttpResponseMessage httpResponseMessage = await clientHttp.GetAsync("GetAll", HttpCompletionOption.ResponseHeadersRead);
				if (httpResponseMessage.IsSuccessStatusCode)
				{
					ReturnedSaveFuncInfoWithValue<List<WebPushNotification>> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.value = await HttpHelpers.ReadJsonResponseAsync<List<WebPushNotification>>(httpResponseMessage);
				}
				else
				{
					ret.AddReturnedHttpStatusCode(httpResponseMessage.StatusCode, null, "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Global\\WebPushNotificationClient.cs", 42);
					if (!ret.HasError)
					{
						ret.AddError(EnSystem.None, httpResponseMessage.StatusCode.ToString(), "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Global\\WebPushNotificationClient.cs", 43);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Global\\WebPushNotificationClient.cs", 48);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebPushNotification>>> GetAll(DateTime ExpireDate, ENSource Source, short top = 500)
	{
		ReturnedSaveFuncInfoWithValue<List<WebPushNotification>> ret = new ReturnedSaveFuncInfoWithValue<List<WebPushNotification>>
		{
			value = new List<WebPushNotification>()
		};
		using (new JaegerMonitor("GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Global\\WebPushNotificationClient.cs"))
		{
			_ = 1;
			try
			{
				using HttpClient clientHttp = new HttpClient
				{
					BaseAddress = new Uri(Url)
				};
				HttpRequestHeaders defaultRequestHeaders = clientHttp.DefaultRequestHeaders;
				short num = (short)Source;
				defaultRequestHeaders.Add("Source", num.ToString());
				clientHttp.DefaultRequestHeaders.Add("ExpireDate", ExpireDate.ToString());
				clientHttp.DefaultRequestHeaders.Add("userName", DepAdmin.UserName);
				clientHttp.DefaultRequestHeaders.Add("password", DepAdmin.Password);
				clientHttp.DefaultRequestHeaders.Add("Top", top.ToString());
				HttpResponseMessage httpResponseMessage = await clientHttp.GetAsync("GetAllbyExpire", HttpCompletionOption.ResponseHeadersRead);
				if (httpResponseMessage.IsSuccessStatusCode)
				{
					ReturnedSaveFuncInfoWithValue<List<WebPushNotification>> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.value = await HttpHelpers.ReadJsonResponseAsync<List<WebPushNotification>>(httpResponseMessage);
				}
				else
				{
					ret.AddReturnedHttpStatusCode(httpResponseMessage.StatusCode, null, "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Global\\WebPushNotificationClient.cs", 75);
					if (!ret.HasError)
					{
						ret.AddError(EnSystem.None, httpResponseMessage.StatusCode.ToString(), "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Global\\WebPushNotificationClient.cs", 76);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Global\\WebPushNotificationClient.cs", 81);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> Save(WebPushNotification model)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ret.AddReturnedValue(model.CheckValidation(), ReturnedState.None, "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Global\\WebPushNotificationClient.cs", 91);
			if (ret.HasError)
			{
				return ret;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("UserName", DepAdmin.UserName);
			dictionary.Add("Password", DepAdmin.Password);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HttpHelpers.PostToApi<bool, WebPushNotification>(model, Url + "Save", dictionary, strongDeserialize: false, 15000, 3), ReturnedState.None, "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Global\\WebPushNotificationClient.cs", 96);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Global\\WebPushNotificationClient.cs", 101);
			ret.AddReturnedValue(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Global\\WebPushNotificationClient.cs", 102);
		}
		return ret;
	}
}
