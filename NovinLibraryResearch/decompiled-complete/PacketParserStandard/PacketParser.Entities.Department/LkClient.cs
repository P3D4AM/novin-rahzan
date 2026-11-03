using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.LkService;
using Serilog;

namespace PacketParser.Entities.Department;

public class LkClient
{
	public static string Url = HttpHelpers.ApiUrl + "crm/locks/";

	public static string LK = HttpHelpers.ApiUrl + "Lk/";

	public static async Task<ReturnedSaveFuncInfo> Lk_Save(LkModel lk)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<ApiResponseModel<bool>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<ApiResponseModel<bool>, LkModel>(lk, Url + "SaveLk", null, strongDeserialize: false, 15000, 3);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				returnedSaveFuncInfoWithValue.AddError(returnedSaveFuncInfoWithValue.ErrorMessage, "Lk_Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 27);
			}
		}
		catch (HttpRequestException ex)
		{
			ret.AddReturnedValue(ex, "", "Lk_Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 31);
			Log.Warning((Exception)ex, "");
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "Lk_Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 36);
			ret.AddReturnedValue(ex2, "", "Lk_Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 37);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<List<LKTamdid>>>> GetAllTamdidAsync()
	{
		int line = 0;
		ReturnedSaveFuncInfoWithValue<WebResponse<List<LKTamdid>>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<List<LKTamdid>>>();
		try
		{
			line = 1;
			using HttpClient clientHttp = new HttpClient
			{
				BaseAddress = new Uri(LK)
			};
			line = 2;
			HttpResponseMessage response = await clientHttp.GetAsync("GetLkTamdidApi", HttpCompletionOption.ResponseHeadersRead);
			line = 3;
			if (response.IsSuccessStatusCode)
			{
				ret.value = await HttpHelpers.ReadJsonResponseAsync<WebResponse<List<LKTamdid>>>(response);
				ret.value.HttpClietnStatusCode = response.StatusCode;
				return ret;
			}
			ret.AddReturnedHttpStatusCode(response.StatusCode, null, "GetAllTamdidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 61);
		}
		catch (HttpRequestException ex)
		{
			ret.AddReturnedValue(ex, "", "GetAllTamdidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 65);
			Log.Warning((Exception)ex, "");
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "GetAllTamdidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 68);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			ret.AddReturnedValue(ex3, "", "GetAllTamdidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 72);
			if (!ex3.Message.Contains("Unable to connect to the remote server"))
			{
				WebErrorLog.Instance.StartLog(ex3, $"Line:{line}", "GetAllTamdidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 74);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<bool>>> ChangeStLkTamdidAsync(List<int> listId)
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<bool>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<bool>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<bool>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<bool, List<int>>(listId, new Uri(LK)?.ToString() + "ChangeStLkTamdidApi", null, strongDeserialize: false, 15000, 3);
			if (returnedSaveFuncInfoWithValue == null)
			{
				throw new NullReferenceException("res is null!!");
			}
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "ChangeStLkTamdidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 85);
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (HttpRequestException ex)
		{
			ret.AddReturnedValue(ex, "", "ChangeStLkTamdidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 90);
			Log.Warning((Exception)ex, "");
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "ChangeStLkTamdidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 93);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			ret.AddReturnedValue(ex3, "", "ChangeStLkTamdidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 97);
			WebErrorLog.Instance.StartLog(ex3, "", "ChangeStLkTamdidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 98);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<ActivationTBL>>> ActivationTbl_ListNotSents(string Username, string Password)
	{
		ReturnedSaveFuncInfoWithValue<List<ActivationTBL>> ret = new ReturnedSaveFuncInfoWithValue<List<ActivationTBL>>
		{
			value = new List<ActivationTBL>()
		};
		int line = 0;
		try
		{
			line = 1;
			using HttpClient ClientHttp = new HttpClient
			{
				BaseAddress = new Uri(Url)
			};
			line = 2;
			ClientHttp.DefaultRequestHeaders.Add("UserName", Username);
			line = 3;
			ClientHttp.DefaultRequestHeaders.Add("Password", Password);
			line = 4;
			HttpResponseMessage httpResponseMessage = await ClientHttp.GetAsync("ActivationTbl_ListNotSents", HttpCompletionOption.ResponseHeadersRead);
			line = 5;
			if (httpResponseMessage.IsSuccessStatusCode)
			{
				ret.value = await HttpHelpers.ReadJsonResponseAsync<List<ActivationTBL>>(httpResponseMessage);
				line = 9;
			}
			else
			{
				ret.AddReturnedHttpStatusCode(httpResponseMessage.StatusCode, null, "ActivationTbl_ListNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 125);
				if (!ret.HasError)
				{
					ret.AddError(EnSystem.None, httpResponseMessage.StatusCode.ToString(), "ActivationTbl_ListNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 127);
				}
			}
		}
		catch (HttpRequestException ex)
		{
			ret.AddReturnedValue(ex, "", "ActivationTbl_ListNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 132);
			Log.Warning((Exception)ex, "");
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ActivationTbl_ListNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 135);
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "ActivationTbl_ListNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 138);
			ret.AddReturnedValue(ex3, $"Line:{line}//User:{Username}//Pass:{Password}", "ActivationTbl_ListNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 139);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> Tbl_SaveDelivery(string Username, string Password, List<int> lstLk)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			using HttpClient clientHttp = new HttpClient
			{
				BaseAddress = new Uri(Url)
			};
			clientHttp.DefaultRequestHeaders.Add("UserName", Username);
			clientHttp.DefaultRequestHeaders.Add("Password", Password);
			StringContent content = new StringContent(Json.ToStringJson(lstLk), Encoding.UTF8, "application/json");
			HttpResponseMessage httpResponseMessage = await clientHttp.PostAsync("Tbl_SaveDelivery", content);
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				ret.AddReturnedHttpStatusCode(httpResponseMessage.StatusCode, null, "Tbl_SaveDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 156);
				if (!ret.HasError)
				{
					ret.AddError(EnSystem.None, httpResponseMessage.ReturnResponseError(), "", "Tbl_SaveDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 158);
				}
			}
		}
		catch (HttpRequestException ex)
		{
			ret.AddReturnedValue(ex, "", "Tbl_SaveDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 163);
			Log.Warning((Exception)ex, "");
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "Tbl_SaveDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 166);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "Tbl_SaveDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 169);
			ret.AddReturnedValue(ex2, "", "Tbl_SaveDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 170);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<LKInfo>> GetLkAsync(long LkSerial, bool ignoreHttpExceptions, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<LKInfo> ret = new ReturnedSaveFuncInfoWithValue<LKInfo>
		{
			value = null
		};
		HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
		string debugeMessage = "";
		try
		{
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			Console.WriteLine($"{DateTime.Now}) check lk with api has beeen started...");
			response = await new HttpClient
			{
				BaseAddress = new Uri(LK),
				DefaultRequestHeaders = { 
				{
					"LkSerial",
					LkSerial.ToString()
				} }
			}.GetAsync(LK + "Get", HttpCompletionOption.ResponseHeadersRead);
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			response.EnsureSuccessStatusCode();
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			ApiResponseModel<LKInfo> apiResponseModel = await HttpHelpers.ReadJsonResponseAsync<ApiResponseModel<LKInfo>>(response);
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			if (apiResponseModel != null)
			{
				ret.value = apiResponseModel.Data;
			}
			else
			{
				ret.AddError("قفلی با این مشخصات یافت نشد", "GetLkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 193);
			}
		}
		catch (HttpRequestException ex)
		{
			switch (response.StatusCode)
			{
			case HttpStatusCode.Found:
				ret.AddError("قفلی با این مشخصات یافت نشد", "GetLkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 199);
				break;
			case HttpStatusCode.InternalServerError:
				ret.AddError("از طرف سرور پاسخی دریافت نشد مجددا تلاش کنید", "GetLkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 200);
				break;
			case HttpStatusCode.Forbidden:
				ret.AddError("پارامترهای ارسالی صحیح نیست", "GetLkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 201);
				break;
			case HttpStatusCode.GatewayTimeout:
				ret.AddError("عدم توانایی در برقراری ارتباط با سرور", "GetLkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 202);
				break;
			default:
				if (ignoreHttpExceptions)
				{
					ret.AddWarning(ex.Message, "", "GetLkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 204);
				}
				else
				{
					ret.AddReturnedValue(ex, "", "GetLkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 205);
				}
				Log.Warning((Exception)ex, "");
				break;
			}
		}
		catch (WebException exception)
		{
			ret.AddReturnedValue(exception, "", "GetLkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 210);
		}
		catch (OperationCanceledException exception2)
		{
			ret.AddReturnedValue(exception2, "", "GetLkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 211);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "DebugMessage:" + debugeMessage, "GetLkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 214);
			ret.AddReturnedValue(ex2, "", "GetLkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 215);
		}
		finally
		{
			Console.WriteLine($"{DateTime.Now}) check lk with api has beeen finished.\r\n" + "ResultHasError:" + (ret?.HasError.ToString() ?? "null!!") + "\r\n" + $"ignoreHttpExceptions:{ignoreHttpExceptions}\r\n" + "ResultError:" + ret.ErrorMessage + "\r\nResultWarnings:" + ret.WarningMessage);
		}
		return ret;
	}

	public static async Task<string> ActivationCodePacageAsync(LkActivationCodeModel Model, Package package)
	{
		_ = 1;
		try
		{
			HttpResponseMessage httpResponseMessage = null;
			using HttpClient ClientHttp = new HttpClient
			{
				BaseAddress = new Uri(LK)
			};
			StringContent content = new StringContent(Json.ToStringJson(Model), Encoding.UTF8, "application/json");
			if (package == Package.HoghoghVaDastmozd)
			{
				httpResponseMessage = await ClientHttp.PostAsync("ActivationCode_Hozor", content);
			}
			if (httpResponseMessage.IsSuccessStatusCode)
			{
				return await httpResponseMessage.Content.ReadAsStringAsync();
			}
			return "";
		}
		catch (HttpRequestException ex)
		{
			Log.Warning((Exception)ex, "Model:" + Json.ToStringJson(Model));
			return "";
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "ActivationCodePacageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 252);
			return "";
		}
	}

	public static async Task<AccountingActivationCode> AccountingActivationCodeAsync(LkActivationCodeModel Model, CancellationToken token)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<AccountingActivationCode>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiAsync<AccountingActivationCode, LkActivationCodeModel>(Model, HttpHelpers.ApiUrl + "LK/AccountingActivationCodeAsync", null, null, 15000, 5);
			if (!returnedSaveFuncInfoWithValue.HasError)
			{
				return returnedSaveFuncInfoWithValue?.value?.Data;
			}
			return null;
		}
		catch (HttpRequestException ex)
		{
			Log.Warning((Exception)ex, "Model:" + Json.ToStringJson(Model));
			return null;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "AccountingActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 272);
			return null;
		}
	}

	public static async Task<string> ActivationCode(LkActivationCodeModel Model)
	{
		_ = 1;
		try
		{
			using HttpClient ClientHttp = new HttpClient
			{
				BaseAddress = new Uri(LK)
			};
			StringContent content = new StringContent(Json.ToStringJson(Model), Encoding.UTF8, "application/json");
			HttpResponseMessage httpResponseMessage = await ClientHttp.PostAsync("ActivationCode", content);
			if (httpResponseMessage.IsSuccessStatusCode)
			{
				return await httpResponseMessage.Content.ReadAsStringAsync();
			}
			return "";
		}
		catch (OperationCanceledException)
		{
			return "";
		}
		catch (HttpRequestException ex2)
		{
			Log.Warning((Exception)ex2, "Model:" + Json.ToStringJson(Model));
			return "";
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "ActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkClient.cs", 296);
			return "";
		}
	}
}
